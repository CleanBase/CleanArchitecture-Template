using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Core.Helpers
{
	public static class Randomize
	{
		private static readonly ThreadLocal<Random> rng = new ThreadLocal<Random>(() => new Random());

		public static void Shuffle<T>(this IList<T> list)
		{
			int n = list.Count;
			while (n > 1)
			{
				n--;
				int k = rng.Value.Next(n + 1);
				T value = list[k];
				list[k] = list[n];
				list[n] = value;
			}
		}

		public static string RandomString(int length)
		{
			const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
			char[] stringChars = new char[length];
			for (int i = 0; i < length; i++)
			{
				stringChars[i] = chars[rng.Value.Next(chars.Length)];
			}
			return new string(stringChars);
		}
	}
}
