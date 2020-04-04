﻿using DAL.DTO.Recall;
using DAL.Entities;
using DAL.Entities.ApplyInfo;
using System;

namespace BLL.Extensions.ApplyExtensions
{
	public static class RecalOrderlExtensions
	{
		public static RecallOrderVDto ToVDto(this RecallOrder model, Apply apply)
		{
			if (model == null) return null;
			return new RecallOrderVDto()
			{
				Apply = apply == null ? Guid.Empty : apply.Id,
				ReturnStamp = model.ReturnStramp,
				Create = model.Create,
				Reason = model.Reason,
				RecallBy = model.RecallBy.ToSummaryDto()
			};
		}
	}
}