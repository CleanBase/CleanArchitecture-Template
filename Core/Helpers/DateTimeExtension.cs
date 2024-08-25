using System;
using System.Globalization;

namespace Core.Helpers
{
	public static class DateTimeExtension
	{
		public static int GetWeeksInYear(this DateTime dt)
		{
			DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
			Calendar cal = dfi.Calendar;
			return cal.GetWeekOfYear(dt, dfi.CalendarWeekRule, DayOfWeek.Monday);
		}

		private static DateTime? TryParseDate(string str, string format, char separator)
		{
			if (string.IsNullOrEmpty(str))
				return null;

			var dateParts = str.Split(separator);
			if (dateParts.Length == 3 &&
				int.TryParse(dateParts[2], out int year) &&
				int.TryParse(dateParts[1], out int month) &&
				int.TryParse(dateParts[0], out int day))
			{
				try
				{
					return new DateTime(year, month, day);
				}
				catch (ArgumentOutOfRangeException)
				{
					return null;
				}
			}

			return DateTime.TryParseExact(str, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDate)
				? parsedDate
				: (DateTime?)null;
		}

		public static DateTime? ParseDate(this string str)
		{
			return TryParseDate(str, "dd/MM/yyyy", '/');
		}

		public static DateTime? ParseDatePIT(this string str)
		{
			return TryParseDate(str, "dd-MM-yyyy", '-');
		}

		public static bool IsDate(this string str)
		{
			return str.ParseDate().HasValue;
		}
	}
}
