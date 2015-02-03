using UnityEngine;
using System.Collections;

public class ScreenResolution : MonoBehaviour {

	public enum ResolutionPolicy{
		EXACT_FIT,
		NO_BORDER,
		SHOW_ALL,
		FIXED_HEIGHT,
		FIXED_WIDTH,
	}

	public Vector2 designResolution = new Vector2(960, 640);
	public float pixelsPerUnit = 100;
	public ResolutionPolicy resolutionPolicy = ResolutionPolicy.SHOW_ALL;

	private float cameraOrthographicSize_ = 0;
	private float cameraAspect_ = 0;

	void Awake()
	{
		cameraOrthographicSize_ = camera.orthographicSize;
		cameraAspect_ = camera.aspect;
		switch (resolutionPolicy)
		{
		case ResolutionPolicy.EXACT_FIT:
			cameraOrthographicSize_ = designResolution.y/pixelsPerUnit/2;
			cameraAspect_ = designResolution.x/designResolution.y;
			break;
		case ResolutionPolicy.NO_BORDER:
			if ((float)Screen.width/(float)Screen.height < designResolution.x/designResolution.y)
			{
				cameraOrthographicSize_ = designResolution.y/pixelsPerUnit/2;
			}
			else
			{
				cameraOrthographicSize_ = designResolution.x/pixelsPerUnit/2/cameraAspect_;
			}
			break;
		case ResolutionPolicy.SHOW_ALL:
			if ((float)Screen.width/(float)Screen.height < designResolution.x/designResolution.y)
			{
				cameraOrthographicSize_ = designResolution.x/pixelsPerUnit/2/cameraAspect_;
			}
			else
			{
				cameraOrthographicSize_ = designResolution.y/pixelsPerUnit/2;
			}
			break;
		case ResolutionPolicy.FIXED_HEIGHT:
			cameraOrthographicSize_ = designResolution.y/pixelsPerUnit/2;
			break;
		case ResolutionPolicy.FIXED_WIDTH:
			cameraOrthographicSize_ = designResolution.x/pixelsPerUnit/2/cameraAspect_;
			break;
		}

		camera.orthographicSize = cameraOrthographicSize_;
		camera.aspect = cameraAspect_;
	}

//	// Use this for initialization
//	void Start () {
//	
//	}
//	
//	// Update is called once per frame
//	void Update () {
//	
//	}
}
