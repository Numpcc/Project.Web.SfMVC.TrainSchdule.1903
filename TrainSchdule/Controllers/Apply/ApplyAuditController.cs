﻿using System;
using System.Linq;
using BLL.Helpers;
using DAL.Entities.ApplyInfo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrainSchdule.Extensions;
using TrainSchdule.ViewModels;
using TrainSchdule.ViewModels.Apply;
using TrainSchdule.ViewModels.Verify;

namespace TrainSchdule.Controllers.Apply
{
	public partial class ApplyController
	{
		/// <summary>
		/// 保存申请
		/// </summary>
		/// <param name="id">申请的id</param>
		/// <returns></returns>
		[HttpPut]
		[AllowAnonymous]
		[ProducesResponseType(typeof(Status),0)]

		public IActionResult Save(string id)
		{
			var modelCheck = CheckApplyModelAndDoTask(id, (x) => _applyService.ModifyAuditStatus(x, AuditStatus.NotPublish));
			if (modelCheck.Code == ActionStatusMessage.Fail.status) return new JsonResult(ActionStatusMessage.Apply.Operation.Save.AllReadySave);
			return new JsonResult(modelCheck);
		}
		/// <summary>
		/// 发布申请
		/// </summary>
		/// <param name="id">申请的id</param>
		/// <returns></returns>
		[HttpPut]
		[AllowAnonymous]

		[ProducesResponseType(typeof(Status), 0)]

		public IActionResult Publish(string id)
		{
			var modelCheck = CheckApplyModelAndDoTask(id, (x) => _applyService.ModifyAuditStatus(x, AuditStatus.Auditing));
			if (modelCheck.Code == ActionStatusMessage.Fail.status) return new JsonResult(ActionStatusMessage.Apply.Operation.Publish.AllReadyPublish);
			return new JsonResult(modelCheck);
		}
		/// <summary>
		/// 撤回申请
		/// </summary>
		/// <param name="id">申请的id</param>
		/// <returns></returns>
		[HttpPut]
		[AllowAnonymous]

		[ProducesResponseType(typeof(Status), 0)]

		public IActionResult Withdrew(string id)
		{
			var modelCheck = CheckApplyModelAndDoTask(id, (x) => _applyService.ModifyAuditStatus(x, AuditStatus.Withdrew));
			if(modelCheck.Code==ActionStatusMessage.Fail.status)return new JsonResult(ActionStatusMessage.Apply.Operation.Withdrew.AllReadyWithdrew);
			return new JsonResult(modelCheck);
		}
		
		private ApplyActionResponseViewModel CheckApplyModelAndDoTask(string id,Func<DAL.Entities.ApplyInfo.Apply,bool>callBack)
		{
			Guid.TryParse(id, out var gid);
			var apply = _applyService.Get(gid);
			if (apply == null) return new ApplyActionResponseViewModel(ActionStatusMessage.Apply.NotExist);
			var userid = _currentUserService.CurrentUser?.Id;
			if (userid == null) return new ApplyActionResponseViewModel(ActionStatusMessage.Account.Auth.Invalid.NotLogin);
			if (apply.BaseInfo.From.Id != userid)
			{
				if (apply.Response.All(r => !_companiesService.CheckManagers(r.Company.Code, userid))) return new ApplyActionResponseViewModel(ActionStatusMessage.Account.Auth.Invalid.Default);
			}

			if (callBack.Invoke(apply))return new ApplyActionResponseViewModel(ActionStatusMessage.Success){Data = new ApplyActionResponseDataModel(){Status = apply.Status}};
			return new ApplyActionResponseViewModel(ActionStatusMessage.Fail);
		}

		/// <summary>
		/// 审核申请(可使用登录状态直接授权，也可使用授权人）
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		[AllowAnonymous]
		[ProducesResponseType(typeof(Status), 0)]

		public IActionResult Audit([FromBody]AuditApplyViewModel model)
		{
			if (!ModelState.IsValid) return new JsonResult(new ModelStateExceptionViewModel(ModelState));
			var currentUserName = _currentUserService.HttpContextAccessor.HttpContext.User.Identity.Name;
			if( !model.Verify(_authService))currentUserName=model.AuthByUserID ;
			if (currentUserName == null) return new JsonResult(ActionStatusMessage.Account.Auth.AuthCode.Invalid);
			model.AuthByUserID = currentUserName;
			try
			{
				model.Data.List = model.Data.List.Distinct(new CompareAudit());
				var results = _applyService.Audit(model.ToAuditVDTO(_usersService, _applyService));
				int count = 0;
				return new JsonResult(new ApplyAuditResponseStatusViewModel()
				{
					Data = results.Select(r=>new ApplyAuditResponseStatusDataModel(model.Data.List.ElementAt(count++).Id,r))
				});
			}
			catch (ActionStatusMessageException e)
			{
				return new JsonResult(e.Status);
			}
		}

	}
}
