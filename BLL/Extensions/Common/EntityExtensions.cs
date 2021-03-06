using BLL.Helpers;
using DAL.Entities;
using DAL.Entities.ApplyInfo;
using DAL.Entities.UserInfo;
using DAL.Entities.ZX.Grade;
using DAL.Entities.ZX.Phy;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Extensions.Common
{
	/// <summary>
	///
	/// </summary>
	public static class EntityExtensions
	{
		/// <summary>
		/// 资源不存在
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public static ApiResult NotExist(this BaseEntity model) => ActionStatusMessage.StaticMessage.ResourceNotExist;

		public static ApiResult NotExist(this Apply model) => ActionStatusMessage.ApplyMessage.NotExist;

		public static ApiResult NotExist(this RecallOrder model) => ActionStatusMessage.ApplyMessage.RecallMessage.NotExist;

		public static ApiResult NotExist(this User model) => ActionStatusMessage.UserMessage.NotExist;

		public static ApiResult NotLogin(this User model) => ActionStatusMessage.Account.Auth.Invalid.NotLogin;

		public static ApiResult NotExist(this GradePhyRecord model) => ActionStatusMessage.Grade.Record.NotExist;

		public static ApiResult NotExist(this GradeExam model) => ActionStatusMessage.Grade.Exam.NotExist;
	}
}