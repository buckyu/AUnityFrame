using System;
using UnityEngine;
using YamlDotNet.RepresentationModel;
using System.Collections.Generic;
/// <summary>
/// YAML读取辅助工具类.
/// </summary>
public class EasyYaml
{
	public EasyYaml ()
	{

	}

	///仅仅用于从MapNode里面获取key对应的value。没有递归功能.
	public static YamlNode FindMapNodeByKey(string key, YamlNode yamlNode)
	{
		if(yamlNode is YamlMappingNode)
		{
			Dictionary<YamlNode,YamlNode> mapNodeDic = ((YamlMappingNode)yamlNode).Children as Dictionary<YamlNode,YamlNode>;
			foreach(YamlNode node in mapNodeDic.Keys)
			{
				if(node.ToString().Equals(key))
				{
					return mapNodeDic[node];
				}
			}
		}
		else
		{
			Debug.Log("Just Be Suitable For MappingNode");
		}
		return null;
	}
	/// <summary>
	/// Finds the node by key.
	/// </summary>
	/// <returns>The node by key.</returns>
	/// <param name="key">Key.</param>
	/// <param name="yamlNode">Yaml node.</param>
//	public static YamlNode FindNodeByKey(string key, YamlNode yamlNode)
//	{
//		if(yamlNode is YamlScalarNode&&yamlNode.ToString().Equals(key))
//		{
//			Debug.Log(key+":"+yamlNode);
//			return yamlNode;
//		}
//		YamlNode resultYamlNode = null;
//		if(yamlNode is YamlMappingNode)
//		{
//			Dictionary<YamlNode,YamlNode> mapNodeDic = ((YamlMappingNode)yamlNode).Children as Dictionary<YamlNode,YamlNode>;
//			foreach(YamlNode node in mapNodeDic.Keys)
//			{
//				resultYamlNode = FindNodeByKey(key,node);
//				if(resultYamlNode!=null)
//				{
//					resultYamlNode = mapNodeDic[node];
//					break;
//				}
//				resultYamlNode = FindNodeByKey(key,mapNodeDic[node]);
//			}
//		}
//		else if(yamlNode is YamlSequenceNode)
//		{
//			List<YamlNode> mapNodeList = ((YamlSequenceNode)yamlNode).Children as List<YamlNode>;
//			foreach(YamlNode node in mapNodeList)
//			{
//				resultYamlNode = FindNodeByKey(key,node);
//				if(resultYamlNode!=null)
//				{
////					resultYamlNode = mapNodeList[node];
//					break;
//				}
////				resultYamlNode = FindNodeByKey(key,mapNodeList[node]);
//			}
//		}
//		return resultYamlNode;
//	}
}

