﻿using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Entities.UserInfo
{
	public class UserSocialInfo : BaseEntity
	{
		public string Phone { get; set; }
		public string Address { get; set; }
		public string AddressDetail { get; set; }

	}
}