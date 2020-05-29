﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities.UserInfo.Settle
{
	/// <summary>
	/// 居住情况
	/// </summary>
	public class Settle : BaseEntity
	{
		private static Moment CreateNewMoment() => new Moment()
		{
			Valid = false,
			Date = DateTime.Now,
			AddressDetail = "未填写",
			Address = new AdminDivision()
			{
				Code = 0,
				Name = "未填写"
			}
		};

		private Moment self;
		private Moment lover;
		private Moment parent;
		private Moment loversParent;

		/// <summary>
		/// 本人所在地
		/// </summary>
		[AddressCodeOnProvince(false, ErrorMessage = "本人")]
		public virtual Moment Self { get => self; set => self = value; }

		/// <summary>
		/// 配偶所在地
		/// </summary>
		[AddressCodeOnProvince(false, ErrorMessage = "配偶")]
		public virtual Moment Lover { get => lover; set => lover = value; }

		/// <summary>
		/// 父母所在地
		/// </summary>
		[AddressCodeOnProvince(false, ErrorMessage = "父母")]
		public virtual Moment Parent { get => parent; set => parent = value; }

		/// <summary>
		/// 配偶的父母所在地
		/// </summary>
		[AddressCodeOnProvince(false, ErrorMessage = "配偶父母")]
		public virtual Moment LoversParent { get => loversParent; set => loversParent = value; }

		/// <summary>
		/// 全年发生变化的记录
		/// </summary>
		public virtual IEnumerable<AppUsersSettleModefyRecord> PrevYealyLengthHistory { get; set; }

		/// <summary>
		/// 年初因上一年度休事假消耗的天数
		/// </summary>
		public int PrevYearlyComsumeLength { get; set; }
	}

	public class AppUsersSettleModefyRecord : BaseEntity
	{
		/// <summary>
		/// 不使用id
		/// </summary>
		[NotMapped]
		public override Guid Id { get; set; }

		[Key]
		public int Code { get; set; }

		/// <summary>
		/// 长度
		/// </summary>
		public double Length { get; set; }

		/// <summary>
		/// 生效时间
		/// </summary>
		public DateTime UpdateDate { get; set; }

		/// <summary>
		/// 本次变动后产生的描述内容
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// 是否是新年初始化的数据
		/// </summary>
		public bool IsNewYearInitData { get; set; }
	}
}