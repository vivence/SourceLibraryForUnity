using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using Ghost.Extensions;

namespace Ghost.EditorTool
{
	public class CheckAssetDependenceLevel {
		
		private static void DoCheckDependenceLevelEachFolder (SelectionMode mode)
		{
			var directories = new List<string>();

			var selectedAssets = Selection.GetFiltered (typeof(Object), mode);
			foreach (Object obj in selectedAssets) 
			{
				var directory = AssetDatabase.GetAssetPath(obj);
				if (File.Exists(directory))
				{
					directory = Path.GetDirectoryName(directory);
				}
				directories.Add(directory);
			}

			directories.MakeUnique();

			foreach (var directory in directories)
			{
				var level = AssetDependenceLevel.GetAssetLevel(null, directory);
				if (AssetDependenceLevel.IsBadAssetLevel(level))
				{
					Debug.LogError(string.Format("{0}\nbad dependence level", directory));
					continue;
				}
				var dependDirectories = AssetManager.GetAssetDirectoryDependenceDirectories(directory);

				foreach (var dependDirectory in dependDirectories)
				{
					var dependDirectoryLevel = AssetDependenceLevel.GetAssetLevel(null, dependDirectory);
					if (dependDirectoryLevel >= level)
					{
						Debug.LogError(string.Format("{0} level: {1}, bad depends on:\n{2} level: {3}", directory, level, dependDirectory, dependDirectoryLevel));
					}
				}
			}
		}

		[MenuItem("Assets/CheckDependenceLevel/DependenceLevelEachFolder")]
		static void CheckDependenceLevelEachFolder () { DoCheckDependenceLevelEachFolder (SelectionMode.Assets); }
		
		[MenuItem("Assets/CheckDependenceLevel/DependenceLevelEachFolder(Deep)")]
		static void CheckDependenceLevelEachFolderDeep () { DoCheckDependenceLevelEachFolder (SelectionMode.DeepAssets); }
		
	}
} // namespace Ghost.EditorTool
