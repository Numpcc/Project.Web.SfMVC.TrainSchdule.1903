﻿using BLL.Extensions.CreateClientInfo;
using BLL.Interfaces;
using DAL.Data;
using DAL.Entities;
using DAL.Entities.Common;
using DAL.Entities.UserInfo;
using DAL.QueryModel;
using Microsoft.AspNetCore.Http;
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

		public UserAction Log(UserOperation operation, string username, string description, bool success = false, ActionRank rank = ActionRank.Debug)
		{
			var context = _httpContextAccessor.HttpContext;
			var ua = context.ClientInfo<UserAction>();
			ua.Date = DateTime.Now;
			ua.Operation = operation;
			ua.UserName = username;
			ua.Success = success;
			ua.Description = description;
			ua.Rank = rank;
			_context.UserActions.Add(ua);
			_context.SaveChanges();
			return ua;
		}

		public bool Permission(Permissions permissions, PermissionDescription key, Operation operation, string permissionUserName, string targetUserCompanyCode)
		{
			var a = Log(UserOperation.Permission, permissionUserName, $"授权到{targetUserCompanyCode}执行{key?.Name} {key?.Description}", false, ActionRank.Danger);
			if (permissions.Check(key, operation, targetUserCompanyCode))
			{
				Status(a, true, $"成功-授权到{targetUserCompanyCode}执行{key?.Name} ");
				return true;
			}
			var u = usersService.Get(permissionUserName);
			if (u != null)
			{
				var uc = u.CompanyInfo;
				var ud = uc.Duties.IsMajorManager;
				var ucmp = uc.Company.Code;
				if ((targetUserCompanyCode == null || targetUserCompanyCode.StartsWith(ucmp)) && ud)
				{
					Status(a, true, $"成功-单位主官-授权到{targetUserCompanyCode}执行{key?.Name} ");
					return true;
				}
				else
				{
					var results = userServiceDetail.InMyManage(u).Result;
					if (targetUserCompanyCode == null && results.Item2 > 0) return true; // 如果无授权对象，则有任意单位权限即可
					else if (results.Item2 > 0 && results.Item1.Any(c => targetUserCompanyCode.StartsWith(c.Code)))
					{
						Status(a, true, $"成功-单位管理-授权到{targetUserCompanyCode}执行{key?.Name}");
						return true;
					}
				}
			}
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
			action.Description = $"{action.Description} {description}";
			_context.UserActions.Update(action);
			_context.SaveChanges();
			return action;
		}
	}
}