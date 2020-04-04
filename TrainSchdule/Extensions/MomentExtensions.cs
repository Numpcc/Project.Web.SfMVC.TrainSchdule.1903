﻿using DAL.Entities;
using DAL.Entities.UserInfo.Settle;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace TrainSchdule.Extensions
{
	/// <summary>
	///
	/// </summary>
	public static class MomentExtensions
	{
		/// <summary>
		///
		/// </summary>
		/// <param name="model"></param>
		/// <param name="db"></param>
		/// <returns></returns>
		public static Moment ToMoment(this Moment model, DbSet<AdminDivision> db)
		{
			var tmp = new Moment()
			{
				Address = db.Where<AdminDivision>(a => a.Code == model.Address.Code).FirstOrDefault(),
				AddressDetail = model.AddressDetail,
				Date = model.Date,
				Valid = model.Valid
			};
			if (tmp.Address == null || tmp.Date.Year < 1900 || tmp.AddressDetail == "") return null;
			return tmp;
		}
	}
}