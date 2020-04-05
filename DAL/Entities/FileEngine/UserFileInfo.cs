﻿using System;

namespace DAL.Entities.FileEngine
{
	public class UserFileInfo : BaseEntity
	{
		/// <summary>
		/// 文件名
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// 文件大小
		/// </summary>
		public long Length { get; set; }

		public DateTime Create { get; set; }

		/// <summary>
		/// 路径
		/// </summary>
		public string Path { get; set; }

		/// <summary>
		/// 文件来源ip
		/// </summary>
		public string FromClient { get; set; }

		/// <summary>
		/// 当文件需要修改或者删除时需要验证身份
		/// </summary>
		public Guid ClientKey { get; set; }
	}
}