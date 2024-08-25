using CleanBase.Core.ViewModels.Response.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels.Responses
{
	public class UserResponse : EntityAuditNameResponseBase
	{
		public int Age { get; set; }
		public string Message { get; set; }
	}
}
