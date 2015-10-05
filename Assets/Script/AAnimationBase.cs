using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using YamlDotNet.RepresentationModel;
using System;
public class YamlSQTData
{
	public float time;
}

public class YamlRotationData : YamlSQTData
{
	public Quaternion value;
	public Quaternion inSlope;
	public Quaternion outSlope;
	public float tangentMode;
}

public class YamlPositionData : YamlSQTData
{
	public Vector3 value;
	public Vector3 inSlope;
	public Vector3 outSlope;
	public float tangentMode;
}

public class YamlScaleData : YamlSQTData
{
	public Vector3 value;
	public Vector3 inSlope;
	public Vector3 outSlope;
	public float tangentMode;
}

/// <summary>
/// 动画文件读取.
/// </summary>
public abstract class AAnimationBase {
	private const string RootPath 	  =  "Assets/Resources/Mickey/";
	private const string KEY_ROOT 	  =  "AnimationClip";
	private const string KEY_PATH	  =  "path";

	private const string KEY_ROTATION =  "m_RotationCurves";
	private const string KEY_POSITION =  "m_PositionCurves";
	private const string KEY_SCALE 	  =  "m_ScaleCurves";
	private const string KEY_CURVE	  =  "curve";
	private const string KEY_M_CURVE  =  "m_Curve";
	private const string KEY_SETTING  =  "m_AnimationClipSettings";
	private const string KEY_STARTTIME=  "m_StartTime";
	private const string KEY_STOPTIME =  "m_StopTime";
	private const string KEY_LOOPTIME =  "m_LoopTime";
	//以关节名为键，存储了所有关节的位置，旋转和缩放信息.
	private Dictionary<string, List<YamlSQTData>> positionInfo = new Dictionary<string, List<YamlSQTData>>();
	private Dictionary<string, List<YamlSQTData>> rotationInfo = new Dictionary<string, List<YamlSQTData>>();
	private Dictionary<string, List<YamlSQTData>> scaleInfo    = new Dictionary<string, List<YamlSQTData>>();
	YamlNode animationNode;
	public string name = "";
	protected float startTime;
	protected float stopTime;
	protected bool  loop;
	protected void ReadAnimationFile(string filePath)
	{	
		filePath = string.Concat(RootPath,filePath,".anim");
		FileStream fileStream = new FileStream(filePath,FileMode.Open);
		StreamReader streamReader = new StreamReader(fileStream);
		ParseStreamToYaml(streamReader);
	}

	/// <summary>
	/// 解析YAML动画文件
	/// </summary>
	/// <param name="sr">Sr.</param>
	void ParseStreamToYaml(StreamReader sr)
	{
		YamlStream yaml = new YamlStream();
		yaml.Load(sr);
		YamlMappingNode map = (YamlMappingNode)yaml.Documents[0].RootNode;
		animationNode = EasyYaml.FindMapNodeByKey("AnimationClip",map);
		//获取关节的旋转信息.
		ParseSQTInfo(KEY_ROTATION, ParseRotationDataToList);
		//获取关节的位置信息.
		ParseSQTInfo(KEY_POSITION, ParsePositionDataToList);
		//获取关节的缩放信息.
		ParseSQTInfo(KEY_SCALE,ParseScaleInfoToList);
		//获取动画长度和循环状态.
		YamlMappingNode settingMapNode = EasyYaml.FindMapNodeByKey(KEY_SETTING,animationNode) as YamlMappingNode;
		startTime = float.Parse(EasyYaml.FindMapNodeByKey(KEY_STARTTIME,settingMapNode).ToString());
		stopTime  = float.Parse(EasyYaml.FindMapNodeByKey(KEY_STOPTIME,settingMapNode).ToString());
		loop  = EasyYaml.FindMapNodeByKey(KEY_LOOPTIME,settingMapNode).ToString()=="0"?false:true;
	}

	void ParseSQTInfo(string sqtKey, Action<YamlSequenceNode, string> parseCallBack)
	{
		YamlSequenceNode rootNode = EasyYaml.FindMapNodeByKey(sqtKey,animationNode) as YamlSequenceNode;
		foreach(YamlNode mapNode in rootNode.Children)
		{
			YamlMappingNode mNode = EasyYaml.FindMapNodeByKey(KEY_CURVE,mapNode) as YamlMappingNode;
			string key = EasyYaml.FindMapNodeByKey(KEY_PATH,mapNode).ToString();
			YamlSequenceNode value = EasyYaml.FindMapNodeByKey(KEY_M_CURVE,mNode) as YamlSequenceNode;
			parseCallBack(value, key);
		}
	}

	void ParsePositionDataToList(YamlSequenceNode node, string key)
	{
		List<YamlSQTData> list = new List<YamlSQTData>();
		IList<YamlNode> nodeList = node.Children;
		foreach(YamlMappingNode valueNode in nodeList)
		{
			YamlPositionData postionData = new YamlPositionData();
			postionData.time = float.Parse(EasyYaml.FindMapNodeByKey("time",valueNode).ToString());
			postionData.value = GetVector3("value",valueNode);
			postionData.inSlope = GetVector3("inSlope",valueNode);
			postionData.outSlope = GetVector3("outSlope",valueNode);
			postionData.tangentMode = float.Parse(EasyYaml.FindMapNodeByKey("tangentMode",valueNode).ToString());
			list.Add(postionData);
		}
		positionInfo.Add(key,list);
	}

	Vector3 GetVector3(string nodeKey, YamlMappingNode valueNode)
	{
		YamlMappingNode vNode = EasyYaml.FindMapNodeByKey(nodeKey, valueNode) as YamlMappingNode;
		Vector3 vector3 = new Vector3();
		vector3.x = float.Parse(EasyYaml.FindMapNodeByKey("x",vNode).ToString());
		vector3.y = float.Parse(EasyYaml.FindMapNodeByKey("y",vNode).ToString());
		vector3.z = float.Parse(EasyYaml.FindMapNodeByKey("z",vNode).ToString());
		return vector3;
	}

	void ParseRotationDataToList(YamlSequenceNode node, string key)
	{
		IList<YamlNode> nodeList = node.Children;
		List<YamlSQTData> list = new List<YamlSQTData>();
		foreach(YamlMappingNode valueNode in nodeList)
		{
			YamlRotationData rotationData = new YamlRotationData();
			rotationData.time = float.Parse(EasyYaml.FindMapNodeByKey("time",valueNode).ToString());
			rotationData.value = GetQuaternion("value", valueNode);
			rotationData.inSlope = GetQuaternion("inSlope", valueNode);
			rotationData.outSlope = GetQuaternion("outSlope", valueNode);
			rotationData.tangentMode = float.Parse(EasyYaml.FindMapNodeByKey("tangentMode",valueNode).ToString());
			list.Add(rotationData);
		}
		rotationInfo.Add(key, list);
	}

	Quaternion GetQuaternion(string nodeKey,YamlMappingNode valueNode)
	{
		YamlMappingNode vNode = EasyYaml.FindMapNodeByKey(nodeKey,valueNode) as YamlMappingNode;
		Quaternion quaternion = new Quaternion();
		quaternion.x = float.Parse(EasyYaml.FindMapNodeByKey("x",vNode).ToString());
		quaternion.y = float.Parse(EasyYaml.FindMapNodeByKey("y",vNode).ToString());
		quaternion.z = float.Parse(EasyYaml.FindMapNodeByKey("z",vNode).ToString());
		quaternion.w = float.Parse(EasyYaml.FindMapNodeByKey("w",vNode).ToString());
		return quaternion;
	}

	void ParseScaleInfoToList(YamlSequenceNode node, string key)
	{
		IList<YamlNode> nodeList = node.Children;
		List<YamlSQTData> list = new List<YamlSQTData>();
		foreach(YamlMappingNode valueNode in nodeList)
		{
			YamlScaleData scaleData = new YamlScaleData();
			scaleData.time = float.Parse(EasyYaml.FindMapNodeByKey("time",valueNode).ToString());
			scaleData.value = GetVector3("value",valueNode);
			scaleData.inSlope = GetVector3("inSlope",valueNode);
			scaleData.outSlope = GetVector3("outSlope",valueNode);
			scaleData.tangentMode = float.Parse(EasyYaml.FindMapNodeByKey("tangentMode",valueNode).ToString());
			list.Add(scaleData);
		}
		scaleInfo.Add(key, list);
	}

	protected Dictionary<string, List<YamlSQTData>> PositionsList{get{return positionInfo;}}
	protected Dictionary<string, List<YamlSQTData>> RotationList{get{return rotationInfo;}}
	protected Dictionary<string, List<YamlSQTData>>    ScaleList{get{return scaleInfo;}}
}
