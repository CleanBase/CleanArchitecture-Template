using CleanBase.Core.Settings;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Core.Settings
{
	public class GeneralSetting : AppSettings<GeneralSetting>
	{
		[JsonConverter(typeof(StringEnumConverter))]
		public AppENV AppEnv { get; set; } = AppENV.DEV;

	}
}
