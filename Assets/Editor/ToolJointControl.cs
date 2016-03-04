using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
public class ToolJointControl : EditorWindow {
	
	private Vector2 scrollPos;
	private static List<bool> foldOut = new List<bool>();
	Dictionary<string,bool> jointsDic = new Dictionary<string, bool>();
	public static List<string> JointsList;
	int TotalJoint = 35;
	#if UNITY_EDITOR
	[MenuItem("JointControl/ShowAllJoints")] // #t
	#endif
	static void init()
	{
		if(AAnimator.JointMaskDic==null)
		{
			return;
		}
		EditorWindow window = EditorWindow.GetWindow(typeof(ToolJointControl));
		window.minSize = new Vector2(400,800);
		AAnimator.JointMaskDic.Clear();
		JointsList = AAnimator.JointsList;
	}
		
	void OnGUI()
	{
		EditorGUILayout.Space();
		EditorGUILayout.LabelField("骨骼列表: ", EditorStyles.boldLabel);
		EditorGUILayout.Space();
		showAllTask();
//		EditorGUILayout.BeginHorizontal();
		if(GUILayout.Button("全选"))
		{
			SetAllStatue(true);
		}
		if(GUILayout.Button("全不选"))
		{
			SetAllStatue(false);
		}
//		EditorGUILayout.EndHorizontal();
	}

	void SetAllStatue(bool value)
	{
		if(JointsList!=null)
		{
			for(int i=0;i<JointsList.Count;i++)
			{
				AAnimator.JointMaskDic[JointsList[i]] = value;
			}
		}
	}

	void showAllTask()
	{
		if(JointsList==null)
		{
			return;
		}
//		scrollPos = EditorGUILayout.BeginScrollView(scrollPos,GUILayout.Width(400),GUILayout.Height(800));
		for(int j=0;j<JointsList.Count;j++)
		{
			if(!AAnimator.JointMaskDic.ContainsKey(JointsList[j]))
			{
				AAnimator.JointMaskDic.Add(JointsList[j],false);
			}
			AAnimator.JointMaskDic[JointsList[j]] = EditorGUILayout.ToggleLeft(GetLastName(JointsList[j]),AAnimator.JointMaskDic[JointsList[j]]);
		}
//		EditorGUILayout.EndScrollView();
	}

	static string GetLastName(string value)
	{
		string[] temp = value.Split('/');
		return temp[temp.Length-1];
	}
}
