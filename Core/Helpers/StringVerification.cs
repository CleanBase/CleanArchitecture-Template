using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Core.Helpers
{
	public static class StringVerification
	{
		/// <summary>
		/// Converts a string into a list of words, removing punctuation and converting to lowercase.
		/// </summary>
		public static List<string> WordsToList(string value)
		{
			if (string.IsNullOrEmpty(value))
			{
				return new List<string>();
			}

			return value
				.Split(' ', '\r', '\n', '\t')
				.Select(item => item.Trim(',', '.', '?', '!', ':', ';', '"').ToLower())
				.Where(item => !string.IsNullOrEmpty(item))
				.ToList();
		}

		/// <summary>
		/// Calculates the percentage of matching words between two lists.
		/// </summary>
		public static decimal DictionaryPercentage(List<string> left, List<string> right)
		{
			if (left == null || right == null)
			{
				return (left == null && right == null) ? 1 : 0;
			}

			var leftSet = new HashSet<string>(left);
			var rightCount = right.Count;
			var matchCount = right.Count(word => leftSet.Contains(word));

			var allCount = Math.Max(left.Count, rightCount);

			return allCount == 0 ? 0 : matchCount / (decimal)allCount;
		}

		/// <summary>
		/// Calculates the percentage of matching words between two strings.
		/// </summary>
		public static decimal StringPercentage(string left, string right)
		{
			return DictionaryPercentage(WordsToList(left), WordsToList(right));
		}

		/// <summary>
		/// Removes Vietnamese diacritics from a string.
		/// </summary>
		public static string UnsignedVietnamese(this string s)
		{
			if (string.IsNullOrEmpty(s))
			{
				return s;
			}

			Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
			string temp = s.Normalize(NormalizationForm.FormD);
			return regex.Replace(temp, string.Empty)
						.Replace('\u0111', 'd')
						.Replace('\u0110', 'D')
						.Replace("  ", " ")
						.Trim();
		}
	}
}
