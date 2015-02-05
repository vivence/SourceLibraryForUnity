using UnityEngine;
using System.Collections;
using Ghost.Attribute;

namespace Ghost.Test
{
	public class TestSetProperty : MonoBehaviour {

		[SerializeField, SetProperty("property")]
		private int property_ = 0;

		public int property
		{
			get
			{
				return property_;
			}
			set
			{
				property_ = value;
				Debug.Log(string.Format("set property: {0}", value));
			}
		}
		
		// Use this for initialization
		void Start () {
			Debug.Log(string.Format("property: {0}", property_));
		}
		
//		// Update is called once per frame
//		void Update () {
//			
//		}

	}
}
