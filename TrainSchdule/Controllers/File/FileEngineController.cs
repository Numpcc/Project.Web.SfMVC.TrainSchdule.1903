﻿using BLL.Helpers;
using BLL.Interfaces.File;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using TrainSchdule.ViewModels.File;
using TrainSchdule.ViewModels.System;

namespace TrainSchdule.Controllers.File
{
	[Route("[controller]/[action]")]
	public class FileController : Controller
	{
		private readonly IFileServices fileServices;

		public FileController(IFileServices fileServices)
		{
			this.fileServices = fileServices;
		}

		/// <summary>
		/// 下载指定文件
		/// </summary>
		/// <param name="fileid"></param>
		/// <returns></returns>
		[HttpGet]
		public async Task<IActionResult> Download(string fileid)
		{
			var file = Guid.Parse(fileid);
			var f = fileServices.Download(file);
			if (f == null) return new JsonResult(ActionStatusMessage.Static.FileNotExist);
			return File(f.Data, "text/plain");
		}

		/// <summary>
		/// 获取指定文件的信息
		/// </summary>
		/// <param name="filepath"></param>
		/// <param name="filename"></param>
		/// <returns></returns>
		[HttpGet]
		public async Task<IActionResult> Load(string filepath, string filename)
		{
			var file = fileServices.Load(filepath, filename);
			if (file == null) return new JsonResult(ActionStatusMessage.Static.FileNotExist);
			return new JsonResult(new FileInfoViewModel()
			{
				Data = new FileInfoDataModel()
				{
					File = file
				}
			});
		}

		/// <summary>
		/// 上传文件
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		[DisableRequestSizeLimit]
		public async Task<IActionResult> Upload([FromForm]FileUploadViewModel model)
		{
			try
			{
				await fileServices.Upload(model.File, model.FilePath, model.FileName, model.ResumeUploadId == null ? Guid.Empty : Guid.Parse(model.ResumeUploadId));
			}
			catch (Exception ex)
			{
				return new JsonResult(new ResponseStatusOrModelExceptionViweModel(new ApiResult(-10303, ex.Message))
				{
				});
			}
			return new JsonResult(ActionStatusMessage.Success);
		}

		/// <summary>
		/// 获取当前文件传输状态
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public IActionResult Status()
		{
			var r = fileServices.Stauts();
			return new JsonResult(new FileTransferStatusViewModel()
			{
				Data = r
			});
		}
	}
}