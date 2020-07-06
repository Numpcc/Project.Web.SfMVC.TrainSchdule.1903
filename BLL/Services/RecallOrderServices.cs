﻿using BLL.Extensions.Common;
using BLL.Helpers;
using BLL.Interfaces;
using DAL.Data;
using DAL.DTO.Recall;
using DAL.Entities;
using DAL.Entities.ApplyInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Services
{
	public class RecallOrderServices : IRecallOrderServices
	{
		private readonly ApplicationDbContext _context;

		public RecallOrderServices(ApplicationDbContext context)
		{
			_context = context;
		}

		public RecallOrder Create(RecallOrderVDto recallOrder)
		{
			var order = new RecallOrder()
			{
				Create = DateTime.Now,
				ReturnStramp = recallOrder.ReturnStamp,
				Reason = recallOrder.Reason,
				HandleBy = _context.AppUsers.Find(recallOrder.HandleBy.Id)
			};
			var apply = _context.AppliesDb.Where(a => a.Id == recallOrder.Apply).FirstOrDefault();
			if (apply == null) throw new ActionStatusMessageException(apply.NotExist());
			if (apply.RecallId != null) throw new ActionStatusMessageException(ActionStatusMessage.ApplyMessage.Recall.Crash);
			if (order.HandleBy == null) throw new ActionStatusMessageException(order.HandleBy.NotExist());
			if (!(apply.ApplyAllAuditStep.LastOrDefault()?.MembersAcceptToAudit.Split("##").Contains(order.HandleBy.Id) ?? false)) throw new ActionStatusMessageException(ActionStatusMessage.ApplyMessage.Recall.RecallByNotSame);
			if (apply.RequestInfo.StampReturn <= order.ReturnStramp) throw new ActionStatusMessageException(ActionStatusMessage.ApplyMessage.Recall.RecallTimeLateThanVacation);
			if (order.ReturnStramp < apply.RequestInfo.StampLeave) throw new ActionStatusMessageException(ActionStatusMessage.ApplyMessage.Recall.RecallTimeEarlyThanVacationLeaveStamp);
			_context.RecallOrders.Add(order);
			apply.RecallId = order.Id;
			apply.ExecuteStatus |= ExecuteStatus.BeenSet;
			apply.ExecuteStatus |= ExecuteStatus.Recall;
			_context.Applies.Update(apply);
			_context.SaveChanges();
			return order;
		}

		public ApplyExecuteStatus Create(Apply apply, ExecuteStatusVDto status)
		{
			if (apply == null || status == null) throw new ActionStatusMessageException(apply.NotExist());
			var s = (int)apply.ExecuteStatus & (int)ExecuteStatus.BeenSet;
			if (s > 0) throw new ActionStatusMessageException(ActionStatusMessage.ApplyMessage.Recall.Crash);
			apply.ExecuteStatus |= ExecuteStatus.BeenSet;
			var m = new ApplyExecuteStatus()
			{
				Create = DateTime.Now,
				HandleBy = _context.AppUsers.Find(status.HandleBy.Id),
				ReturnStramp = status.ReturnStamp,
				Reason = status.Reason
			};
			var rawReturn = apply.RequestInfo.StampReturn?.Date;
			if (m.ReturnStramp.Date < rawReturn) throw new ActionStatusMessageException(ActionStatusMessage.ApplyMessage.Recall.SelfReturnNotPermit);
			else if (m.ReturnStramp.Date > rawReturn)
				apply.ExecuteStatus |= ExecuteStatus.Delay;
			_context.ApplyExcuteStatus.Add(m);
			apply.ExecuteStatusDetailId = m.Id;
			_context.Applies.Update(apply);
			_context.SaveChanges();
			return m;
		}
	}
}