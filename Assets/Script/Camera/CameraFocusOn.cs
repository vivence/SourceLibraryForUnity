using UnityEngine;
using System.Collections;
using Ghost.Attribute;

[ExecuteInEditMode]
public class CameraFocusOn : MonoBehaviour {

	public GameObject focusObject = null;
	public Vector2 focusViewPoint = new Vector2(0.5f, 0.5f);

	private void AdjustPosition()
	{
		if (focusObject && camera)
		{
			Vector2 viewPointPosition = camera.ViewportToWorldPoint(focusViewPoint);
			Vector2 delta = (Vector2)focusObject.transform.position - viewPointPosition;
			if (!Vector2.zero.Equals(delta))
			{
				camera.transform.Translate(delta);
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
