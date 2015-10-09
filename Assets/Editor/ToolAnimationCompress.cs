using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections;

public class ToolAnimationCompress : MonoBehaviour {
	private static int number = 0;
	[MenuItem("AnimationTool/Compress")]
	static void Execute()
	{
		number = 0;
		foreach (Object o in Selection.GetFiltered(typeof(AnimationClip), SelectionMode.DeepAssets))
		{
			number++;
			ToolYamlReader.ParseAnimationFile(EditorUtility.GetAssetPath(o));
		}
		Debug.Log("一共压缩了"+number+"个动画文件!");
	}
}
