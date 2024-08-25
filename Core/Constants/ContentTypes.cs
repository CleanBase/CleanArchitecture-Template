using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Constants
{
	public class ContentTypes
	{
		public static readonly string ZIP = "application/zip";
		public static readonly string DOCX = "application/vnd.openxmlformats";
		public static readonly string XLSX = "application/vnd.ms-excel";
		public static readonly string JSON = "application/json";
		public static readonly string OCTET_STREAM = "application/octet-stream";
		public static readonly string IMAGE_JPEG = "image/jpeg";
		public static readonly string IMAGE_PNG = "image/png";

		private readonly Dictionary<string, string> ContentTypeMapping = new Dictionary<string, string>
		{
			{ ".zip", ZIP },
			{ ".jpg", IMAGE_JPEG },
			{ ".png", IMAGE_PNG },
			{ ".xlsx", XLSX },
			{ ".docx", DOCX },
			{ ".json", JSON }
		};

		public string GetContentType(string ext)
		{
			if (ext == null)
				return OCTET_STREAM;

			return ContentTypeMapping.TryGetValue(ext.ToLower(), out var contentType)
				? contentType
				: OCTET_STREAM;
		}
	}
}

