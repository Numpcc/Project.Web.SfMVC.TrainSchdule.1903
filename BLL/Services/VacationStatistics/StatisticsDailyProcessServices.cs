using BLL.Extensions;
using BLL.Extensions.ApplyExtensions;
using BLL.Interfaces.IVacationStatistics;
using DAL.Data;
using DAL.Entities;
using DAL.Entities.ApplyInfo;
using DAL.Entities.UserInfo;
using DAL.Entities.Vacations.Statistics;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL.Services.VacationStatistics
{
	/// <summary>
	/// 每日累计统计项
	/// </summary>
	public class StatisticsDailyProcessServices : IStatisticsDailyProcessServices
	{
		private readonly ApplicationDbContext _context;

		public StatisticsDailyProcessServices(ApplicationDbContext context)
		{
			this._context = context;
		}

		public IEnumerable<StatisticsDailyProcessRate> CaculateCompleteApplies(string companyCode, DateTime vStart, DateTime vEnd)
			=> _context.StatisticsDailyProcessRates.AsQueryable()
			.CaculateIStatisticsBaseApplies(_context, GetTargetStatistics, ApplyInfoExtensions.GetCompletedApplies, ApplyInfoExtensions.FirstYearOfTheDay
		, item => _context.StatisticsDailyProcessRates.AddRange(item), companyCode, vStart, vEnd);

		public IEnumerable<StatisticsDailyProcessRate> DirectGetCompleteApplies(string companyCode, DateTime vStart, DateTime vEnd) =>
			_context.StatisticsDailyProcessRates.AsQueryable().CheckDb(companyCode, vStart, vEnd);

		public void RemoveCompleteApplies(string companyCode, DateTime vStart, DateTime vEnd)
		{
			var list = _context.StatisticsDailyProcessRates.Where(s => s.CompanyCode.StartsWith(companyCode)).Where(s => s.Target >= vStart).Where(s => s.Target <= vEnd);
			_context.StatisticsDailyProcessRates.RemoveRange(list);
		}

		private Tuple<IEnumerable<T>, bool> GetTargetStatistics<T>(string companyCode, DateTime target, IQueryable<T> db, IQueryable<Apply> applies, IQueryable<RecallOrder> recallDb) where T : StatisticsDailyProcessRate, new()
		{
			if (db.CheckDb(companyCode, target).Any()) return new Tuple<IEnumerable<T>, bool>(db.CheckDb(companyCode, target), false);
			var records = applies.Select(a => new StatisticsAppliesInfo()
			{
				Type = a.BaseInfo.Duties.Type,
				FromId = a.BaseInfo.FromId,
				Days = a.RequestInfo.VacationLength,
                // 此处未考虑召回导致的假期损失
                // 后续可以通过将RecallDb加以考虑
                // @serfend 20201111 添加召回数据
                // @serfend 20201207 bug fix use EF.Functions
                // @serfend 20201208 TODO 后续应将每个假期信息加以统计以减少动态统计
                //RecallReduceDay = recallDb
                //    .Where(r => r.Id == a.RecallId)
                //    .Sum(r => EF.Functions.DateDiffDay(a.RequestInfo.StampLeave, r.ReturnStamp) ?? 0)
            }).ToList();
			var groupRecords = records.GroupBy(a => new { a.Type });
			var companyLength = companyCode.Length;
			var companyAllMembers = _context.AppUsersDb
				.Where(u => u.Application.Create <= target)
				.Where(u => ((int)u.AccountStatus & (int)AccountStatus.DisableVacation) == 0)
				.Where(u => u.CompanyInfo.CompanyCode.Length >= companyLength
				&& u.CompanyInfo.CompanyCode.StartsWith(companyCode));
			var result = groupRecords.ToList().Select(r =>
			{
				var users = r.GroupBy(a => a.FromId);
				var companyAtTypeMembers = companyAllMembers.Where(u => u.CompanyInfo.Duties.Type == r.Key.Type);
				var companyAtTypeMembersList = companyAtTypeMembers.ToList();
				var memberCount = companyAtTypeMembersList.Count;
				// 每个人的 -> 全年假天，已休天，被召回天
				var userYealyStatisticsDict = new Dictionary<string, YearlyStatistics>();
				foreach (var u in companyAtTypeMembersList)
					userYealyStatisticsDict[u.Id] = new YearlyStatistics()
					{
						YearlyLength = u.SocialInfo.Settle.GetYearlyLengthInner(u, out var m, out var d, out var actionOnDate, out var requireUpdate)
					};
				foreach (var p in users)
				{
					// Sturct 是值类型，不可直接修改
					if (!userYealyStatisticsDict.ContainsKey(p.Key)) userYealyStatisticsDict[p.Key] = new YearlyStatistics();
					var d = userYealyStatisticsDict[p.Key];
					userYealyStatisticsDict[p.Key] = new YearlyStatistics()
					{
						CompleteLength = p.Sum(pInfo => pInfo.Days),
						RecallLength = p.Sum(pInfo => pInfo.RecallReduceDay),
						YearlyLength = d.YearlyLength
					};
				}
				return new T()
				{
					Target = target,
					Type = r.Key.Type,
					CompanyCode = companyCode,
					ApplyMembersCount = users.Count(),
					CompleteYearlyVacationCount = userYealyStatisticsDict.Count(a => a.Value.YearlyLength <= a.Value.CompleteLength - a.Value.RecallLength), // 总假期<=已休-召回
					MembersVacationDayLessThanP60 = userYealyStatisticsDict.Count(a => a.Value.YearlyLength * 0.6 > a.Value.CompleteLength - a.Value.RecallLength), // 总假期>已休-召回
					MembersCount = memberCount,
					CompleteVacationExpectDayCount = userYealyStatisticsDict.Sum(a => a.Value.YearlyLength),
					CompleteVacationRealDayCount = userYealyStatisticsDict.Sum(a => a.Value.CompleteLength - a.Value.RecallLength),
				};
			});
			return new Tuple<IEnumerable<T>, bool>(result, true);
		}

		private struct YearlyStatistics
		{
			/// <summary>
			/// 全年总可用天数
			/// </summary>
			public int YearlyLength { get; set; }

			/// <summary>
			/// 全年完成的天寿
			/// </summary>
			public int CompleteLength { get; set; }

			/// <summary>
			/// 被召回的天数
			/// </summary>
			public int RecallLength { get; set; }
		}
	}
}