using System;
using System.Text;

namespace Core.Helpers
{
    public static class TextHelper
    {
        public static string ToBase64(this string content)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(content));
        }
    }
}