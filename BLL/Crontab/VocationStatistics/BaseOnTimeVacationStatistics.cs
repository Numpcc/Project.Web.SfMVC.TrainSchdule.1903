﻿using BLL.Extensions;
using BLL.Helpers;
using DAL.Data;
using DAL.Entities;
using DAL.Entities.ApplyInfo;
using DAL.Entities.Vacations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrainSchdule.Crontab
{
	/// <summary>
	/// 对一段时间的休假情况进行统计并存入到数据库
	/// </summary>
	public class BaseOnTimeVacationStatistics : ICrontabJob
	{
		private readonly ApplicationDbContext _context;

		public BaseOnTimeVacationStatistics(ApplicationDbContext context, DateTime start, DateTime end, string statisticsId = null)
		{
			_context = context;
			Start = start;
			End = end;
			StatisticsId = statisticsId ?? $"{Start.ToString("yyyyMMdd")}_{End.ToString("yyyyMMdd")}";
		}

		/// <summary>
		/// 生成统计ID
		/// </summary>
		public string StatisticsId { get; }

		private DateTime Start { get; set; }
		private DateTime End { get; set; }

		/// <summary>
		/// 统计的原因
		/// </summary>
		public string Description { get; set; } = "系统定时生成的统计";

		/// <summary>
		/// 统计的单位
		/// </summary>
		public string CompanyCode { get; set; } = "A";

		public async Task RunAsync()
		{
			await Task.Run(Run).ConfigureAwait(true);
		}

		public void Run()
		{
			var rootCompany = _context.Companies.Find(CompanyCode);
			if (rootCompany == null) throw new ActionStatusMessageException(ActionStatusMessage.CompanyMessage.NotExist);
			var statistics = new VacationStatistics()
			{
				Start = Start,
				End = End,
				CurrentYear = Start.Year,
				Id = StatisticsId,
				Description = Description,
				RootCompanyStatistics = GenerateStatistics(rootCompany)
			};
			var dbStatistics = _context.VacationStatistics.Find(statistics.Id);
			if (dbStatistics != null) return;
			VacationStatisticsDescription tmp = statistics.RootCompanyStatistics;
			VacationStatisticsExtensions.StatisticsInit(ref tmp, _context, statistics.CurrentYear, StatisticsId);
			statistics.RootCompanyStatistics = tmp;
			_context.VacationStatistics.Add(statistics);
			_context.SaveChanges();
		}

		private VacationStatisticsDescription GenerateStatistics(Company company)
		{
			if (company == null) return null;
			var statistics = new VacationStatisticsDescription();
			statistics.Applies = _context.AppliesDb.
				Where<Apply>(a => a.BaseInfo.From.CompanyInfo.Company.Code == company.Code && a.Create.Value >= Start && a.Create.Value <= End);
			statistics.Company = company;
			var companies = _context.Companies.Where<Company>(c => ParentCode(c.Code) == company.Code);
			var list = new List<VacationStatisticsDescription>();
			foreach (var childCompany in companies)
			{
				var s = GenerateStatistics(childCompany);
				if (s != null) list.Add(s);
			}
			statistics.Childs = list;
			return statistics;
		}

		private static string ParentCode(string code) => (code != null && code.Length > 1) ? code.Substring(0, code.Length - 1) : null;
	}
}