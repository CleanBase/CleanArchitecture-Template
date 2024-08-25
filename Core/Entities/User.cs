using CleanBase.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
	public class User : EntityNameAuditActive
	{
		public int Age { get; set; }
	}
}
