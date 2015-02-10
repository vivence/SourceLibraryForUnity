using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace Ghost.EditorTool
{
	public abstract class GhostEditorWindowItem{
		
		public string name{get; set;}
		public bool foldout{get; set;}
		
		public GhostEditorWindowItem(string n)
		{
			name = n;
			foldout = false;
		}
		
		public abstract void OnGUI();
		
	}
	
	public class GhostEditorWindow : EditorWindow
	{
		[MenuItem ("Window/Ghost")]
		static void ShowWindow ()
		{       
			EditorWindow.GetWindow<GhostEditorWindow>(false, "Ghost", true);	
		}
		
		private List<GhostEditorWindowItem> items_;
		
		public GhostEditorWindow()
		{
			items_ = new List<GhostEditorWindowItem>();
			items_.Add(new AlignPosition());
			items_.Add(new TileObject());
		}
		
		public void Awake () 
		{
			
		}
		
		//绘制窗口时调用
		void OnGUI () 
		{
			EditorGUILayout.BeginVertical();
			foreach (var item in items_)
			{
				item.foldout = EditorGUILayout.Foldout(item.foldout, item.name);
				if (item.foldout)
				{
					item.OnGUI();
				}
			}
			EditorGUILayout.EndVertical();
		}
		
		//更新
		void Update()
		{
			
		}
		
		// 当窗口获得焦点时调用一次
		void OnFocus()
		{
			
		}
		
		// 当窗口丢失焦点时调用一次
		void OnLostFocus()
		{
			
		}
		
		// 当Hierarchy视图中的任何对象发生改变时调用一次
		void OnHierarchyChange()
		{
			Repaint();
		}
		
		// 当Project视图中的资源发生改变时调用一次
		void OnProjectChange()
		{
			Repaint();
		}
		
		// "窗口面板的更新
		void OnInspectorUpdate()
		{
			// 这里开启窗口的重绘，不然窗口信息不会刷新
			Repaint();
		}
		
		//当窗口出去开启状态，并且在Hierarchy视图中选择某游戏对象时调用
		void OnSelectionChange()
		{
			Repaint();
		}
		
		// 当窗口关闭时调用
		void OnDestroy()
		{
			
		}
	}
	
} // namespace Ghost.EditorTool