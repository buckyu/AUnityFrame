using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ToolTraversal 
{
	static List<string> pathList = new List<string>();
	public static List<string> Traversal(Transform transform, string name)
	{
		pathList.Clear();
		DoTraversal(transform,name);
		return pathList;
	}

	static void DoTraversal(Transform transform, string name)
	{
		string pathName = string.Concat(name,transform.name);
		pathList.Add(pathName);
		foreach(Transform trans in transform)
		{
			DoTraversal(trans,pathName+"/");
		}
	}
}
