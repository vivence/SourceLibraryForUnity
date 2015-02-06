using UnityEngine;
using System.Collections.Generic;

namespace Ghost.Extensions
{
	public static class GameObjectExtensions {
		
		public static Rect CalcCompositeRect(this GameObject obj)
		{
			Vector2 min = Vector2.zero;
			Vector2 max = Vector2.zero;
			if (obj.renderer)
			{
				min = obj.renderer.bounds.min;
				max = obj.renderer.bounds.max;
			}
			var renderers = obj.GetComponentsInChildren<Renderer>();
			if (!renderers.IsNullOrEmpty())
			{
				foreach (var renderer in renderers)
				{
					var bounds = renderer.bounds;
					min.x = Mathf.Min(min.x, bounds.min.x);
					min.y = Mathf.Min(min.y, bounds.min.y);
					max.x = Mathf.Max(max.x, bounds.max.x);
					max.y = Mathf.Max(max.y, bounds.max.y);
				}
			}
			return new Rect(min.x-obj.transform.position.x, min.y-obj.transform.position.y, max.x-min.x, max.y-max.y);
		}

		public static Bounds CalcCompositeBounds(this GameObject obj)
		{
			var rect = obj.CalcCompositeRect();
			var center = rect.center + (Vector2)obj.transform.position;
			return new Bounds(new Vector3(center.x, center.y, obj.transform.position.z), new Vector3(rect.size.x, rect.size.y, obj.transform.position.z));
		}
		
	}
} // namespace Ghost.Extensions
