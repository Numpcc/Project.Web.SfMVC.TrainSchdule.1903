﻿using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.DTO.Apply.ApplyAuditStreamDTO
{
	public class ApplyAuditStreamNodeActionDto : MembersFilterDto
	{
		/// <summary>
		/// 审批节点的名称
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// 说明
		/// </summary>
		public string Description { get; set; }

		public DateTime Create { get; set; }
	}
}