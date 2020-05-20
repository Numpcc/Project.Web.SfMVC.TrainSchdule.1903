﻿using DAL.DTO.User;
using DAL.Entities;
using System.Collections.Generic;
using System.Text;

namespace BLL.Extensions
{
	public static class VacationExtensions
	{
		public static string VacationDescription(this UserVacationInfoVDto info)
		{
			return $"全年假期{info?.YearlyLength}天,已休{info.NowTimes}次,{info.YearlyLength - info.LeftLength}天,剩余{info.LeftLength}天。其中可休路途{info.MaxTripTimes}次,已休{info.OnTripTimes}次";
		}

		public static string CombineVacationDescription(this IEnumerable<VacationDescription> model, bool CaculateAdditionalAndTripLength)
		{
			if (model == null) return "无福利假";
			if (!CaculateAdditionalAndTripLength) return "不计算路途和福利";
			var cs = new StringBuilder();
			foreach (var i in model)
			{
				cs.Append(i.Name).Append(i.Length).AppendLine("天");
			}
			return cs.ToString();
		}
	}
}