﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Entities;
using TrainSchdule.Web.ViewModels;

namespace TrainSchdule.ViewModels.Apply
{
	public class ApplyAuditStatusViewModel:APIDataModel
	{
		public ApplyAuditStatusDataModel Data { get; set; }
	}

	public class ApplyAuditStatusDataModel
	{
		public  Dictionary<int, AuditStatusMessage> List { get; set; }
	}
}
