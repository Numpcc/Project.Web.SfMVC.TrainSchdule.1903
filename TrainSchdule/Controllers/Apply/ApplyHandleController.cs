﻿using System;
using System.Collections.Generic;
using BLL.Extensions;
using BLL.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using DAL.DTO.Apply;
using Microsoft.AspNetCore.Authorization;
using TrainSchdule.ViewModels.Apply;
using DAL.DTO.Recall;
using TrainSchdule.ViewModels.System;
using DAL.Entities;
using BLL.Extensions.ApplyExtensions;
using TrainSchdule.ViewModels.Verify;
using Newtonsoft.Json;
using TrainSchdule.Extensions;
using DAL.QueryModel;

namespace TrainSchdule.Controllers.Apply
{
	public partial class ApplyController
	{
		[HttpPost]
		public IActionResult List([FromBody]QueryApplyDataModel model)
		{
			if (model == null) return new JsonResult(ActionStatusMessage.Apply.Default);

			var currentUser = _currentUserService.CurrentUser;
			var list = _applyService.QueryApplies(model, out var totalCount)?.Select(a => a.ToSummaryDto());
			return new JsonResult(new ApplyListViewModel()
			{
				Data = new ApplyListDataModel()
				{
					List =list,
					TotalCount= totalCount
				}
			}); ;
		}

		/// <summary>
		/// 获取申请的详情
		/// </summary>
		/// <param name="id">申请的id</param>
		/// <returns></returns>
		[HttpGet]
		[AllowAnonymous]
		public IActionResult Detail(string id)
		{
			Guid.TryParse(id, out var aId);
			var apply = _applyService.GetById(aId);
			if (apply == null) return new JsonResult(ActionStatusMessage.Apply.NotExist);
			var managedCompany = _usersService.InMyManage(_currentUserService.CurrentUser.Id,out var totalCount);
			var userPermitCompany = managedCompany.Any<Company>(c=>c.Code==apply.Response.NowAuditCompany()?.Code);
			return new JsonResult(new InfoApplyDetailViewModel()
			{
				Data = apply.ToDetaiDto(_usersService.VocationInfo(apply.BaseInfo.From),userPermitCompany)
			});
		}
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		[HttpPost]
		[AllowAnonymous]
		public IActionResult RemoveAllUnSaveApply()
		{
			_applyService.RemoveAllUnSaveApply();
			return new JsonResult(ActionStatusMessage.Success);
		}
		/// <summary>
		/// 召回休假
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		[AllowAnonymous]
		public IActionResult RecallOrder([FromBody]RecallCreateViewModel model)
		{
			if(!ModelState.IsValid) return new JsonResult(new ApiResult(ActionStatusMessage.Fail.Status, JsonConvert.SerializeObject(ModelState.AllModelStateErrors())));
			if (!model.Auth.Verify(_authService))return new JsonResult(ActionStatusMessage.Account.Auth.AuthCode.Invalid);
			RecallOrder result;
			try
			{
				result=recallOrderServices.Create(model.Data.ToVDto());
			}
			catch (ActionStatusMessageException ex)
			{
				return new JsonResult(ex.Status);
			}
			return new JsonResult(new APIResponseIdViewModel(result.Id, ActionStatusMessage.Success));
		}
		[HttpGet]
		[AllowAnonymous]
		public IActionResult RecallOrder(Guid id)
		{
			var recall = _context.RecallOrders.Where(r => r.Id == id).FirstOrDefault();
			if (recall == null) return new JsonResult(ActionStatusMessage.Apply.Recall.NotExist);
			return new JsonResult(new RecallViewModel()
			{
				Data = recall.ToVDto()
			});
		}
	}
}
