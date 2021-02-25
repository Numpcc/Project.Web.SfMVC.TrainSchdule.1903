﻿using Abp.Extensions;
using BLL.Extensions.Common;
using BLL.Helpers;
using BLL.Interfaces.Common;
using DAL.Data;
using DAL.Entities.ClientDevice;
using DAL.Entities.Common.DataDictionary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainSchdule.ViewModels.ClientDevice;
using TrainSchdule.ViewModels.System;

namespace TrainSchdule.Controllers.ClientDevices
{

    /// <summary>
    /// 终端日志记录
    /// </summary>
    [Route("[controller]/[action]")]
    [ApiController]
    public partial class ClientRecordsController:Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IDataDictionariesServices dataDictionariesServices;

        /// <summary>
        /// 
        /// </summary>
        public ClientRecordsController(ApplicationDbContext context, IDataDictionariesServices dataDictionariesServices)
        {
            this.context = context;
            this.dataDictionariesServices = dataDictionariesServices;
        }
    }

    public partial class ClientRecordsController 
    {
        /// <summary>
        /// 处置记录编辑
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        public IActionResult Info([FromBody] VirusHandleRecordDataModel model)
        {
            var is_edit = model.Id != Guid.Empty;
            var client = is_edit?(context.VirusHandleRecords.FirstOrDefault(i => i.Id == model.Id)) : new VirusHandleRecord();
            model.ToModel(context.Viruses, client);
            if (client.IsRemoved && is_edit)
                client.Remove();
            else
            {
                if (client.HandleStatus.IsSuccess())
                    client.Virus.Status &= VirusStatus.Success;
                else
                {
                    switch (client.HandleStatus)
                    {
                        case VirusHandleStatus.ClientDeviceVirusMessage:
                            {
                                client.Virus.Status &= VirusStatus.MessageSend;
                                break;
                            }
                        case VirusHandleStatus.ClientDeviceVirusNotify:
                            {
                                client.Virus.Status &= VirusStatus.ClientNotify;
                                break;
                            }
                    }
                }
                if (!is_edit) context.VirusHandleRecords.Add(client);
                else context.VirusHandleRecords.Update(client);
            }
            context.SaveChanges();
            return new JsonResult(ActionStatusMessage.Success);
        }


        /// <summary>
        /// 查询处置记录 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Info([FromBody] VirusHandleRecordQueryDataModel model)
        {
            var list = context.VirusHandleRecordsDb.AsQueryable();
            var createStart = model.Create?.Start;
            var createEnd = model.Create?.End;
            if (createStart != null && createEnd != null && createStart > DateTime.MinValue && createEnd > DateTime.MinValue)
                list = list.Where(i => i.Create >= createStart && i.Create <= createEnd);
            var virus = model.Virus?.Value;
            if (virus != null) list = list.Where(i => i.VirusKey == virus);
            var client = model.Client?.Value;
            if (client != null) list = list.Where(i => i.ClientMachineId == client);
            var status = model.HandleStatus?.Arrays;
            if (status != null) list = list.Where(i => status.Contains((int)i.HandleStatus));
            var remark = model.Remark?.Value;
            if (remark != null) list = list.Where(i => i.Remark.Contains(remark));
            var result = list.OrderByDescending(i => i.Create).SplitPage(model.Pages);
            return new JsonResult(new EntitiesListViewModel<VirusHandleRecordDataModel>(result.Item1.Select(i => i.ToModel()), result.Item2));
        }
        /// <summary>
        /// 编辑备注
        /// </summary>
        /// <param name="model">id,remark</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Remark([FromBody] VirusHandleRecordDataModel model) {
            var record = context.VirusHandleRecordsDb.FirstOrDefault(i => i.Id == model.Id);
            if (record == null) return new JsonResult(record.NotExist());
            record.Remark = model.Remark;
            context.VirusHandleRecords.Update(record);
            context.SaveChanges();
            return new JsonResult(ActionStatusMessage.Success);
        }
    }

   public partial class ClientRecordsController
    {
        /// <summary>
        /// 获取状态列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult RecordStatusDict() {
            var dict = dataDictionariesServices.GetByGroupName(ApplicationDbContext.clientVirusHandleStatus);
            return new JsonResult(new EntityViewModel<Dictionary<string, CommonDataDictionary>>(new Dictionary<string, CommonDataDictionary>(dict.Select(s => new KeyValuePair<string, CommonDataDictionary>(s.Value.ToString(), s)))));
        }
    }
}