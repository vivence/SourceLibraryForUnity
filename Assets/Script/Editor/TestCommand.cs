﻿using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Ghost.Utils;
using Ghost.Extensions;

namespace Ghost.Editor
{
	public static class TestCommand{

		[MenuItem("Test/Command/MakeListUnique")]
		public static void MakeListUnique()
		{
			var testList = new List<int>();
			testList.Add(1);
			testList.Add(2);
			testList.Add(3);
			testList.Add(2);
			testList.Add(3);
			testList.Add(3);
			Debug.Log(testList.DumpString());
			testList = testList.ToUnique();
			Debug.Log(testList.DumpString());
		}

		[MenuItem("Test/Command/EnumPath")]
		public static void EnumPath()
		{
			Debug.Log(new PathEnumerable(Application.streamingAssetsPath).DumpString());
		}

		[MenuItem("Test/Command/ReverseEnumPath")]
		public static void ReverseEnumPath()
		{
			var enumerable = new PathEnumerable(Application.streamingAssetsPath);
			var enumerator = enumerable.GetReverseEnumerator();
			var list = new List<string>();
			while (enumerator.MoveNext())
			{
				list.Add(enumerator.Current);
			}
			Debug.Log(list.DumpString());
		}

	}
} // namespace Ghost.Editor