using UnityEngine;
using System.Collections.Generic;
using System.Text;
using Ghost.Utils;

namespace Ghost.Extensions
{
	public static class StringExtensions {

		public static byte[] ToBytes(this string str, Encoding encoding)
		{
			return encoding.GetBytes(str);
		}
		
		public static int ToHashInt(this string str, Encoding encoding)
		{
			if (null == str)
			{
				return -1;
			}
			if (0 >= str.Length)
			{
				return 0;
			}
			return str.ToBytes(encoding).ToHashInt();
		}
		
	}
} // namespace Ghost.Extensions
