﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.DTO.User;
using TrainSchdule.ViewModels;
using TrainSchdule.ViewModels.User;

namespace TrainSchdule.ViewModels.Company
{
	public class CompanyManagerViewModel:APIDataModel
	{
		public CompanyManagerDataModel Data { get; set; }
	}

	public class CompanyManagerDataModel
	{
		public IEnumerable<UserSummaryDTO> List { get; set; }
	}
}
