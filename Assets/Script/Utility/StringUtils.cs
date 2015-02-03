using System.Text;
using Ghost.Extensions;

namespace Ghost.Utils
{
	public static class StringUtils {

		public static string ArrayToString<T>(T[] objs)
		{
			if (objs.IsNullOrEmpty())
			{
				return string.Empty;
			}
			var builder = new StringBuilder();
			for (int i = 0; i < objs.Length; ++i)
			{
				builder.Append(objs[i]);
			}
			return builder.ToString();
		}

		public static string ConnectToString(params object[] objs)
		{
			return ArrayToString(objs);
		}

		public static string ArrayToStringWithSeparator<T>(string separator, T[] objs)
		{
			if (objs.IsNullOrEmpty())
			{
				return string.Empty;
			}
			var builder = new StringBuilder();
			builder.Append(objs[0]);
			for (int i = 1; i < objs.Length; ++i)
			{
				builder.Append(separator).Append(objs[i]);
			}
			return builder.ToString();
		}

		public static string ConnectToStringWithSeparator(string separator, params object[] objs)
		{
			return ArrayToStringWithSeparator(separator, objs);
		}
		
	}
} // namespace Ghost.Utils
