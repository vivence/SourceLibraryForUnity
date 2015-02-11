﻿using System.Collections.Generic;
using System.IO;
using Ghost.Config;

namespace Ghost.Utils
{
	public class PathEnumerableBase : System.Collections.IEnumerable
	{
		
		public string path {get;set;}
		
		public PathEnumerableBase(string p)
		{
			path = p;
		}

		public System.Collections.IEnumerator GetEnumerator ()
		{
			return FileSystemUtils.GetPathEnumerator(path);
		}

		public System.Collections.IEnumerator GetReverseEnumerator ()
		{
			return FileSystemUtils.GetPathReverseEnumerator(path);
		}
	}

	public class PathEnumerable : PathEnumerableBase, IEnumerable<string>
	{
		public PathEnumerable(string p)
			: base(p)
		{
		}

		new public IEnumerator<string> GetEnumerator ()
		{
			return FileSystemUtils.GetPathEnumerator(path);
		}

		new public IEnumerator<string> GetReverseEnumerator ()
		{
			return FileSystemUtils.GetPathReverseEnumerator(path);
		}
	}

	public static class FileSystemUtils {
		
		public static IEnumerator<string> GetPathEnumerator(string path)
		{
			if (null != path)
			{
				var separators = new char[]{
					Path.AltDirectorySeparatorChar, 
					Path.DirectorySeparatorChar, 
					Path.PathSeparator, 
					Path.VolumeSeparatorChar
				};
				path = path.TrimStart(separators).TrimEnd(separators);
				while (0 < path.Length)
				{
					var i = path.IndexOfAny(separators);
					if (0 > i)
					{
						var part = path;
						path = string.Empty;
						yield return part;
						break;
					}
					else
					{
						var part = path.Substring(0, i);
						path = path.Substring(part.Length);
						path = path.TrimStart(separators);
						yield return part;
					}
				}
			}
		}

		public static IEnumerator<string> GetPathReverseEnumerator(string path)
		{
			if (null != path)
			{
				path = path.TrimStart(PathConfig.SEPARATORS).TrimEnd(PathConfig.SEPARATORS);
				while (0 < path.Length)
				{
					var i = path.LastIndexOfAny(PathConfig.SEPARATORS);
					if (0 > i)
					{
						var part = path;
						path = string.Empty;
						yield return part;
						break;
					}
					else
					{
						var part = path.Substring(i+1, path.Length-i-1);
						path = path.Substring(0, i);
						yield return part;
					}
				}
			}
		}
		
	}
} // namespace Ghost.Util
