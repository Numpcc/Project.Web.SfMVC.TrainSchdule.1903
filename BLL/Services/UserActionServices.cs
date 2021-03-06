using Abp.Extensions;
using BLL.Extensions.CreateClientInfo;
using BLL.Helpers;
using BLL.Interfaces;
using DAL.Data;
using DAL.Entities;
using DAL.Entities.Common;
using DAL.Entities.UserInfo;
using DAL.QueryModel;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
	public class UserActionServices : IUserActionServices
	{
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly ApplicationDbContext _context;
		private readonly IUserServiceDetail userServiceDetail;
		private readonly IUsersService usersService;

		public UserActionServices(IHttpContextAccessor httpContextAccessor, ApplicationDbContext context, IUserServiceDetail userServiceDetail, IUsersService usersService)
		{
			_httpContextAccessor = httpContextAccessor;
			_context = context;
			this.userServiceDetail = userServiceDetail;
			this.usersService = usersService;
		}
		public async Task<UserAction> LogAsync(UserOperation operation, string username, string description, bool success = false, ActionRank rank = ActionRank.Debug)
        {
			var context = _httpContextAccessor.HttpContext;
			var ua = context.ClientInfo<UserAction>();
			ua.Date = DateTime.Now;
			ua.Operation = operation;
			ua.UserName = username;
			ua.Success = success;
			ua.Description = description;
			ua.Rank = rank;
			await _context.UserActions.AddAsync(ua);
			await _context.SaveChangesAsync();
			return ua;
		}
		public UserAction Log(UserOperation operation, string username, string description, bool success = false, ActionRank rank = ActionRank.Debug) => LogAsync(operation, username, description, success, rank).Result;

		public bool Permission(Permissions permissions, PermissionDescription key, Operation operation, string permissionUserName, string targetUserCompanyCode, string description = null) => PermissionAsync(permissions, key, operation, permissionUserName, targetUserCompanyCode, description).Result;
		public async Task<bool> PermissionAsync(Permissions permissions, PermissionDescription key, Operation operation, string permissionUserName, string targetUserCompanyCode, string description = null)
		{
			var a =await LogAsync(UserOperation.Permission, permissionUserName, $"授权到{targetUserCompanyCode}执行{key?.Name} {key?.Description}@{operation} {description}", false, ActionRank.Danger);
			if (permissions.Check(key, operation, targetUserCompanyCode))
			{
				Status(a, true, "直接权限");
				return true;
			}
			var u = usersService.GetById(permissionUserName);
			if (u != null)
			{
				var uc = u.CompanyInfo;
				var ud = uc.Duties.IsMajorManager;
				var ucmp = uc.CompanyCode;
				if (targetUserCompanyCode == null || (targetUserCompanyCode.Length >= ucmp.Length && targetUserCompanyCode.StartsWith(ucmp)) && ud)
				{
					Status(a, true, $"单位主官");
					return true;
				}
				else
				{
					var results = userServiceDetail.InMyManage(u).Result;
					if (targetUserCompanyCode == null && results.Item2 > 0) return true; // 如果无授权对象，则有任意单位权限即可
					else if (results.Item2 > 0 && results.Item1.Any(c => targetUserCompanyCode.Length >= c.Code.Length && targetUserCompanyCode.StartsWith(c.Code)))
					{
						Status(a, true, $"单位管理");
						return true;
					}
				}
			}
			//throw new ActionStatusMessageException(ActionStatusMessage.Account.Auth.Invalid.Default);
			return false;

		}
		public async Task<IEnumerable<UserAction>> Query(QueryUserActionViewModel model)
		{
			return await Task.Run(() =>
			{
				if (model == null) return null;
				var r = _context.UserActionsDb.AsQueryable();
				if (model.UserName?.Value != null) r = r.Where(h => h.UserName == model.UserName.Value);
				if (model.Date?.Start != null && model.Date?.End != null)
				{
					r = r.Where(h => h.Date >= model.Date.Start).Where(h => h.Date <= model.Date.End);
				}
				if (model.Rank?.Arrays != null) r = r.Where(h => model.Rank.Arrays.Contains((int)h.Rank));
				if (model.Ip?.Arrays != null) r = r.Where(h => model.Ip.Arrays.Contains(h.Ip));
				if (model.Device?.Arrays != null) r = r.Where(h => model.Device.Arrays.Contains(h.Device));
				if (model.Message?.Value != null) r = r.Where(h => h.Description.Contains(model.Message.Value));
				if (model.Page == null) model.Page = new QueryByPage()
				{
					PageIndex = 0,
					PageSize = 20
				};
				return r.OrderByDescending(h => h.Date).Skip(model.Page.PageSize * model.Page.PageIndex).Take(model.Page.PageSize);
			}).ConfigureAwait(true);
		}

		public UserAction Status(UserAction action, bool success, string description = null)
		{
			if (action == null) return null;
			action.Success = success;
			if (description != null) description = $"$${description}";
			action.Description = $"{action.Description}{description}";
			_context.UserActions.Update(action);
			_context.SaveChanges();
			return action;
		}

		public ApiResult LogNewActionInfo(UserAction action, ApiResult message)
		{
			Status(action, message.Status == 0, message.Message.IsNullOrEmpty() ? null : message.Message);
			return message;
		}
	}
}