namespace BLL.Helpers
{
	public static partial class ActionStatusMessage
	{
		public static partial class Account
		{
			public static class Login
			{
				public static readonly ApiResult Default = new ApiResult(11000, "登录验证失败");
				public static readonly ApiResult ByUnknown = new ApiResult(11100, "存在异常导致登录失败");
				public static readonly ApiResult AuthFormat = new ApiResult(11200, "新旧密码无效");
				public static readonly ApiResult AuthException = new ApiResult(11300, "异常登录，需要二次验证");
				public static readonly ApiResult AuthBlock = new ApiResult(11400, "账号存在危险,已阻止");
				public static readonly ApiResult AuthAccountOrPsw = new ApiResult(11500, "账号或密码错误");
				public static readonly ApiResult PasswordIsSame = new ApiResult(11600, "新的密码与旧密码相同");
				public static readonly ApiResult BeenBanned = new ApiResult(11700, "账号已被封禁");
				public static readonly ApiResult AccountRemoved = new ApiResult(11700, "账号已被移除，请联系管理员");
			}
		}
	}
}