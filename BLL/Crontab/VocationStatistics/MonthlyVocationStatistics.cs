﻿using DAL.Data;
using System;

namespace TrainSchdule.Crontab
{
	/// <summary>
	/// 每月休假情况统计
	/// </summary>
	public class MonthlyVacationStatstics : BaseOnTimeVacationStatistics
	{
		private static DateTime start = DateTime.Today.AddMonths(-1);
		private static DateTime end = DateTime.Today;

		public static DateTime Start { get => start; set => start = value; }
		public static DateTime End { get => end; set => end = value; }

		public MonthlyVacationStatstics(ApplicationDbContext context) : base(context, Start, End, $"{Start.Year}_Month{Start.Month}")
		{
		}
	}
}