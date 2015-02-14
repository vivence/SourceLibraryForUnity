using UnityEngine;
using Ghost.Game;
using Ghost.Attribute;

namespace Ghost.Test
{
	public class TestValueData : ValueData {

		[System.Flags]
		private enum Dirty{
			NOTHING = 0,
			VALUE_v = 1 << 0
		}

		private Dirty dirty_ = Dirty.NOTHING;
		
		private ValueInt i_ = new ValueInt(0);
		private ValueFloat f_ = new ValueFloat(0);
		private ValueInt v_ = new ValueInt(0);
		
		[PropertyField("i_")]
		public int i {get{return i_.value;} set{i_.value = value;SetValueDirty(Dirty.VALUE_v);}}
		[PropertyField("f_")]
		public float f {get{return f_.value;} set{f_.value = value;}}

		[PropertyField("v_")]
		public int v 
		{
			get
			{
				if (GetValueDirty(Dirty.VALUE_v))
				{
					v_.value = i*2; // expression
					ClearValueDirty(Dirty.VALUE_v);
				}
				return v_.value;
			}
		}

		private void SetValueDirty(Dirty valueField)
		{
			dirty_ |= valueField;
		}
		private bool GetValueDirty(Dirty valueField)
		{
			return valueField == (dirty_ & Dirty.VALUE_v);
		}
		private void ClearValueDirty(Dirty valueField)
		{
			dirty_ &= (~valueField);
		}
		
//		// Use this for initialization
//		void Start () {
//			
//		}
//		
//		// Update is called once per frame
//		void Update () {
//			
//		}
	}
} // namespace Ghost.Test
