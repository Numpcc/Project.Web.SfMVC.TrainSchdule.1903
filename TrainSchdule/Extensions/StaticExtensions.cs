﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Entities;
using TrainSchdule.ViewModels.Static;

namespace TrainSchdule.Extensions
{
	public static class StaticExtensions
	{
		public static LocationChildNodeDataModel ToDataModel(this AdminDivision model)
		{
			var b=new LocationChildNodeDataModel()
			{
				Code = model.Code,
				Name = model.ShortName
			};
			return b;
		}
	}
}