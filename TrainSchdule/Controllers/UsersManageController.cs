﻿using BLL.Extensions;
using BLL.Helpers;
using BLL.Interfaces;
using DAL.DTO.Company;
using DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using TrainSchdule.ViewModels;
using TrainSchdule.ViewModels.User;
using TrainSchdule.ViewModels.Verify;

namespace TrainSchdule.Controllers
{
	public partial class UsersController
	{
		private readonly ICompanyManagerServices _companyManagerServices;

		/// <summary>
		/// 此用户所管辖的单位
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpGet]
		[AllowAnonymous]
		[ProducesResponseType(typeof(UserManageRangeDataModel), 0)]
		public IActionResult OnMyManage(string id)
		{
			id = id ?? _currentUserService.CurrentUser?.Id;
			var targetUser = _usersService.Get(id);
			if (targetUser == null) return new JsonResult(ActionStatusMessage.UserMessage.NotExist);
			var result = _usersService.InMyManage(targetUser).Result;
			var list = result.Item1.Select(c => c.ToDto(_companiesService));
			return new JsonResult(new UserManageRangeViewModel()
			{
				Data = new UserManageRangeDataModel()
				{
					List = list,
					TotalCount = result.Item2
				}
			});
		}

		/// <summary>
		/// 移除管辖单位
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpDelete]
		[AllowAnonymous]
		[ProducesResponseType(typeof(ApiResult), 0)]
		public IActionResult OnMyManage([FromBody] UserManageRangeModifyViewModel model)
		{
			if (!ModelState.IsValid) return new JsonResult(new ModelStateExceptionViewModel(ModelState));
			if (model.Auth == null || !model.Auth.Verify(_authService, _currentUserService.CurrentUser?.Id)) return new JsonResult(ActionStatusMessage.Account.Auth.Invalid.Default);
			var id = model.Id ?? _currentUserService.CurrentUser?.Id;
			var authUser = _usersService.Get(model.Auth.AuthByUserID);
			if (authUser == null) return new JsonResult(ActionStatusMessage.UserMessage.NotExist);
			var targetUser = _usersService.Get(id);
			var permit = _userActionServices.Permission(authUser.Application.Permission, DictionaryAllPermission.User.Application, Operation.Remove, authUser.Id, model.Code);
			if (targetUser == null) return new JsonResult(ActionStatusMessage.UserMessage.NotExist);
			var manages = _companyManagerServices.GetManagers(model.Code);
			var manage = manages.Where(u => u.User.Id == targetUser.Id).FirstOrDefault();
			; if (manage == null) return new JsonResult(ActionStatusMessage.CompanyMessage.ManagerMessage.NotExist);
			var unused = _companyManagerServices.Delete(manage);
			return new JsonResult(ActionStatusMessage.Success);
		}

		/// <summary>
		/// 新增管辖单位
		/// </summary>
		/// <param name="model"></param>
		/// <param name="mdzz">填充参数，无需填写</param>
		/// <returns></returns>
		[HttpPost]
		[AllowAnonymous]
		[ProducesResponseType(typeof(ApiResult), 0)]
		public IActionResult OnMyManage([FromBody] UserManageRangeModifyViewModel model, string mdzz)
		{
			if (!model.Auth.Verify(_authService, _currentUserService.CurrentUser?.Id)) return new JsonResult(ActionStatusMessage.Account.Auth.Invalid.Default);
			var authByUser = _usersService.Get(model.Auth.AuthByUserID);
			var id = model.Id ?? _currentUserService.CurrentUser?.Id;
			var targetUser = _usersService.Get(id);
			if (targetUser == null) return new JsonResult(ActionStatusMessage.UserMessage.NotExist);
			var permit = _userActionServices.Permission(authByUser.Application.Permission, DictionaryAllPermission.User.Application, Operation.Create, authByUser.Id, model.Code);
			if (!permit) return new JsonResult(ActionStatusMessage.Account.Auth.Invalid.Default);
			var manages = _companyManagerServices.GetManagers(model.Code);
			var manage = manages.Where(u => u.User.Id == targetUser.Id).FirstOrDefault();
			if (manage != null) return new JsonResult(ActionStatusMessage.CompanyMessage.ManagerMessage.Existed);
			var dto = new CompanyManagerVdto()
			{
				AuditById = model.Auth.AuthByUserID,
				CompanyCode = model.Code,
				UserId = model.Id
			};
			manage = _companyManagerServices.CreateManagers(dto);
			if (manage == null) return new JsonResult(ActionStatusMessage.CompanyMessage.ManagerMessage.Default);
			return new JsonResult(ActionStatusMessage.Success);
		}
	}
}