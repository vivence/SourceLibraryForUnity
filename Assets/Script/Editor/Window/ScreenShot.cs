using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

namespace Ghost.EditorTool
{
	public class ScreenShot : GhostEditorWindowItem {
		
		private class Worker{
			private string fileName_;
			private float frameInterval_;
			
			private string rootFolder_ = null;
			private string folder_ = null;
			private float nextFrameTime_ = 0;
			private int frameNumber_ = 0;

			private static float GetNowSeconds()
			{
//				return Time.time;
				return System.DateTime.Now.Ticks/10000000f;
			}
			
			public Worker(string rootFolder, string fileName, int FPS)
			{
				if (0 >= FPS)
				{
					FPS = 1;
				}
				rootFolder_ = rootFolder;
				fileName_ = fileName;
				frameInterval_ = 1.0f/FPS;
				nextFrameTime_ = GetNowSeconds();
				frameNumber_ = 0;

				folder_ = Path.Combine(rootFolder_, System.DateTime.Now.ToString("yyyy-MM-dd HH.mm.ss"));
				if (!Directory.Exists(folder_))
				{
					Directory.CreateDirectory(folder_);
				}
			}
			
			public void Update()
			{
				var now = GetNowSeconds();
				if (now >= nextFrameTime_)
				{
					nextFrameTime_ = now + frameInterval_;
					var path = Path.Combine(folder_, string.Format("{0}_{1}.png", fileName_, frameNumber_++));
					Work3(Camera.main, path);
					Debug.Log(string.Format("Created ScreenShot: {0}\n", path));
				}
			}
			
			private static void Work1(string path)
			{
				Application.CaptureScreenshot(path);
			}
			
			private static void Work2(string path)
			{
				Rect rect = new Rect(0, 0, Screen.width, Screen.height);
				WorkByTexture(rect, path);
			}

			private static void Work3(Camera camera, string path)
			{
				if (!camera)
				{
					return;
				}
				Vector2 leftBottom = camera.ViewportToScreenPoint(Vector2.zero);
				Vector2 rightTop = camera.ViewportToScreenPoint(Vector2.one);
				Rect rect = new Rect(0, 0, rightTop.x-leftBottom.x, rightTop.y-leftBottom.y);

				var oldTexture = camera.targetTexture;
				RenderTexture texture = new RenderTexture((int)rect.width, (int)rect.height, 0);  
				camera.targetTexture = texture;  
				camera.Render();  

				var oldActive = RenderTexture.active;
				RenderTexture.active = texture;  

				WorkByTexture(rect, path);

				camera.targetTexture = oldTexture;
				RenderTexture.active = oldActive; 
				GameObject.DestroyImmediate(texture);  
			}

			private static void WorkByTexture(Rect rect, string path)
			{
				Texture2D texture = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGB24, false);
				texture.ReadPixels(rect, 0, 0);  
				texture.Apply();

//				byte[] bytes = texture.EncodeToPNG(); 
				byte[] bytes = texture.EncodeToJPG(100);
				path = Path.ChangeExtension(path, "jpg");
				
				FileInfo file = new FileInfo(path);
				var writer = file.OpenWrite();
				writer.Write(bytes, 0, bytes.Length);
				writer.Close();
				writer.Dispose();
			}
			
		}

		private string rootFolder_ = string.Empty;
		private string fileName_ = "ScreenShot";
		private int FPS_ = 60;
		private bool shortcut_ = false;
		private string toggleKey_ = string.Empty;

		private Worker worker_ = null;

		private bool working{
			get
			{
				return null != worker_;
			}
			set
			{
				if (!value)
				{
					worker_ = null;
				}
				else
				{
					worker_ = new Worker(rootFolder_, fileName_, FPS_);
				}
			}
		}

		public ScreenShot()
			: base("ScreenShot")
		{
		}
		
		public void StartScreenShot()
		{
			if (working)
			{
				return;
			}
			if (string.IsNullOrEmpty(rootFolder_))
			{
				var path = EditorUtility.SaveFolderPanel("Save Screenshot", "", "");
				if (string.IsNullOrEmpty(path))
				{
					return;
				}
				rootFolder_ = path;
			}
			working = true;
		}
		
		public void EndScreenShot()
		{
			if (!working)
			{
				return;
			}
			working = false;
		}
		
		private void ToggleScreenShot()
		{
			if (working)
			{
				EndScreenShot();
			}
			else
			{
				StartScreenShot();
			}
		}
		
		private void CaptureKey()
		{
			if (!string.IsNullOrEmpty(toggleKey_) && Input.GetKeyDown(toggleKey_))
			{
				ToggleScreenShot();
			}
		}

		public override void OnGUI ()
		{
			fileName_ = EditorGUILayout.TextField("File Name", fileName_);
			FPS_ = EditorGUILayout.IntField("FPS", FPS_);

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField(string.Format("Save At: {0}", rootFolder_));
			if (GUILayout.Button("Change"))
			{
				var path = EditorUtility.SaveFolderPanel("Save Screenshot", "", "");
				if (!string.IsNullOrEmpty(path))
				{
					rootFolder_ = path;
				}
			}
			EditorGUILayout.EndHorizontal();
			if (working)
			{
				if (GUILayout.Button("End"))
				{
					EndScreenShot();
				}
			}
			else
			{
				if (GUILayout.Button("Start"))
				{
					StartScreenShot();
				}
			}

			EditorGUILayout.BeginHorizontal();
			shortcut_ = EditorGUILayout.ToggleLeft("Use Toggle Key", shortcut_);
			if (shortcut_)
			{
				toggleKey_ = EditorGUILayout.TextField(toggleKey_);
			}
			EditorGUILayout.EndHorizontal();
		}

		public override void Update () 
		{
			if (null != worker_)
			{
				worker_.Update();
			}
			if (shortcut_)
			{
				CaptureKey();
			}
		}
		
	}
}
