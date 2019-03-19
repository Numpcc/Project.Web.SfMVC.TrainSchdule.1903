﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PhotoHub.DAL.Entities;
using PhotoHub.BLL.DTO;

namespace PhotoHub.BLL.Interfaces
{
    /// <summary>
    /// 用户服务接口.
    /// 包含用户信息的相关处理.
    /// </summary>
    /// <remarks>
    /// 此接口需要反射调用.
    /// </remarks>
    public interface IUsersService : IDisposable
    {
        /// <summary>
        /// 加载所有用户的信息
        /// </summary>
        IEnumerable<UserDTO> GetAll(int page, int pageSize);

        /// <summary>
        /// 通过userName加载用户
        /// </summary>
        UserDetailsDTO Get(string userName);

        /// <summary>
        /// Loads blocked users by this user.
        /// </summary>
        IEnumerable<UserDTO> GetBlocked(int page, int pageSize);

        /// <summary>
        /// Method for searching users.
        /// </summary>
        IEnumerable<UserDTO> Search(int page, string search, int pageSize);

        /// <summary>
        /// Follows user by current user.
        /// </summary>
        void Follow(string follow);

        /// <summary>
        /// Async follows user by current user.
        /// </summary>
        Task FollowAsync(string follow);

        /// <summary>
        /// Dismiss following on user by current user.
        /// </summary>
        void DismissFollow(string follow);

        /// <summary>
        /// Async dismiss following on user by current user.
        /// </summary>
        Task DismissFollowAsync(string follow);

        /// <summary>
        /// Blocks user by current user.
        /// </summary>
        void Block(string block);

        /// <summary>
        /// Async blocks user by current user.
        /// </summary>
        Task BlockAsync(string block);

        /// <summary>
        /// Dismiss blocking user by current user.
        /// </summary>
        void DismissBlock(string block);

        /// <summary>
        /// Async dismiss blocking user by current user.
        /// </summary>
        Task DismissBlockAsync(string block);

        /// <summary>
        /// Reports user by current user.
        /// </summary>
        void Report(string userName, string text);

        /// <summary>
        /// Async reports user by current user.
        /// </summary>
        Task ReportAsync(string userName, string text);

        /// <summary>
        /// Creates user.
        /// </summary>
        ApplicationUser Create(string userName, string email, string password);

        /// <summary>
        /// Async creates user.
        /// </summary>
        Task<ApplicationUser> CreateAsync(string userName, string email, string password);

        /// <summary>
        /// Edits user.
        /// </summary>
        void Edit(string userName, string realName, string about, string webSite, string gender, string avatar = null);

        /// <summary>
        /// Async edits user.
        /// </summary>
        Task EditAsync(string userName, string realName, string about, string webSite, string gender, string avatar = null);
    }
}
