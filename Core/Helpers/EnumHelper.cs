using System;
using System.ComponentModel;
using System.Reflection;

namespace Core.Helpers
{
	[AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
	public sealed class DisplayText : Attribute
	{
		public DisplayText(string text)
		{
			Text = text;
		}

		public string Text { get; }
	}

	public static class EnumHelper
	{
		private static string GetAttributeText(MemberInfo member, Type attributeType)
		{
			var attribute = member.GetCustomAttribute(attributeType, false);
			if (attribute != null)
			{
				if (attribute is DisplayText displayTextAttribute)
				{
					return displayTextAttribute.Text;
				}
				else if (attribute is DescriptionAttribute descriptionAttribute)
				{
					return descriptionAttribute.Description;
				}
			}
			return null;
		}

		public static string GetEnumDescription(Enum enumValue)
		{
			var type = enumValue.GetType();
			var memberInfo = type.GetField(enumValue.ToString());
			return memberInfo != null ? GetAttributeText(memberInfo, typeof(DescriptionAttribute)) ?? enumValue.ToString() : enumValue.ToString();
		}

		public static string ToDescription(this Enum enumValue)
		{
			if (enumValue == null)
			{
				return string.Empty;
			}

			var type = enumValue.GetType();
			var memberInfo = type.GetField(enumValue.ToString());

			if (memberInfo != null)
			{
				return GetAttributeText(memberInfo, typeof(DisplayText))
					?? GetAttributeText(memberInfo, typeof(DescriptionAttribute))
					?? enumValue.ToString();
			}

			return enumValue.ToString();
		}
	}
}
