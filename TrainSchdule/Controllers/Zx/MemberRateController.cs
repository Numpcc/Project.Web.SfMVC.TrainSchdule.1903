﻿using Abp.Extensions;
using BLL.Helpers;
using BLL.Interfaces;
using BLL.Interfaces.File;
using DAL.Data;
using DAL.DTO.ZX.MemberRate;
using DAL.Entities;
using DAL.Entities.ZX.MemberRate;
using Magicodes.ExporterAndImporter.Core.Models;
using Magicodes.ExporterAndImporter.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TrainSchdule.Extensions;
using TrainSchdule.ViewModels;
using TrainSchdule.ViewModels.System;
using TrainSchdule.ViewModels.ZX;

namespace TrainSchdule.Controllers.Zx
{
    /// <summary>
    /// 成员评分
    /// </summary>
    [Route("[controller]/[action]")]
    [Authorize]
    public partial class MemberRateController:Controller
    {
        private readonly ApplicationDbContext context;
        private readonly ICurrentUserService currentUserService;
        private readonly IFileServices fileServices;
        private readonly IUserActionServices userActionServices;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="currentUserService"></param>
        /// <param name="fileServices"></param>
        /// <param name="userActionServices"></param>
        public MemberRateController(ApplicationDbContext context,ICurrentUserService currentUserService,IFileServices fileServices,IUserActionServices userActionServices)
        {
            this.context = context;
            this.currentUserService = currentUserService;
            this.fileServices = fileServices;
            this.userActionServices = userActionServices;
        }
    }
    public partial class MemberRateController
    {
        private const string cacheMemberRate = "cache.memberRate.upload";
        private const string cacheMemberRateXlsModel = "cache.memberRate.upload.model";
        /// <summary>
        /// 导入数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> XlsUpload(MemberRateXlsDto model)
        {
            if (!ModelState.IsValid) return new JsonResult(new ModelStateExceptionViewModel(ModelState)) ;
            var importer = new ExcelImporter();
            ImportResult<MemberRateImportDto> data;
            HttpContext.Session.Remove(cacheMemberRate);
            HttpContext.Session.Remove(cacheMemberRateXlsModel);
            using (var inputStream = model.File.OpenReadStream())
            {
                data = await importer.ImportWithErrorCheck<MemberRateImportDto>(inputStream);
            }
            return await CheckData(data.Data.ToList(), model) ?? new JsonResult(ActionStatusMessage.Success);
        }
        /// <summary>
        /// 检查数据并保存
        /// </summary>
        /// <param name="data"></param>
        /// <param name="model"></param>
        private async Task<IActionResult> CheckData(List<MemberRateImportDto>data, MemberRateXlsDto model)
        {
            var currentUser = currentUserService.CurrentUser;
            userActionServices.Permission(currentUser.Application.Permission, DictionaryAllPermission.Grade.MemberRate, Operation.Update, currentUser.Id, model.Company, "批量授权录入");
            // convert to data
            var notExistUser = new List<string>();
            var list = data.Select(i => {
                var f = i.ToModel(context.CompaniesDb, context.AppUsersDb);
                if (f.User == null) notExistUser.Add(i.UserCid);
                return f;
            }).Select(i => {
                i.RatingCycleCount = model.RatingCycleCount;
                i.RatingType = model.RatingType;

                if(!i.CompanyCode.IsNullOrEmpty() && i.CompanyCode != model.Company)
                {
                    userActionServices.Permission(currentUser.Application.Permission, DictionaryAllPermission.Grade.MemberRate, Operation.Update, currentUser.Id, i.CompanyCode, "单点授权录入");
                }
                return i;
            }).ToList();
            if (notExistUser.Any())
                return new JsonResult(new EntitiesListViewModel<string>(notExistUser));

            // check if exist
            var currentListRaw = context.NormalRates
                  .Where(i => i.RatingType == model.RatingType)
                  .Where(i => i.RatingCycleCount == model.RatingCycleCount);
            var currentList = currentListRaw
                  .Select(i => i.ToDataModel())
                  .ToList();
            if (currentList.Any())
            {
                if (model.Confirm)
                {
                    context.NormalRates.RemoveRange(currentListRaw);
                }
                else
                {
                    HttpContext.Session.Set(cacheMemberRate, Encoding.UTF8.GetBytes(JsonSerializer.Serialize(data)));
                    model.File = null;
                    HttpContext.Session.Set(cacheMemberRateXlsModel, Encoding.UTF8.GetBytes(JsonSerializer.Serialize(model)));
                    return new JsonResult(new EntitiesListViewModel<MemberRateDataModel>(currentList));
                }
               
            }
            // save
            await context.NormalRates.AddRangeAsync(list);
            await context.SaveChangesAsync();
            return null;
        }
        /// <summary>
        /// 确认上次的上传
        /// </summary>
        /// <returns></returns>
        [HttpPatch]
        public async Task<IActionResult> ConfirmLastXlsUpload()
        {
            if (!HttpContext.Session.TryGetValue(cacheMemberRate, out var data)) return new JsonResult(ActionStatusMessage.StaticMessage.ResourceNotExist);
            var _ = HttpContext.Session.TryGetValue(cacheMemberRateXlsModel, out var model);
            var datas = JsonSerializer.Deserialize<List<MemberRateImportDto>>(Encoding.UTF8.GetString(data));
            var models = JsonSerializer.Deserialize<MemberRateXlsDto>(Encoding.UTF8.GetString(model));
            models.Confirm = true;
            var result = await CheckData(datas, models) ?? new JsonResult(ActionStatusMessage.Success);
            return result;
        }
        private const string TemplateName = "评分导入模板.xlsx";
        /// <summary>
        /// 获取模板
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> XlsTemplate()
        {
            var importer = new ExcelImporter();
            var content = await importer.GenerateTemplateBytes<MemberRateImportDto>();
			new FileExtensionContentTypeProvider().TryGetContentType(TemplateName, out var contentType);
            return File(content, contentType ?? "text/plain", TemplateName);
        }
    }
}
