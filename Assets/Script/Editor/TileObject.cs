using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using Ghost.Extensions;

namespace Ghost.EditorTool
{
	[CustomEditor(typeof(Transform), true), CanEditMultipleObjects]
	public class TileObjectEditor : Editor {
		
		public enum Mode{
			CLOSELY_SPACED,
			RANDOM_GAP
		}
		
		public enum Direction{
			HORIZONTAL,
			VERTICAL
		}
		
		public enum BaseOn{
			SIZE,
			AMOUNT
		}

		public abstract class Operator{
			public abstract float GetValue(Vector2 vec);
			public abstract void SetValue(ref Vector2 vec, float v);
			public abstract float GetOtherValue(Vector2 vec);
			public abstract void SetOtherValue(ref Vector2 vec, float v);

			public abstract float GetMin(Rect rect);
			public abstract float GetMax(Rect rect);
		}
		public class HorizontalOperator : Operator{

			private HorizontalOperator(){}
			public static HorizontalOperator Global = new HorizontalOperator();

			public override float GetValue(Vector2 vec)
			{
				return vec.x;
			}
			public override void SetValue(ref Vector2 vec, float v)
			{
			 	vec.x = v;
			}
			public override float GetOtherValue(Vector2 vec)
			{
				return vec.y;
			}
			public override void SetOtherValue(ref Vector2 vec, float v)
			{
				vec.y = v;
			}

			public override float GetMin(Rect rect)
			{
				return rect.xMin;
			}
			public override float GetMax(Rect rect)
			{
				return rect.xMax;
			}

		}
		public class VerticalOperator : Operator{

			private VerticalOperator(){}
			public static VerticalOperator Global = new VerticalOperator();

			public override float GetValue(Vector2 vec)
			{
				return vec.y;
			}
			public override void SetValue(ref Vector2 vec, float v)
			{
				vec.y = v;
			}
			public override float GetOtherValue(Vector2 vec)
			{
				return vec.x;
			}
			public override void SetOtherValue(ref Vector2 vec, float v)
			{
				vec.x = v;
			}

			public override float GetMin(Rect rect)
			{
				return rect.yMin;
			}
			public override float GetMax(Rect rect)
			{
				return rect.yMax;
			}

		}
		
		private bool foldout_ = true;
		
		private string name_ = "TiledLayer";
		private Mode mode_ = Mode.CLOSELY_SPACED;
		private Vector2 gapMin_ = Vector2.zero;
		private Vector2 gapMax_ = Vector2.zero;
		private Direction direction_ = Direction.HORIZONTAL;
		private BaseOn baseOn_ = BaseOn.AMOUNT;
		private float size_ = 0;
		private int count_ = 0;
		private bool flip_ = true;
		private bool randomPrefab_ = false;
		
		private Vector2 basePosition_ = Vector2.zero;
		
		public override void OnInspectorGUI ()
		{
			base.OnInspectorGUI();
			
			var objs = Selection.GetFiltered(typeof(GameObject), SelectionMode.Assets);
			if (objs.IsNullOrEmpty())
			{
				return;
			}
			
			var prefabs = new List<GameObject>();
			foreach (var obj in objs)
			{
				var path = AssetDatabase.GetAssetPath(Selection.activeGameObject);
				if (!Path.GetExtension(path).ToLower().Equals(".prefab"))
				{
					continue;
				}
				prefabs.Add(obj as GameObject);
			}
			
			if (0 >= prefabs.Count)
			{
				return;
			}
			
			foldout_ = EditorGUILayout.Foldout(foldout_, "TileObject");
			if (foldout_)
			{
				name_ = EditorGUILayout.TextField("Name", name_);
				mode_ = (Mode)EditorGUILayout.EnumPopup("Mode", mode_);
				switch (mode_)
				{
				case Mode.CLOSELY_SPACED:
					if (1 == prefabs.Count)
					{
						flip_ = EditorGUILayout.ToggleLeft("Flip", flip_);
					}
					break;
				case Mode.RANDOM_GAP:
					flip_ = false;
					gapMin_ = EditorGUILayout.Vector2Field("GapMin", gapMin_);
					gapMax_ = EditorGUILayout.Vector2Field("GapMax", gapMax_);
					break;
				default:
					flip_ = false;
					break;
				}
				direction_ = (Direction)EditorGUILayout.EnumPopup("Direction", direction_);
				baseOn_ = (BaseOn)EditorGUILayout.EnumPopup("BaseOn", baseOn_);
				
				switch (baseOn_)
				{
				case BaseOn.SIZE:
					switch (direction_)
					{
					case Direction.HORIZONTAL:
						size_ = EditorGUILayout.FloatField("width", size_);
						break;
					case Direction.VERTICAL:
						size_ = EditorGUILayout.FloatField("height", size_);
						break;
					}
					break;
				case BaseOn.AMOUNT:
					if (0 > count_)
					{
						count_ = 0;
					}
					count_ = EditorGUILayout.IntField("count", count_);
					break;
				}
				
				if (1 < prefabs.Count)
				{
					randomPrefab_ = EditorGUILayout.ToggleLeft("Random", randomPrefab_);
				}
				
				if (GUILayout.Button("Create New"))
				{
					CreateNew(prefabs.ToArray());
				}
			}
		}

		private float PlacePart(GameObject part, GameObject prevPart)
		{
			Operator op = null;
			switch (direction_)
			{
			case Direction.HORIZONTAL:
				op = HorizontalOperator.Global;
				break;
			case Direction.VERTICAL:
				op = VerticalOperator.Global;
				break;
			}

			if (null != prevPart)
			{
				if (flip_)
				{
					Vector2 scale = part.transform.localScale;
					Vector2 prevScale = prevPart.transform.localScale;

					var value = op.GetValue(scale)*(-(op.GetValue(prevScale)/Mathf.Abs(op.GetValue(prevScale))));
					op.SetValue(ref scale, value);
					
					part.transform.localScale = scale;
				}
				
				var rect = part.CalcCompositeRect();
				var prevRect = prevPart.CalcCompositeRect();
				
				Vector2 position = prevPart.transform.localPosition;
				position.x += op.GetMax(prevRect)+(-op.GetMin(rect));
				
				float size = op.GetValue(rect.size);
				switch (mode_)
				{
				case Mode.RANDOM_GAP:
				{
					var gap = Random.Range(op.GetValue(gapMin_), op.GetValue(gapMax_));
					size += gap;
					op.SetValue(ref position, op.GetValue(position) + gap);
					op.SetOtherValue(ref position, op.GetOtherValue(basePosition_)+Random.Range(op.GetOtherValue(gapMin_), op.GetOtherValue(gapMax_)));
				}
					break;
				}

				part.transform.localPosition = position;

				return size;
			}
			else
			{
				basePosition_ = part.transform.localPosition;
				return op.GetValue(part.CalcCompositeRect().size);
			}
		}

		delegate bool DelegateTilingFinished(int tiledCount, float tiledSize);
		private void CreateNew(GameObject[] prefabs)
		{
			DelegateTilingFinished tilingFinished = null;
			switch (baseOn_)
			{
			case BaseOn.SIZE:
				tilingFinished = delegate(int tiledCount, float tiledSize) {
					return tiledSize >= size_;
				};
				break;
			case BaseOn.AMOUNT:
				tilingFinished = delegate(int tiledCount, float tiledSize) {
					return tiledCount >= count_;
				};
				break;
			}
			doCreateNew(prefabs, tilingFinished);
		}
		
		private void doCreateNew(
			GameObject[] prefabs, 
			DelegateTilingFinished tilingFinished)
		{
			var layer = new GameObject();
			layer.name = name_;
			
			int tiledCount = 0;
			float tiledSize = 0;
			GameObject prevPart = null;
			while (!tilingFinished(tiledCount, tiledSize))
			{
				var i = tiledCount;
				
				var gameObject = GameObject.Instantiate(prefabs[randomPrefab_ ? Random.Range(0, prefabs.Length) : i%prefabs.Length]) as GameObject;
				gameObject.name = string.Format("{0}_{1}", i, gameObject.name);
				gameObject.transform.parent = layer.transform;
				
				tiledSize += PlacePart(gameObject, prevPart);
				
				++tiledCount;
				prevPart = gameObject;
			}
		}
		
	}
}
