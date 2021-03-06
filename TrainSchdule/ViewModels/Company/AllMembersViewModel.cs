using BLL.Helpers;
using DAL.DTO.User;
using System.Collections.Generic;

namespace TrainSchdule.ViewModels.Company
{
	/// <summary>
	/// 单位全部成员
	/// </summary>
	public class AllMembersViewModel : ApiResult
	{
		/// <summary>
		///
		/// </summary>
		public AllMembersDataModel Data { get; set; }
	}

	/// <summary>
	/// 单位全部成员
	/// </summary>
	public class AllMembersDataModel
	{
		/// <summary>
		/// 用户列表
		/// </summary>
		public IEnumerable<UserSummaryDto> List { get; set; }

		/// <summary>
		/// 总量
		/// </summary>
		public int TotalCount { get; set; }
	}
}