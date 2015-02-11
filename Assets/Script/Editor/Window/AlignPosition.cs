using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Ghost.Extensions;

namespace Ghost.EditorTool
{
	public class AlignPosition : GhostEditorWindowItem {
		
		public enum Mode{
			CAMERA_VIEW_POINT
		}

		private Mode mode_ = Mode.CAMERA_VIEW_POINT;
		private Vector2 position_ = Vector2.zero;

		public AlignPosition()
			: base("Align Position")
		{
		}
		
		private void Align()
		{
			Vector2 targetPosition = Vector2.zero;
			switch (mode_)
			{
			case Mode.CAMERA_VIEW_POINT:
				targetPosition = Camera.main.ViewportToWorldPoint(position_);
				break;
			}
			foreach (var transform in Selection.transforms)
			{
				transform.position = new Vector3(targetPosition.x, targetPosition.y, transform.position.z);
			}
		}
		private void GetAlign()
		{
			switch (mode_)
			{
			case Mode.CAMERA_VIEW_POINT:
				position_ = Camera.main.WorldToViewportPoint(Selection.activeTransform.position);
				break;
			}
		}
		
		public override void OnGUI ()
		{
			if (Selection.transforms.IsNullOrEmpty())
			{
				return;
			}
			mode_ = (Mode)EditorGUILayout.EnumPopup("Mode", mode_);
			
			switch (mode_)
			{
			case Mode.CAMERA_VIEW_POINT:
				position_ = EditorGUILayout.Vector2Field("Camera View Point", position_);
				break;
			}
			
			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Button("Align"))
			{
				Align();
			}
			if (Selection.activeTransform && 1 == Selection.transforms.Length)
			{
				if (GUILayout.Button("Get Align"))
				{
					GetAlign();
				}
			}
			EditorGUILayout.EndHorizontal();
		}
		
	}
} // namespace Ghost.EditorTool
