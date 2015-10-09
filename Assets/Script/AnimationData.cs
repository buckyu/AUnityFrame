using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using YamlDotNet.RepresentationModel;
using YamlDotNet.Serialization;
using YamlDotNet.Core;
public class AnimationData
{
	public AnimationClipData AnimationClip {set;get;}
}

public class AnimationClipData
{
	public string m_ObjectHideFlags {set;get;}
	public object m_PrefabParentObject {set;get;}
	public object m_PrefabInternal {set;get;}
	public string m_Name {set;get;}
	public int serializedVersion {set;get;}
	public int m_AnimationType {set;get;}
	public int m_Compressed {set;get;}
	public int m_UseHighQualityCurve{set;get;}
	public List<RotationData> m_RotationCurves {set;get;}
	public List<PositionData> m_PositionCurves {set;get;}
	public List<ScaleData> 	  m_ScaleCurves {set;get;}
	public object m_FloatCurves {set;get;}
	public object m_PPtrCurves {set;get;}
	public object m_SampleRate {set;get;}
	public int m_WrapMode {set;get;}
	public int m_Bounds {set;get;}
	public object m_ClipBindingConstant {set;get;}
	public object m_AnimationClipSettings {set;get;}
	public object m_EditorCurves {set;get;}
	public object m_EulerEditorCurves {set;get;}
	public object m_Events {set;get;}
}


/*-----------------------旋转数据结构-----------------------*/
public class RotationData
{
	public RotationCurve curve {set;get;}
	public string path {set;get;}
}

public class RotationCurve
{
	public string serializedVersion {set;get;}
	public List<QuaternionCurveData> m_Curve {set;get;}
	public int m_PreInfinity {set;get;}
	public int m_PostInfinity {set;get;}
	
}

public class QuaternionCurveData
{
	public string time {set;get;}
	public Quaternion value {set;get;}
	public Quaternion inSlope {set;get;}
	public Quaternion outSlope {set;get;}
	public string tangentMode {set;get;}
	public class Quaternion
	{
		public double x {set;get;}
		public double y {set;get;}
		public double z {set;get;}
		public double w {set;get;}
	}
}
/*-----------------------旋转数据结构-----------------------*/

/*-----------------------位置数据结构-----------------------*/
public class PositionData
{
	public PositionCurve curve {set;get;}
	public string path {set;get;}
}

public class PositionCurve
{
	public string serializedVersion {set;get;}
	public List<Vector3CurveData> m_Curve {set;get;}
	public int m_PreInfinity {set;get;}
	public int m_PostInfinity {set;get;}
	
}

public class Vector3CurveData
{
	public string time {set;get;}
	public Vector3 value {set;get;}
	public Vector3 inSlope {set;get;}
	public Vector3 outSlope {set;get;}
	public string tangentMode {set;get;}
	public class Vector3
	{
		public double x {set;get;}
		public double y {set;get;}
		public double z {set;get;}
	}
}
/*-----------------------位置数据结构-----------------------*/

/*-----------------------缩放数据结构-----------------------*/
public class ScaleData
{
	public ScaleCurve curve {set;get;}
	public string path {set;get;}
}

public class ScaleCurve
{
	public string serializedVersion {set;get;}
	public List<Vector3CurveData> m_Curve {set;get;}
	public int m_PreInfinity {set;get;}
	public int m_PostInfinity {set;get;}
	
}
/*-----------------------缩放数据结构-----------------------*/