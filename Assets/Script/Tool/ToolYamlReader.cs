using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using UnityEngine;
//public AnimationData
public class ToolYamlReader {
	private const string Unity_Head = @"%YAML 1.1
%TAG !u! tag:unity3d.com,2011:
--- !u!74 &7400000";
	private static string filePath;
    public static void ParseAnimationFile(string yamlPath)
    {
		filePath = yamlPath;
        FileStream fileStream = new FileStream(yamlPath,FileMode.Open);
		StreamReader streamReader = new StreamReader(fileStream);
		Deserializer deserialize = new Deserializer(null,null,true);
		AnimationData ad = deserialize.Deserialize<AnimationData>(new StringReader(streamReader.ReadToEnd()));
		streamReader.Close();
		fileStream.Close();
//		Debug.Log(ad.AnimationClip.m_RotationCurves[0].curve.m_Curve[0].value);
		Serializer serializer = new Serializer(SerializationOptions.EmitDefaults);
		//删掉缩放通道.
//		ad.AnimationClip.m_ScaleCurves.Clear();
		//减少位置浮点数精度.
		foreach(PositionData pd in ad.AnimationClip.m_PositionCurves)
		{
			foreach(Vector3CurveData v3d in pd.curve.m_Curve)
			{
				CompressVector3(v3d.value);
				CompressVector3(v3d.inSlope);
				CompressVector3(v3d.outSlope);
            }
        }
		//减少缩放浮点数精度.
		foreach(ScaleData pd in ad.AnimationClip.m_ScaleCurves)
		{
			foreach(Vector3CurveData v3d in pd.curve.m_Curve)
			{
				CompressVector3(v3d.value);
				CompressVector3(v3d.inSlope);
                CompressVector3(v3d.outSlope);
            }
        }
        //减少旋转浮点数精度.
		foreach(RotationData pd in ad.AnimationClip.m_RotationCurves)
		{
			foreach(QuaternionCurveData v3d in pd.curve.m_Curve)
			{
				CompressQuaternion(v3d.value);
				CompressQuaternion(v3d.inSlope);
				CompressQuaternion(v3d.outSlope);
            }
        }
		StreamWriter sw = new StreamWriter(filePath);
        sw.Write(Unity_Head+"\n");
		serializer.Serialize(sw,ad);
		sw.Flush();
		sw.Close();
	}

	static Vector3CurveData.Vector3 CompressVector3(Vector3CurveData.Vector3 v, int num = 3)
	{
		v.x = double.Parse(v.x.ToString("0.###"));
		v.y = double.Parse(v.y.ToString("0.###"));
		v.z = double.Parse(v.z.ToString("0.###"));
		return v;
	}

	static QuaternionCurveData.Quaternion CompressQuaternion(QuaternionCurveData.Quaternion q, int num = 3)
	{
		q.x = double.Parse(q.x.ToString("0.###"));
     	q.y = double.Parse(q.y.ToString("0.###"));
		q.z = double.Parse(q.z.ToString("0.###"));
		q.w = double.Parse(q.w.ToString("0.###"));
		return q;
    }
}
