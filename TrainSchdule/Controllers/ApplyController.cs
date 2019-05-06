﻿using System.Threading.Tasks;
using BLL.Extensions;
using BLL.Helpers;
using BLL.Interfaces;
using DAL.Data;
using DAL.Entities.ApplyInfo;
using DAL.Entities.UserInfo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrainSchdule.ViewModels.Apply;
using TrainSchdule.ViewModels.System;

namespace TrainSchdule.Web.Controllers
{
	[Authorize]
	[Route("[controller]/[action]")]
	public class ApplyController: Controller
	{
		#region filed
		private readonly IUsersService _usersService;
		private readonly ICurrentUserService _currentUserService;
		private readonly IApplyService _applyService;
		private readonly ApplicationDbContext _context;
		private readonly ICompaniesService _companiesService;
		private readonly IVerifyService _verifyService;


		public ApplyController(IUsersService usersService, ICurrentUserService currentUserService, IApplyService applyService, ICompaniesService companiesService, IVerifyService verifyService, ApplicationDbContext context)
		{
			_usersService = usersService;
			_currentUserService = currentUserService;
			_applyService = applyService;
			_companiesService = companiesService;
			_verifyService = verifyService;
			_context = context;
		}

		#endregion

		#region Logic

		[HttpGet]
		public IActionResult AllStatus()
		{
			return new JsonResult(new ApplyAuditStatusViewModel()
			{
				Data = new ApplyAuditStatusDataModel()
				{
					List = ApplyExtensions.StatusDic
				}
			});
		}
		[HttpPost]
		[AllowAnonymous]
		public async Task<IActionResult> BaseInfo([FromBody]SubmitBaseInfoViewModel model)
		{
			if (!ModelState.IsValid) return new JsonResult(new ModelStateExceptionViewModel(ModelState));
			var targetUser = _usersService.Get(model.Id);
			if(targetUser==null)return new JsonResult(ActionStatusMessage.User.NotExist);
			var info=await _applyService.SubmitBaseInfoAsync(Extensions.ApplyExtensions.ToDTO(model,_usersService));
			if(info.Company==null)ModelState.AddModelError("company",$"不存在编号为{model.Company}的单位");
			if(info.Duties==null)ModelState.AddModelError("duties",$"不存在职务代码:{model.Duties}");
			if(info.Social.Address==null)ModelState.AddModelError("home",$"不存在的行政区划{model.HomeAddress}");
			if (!ModelState.IsValid) return new JsonResult(new ModelStateExceptionViewModel(ModelState));
			return new JsonResult(ActionStatusMessage.Success);
		}

		[HttpPost]
		[AllowAnonymous]
		public  IActionResult RequestInfo([FromBody] SubmitRequestInfoViewModel model)
		{
			if (!ModelState.IsValid) return new JsonResult(new ModelStateExceptionViewModel(ModelState));
			var targetUser = _usersService.Get(model.Id);
			if (targetUser == null) return new JsonResult(ActionStatusMessage.User.NotExist);
			var info =  _applyService.SubmitRequest(Extensions.ApplyExtensions.ToDTO(model,_context));
			if(info.VocationPlace==null) ModelState.AddModelError("home", $"不存在的行政区划{model.VocationPlace}");
			if (!ModelState.IsValid) return new JsonResult(new ModelStateExceptionViewModel(ModelState));
			return new JsonResult(ActionStatusMessage.Success);
		}
		#endregion


	}
}
