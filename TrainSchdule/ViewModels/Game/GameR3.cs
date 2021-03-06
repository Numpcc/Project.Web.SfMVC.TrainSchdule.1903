using BLL.Helpers;
using DAL.Entities.Game_r3;

namespace TrainSchdule.ViewModels.Game
{
	/// <summary>
	///
	/// </summary>
	public class UserInfoViewModel : ApiResult
	{
		/// <summary>
		///
		/// </summary>
		public UserInfoDataModel Data { get; set; }
	}

	/// <summary>
	///
	/// </summary>
	public class UserInfoDataModel
	{
		/// <summary>
		///
		/// </summary>
		public UserInfo User { get; set; }
	}
}