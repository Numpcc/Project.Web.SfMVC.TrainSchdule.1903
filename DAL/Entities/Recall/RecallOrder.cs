﻿using DAL.Entities.ApplyInfo;
using DAL.Entities.UserInfo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DAL.Entities
{
	public class RecallOrder:BaseEntity
	{
		/// <summary>
		/// 召回原因
		/// </summary>
		public string Reason { get; set; }
		public virtual User RecallBy { get; set; }
		/// <summary>
		/// 创建时间
		/// </summary>
		public DateTime Create { get; set; }
		/// <summary>
		/// 归队时间
		/// </summary>
		public DateTime ReturnStramp { get; set; }
	}
}
