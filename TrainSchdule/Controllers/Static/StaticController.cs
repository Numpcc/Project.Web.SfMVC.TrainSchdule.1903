﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BLL.Extensions;
using BLL.Extensions.ApplyExtensions;
using BLL.Helpers;
using BLL.Interfaces;
using DAL.Data;
using DAL.DTO.Apply;
using DAL.Entities;
using DAL.Entities.ApplyInfo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using TrainSchdule.Extensions;
using TrainSchdule.ViewModels.Static;
using TrainSchdule.ViewModels.System;
using TrainSchdule.ViewModels.Verify;
using CollectionExtensions = Castle.Core.Internal.CollectionExtensions;

namespace TrainSchdule.Controllers
{
	/// <summary>
	/// 
	/// </summary>
	[Route("[controller]")]
	public partial class StaticController : Controller
	{

		private readonly IVerifyService _verifyService;
		private readonly IVocationCheckServices _vocationCheckServices;
		private readonly ApplicationDbContext _context;
		private readonly IApplyService _applyService;
		private readonly IHostingEnvironment _hostingEnvironment;
		private readonly ICurrentUserService _currentUserService;
		private readonly IUsersService _usersService;
		private readonly ICompaniesService _companiesService;

		public StaticController(IVerifyService verifyService, IVocationCheckServices vocationCheckServices, ApplicationDbContext context, IApplyService applyService, IHostingEnvironment hostingEnvironment, ICurrentUserService currentUserService, IUsersService usersService, ICompaniesService companiesService)
		{
			_verifyService = verifyService;
			_vocationCheckServices = vocationCheckServices;
			_context = context;
			_applyService = applyService;
			_hostingEnvironment = hostingEnvironment;
			_currentUserService = currentUserService;
			_usersService = usersService;
			_companiesService = companiesService;
		}

		/// <summary>
		/// 获取法定节假日情况
		/// </summary>
		/// <param name="start">开始日期</param>
		/// <param name="length">长度</param>
		/// <param name="caculateLawVocation">是否计算法定节假日</param>
		/// <returns></returns>
		[HttpGet]
		[AllowAnonymous]
		[ProducesResponseType(typeof(VocationDescriptionDataModel), 0)]
		[Route("VocationDate")]
		public IActionResult VocationDate(DateTime start, int length,bool caculateLawVocation)
		{
			var list = _vocationCheckServices.GetVocationDescriptions(start, length, caculateLawVocation);
			return new JsonResult(new VocationDescriptionViewModel()
			{
				Data = new VocationDescriptionDataModel()
				{
					Descriptions = list,
					EndDate = _vocationCheckServices.EndDate,
					StartDate = start,
					VocationDays = _vocationCheckServices.EndDate.Subtract(start).Days
				}
			});
		}
		/// <summary>
		/// 上传休假导出模板
		/// </summary>
		/// <param name="file"></param>
		/// <returns></returns>
		[HttpPost]
		[ProducesResponseType(typeof(string), 0)]
		[Route("XlsExport")]
		public IActionResult XlsExport(IEnumerable<IFormFile> file)
		{
			return new JsonResult(new { file });
		}
	}
}