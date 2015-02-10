using UnityEngine;
using System.Collections;
using Ghost.Attribute;

namespace Ghost.CameraController
{
	[ExecuteInEditMode]
	public class CameraFocusOn : MonoBehaviour {
		
		public GameObject focusObject = null;
		public Vector2 focusViewPoint = new Vector2(0.5f, 0.5f);
		public float smoothTime = 0;

		private Vector2 velocity_ = Vector2.zero;
		
		private void AdjustPosition()
		{
			if (focusObject && camera)
			{
				Vector2 from = camera.ViewportToWorldPoint(focusViewPoint);
				Vector2 to = focusObject.transform.position;
				if (!Vector2.Equals(from, to))
				{
					if (0 < smoothTime && 0 < Time.deltaTime && 0.01f < Vector2.Distance(from, to))
					{
						var current = Vector2.SmoothDamp(from, to, ref velocity_, smoothTime);
						camera.transform.Translate(current-from);
					}
					else
					{
						camera.transform.Translate(to-from);
					}
				}
			}
		}
		
		//	// Use this for initialization
		//	void Start () {
		//	
		//	}
		
		// Update is called once per frame
		void Update () {
			AdjustPosition();
		}
	}
} // namespace Ghost.CameraController
