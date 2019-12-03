﻿using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Entities.UserInfo
{
	public class UserAction:BaseEntity
	{
		public string UserName { get; set; }
		/// <summary>
		/// 操作内容
		/// </summary>
		public UserOperation Operation { get; set; }
		/// <summary>
		/// 操作发生的时间
		/// </summary>
		public DateTime Date { get; set; }
		/// <summary>
		/// 操作时使用的ip
		/// </summary>
		public string Ip { get; set; }
		/// <summary>
		/// 通过接口类型判断用户所用的终端类型
		/// </summary>
		public string Device { get; set; }
		/// <summary>
		/// 用户浏览器UserAgent
		/// </summary>
		public string UA { get; set; }
		/// <summary>
		/// 操作是否成功
		/// </summary>
		public bool Success { get; set; }
		/// <summary>
		/// 操作描述
		/// </summary>
		public string Description { get; set; }
	}
	public enum UserOperation
	{
		Default=0,
		Register=1,
		Remove=2,
		Login=4,
		Logout=5,
		ModefyPsw=8,
		CreateApply=16,
		RemoveApply = 17,
		AuditApply=18,
		Permission=32
	}
}