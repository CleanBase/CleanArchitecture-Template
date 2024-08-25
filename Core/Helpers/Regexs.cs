using Core.Validators;
using System.Text.RegularExpressions;

namespace Core.Helpers
{
	public static class Regexs
	{
		public const string PHONE_REGEX = @"^((03|05|07|08|09)\d{8}|(02)\d{9})$";
		public const string PIT_REGEX = @"^\d{10}$";
		public const string CMND_REGEX = @"^\d{9}$";
		public const string CCCD_REGEX = @"^\d{12}$";
		public const string EMAIL_REGEX = @"^(?!\.|_)(?!.*?\.\.|.*?__|.*?\._|.*?_\.)[a-zA-Z0-9_\.]{0,}[a-zA-Z0-9]{1,1}@[a-zA-Z0-9]{1,1}[a-zA-Z0-9_\.]{0,}(\.[a-zA-Z0-9]{2,}){1,2}$";
		public const string SEARCH_REGEX = @"([^a-zA-Z0-9\s])";

		private static readonly EmailValidator _emailValidator = new EmailValidator();

		public static string ReplaceSpecialCharactersForSearch(string value)
		{
			if (string.IsNullOrEmpty(value))
			{
				return string.Empty;
			}

			return Regex.Replace(value, SEARCH_REGEX, @"\$1");
		}

		public static bool IsPhone(string phone)
		{
			return !string.IsNullOrEmpty(phone) && Regex.IsMatch(phone, PHONE_REGEX, RegexOptions.Compiled);
		}

		public static bool IsValidEmailFormat(string email)
		{
			return !string.IsNullOrEmpty(email) && Regex.IsMatch(email, EMAIL_REGEX, RegexOptions.Compiled);
		}

		public static bool IsEmail(string email)
		{
			return _emailValidator.Validate(email).IsValid;
		}

		public static bool IsPIT(string pit)
		{
			return !string.IsNullOrEmpty(pit) && Regex.IsMatch(pit, PIT_REGEX, RegexOptions.Compiled);
		}

		public static bool IsCMNDCCCD(string id)
		{
			return !string.IsNullOrEmpty(id) &&
				   (Regex.IsMatch(id, CMND_REGEX, RegexOptions.Compiled) ||
					Regex.IsMatch(id, CCCD_REGEX, RegexOptions.Compiled));
		}
	}
}
