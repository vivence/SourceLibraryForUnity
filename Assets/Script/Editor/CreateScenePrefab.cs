using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using Ghost.Config;

namespace Ghost.EditorTool
{
	public class CreateScenePrefab {
		
		public const string DIRECTORY_SCENE_PREFAB = "ScenePrefab";
		
		public static void DoCreate(string sceneFile)
		{
			if (!string.Equals(Path.GetExtension(sceneFile).TrimStart('.'), PathConfig.EXTENSION_SCENE))
			{
				return;
			}
			if (EditorApplication.OpenScene(sceneFile))
			{
				Debug.Log("\topen scene succeed\n");
				
				string prefabPath = Path.Combine(PathConfig.DIRECTORY_ASSETS, DIRECTORY_SCENE_PREFAB);
				prefabPath = Path.Combine(prefabPath, Path.GetFileNameWithoutExtension(sceneFile));
				if (!Directory.Exists(prefabPath))
				{
					Directory.CreateDirectory(prefabPath);
				}
				
				foreach (GameObject obj in Object.FindObjectsOfType(typeof(GameObject)))
				{
					if (null == obj.transform.parent)
					{
						Debug.Log(string.Format ("\tfind object: {0}\n", obj.name));
						
						var prefabFile = Path.Combine(prefabPath, obj.name);
						prefabFile = Path.ChangeExtension(prefabFile, PathConfig.EXTENSION_PREFAB);
						var prefab = PrefabUtility.CreatePrefab(prefabFile, obj, ReplacePrefabOptions.ReplaceNameBased);
						if (null != prefab)
						{
							Debug.Log(string.Format("\tcreate prefab succeed\n\t{0}\n", prefabFile));
						}
						else
						{
							Debug.Log(string.Format("\tcreate prefab failed\n\t{0}\n", prefabFile));
						}
					}
				}
			}
			else
			{
				Debug.Log("\topen scene failed\n");
			}
		}
		
		private static void DoCreateEach (SelectionMode mode)
		{
			Object[] selectedAssets = Selection.GetFiltered (typeof(Object), mode);
			foreach (Object obj in selectedAssets) 
			{
				DoCreate(AssetDatabase.GetAssetPath(obj));
			}
			AssetDatabase.Refresh();
		}
		
		[MenuItem("Assets/Create/ScenePrefabEach")]
		static void CreateEach () { DoCreateEach (SelectionMode.Assets); }
		
		[MenuItem("Assets/Create/ScenePrefabEach(Deep)")]
		static void CreateDeep () { DoCreateEach (SelectionMode.DeepAssets); }
		
	}
}// namespace Ghost.Editor
