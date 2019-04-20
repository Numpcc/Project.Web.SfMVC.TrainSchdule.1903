﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TrainSchdule.DAL.Entities
{
	public class Company:BaseEntity
	{
		public string Name { get; set; }

		/// <summary>
		/// 由<see cref="Path"/>决定的父单位
		/// </summary>
		public virtual Company Parent { get; set; }
		public bool IsPrivate { get; set; }
		/// <summary>
		/// 单位所在路径
		/// </summary>
		public string Path{get; set;}
	}
		
}