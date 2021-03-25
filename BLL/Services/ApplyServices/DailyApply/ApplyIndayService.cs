﻿using Abp.Linq.Expressions;
using BLL.Extensions.ApplyExtensions.ApplyAuditStreamExtension;
using BLL.Helpers;
using BLL.Interfaces;
using BLL.Interfaces.ApplyInfo;
using BLL.Interfaces.Audit;
using DAL.Data;
using DAL.DTO.Apply;
using DAL.Entities.ApplyInfo;
using DAL.Entities.ApplyInfo.DailyApply;
using DAL.Entities.UserInfo;
using DAL.QueryModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.ApplyServices.DailyApply
{
    public partial class ApplyIndayService
    {
        public IEnumerable<ApplyInday> QueryApplies(QueryApplyDataModel model, bool getAllAppliesPermission, out int totalCount)
        {
            throw new NotImplementedException();
        }
    }
    public partial class ApplyIndayService : IApplyInDayService
    {
        private readonly ApplicationDbContext context;
        private readonly IUsersService usersService;
        private readonly IAuditStreamServices auditStreamServices;

        public ApplyIndayService(ApplicationDbContext context, IUsersService usersService, IAuditStreamServices auditStreamServices)
        {
            this.context = context;
            this.usersService = usersService;
            this.auditStreamServices = auditStreamServices;
        }
        public IQueryable<ApplyInday> CheckIfHaveSameRangeVacation(ApplyInday apply)
        {
            var r = apply.RequestInfo;
            var list = new List<AuditStatus>() {
                    AuditStatus.Accept,
                    AuditStatus.AcceptAndWaitAdmin,
                    AuditStatus.Auditing
            };
            var userid = apply.BaseInfo.FromId;
            var recallDb = context.RecallOrders;
            var execDb = context.ApplyExcuteStatus;

            var exp = PredicateBuilder.New<ApplyInday>(false);
            // 存在确认时间，则判断确认时间
            exp = exp.Or(a => a.RecallId == null && a.ExecuteStatusDetailId != null && !(execDb.FirstOrDefault(exec => exec.Id == a.ExecuteStatusDetailId).ReturnStamp <= r.StampLeave || a.RequestInfo.StampLeave >= r.StampReturn));
            // 不存在召回时间，则判断应归队时间（必定不晚于确认时间）
            exp = exp.Or(a => a.ExecuteStatusDetailId == null && a.RecallId == null && !(a.RequestInfo.StampLeave >= r.StampReturn || a.RequestInfo.StampReturn <= r.StampLeave));
            // 如果存在召回，则判断召回时间
            exp = exp.Or(a => a.RecallId != null && !(recallDb.FirstOrDefault(rec => rec.Id == a.RecallId).ReturnStamp <= r.StampLeave || a.RequestInfo.StampLeave >= r.StampReturn));
            /* 20200917@胡琪blanche881
			 * 两个日期范围存在冲突的条件：
			!(A2<=B1||B2<=A1)
			*
			*/
            var userVacationsInTime = context.AppliesIndayDb
                .Where(a => a.BaseInfo.FromId == userid)
                .Where(exp)
                .Where(a => list.Contains(a.Status));
            return userVacationsInTime;
        }

        public ApplyInday Create(ApplyInday item)
        {
            if (item == null) return null;
            var appSetting = item.BaseInfo.From.Application.ApplicationSetting;
            if (appSetting != null)
            {
                var time = appSetting.LastSubmitApplyTime;
                // 若1分钟内连续提交两次，则下次提交限定到10分钟后
                if (time == null) appSetting.LastSubmitApplyTime = DateTime.Now;
                else if (time > DateTime.Now.AddMinutes(10)) throw new ActionStatusMessageException(ActionStatusMessage.ApplyMessage.Operation.Submit.Crash);
                else if (time?.AddMinutes(1) > DateTime.Now)
                    appSetting.LastSubmitApplyTime = DateTime.Now.AddMinutes(20);
            }
            context.AppliesInday.Add(item);
            context.AppUserApplicationSettings.Update(appSetting);
            context.SaveChanges();
            return item;
        }

        public async Task Delete(ApplyInday item) => await RemoveApplies(new List<ApplyInday>() { item }).ConfigureAwait(true);


        public bool Edit(string id, Action<ApplyInday> editCallBack) => EditAsync(id, editCallBack).Result;
        public async Task<bool> EditAsync(string id, Action<ApplyInday> editCallBack)
        {
            if (editCallBack == null) return false;
            if (!Guid.TryParse(id, out var guid)) return false;
            var target = context.AppliesIndayDb.Where(a => a.Id == guid).FirstOrDefault();
            if (target == null) return false;
            await Task.Run(() => editCallBack.Invoke(target)).ConfigureAwait(true);
            context.AppliesInday.Update(target);
            await context.SaveChangesAsync().ConfigureAwait(true);
            return true;
        }

       

        public ApplyInday GetById(Guid id) => context.AppliesIndayDb.Where(a => a.Id == id).FirstOrDefault();


        public async Task RemoveApplies(IEnumerable<ApplyInday> list)
        {
            if (list == null) return;
            bool anyRemove = false;
            foreach (var s in list)
            {
                s.Remove();
                context.AppliesInday.Update(s);
                anyRemove = true;
            }
            if (anyRemove)
                await context.SaveChangesAsync().ConfigureAwait(true);
        }

        public ApplyInday Submit(ApplyVdto model)
        {
            if (model == null) throw new ActionStatusMessageException(ActionStatusMessage.ApplyMessage.Default);
            var apply = new ApplyInday()
            {
                BaseInfo = context.ApplyBaseInfos.Find(model.BaseInfoId),
                Create = DateTime.Now,
                RequestInfo = context.ApplyIndayRequests.Find(model.RequestInfoId),
                Status = AuditStatus.NotSave,
                MainStatus = model.IsPlan ? MainStatus.IsPlan : MainStatus.Normal
            };
            if (apply.RequestInfo == null) throw new ActionStatusMessageException(ActionStatusMessage.ApplyMessage.Operation.Submit.NoRequestInfo);
            if (apply.BaseInfo == null) throw new ActionStatusMessageException(ActionStatusMessage.ApplyMessage.Operation.Submit.NoBaseInfo);
            var company = apply.BaseInfo?.Company;
            if (company == null) throw new ActionStatusMessageException(ActionStatusMessage.CompanyMessage.NotExist);
            AuditStreamModel auditItem = apply.ToModel();
            auditStreamServices.InitAuditStream(ref auditItem, model.EntityType, apply.BaseInfo?.From);
            apply = auditItem.ToModel(apply);
            apply = Create(apply); // 创建成功，记录本次创建详情
            return apply;
        }

        public ApplyIndayRequest SubmitRequestAsync(User targetUser, ApplyIndayRequestVdto model)
        {

            if (model == null) return null;
            var r = new ApplyIndayRequest()
            {
                Reason = model.Reason,
                StampLeave = model.StampLeave,
                StampReturn = model.StampReturn,
                VacationPlace = model.VacationPlace,
                VacationPlaceName = model.VacationPlaceName,
                CreateTime = DateTime.Now,
            };
            context.ApplyIndayRequests.Add(r);
            context.SaveChanges();
            return r;
        }
    }
    public partial class ApplyIndayService
    {
        public byte[] ExportExcel(string templete, ApplyDetailDto<ApplyIndayRequest> model)
        {
            throw new NotImplementedException();
        }

        public byte[] ExportExcel(string templete, IEnumerable<ApplyDetailDto<ApplyIndayRequest>> model)
        {
            throw new NotImplementedException();
        }
    }
    public partial class ApplyIndayService
    {

        public Task<int> RemoveAllNoneFromUserApply(TimeSpan interval)
        {
            throw new NotImplementedException();
        }

        public Task<int> RemoveAllRemovedUsersApply()
        {
            throw new NotImplementedException();
        }

        public Task<int> RemoveAllUnSaveApply(TimeSpan interval)
        {
            throw new NotImplementedException();
        }

    }
}