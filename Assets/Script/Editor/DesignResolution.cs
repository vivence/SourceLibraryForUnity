using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Ghost.Resolution;

namespace Ghost.EditorTool
{
	[CustomEditor(typeof(ScreenResolution), true)]
	public class DesignResolution : Editor {

		private float zoomValue_ = 1;

		public override void OnInspectorGUI ()
		{
			base.OnInspectorGUI();

			ScreenResolution resolution = target as ScreenResolution;
			if (resolution.preview)
			{
				zoomValue_ = resolution.zoom;
				zoomValue_ = EditorGUILayout.Slider(zoomValue_, resolution.zoomMin, resolution.zoomMax);
				resolution.zoom = zoomValue_;
			}
		}

	}
} // namespace Ghost.EditorTool
