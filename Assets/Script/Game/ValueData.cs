using UnityEngine;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Ghost.Attribute;

namespace Ghost.Game
{
	public abstract class ValueData : MonoBehaviour {

		public class SecurityValue
		{
			[StructLayout(LayoutKind.Explicit, Size=sizeof(float))]
			private struct ValueUnion
			{
				[FieldOffset(0)]
				public float fValue;
				
				[FieldOffset(0)]
				public int iValue;
				
			}

			private const int XOR_VALUE = int.MaxValue/2;

			private ValueUnion v_;

			protected SecurityValue()
			{
				SetValue(default(float));
			}

			protected int GetValueStored(int useless)
			{
				return v_.iValue;
			}
			protected int GetValue(int useless)
			{
				return Decrypt().iValue;
			}
			protected void SetValue(int v)
			{
				v_.iValue = v;
				Encrypt();
				Debug.Log(string.Format("set int({0}): stored({1}, get({2}))", v, GetValueStored(v), GetValue(v)));
			}

			protected float GetValueStored(float useless)
			{
				return v_.fValue;
			}
			protected float GetValue(float useless)
			{
				return Decrypt().fValue;
			}
			protected void SetValue(float v)
			{
				v_.fValue = v;
				Encrypt();
				Debug.Log(string.Format("set float({0}): stored({1}, get({2}))", v, GetValueStored(v), GetValue(v)));
			}

			private void Encrypt()
			{
				v_.iValue ^= XOR_VALUE;
			}

			private ValueUnion Decrypt()
			{
				var tempV = v_;
				tempV.iValue ^= XOR_VALUE;
				return tempV;
			}
		}

		protected class ValueInt : SecurityValue
		{
			public ValueInt(int v) {value = v;}

			public int valueStored {get{return GetValueStored(default(int));}}
			public int value {get{return GetValue(default(int));} set{SetValue(value);}}
		
//			public static implicit operator int(ValueInt v) {return v.value;}
		}

		protected class ValueFloat : SecurityValue
		{
			public ValueFloat(float v) {value = v;}
			
			public float valueStored{get{return GetValueStored(default(float));}}
			public float value{get{return GetValue(default(float));} set{SetValue(value);}}

//			public static implicit operator float(ValueFloat v) {return v.value;}
		}
		
//		// Use this for initialization
//		void Start () {
//		}
//		
//		// Update is called once per frame
//		void Update () {
//			
//		}
	}
} // namespace Ghost.Game
