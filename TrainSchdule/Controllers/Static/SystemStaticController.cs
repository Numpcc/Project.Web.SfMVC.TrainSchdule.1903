﻿using BLL.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainSchdule.Extensions;
using TrainSchdule.ViewModels.Static;
using TrainSchdule.ViewModels.System;
using TrainSchdule.ViewModels.Verify;

namespace TrainSchdule.Controllers
{
	/// <summary>
	/// 系统内部的静态
	/// </summary>
	public partial class StaticController
	{
		/// <summary>
		/// 产生一个二维码
		/// </summary>
		/// <returns></returns>
		[HttpPost]
		[AllowAnonymous]
		public IActionResult QrCodeGenerate([FromBody] QrCodeDataModel model)
		{
			if (model == null) return new JsonResult(ActionStatusMessage.Static.QrCode.NoData);
			var qrEncoder = new SfQRCoder();
			var rawText = model.Data;
			if (rawText == null)
			{
				if (model.Data == null) return new JsonResult(ActionStatusMessage.Static.QrCode.NoData);
				rawText = qrEncoder.DecodeQrCode(model.Img);
			}
			if (model.Config != null) qrEncoder.Config = model.Config;
			var img = qrEncoder.GenerateBytes(rawText);
			return new JsonResult(new QrCodeViewModel()
			{
				Data = new QrCodeDataModel()
				{
					Data = rawText,
					Img = img,
					Config = qrEncoder.Config
				}
			});
		}

		/// <summary>
		/// 识别二维码
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		[AllowAnonymous]
		public IActionResult QrCodeScan([FromBody] QrCodeDataModel model)
		{
			if (model == null) return new JsonResult(ActionStatusMessage.Static.QrCode.NoData);

			var qrEncoder = new SfQRCoder();
			return new JsonResult(new QrCodeViewModel()
			{
				Data = new QrCodeDataModel()
				{
					Data = qrEncoder.DecodeQrCode(model.Img),
					Img = model.Img,
					Config = qrEncoder.Config
				}
			});
		}

		/// <summary>
		///
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		[AllowAnonymous]
		public IActionResult VerifyCode()
		{
			var imgId = _verifyService.Generate().ToString();
			if (_verifyService.Status != null)
			{
				var status = _verifyService.Status;
				_verifyService.Generate();
				return new JsonResult(new ApiResult(ActionStatusMessage.Account.Auth.Verify.Invalid.Status, status));
			}

			return new JsonResult(new ScrollerVerifyGeneratedViewModel()
			{
				Data = new ScrollerVerifyGeneratedDataModel()
				{
					Id = imgId,
					PosY = _verifyService.Pos.Y
				}
			});
		}

		/// <summary>
		///
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		[AllowAnonymous]
		public IActionResult VerifyCodeFront()
		{
			var img = _verifyService.Front();
			if (img == null) return new JsonResult(new ApiResult()
			{
				Status = -1,
				Message = _verifyService.Status
			});
			HttpContext.Response.Cookies.Append("posY", _verifyService.Pos.Y.ToString());
			return new FileContentResult(img, "image/jpg");
		}

		/// <summary>
		///
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		[AllowAnonymous]
		public IActionResult VerifyCodeBackground()
		{
			var img = _verifyService.Background();
			if (img == null) return new JsonResult(new ApiResult()
			{
				Status = -1,
				Message = _verifyService.Status
			});
			HttpContext.Response.Cookies.Append("posY", _verifyService.Pos.Y.ToString());
			return new FileContentResult(img, "image/jpg");
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="code"></param>
		/// <returns></returns>
		[HttpGet]
		[AllowAnonymous]
		[ProducesResponseType(typeof(LocationDataModel), 0)]
		public IActionResult Location(int code)
		{
			var location = _context.AdminDivisions.Find(code);
			if (location == null) return new JsonResult(ActionStatusMessage.Static.AdminDivision.NoSuchArea);
			return new JsonResult(new LocationViewModel()
			{
				Data = new LocationDataModel()
				{
					Code = location.Code,
					ParentCode = location.ParentCode,
					Name = location.Name,
					ShortName = location.ShortName
				}
			});
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="code"></param>
		/// <returns></returns>
		[HttpGet]
		[AllowAnonymous]
		[ProducesResponseType(typeof(LocationChildrenDataModel), 0)]
		public IActionResult LocationChildren(int code)
		{
			var location = _context.AdminDivisions.Find(code);
			if (location == null) return new JsonResult(ActionStatusMessage.Static.AdminDivision.NoChildArea);
			var list = _context.AdminDivisions.Where(a => a.ParentCode == code);
			var totalCount = list.Count();
			return new JsonResult(new LocationChildrenViewModel()
			{
				Data = new LocationChildrenDataModel()
				{
					List = list.Select(t => t.ToDataModel()),
					TotalCount = totalCount
				}
			});
		}
	}
}