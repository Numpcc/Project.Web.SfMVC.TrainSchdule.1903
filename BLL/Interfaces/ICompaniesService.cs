using DAL.Entities.UserInfo;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DAL.Entities;

namespace BLL.Interfaces
{
	/// <summary>
	/// 用户服务接口.
	/// 包含用户信息的相关处理.
	/// </summary>
	/// <remarks>
	/// 此接口需要反射调用.
	/// </remarks>
	public interface ICompaniesService
	{
		/// <summary>
		/// 获取授权可访问的单位
		/// </summary>
		/// <param name="currentUser"></param>
		/// <returns></returns>
		List<Company> PermissionViewCompanies(User currentUser);

		Duties GetDuties(int code);

		/// <summary>
		/// 加载所有单位的信息
		/// </summary>
		IEnumerable<Company> GetAll(int page, int pageSize);

		IEnumerable<Company> GetAll(Expression<Func<Company, bool>> predicate, int page, int pageSize);

		/// <summary>
		/// 通过单位路径
		/// </summary>
		Company GetById(string Code);

		/// <summary>
		/// 找到所有子单位
		/// </summary>
		/// <param name="code"></param>
		/// <returns></returns>
		IEnumerable<Company> FindAllChild(string code);

		/// <summary>
		/// 获取父单位
		/// </summary>
		/// <param name="code"></param>
		/// <returns></returns>
		Company FindParent(string code);

		/// <summary>
		/// 新增一个单位
		/// </summary>
		/// <param name="name">本单位的名称</param>
		/// <param name="code">单位代码</param>
		/// <returns></returns>
		Company Create(string name, string code);

		/// <summary>
		/// 异步新增一个单位
		/// </summary>
		Task<Company> CreateAsync(string name, string code);

		bool Edit(string code, Action<Company> editCallBack);

		Task<bool> EditAsync(string code, Action<Company> editCallBack);

		/// <summary>
		/// 返回显示指定的管理
		/// </summary>
		/// <param name="code"></param>
		/// <param name="userid"></param>
		/// <returns></returns>
		IEnumerable<User> GetCompanyManagers(string code, string userid);

		bool CheckManagers(string code, string userid);
	}
}