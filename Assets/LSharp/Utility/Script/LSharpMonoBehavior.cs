using UnityEngine;
using System.Collections;

namespace Ghost.LSharp.Script
{
	public abstract class LSharpMonoBehavior {
		
		private static void LogTime(string str)
		{
			Debug.Log(string.Format("[L#] {0} at: {1}", str, System.DateTime.Now.ToLongTimeString()));
		}

		public GameObject gameObject{get; protected set;}

		protected LSharpMonoBehavior(GameObject go)
		{
			gameObject = go;
		}

		public virtual void Awake()
		{
			LogTime("Awake");
		}
		public virtual void Start() 
		{
			LogTime("Start");
		}
		public virtual void Update() 
		{
			LogTime("Update");
		}
//		public virtual void FixedUpdate()
//		{
//			LogTime("FixedUpdate");
//		}
//		public virtual void LateUpdate()
//		{
//			LogTime("LateUpdate");
//		}
//		public virtual void Reset()
//		{
//			LogTime("Reset");
//		}
//		
//		public virtual void OnMouseEnter()
//		{
//			LogTime("OnMouseEnter");
//		}
//		public virtual void OnMouseOver()
//		{
//			LogTime("OnMouseOver");
//		}
//		public virtual void OnMouseExit()
//		{
//			LogTime("OnMouseExit");
//		}
//		public virtual void OnMouseDown()
//		{
//			LogTime("OnMouseDown");
//		}
//		public virtual void OnMouseUp()
//		{
//			LogTime("OnMouseUp");
//		}
//		public virtual void OnMouseUpAsButton()
//		{
//			LogTime("OnMouseUpAsButton");
//		}
//		public virtual void OnMouseDrag()
//		{
//			LogTime("OnMouseDrag");
//		}
//		
//		public virtual void OnTriggerEnter()
//		{
//			LogTime("OnTriggerEnter");
//		}
//		public virtual void OnTriggerExit()
//		{
//			LogTime("OnTriggerExit");
//		}
//		public virtual void OnTriggerStay()
//		{
//			LogTime("OnTriggerStay");
//		}
//		public virtual void OnCollisionEnter()
//		{
//			LogTime("OnCollisionEnter");
//		}		
//		public virtual void OnCollisionExit()
//		{
//			LogTime("OnCollisionExit");
//		}
//		public virtual void OnCollisionStay()
//		{
//			LogTime("OnCollisionStay");
//		}
//		public virtual void OnControllerColliderHit()
//		{
//			LogTime("OnControllerColliderHit");
//		}
//		public virtual void OnJointBreak()
//		{
//			LogTime("OnJointBreak");
//		}
//		public virtual void OnParticleCollision()
//		{
//			LogTime("OnParticleCollision");
//		}
//		
//		public virtual void OnBecameVisible()
//		{
//			LogTime("OnBecameVisible");
//		}
//		public virtual void OnBecameInvisible()
//		{
//			LogTime("OnBecameInvisible");
//		}
//		public virtual void OnEnable()
//		{
//			LogTime("OnEnable");
//		}
//		public virtual void OnDisable()
//		{
//			LogTime("OnDisable");
//		}
//		public virtual void OnDestroy()
//		{
//			LogTime("OnDestroy");
//		}
//		
//		public virtual void OnLevelWasLoaded()
//		{
//			LogTime("OnLevelWasLoaded");
//		}
//		public virtual void OnPreCull()
//		{
//			LogTime("OnPreCull");
//		}
//		public virtual void OnPreRender()
//		{
//			LogTime("OnPreRender");
//		}
//		public virtual void OnPostRender()
//		{
//			LogTime("OnPostRender");
//		}
//		public virtual void OnRenderObject()
//		{
//			LogTime("OnRenderObject");
//		}
//		public virtual void OnWillRenderObject()
//		{
//			LogTime("OnWillRenderObject");
//		}
//		public virtual void OnGUI()
//		{
//			LogTime("OnGUI");
//		}	
//		public virtual void OnRenderImage(RenderTexture src, RenderTexture dest)
//		{
//			LogTime("OnRenderImage");
//		}
//		public virtual void OnDrawGizmosSelected()
//		{
//			LogTime("OnDrawGizmosSelected");
//		}
//		public virtual void OnDrawGizmos()
//		{
//			LogTime("OnDrawGizmos");
//		}
//		public virtual void OnApplicationPause()
//		{
//			LogTime("OnApplicationPause");
//		}
//		public virtual void OnApplicationFocus()
//		{
//			LogTime("OnApplicationFocus");
//		}
//		public virtual void OnApplicationQuit()
//		{
//			LogTime("OnApplicationQuit");
//		}
//		
//		public virtual void OnPlayerConnected()
//		{
//			LogTime("OnPlayerConnected");
//		}
//		public virtual void OnServerInitialized()
//		{
//			LogTime("OnServerInitialized");
//		}
//		public virtual void OnConnectedToServer()
//		{
//			LogTime("OnConnectedToServer");
//		}
//		public virtual void OnPlayerDisconnected()
//		{
//			LogTime("OnPlayerDisconnected");
//		}
//		public virtual void OnDisconnectedFromServer()
//		{
//			LogTime("OnDisconnectedFromServer");
//		}
//		public virtual void OnFailedToConnect()
//		{
//			LogTime("OnFailedToConnect");
//		}
//		public virtual void OnFailedToConnectToMasterServer()
//		{
//			LogTime("OnFailedToConnectToMasterServer");
//		}
//		public virtual void OnMasterServerEvent()
//		{
//			LogTime("OnMasterServerEvent");
//		}
//		public virtual void OnNetworkInstantiate(NetworkMessageInfo msg)
//		{
//			LogTime("OnNetworkInstantiate");
//		}
//		public virtual void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
//		{
//			LogTime("OnSerializeNetworkView");
//		}
		
	}
} // namespace Ghost.LSharp.Script
