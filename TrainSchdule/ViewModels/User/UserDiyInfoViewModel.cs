﻿using DAL.Entities.UserInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainSchdule.ViewModels.System;
using TrainSchdule.ViewModels.Verify;

namespace TrainSchdule.ViewModels.User
{
	public class UserDiyInfoViewModel: ApiDataModel
	{
		public UserDiyInfoDataModel Data { get; set; }
	}
public class UserDiyInfoModefyModel:GoogleAuthViewModel
	{
		public UserDiyInfoDataModel Data { get; set; }
	}	
 public class UserDiyInfoDataModel
	{
		public string About { get; set; }
		public string Avatar { get; set; }
	}

	public static class UserDiyInfoExtension
	{
		/// <summary>
		/// 用户头像默认目录
		/// </summary>
		public static readonly string avatar="images/avatar/";
		/// <summary>
		/// 男生默认头像
		/// </summary>
		public static readonly string avatar_male =$"{avatar}/male.png";
		/// <summary>
		/// 女生默认头像
		/// </summary>
		public static readonly string avatar_female =$"{avatar}/female.png";
		/// <summary>
		/// 未知默认头像
		/// </summary>
		public static readonly string avatar_unknown =$"{avatar}/unknown.png";
		/// <summary>
		/// 
		/// </summary>
		/// <param name="model"></param>
		/// <param name="user">需要传入用户本身</param>
		/// <returns></returns>
		public static UserDiyInfoDataModel ToViewModel (this UserDiyInfo model,DAL.Entities.UserInfo.User user)
		{
			return new UserDiyInfoDataModel()
			{
				About = model.About,
				Avatar = model.Avatar!=null?model.Avatar:
				(user.BaseInfo.Gender == GenderEnum.Male ? avatar_male: 
				(user.BaseInfo.Gender == GenderEnum.Female ? avatar_female : 
				avatar_unknown))
			};
		}
		public static UserDiyInfo ToModel(this UserDiyInfoDataModel model,UserDiyInfo raw)
		{
			if (raw == null) raw = new UserDiyInfo();
			raw.About = model?.About;
			raw.Avatar = model?.Avatar;
			return raw;
		}
	}
}
