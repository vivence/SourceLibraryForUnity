using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using Ghost.Extensions;

namespace Ghost.EditorTool
{
	public class GetAssetInfo {
		
		// type
		[MenuItem("Assets/GetInfo/Type")]
		static void GetTypeInfo () 
		{
			var selectedAssets = Selection.GetFiltered (typeof(Object), SelectionMode.DeepAssets);
			foreach (Object obj in selectedAssets) 
			{
				Debug.Log(string.Format("{0}\ntype: {1}\n", AssetDatabase.GetAssetPath(obj), obj.GetType().ToString()));
			}
		}
		
		// dependence
		private static void DebugLogDependencies(string[] depends)
		{
			if (null != depends && 0 < depends.Length)
			{
				var dependsMap = new Dictionary<string, List<string>>(); 
				foreach (var path in depends)
				{
					var key = Path.GetExtension(path);
					List<string> list = null;
					if (!dependsMap.ContainsKey(key))
					{
						list = new List<string>();
						dependsMap.Add(key, list);
					}
					else
					{
						list = dependsMap[key];
					}
					
					list.Add(path);
				}
				foreach (var list in dependsMap.Values)
				{
					list.MakeUnique();
				}
				foreach(var key_value in dependsMap)
				{
					string log = string.Format("\t{0}: \n\n", key_value.Key);
					var list = key_value.Value;
					foreach (var path in list)
					{
						log += string.Format("{0}\n", path);
					}
					Debug.Log (log);
				}
			}
		}
		
		private static void DoGetDependenciesEach (SelectionMode mode)
		{
			var selectedAssets = Selection.GetFiltered (typeof(Object), mode);
			foreach (Object obj in selectedAssets) 
			{
				Debug.Log (string.Format("{0}\ndepends on:", AssetDatabase.GetAssetPath(obj)));	
				string[] depends = AssetDatabase.GetDependencies(new string[]{AssetDatabase.GetAssetPath(obj)});
				DebugLogDependencies(depends);
			}
		}
		
		private static void DoGetDependenciesAll (SelectionMode mode)
		{
			var selectedAssets = Selection.GetFiltered (typeof(Object), mode);
			List<string> selectedAssetsPath = new List<string>();
			foreach (var obj in selectedAssets)
			{
				selectedAssetsPath.Add(AssetDatabase.GetAssetPath(obj));
			}
			Debug.Log ("selected all\ndepends on:");	
			string[] depends = AssetDatabase.GetDependencies(selectedAssetsPath.ToArray());
			DebugLogDependencies(depends);
		}
		
		[MenuItem("Assets/GetInfo/DependenciesEach")]
		static void GetDependenciesEach () { DoGetDependenciesEach (SelectionMode.Assets); }
		
		[MenuItem("Assets/GetInfo/DependenciesEach(Deep)")]
		static void GetDependenciesEachDeep () { DoGetDependenciesEach (SelectionMode.DeepAssets); }
		
		[MenuItem("Assets/GetInfo/DependenciesAll")]
		static void GetDependenciesAll () { DoGetDependenciesAll (SelectionMode.Assets); }
		
		[MenuItem("Assets/GetInfo/DependenciesAll(Deep)")]
		static void GetDependenciesAllDeep () { DoGetDependenciesAll (SelectionMode.DeepAssets); }

		// dependence level
		private static void DoGetDependenceLevelEach (SelectionMode mode)
		{
			var selectedAssets = Selection.GetFiltered (typeof(Object), mode);
			foreach (Object obj in selectedAssets) 
			{
				var path = AssetDatabase.GetAssetPath(obj);
				Debug.Log (string.Format("{0}\ndepends level: {1}", path, AssetDependenceLevel.GetAssetLevel(obj, path)));
			}
		}

		[MenuItem("Assets/GetInfo/DependenceLevelEach")]
		static void GetDependenceLevelEach () { DoGetDependenceLevelEach (SelectionMode.Assets); }
		
		[MenuItem("Assets/GetInfo/DependenceLevelEach(Deep)")]
		static void GetDependenceLevelEachDeep () { DoGetDependenceLevelEach (SelectionMode.DeepAssets); }

		// bounds
		private static void DoGetRendererBoundsEach (SelectionMode mode)
		{
			var selectedAssets = Selection.GetFiltered (typeof(Renderer), mode);
			foreach (var obj in selectedAssets)
			{
				var log = new System.Text.StringBuilder();
				log.Append(AssetDatabase.GetAssetPath(obj));
				var renderer = obj as Renderer;
				var bounds = renderer.bounds;
				log.AppendFormat("\nrenderer bounds: {0}\n", bounds);
				Debug.Log(log.ToString());
			}
		}
		
		[MenuItem("Assets/GetInfo/RendererBoundsEach")]
		static void GetRendererBoundsEach () { DoGetRendererBoundsEach (SelectionMode.DeepAssets); }
		
	}
}// namespace Ghost.Editor
