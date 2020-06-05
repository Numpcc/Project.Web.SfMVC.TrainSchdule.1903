﻿using BLL.Interfaces;
using DAL.Data;
using DAL.Entities.UserInfo;
using Microsoft.AspNetCore.Http;
using System;

namespace BLL.Services
{
	/// <summary>
	/// Contains properties that returns current user.
	/// Realization of <see cref="ICurrentUserService"/>.
	/// </summary>
	public class CurrentUserService : ICurrentUserService
    {
        #region Fields
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApplicationDbContext _context;

        #endregion

        #region Properties

        /// <returns>
        /// Returns current user entity.
        /// </returns>
        public User CurrentUser =>
	        _context.AppUsers
		        .Find(_httpContextAccessor.HttpContext.User.Identity.Name);

		public IHttpContextAccessor HttpContextAccessor => _httpContextAccessor;

		#endregion

		#region .ctors

		/// <summary>
		/// Initializes a new instance of the <see cref="CurrentUserService"/>.
		/// </summary>
		public CurrentUserService( IHttpContextAccessor httpContextAccessor, ApplicationDbContext context)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        #endregion

    }
}