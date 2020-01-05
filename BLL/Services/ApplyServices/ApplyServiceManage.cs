﻿
using DAL.DTO.Apply;
using DAL.Entities.ApplyInfo;
using ExcelReport;
using ExcelReport.Renderers;
using System;
using System.Collections.Generic;
using System.Linq;
using DAL.DTO.Company;
using BLL.Extensions.ApplyExtensions;
using BLL.Extensions;
using DAL.QueryModel;
using System.Threading.Tasks;

namespace BLL.Services.ApplyServices
{
	public partial class ApplyService
	{
		public IEnumerable<Apply> QueryApplies(QueryApplyDataModel model, bool getAllAppliesPermission, out int totalCount)
		{
			totalCount = 0;
			var list = _context.Applies.AsQueryable();
			if (model == null) return null;
			if (model.Status != null) list = list.Where(a => (model.Status.Arrays != null && model.Status.Arrays.Contains((int)a.Status)) || (model.Status.Start <= (int)a.Status && model.Status.End >= (int)a.Status));
			if (model.AuditByCompany != null) list = list.Where(a => a.Response.NowAuditCompany().Code == model.AuditByCompany.Value);
			if (model.CreateCompany != null) list = list.Where(a => a.BaseInfo.From.CompanyInfo.Company.Code == model.CreateCompany.Value);
			if (model.CreateBy != null)
			{
				list = list.Where(a => a.BaseInfo.CreateBy.Id == model.CreateBy.Value || a.BaseInfo.CreateBy.BaseInfo.RealName.Contains(model.CreateBy.Value));
			}
			if (model.CreateFor != null) list = list.Where(a => a.BaseInfo.From.Id == model.CreateFor.Value || a.BaseInfo.From.BaseInfo.RealName.Contains(model.CreateFor.Value));
			bool anyDateFilterIsLessThan30Days = false;
			if (model.Create != null)
			{
				list = list.Where(a => (a.Create >= model.Create.Start && a.Create <= model.Create.End) || (model.Create.Dates != null && model.Create.Dates.Any(d => d.Date.Subtract(a.Create.Value).Days == 0)));
				anyDateFilterIsLessThan30Days |= model.Create.End.Subtract(model.Create.Start).Days <= 360;
			}
			//  默认查询7天内的申请
			if (model.StampLeave == null)
			{
				model.StampLeave = new QueryByDate()
				{
					Start = DateTime.Now,
					End = DateTime.Now.AddDays(7)
				};
			}
			list = list.Where(a => (a.RequestInfo.StampLeave >= model.StampLeave.Start && a.RequestInfo.StampLeave <= model.StampLeave.End) || (model.StampLeave.Dates != null && model.StampLeave.Dates.Any(d => d.Date.Subtract(a.RequestInfo.StampLeave.Value).Days == 0)));
			anyDateFilterIsLessThan30Days |= model.StampLeave.End.Subtract(model.StampLeave.Start).Days <= 360;

			if (model.StampReturn != null)
			{
				list = list.Where(a => (a.RequestInfo.StampReturn >= model.StampReturn.Start && a.RequestInfo.StampReturn <= model.StampReturn.End) || (a.RequestInfo.StampReturn != null && model.StampReturn.Dates.Any(d => d.Date.Subtract(a.RequestInfo.StampReturn.Value).Days == 0)));
				anyDateFilterIsLessThan30Days |= model.StampReturn.End.Subtract(model.StampReturn.Start).Days <= 360;
			}
			if (!getAllAppliesPermission && !anyDateFilterIsLessThan30Days) list = list.Where(a => a.RequestInfo.StampLeave >= new DateTime(DateTime.Now.Year, 1, 1)); //默认返回今年以来所有假期
			if (model.AuditBy != null) list = list.Where(a => a.Response.Any(r => _context.CompanyManagers.Any(m => m.Company.Code == r.Company.Code && m.User.Id == model.AuditBy.Value)));
			if (model.Id != null) list = list.Where(a => model.Id.Arrays.Contains(a.Id));
			list = list.OrderByDescending(a => a.Status).ThenBy(a => a.BaseInfo.Company.Code);
			totalCount = list.Count();
			if (model.Pages == null || model.Pages.PageIndex < 0 || model.Pages.PageSize <= 0) model.Pages = new QueryByPage()
			{
				PageIndex = 0,
				PageSize = 20
			};
			if (model.Pages != null) list = list.Skip(model.Pages.PageIndex * model.Pages.PageSize).Take(model.Pages.PageSize);
			return list.ToList();
		}
		public Task RemoveAllUnSaveApply()
		{
			//寻找所有找过1天未保存的申请
			var list = _context.Applies
						 .Where(a => a.Status == AuditStatus.NotSave)
						 .Where(a => a.Create.HasValue && a.Create.Value.AddDays(1).Subtract(DateTime.Now).TotalDays < 0).ToList();
			RemoveApplies(list);
			return Task.CompletedTask;
		}
		public Task RemoveAllNoneFromUserApply()
		{
			var list = _context.Applies.Where(a => a.BaseInfo.From == null);
			RemoveApplies(list);
			return Task.CompletedTask;
		}
		public Task RemoveApplies(IEnumerable<Apply> list)
		{
			if (list == null) return Task.CompletedTask;
			//删除这些申请的审批流
			foreach (var apply in list) _context.ApplyResponses.RemoveRange(apply.Response);
			//删除这些申请
			_context.Applies.RemoveRange(list);
			var applies = _context.Applies;
			//寻找所有没有创建申请且不是今天创建的 请求信息
			var request = _context.ApplyRequests.Where(r => !applies.Any(a => a.RequestInfo.Id == r.Id)).Where(r => DateTime.Now.Day != r.CreateTime.Day).ToList();
			//删除这些请求信息的福利信息		
			foreach (var add in request) _context.VocationAdditionals.RemoveRange(add.AdditialVocations);
			//删除这些请求信息
			_context.ApplyRequests.RemoveRange(request);
			//寻找所有没有创建申请且不是今天创建的 基础信息
			var baseInfos = _context.ApplyBaseInfos.Where(r => !applies.Any(a => a.BaseInfo.Id == r.Id)).Where(r => DateTime.Now.Day != r.CreateTime.Day);
			//删除这些基础信息		
			_context.ApplyBaseInfos.RemoveRange(baseInfos);
			_context.SaveChanges();
			return Task.CompletedTask;
		}
		public byte[] ExportExcel(string templete, ApplyDetailDto model)
		{
			var list = SheetRenderer.ExtractModelToRender<ApplyDetailDto>(model, (key, value) =>
			{
				switch (key)
				{
					default: return value;
				}
			}).ToList();
			list.Add(new ParameterRenderer("RequestInfo_VocationDescription", model.RequestInfo.RequestInfoVocationDescription()));
			list.Add(new ParameterRenderer("RequestInfo_VocationTotalLength", model.RequestInfo.VocationTotalLength()));
			list.Add(new ParameterRenderer("UserVocationInfo_DetailDescription", model.UserVocationDescription.VocationDescription()));
			list.Add(new ParameterRenderer("Social_IsMarried", model.Social.Settle.Lover.Valid ? "已婚" : "未婚"));

			var sheetRenderers = new SheetRenderer[]
				{
				new SheetRenderer("Sheet1",list.ToArray())
				};
			return Export.ExportToBuffer(templete, sheetRenderers);
		}

		public byte[] ExportExcel(string templete, IEnumerable<ApplyDetailDto> model)
		{
			var list = model.ToList();
			int index = 1;
			if (list.Count == 0) return null;
			var mapList = new List<ParameterRenderer<ApplyDetailDto>>()
			{

				new ParameterRenderer<ApplyDetailDto>("UserVocationInfo_LeftLength", t => t.UserVocationDescription?.LeftLength),
				new ParameterRenderer<ApplyDetailDto>("UserVocationInfo_MaxTripTimes", t => t.UserVocationDescription?.MaxTripTimes),
				new ParameterRenderer<ApplyDetailDto>("UserVocationInfo_NowTimes", t => t.UserVocationDescription?.NowTimes),
				new ParameterRenderer<ApplyDetailDto>("UserVocationInfo_OnTripTimes", t => t.UserVocationDescription?.OnTripTimes),
				new ParameterRenderer<ApplyDetailDto>("UserVocationInfo_YearlyLength", t => t.UserVocationDescription?.YearlyLength),
				new ParameterRenderer<ApplyDetailDto>("UserVocationInfo_Description", t => t.UserVocationDescription?.Description),
				new ParameterRenderer<ApplyDetailDto>("UserVocationInfo_VocationDescription", t => t.UserVocationDescription?.VocationDescription()),

				new ParameterRenderer<ApplyDetailDto>("RequestInfo_VocationTotalLength",
					t => t.RequestInfo.VocationTotalLength()),
				new ParameterRenderer<ApplyDetailDto>("RequestInfo_VocationDescription",
					t => t.RequestInfo.RequestInfoVocationDescription()),
				new ParameterRenderer<ApplyDetailDto>("RequestInfo_OnTripLength", t => t.RequestInfo?.OnTripLength),
				new ParameterRenderer<ApplyDetailDto>("RequestInfo_StampLeave", t => t.RequestInfo?.StampLeave),
				new ParameterRenderer<ApplyDetailDto>("RequestInfo_StampReturn", t => t.RequestInfo?.StampReturn),
				new ParameterRenderer<ApplyDetailDto>("RequestInfo_VocationLength", t => t.RequestInfo?.VocationLength),
				new ParameterRenderer<ApplyDetailDto>("RequestInfo_VocationType", t => t.RequestInfo?.VocationType),
				new ParameterRenderer<ApplyDetailDto>("RequestInfo_ByTransportation",
					t => t.RequestInfo?.ByTransportation),
				new ParameterRenderer<ApplyDetailDto>("RequestInfo_CreateTime", t => t.RequestInfo?.CreateTime),
				new ParameterRenderer<ApplyDetailDto>("RequestInfo_Reason", t => t.RequestInfo?.Reason),
				new ParameterRenderer<ApplyDetailDto>("RequestInfo_Id", t => t.RequestInfo?.Id),
				new ParameterRenderer<ApplyDetailDto>("RequestInfo_VocationPlace", t => t.RequestInfo?.VocationPlace.Name),
				new ParameterRenderer<ApplyDetailDto>("Base_Company", t => t.Base?.CompanyName),
				new ParameterRenderer<ApplyDetailDto>("Base_Duties", t => t.Base?.DutiesName),
				new ParameterRenderer<ApplyDetailDto>("Base_Title", t => t.Base?.UserTitle),
				new ParameterRenderer<ApplyDetailDto>("Base_RealName", t => t.Base?.RealName),
				new ParameterRenderer<ApplyDetailDto>("Base_Id", t => t.Base?.Id),
				new ParameterRenderer<ApplyDetailDto>("Company_Name", t => t.Company?.Name),
				new ParameterRenderer<ApplyDetailDto>("Company_Code", t => t.Company?.Code),
				new ParameterRenderer<ApplyDetailDto>("Status", t => t.Status),
				new ParameterRenderer<ApplyDetailDto>("Create", t => t.Create),
				new ParameterRenderer<ApplyDetailDto>("Duties_Name", t => t.Duties?.Name),
				new ParameterRenderer<ApplyDetailDto>("Social_Phone", t => t.Social?.Phone),
				new ParameterRenderer<ApplyDetailDto>("Social_Settle_Self_AddressDetail]", t => t.Social?.Settle?.Self?.AddressDetail),
				new ParameterRenderer<ApplyDetailDto>("Social_Settle_Self_Address_Name]", t => t.Social?.Settle?.Self?.Address?.Name),
				new ParameterRenderer<ApplyDetailDto>("Social_Id", t => t.Social?.Id),
				new ParameterRenderer<ApplyDetailDto>("Id", t => t.Id),
				new ParameterRenderer<ApplyDetailDto>("Response_SelfRankAudit", t => t.Response?.SelfRankAuditStatus().AuditResult()),
				new ParameterRenderer<ApplyDetailDto>("Response_LastRankAudit", t => t.Response?.LastRankAuditStatus().AuditResult()),
				new ParameterRenderer<ApplyDetailDto>("AuditLeader", t => t.AuditLeader),
				new ParameterRenderer<ApplyDetailDto>("Index", t => index++)
			};
			return Export.ExportToBuffer(templete, new SheetRenderer("Sheet1",
				new RepeaterRenderer<ApplyDetailDto>("Roster", list, mapList.ToArray()),
				new ParameterRenderer("Audit_SelfCompanyName", "科/室"),
				new ParameterRenderer("Audit_HeadCompanyName", "部")
			));
		}
	}
}
