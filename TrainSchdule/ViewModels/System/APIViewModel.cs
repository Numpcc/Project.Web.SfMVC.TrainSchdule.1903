﻿using BLL.Helpers;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace TrainSchdule.ViewModels.System
{

	/// <summary>
	/// API返回状态
	/// </summary>
	public  class ApiDataModel
	{
		/// <summary>
		/// 
		/// </summary>
		[JsonProperty("status")]
		public int Code { get; set; }
		/// <summary>
		/// 
		/// </summary>
		[JsonProperty("message")]
		public string Message { get; set; }
		public ApiDataModel() { }

		public ApiDataModel(int code, string message)
		{
			Code = code;
			Message = message;
		}
		public ApiDataModel(ApiResult status) : this(status.Status,status.Message) { }
	}
	/// <summary>
	/// 批量请求情况回复
	/// </summary>
	public class ResponseStatusOrModelExceptionViweModel:ApiDataModel
	{
		public ResponseStatusOrModelExceptionViweModel(ApiResult status) : base(status.Status, status.Message) { }
		/// <summary>
		/// 返回键对应的错误
		/// </summary>
		public Dictionary<string, ApiResult> StatusException { get; set; }
		/// <summary>
		/// 键对应的格式错误
		/// </summary>
		public Dictionary<string,ModelStateExceptionDataModel> ModelStateException { get; set; }
	}
}
