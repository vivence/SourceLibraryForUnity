using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Ghost.Game;
using Ghost.Attribute;

namespace Ghost.EditorTool
{
	[CustomEditor(typeof(ValueData), true)]
	public class ValueDataEditor : Editor {

		private bool foldout_ = true;
		
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			foldout_ = EditorGUILayout.Foldout(foldout_, "Security Values");
			if (!foldout_)
			{
				return;
			}

			var type = target.GetType();
			var properties = type.GetProperties();

			EditorGUILayout.BeginVertical();
			foreach (var property in properties)
			{
				PropertyFieldAttribute fieldAttribute = null;
				foreach (var attribute in property.GetCustomAttributes(false))
				{
					if (null != (fieldAttribute = attribute as PropertyFieldAttribute))
					{
						break;
					}
				}
				if (null == fieldAttribute)
				{
					continue;
				}

				var field = type.GetField(fieldAttribute.name, 
				                          System.Reflection.BindingFlags.Public 
				                          | System.Reflection.BindingFlags.NonPublic
				                          | System.Reflection.BindingFlags.Instance);
				if (null == field)
				{
					continue;
				}

				var fieldType = field.FieldType;
				if (!fieldType.IsSubclassOf(typeof(ValueData.SecurityValue)))
				{
					continue;
				}

				var valueStoredProperty = fieldType.GetProperty("valueStored");
				if (null == valueStoredProperty)
				{
					continue;
				}

				var propertyType = property.PropertyType;
				var propertyValue = property.GetValue(target, null);
				object newV = null;

				EditorGUI.BeginChangeCheck();
				var labelText = string.Format("{0} ({1})", property.Name, valueStoredProperty.GetValue(field.GetValue(target), null));
				if (typeof(int) == propertyType)
				{
					newV = EditorGUILayout.IntField(labelText, (int)propertyValue);
				}
				else if (typeof(float) == propertyType)
				{
					newV = EditorGUILayout.FloatField(labelText, (float)propertyValue);
				}
				if (EditorGUI.EndChangeCheck() && null != newV)
				{
					property.SetValue(target, newV, null);
				}
			}
			EditorGUILayout.EndVertical();
		}
		
	}
} // namespace Ghost.EditorTool
