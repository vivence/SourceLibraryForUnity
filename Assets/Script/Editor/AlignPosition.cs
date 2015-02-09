using UnityEngine;
using UnityEditor;
using System.Collections;
using Ghost.Extensions;

namespace Ghost.EditorTool
{
	[CustomEditor(typeof(Transform), true), CanEditMultipleObjects]
	public class AlignPosition : Editor {

		public enum Mode{
			CAMERA_VIEW_POINT
		}

		private bool foldout_ = false;
		private Mode mode_ = Mode.CAMERA_VIEW_POINT;
		private Vector2 position_ = Vector2.zero;

		private void Align()
		{
			Vector2 targetPosition = Vector2.zero;
			switch (mode_)
			{
			case Mode.CAMERA_VIEW_POINT:
				targetPosition = Camera.main.ViewportToWorldPoint(position_);
				break;
			}
			foreach (var target in targets)
			{
				var transform = target as Transform;
				transform.position = new Vector3(targetPosition.x, targetPosition.y, transform.position.z);
			}
		}
		private void GetAlign()
		{
			switch (mode_)
			{
			case Mode.CAMERA_VIEW_POINT:
				position_ = Camera.main.WorldToViewportPoint((target as Transform).position);
				break;
			}
		}
		
		public override void OnInspectorGUI ()
		{
			base.OnInspectorGUI();

			foldout_ = EditorGUILayout.Foldout(foldout_, "AlignPosition");
			if (foldout_)
			{
				mode_ = (Mode)EditorGUILayout.EnumPopup("Mode", mode_);

				switch (mode_)
				{
				case Mode.CAMERA_VIEW_POINT:
					position_ = EditorGUILayout.Vector2Field("CameraViewPoint", position_);
					break;
				}

				EditorGUILayout.BeginHorizontal();
				if (GUILayout.Button("Align"))
				{
					Align();
				}
				if (GUILayout.Button("GetAlign"))
				{
					GetAlign();
				}
				EditorGUILayout.EndHorizontal();
			}
		}

	}
} // namespace Ghost.EditorTool
