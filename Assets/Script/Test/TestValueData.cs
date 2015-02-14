using UnityEngine;
using Ghost.Game;
using Ghost.Attribute;

namespace Ghost.Test
{
	public class TestValueData : ValueData {
		
		private ValueInt i_ = new ValueInt(0);
		private ValueFloat f_ = new ValueFloat(0);
		
		[PropertyField("i_")]
		public int i {get{return i_.value;} set{i_.value = value;}}
		[PropertyField("f_")]
		public float f {get{return f_.value;} set{f_.value = value;}}
		
		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			
		}
	}
} // namespace Ghost.Test
