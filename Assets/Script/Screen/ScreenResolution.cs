using UnityEngine;
using Ghost.Attribute;

namespace Ghost.Resolution
{
	[ExecuteInEditMode]
	public class ScreenResolution : MonoBehaviour {
		
		public enum ResolutionPolicy{
			EXACT_FIT,
			NO_BORDER,
			SHOW_ALL,
			FIXED_HEIGHT,
			FIXED_WIDTH,
		}

		private static readonly Vector2 DEFAULT_DESIGN_RESOLUTION = new Vector2(960, 640);
		private const ResolutionPolicy DEFAULT_RESOLUTION_POLICY = ResolutionPolicy.SHOW_ALL;
		private const float ZOOM_MIN_LIMIT = 0.1f;
		private const float ZOOM_MAX_LIMIT = 10.0f;

		public float pixelsPerUnit = 100;

		public Vector2 designResolution = DEFAULT_DESIGN_RESOLUTION;
		public ResolutionPolicy resolutionPolicy = DEFAULT_RESOLUTION_POLICY;

		[SerializeField, SetProperty("zoomMin")]
		public float zoomMin_ = 1;
		[SerializeField, SetProperty("zoomMax")]
		public float zoomMax_ = 1;

		public bool preview = false;
		
		private float cameraOrthographicSize_ = 0;
		private float cameraAspect_ = 0;
		private float zoom_ = 1;

		public float cameraOrthographicSize
		{
			get
			{
				return cameraOrthographicSize_;
			}
		}

		public float cameraAspect
		{
			get
			{
				return cameraAspect_;
			}
		}

		public float zoomMin
		{
			get
			{
				return zoomMin_;
			}
			set
			{
				zoomMin_ = TrimZoomLimitValue(value);
//				if (zoomMin_ > zoomMax_)
//				{
//					zoomMax_ = zoomMin_;
//				}
//				if (zoomMin_ > zoom)
//				{
//					zoom = zoomMin_;
//				}
			}
		}
		public float zoomMax
		{
			get
			{
				return zoomMax_;
			}
			set
			{
				zoomMax_ = TrimZoomLimitValue(value);
//				if (zoomMax_ < zoomMin_)
//				{
//					zoomMin_ = zoomMax_;
//				}
//				if (zoomMax_ < zoom)
//				{
//					zoom = zoomMax_;
//				}
			}
		}

		public float zoom
		{
			get
			{
				return zoom_;
			}
			set
			{
				var newZoom = TrimZoomValue(value);
				if (newZoom != zoom_)
				{
					zoom_ = newZoom;
					ResetZoom();
				}
			}
		}

		private float TrimZoomLimitValue(float value)
		{
			return Mathf.Clamp(value, ZOOM_MIN_LIMIT, ZOOM_MAX_LIMIT);
		}

		private float TrimZoomValue(float value)
		{
			return Mathf.Clamp(value, zoomMin, zoomMax);
		}

		private void ResetView(Camera camera)
		{
			CalcCameraSizeAndAspect(camera, resolutionPolicy, designResolution, pixelsPerUnit, 
			                        out cameraOrthographicSize_, out cameraAspect_);
			camera.aspect = cameraAspect_;
			ResetZoom();
		}

		private void ResetZoom()
		{
			var cameraOrthographicSize = cameraOrthographicSize_ * zoom;
			Camera.main.orthographicSize = cameraOrthographicSize;
		}

		public void ResetView()
		{
			ResetView(Camera.main);
		}
		
		void Awake()
		{
			ResetView();
		}
		
//		// Use this for initialization
//		void Start () {
//			
//		}
//			
		// Update is called once per frame
		void Update () {
			if (preview)
			{
				ResetView();
			}
		}

		public static void CalcCameraSizeAndAspect(Camera camera, ResolutionPolicy resolutionPolicy, Vector2 designResolution, float pixelsPerUnit, 
		                                           out float cameraOrthographicSize, out float cameraAspect)
		{
			camera.aspect = System.Convert.ToSingle(Screen.width)/System.Convert.ToSingle(Screen.height);
			cameraOrthographicSize = camera.orthographicSize;
			cameraAspect = camera.aspect;
			switch (resolutionPolicy)
			{
			case ResolutionPolicy.EXACT_FIT:
				cameraOrthographicSize = designResolution.y/pixelsPerUnit/2;
				cameraAspect = designResolution.x/designResolution.y;
				break;
			case ResolutionPolicy.NO_BORDER:
				if (cameraAspect < designResolution.x/designResolution.y)
				{
					cameraOrthographicSize = designResolution.x/pixelsPerUnit/2/cameraAspect;
				}
				else
				{
					cameraOrthographicSize = designResolution.y/pixelsPerUnit/2;
				}
				break;
			case ResolutionPolicy.SHOW_ALL:
				if (cameraAspect < designResolution.x/designResolution.y)
				{
					cameraOrthographicSize = designResolution.y/pixelsPerUnit/2;
				}
				else
				{
					cameraOrthographicSize = designResolution.x/pixelsPerUnit/2/cameraAspect;
				}
				break;
			case ResolutionPolicy.FIXED_HEIGHT:
				cameraOrthographicSize = designResolution.y/pixelsPerUnit/2;
				break;
			case ResolutionPolicy.FIXED_WIDTH:
				cameraOrthographicSize = designResolution.x/pixelsPerUnit/2/cameraAspect;
				break;
			}
		}
			
	}
} // namespace Ghost.Resolution
