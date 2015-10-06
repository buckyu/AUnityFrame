using UnityEngine;
using System.Collections;

public enum LerpType
{
	LineLerp,
	HermiteLerp,
}

/// <summary>
/// Hermite tool.埃尔米特插值工具.
/// </summary>
public class HermiteTool
{
	///贝塞尔曲线的算法.
	public static Vector3 GetBezierPoint(float t, Vector3 p1, Vector3 p2, Vector3 c1, Vector3 c2)
	{
		return p1*(1-t)*(1-t)*(1-t)+3*c1*t*(1-t)*(1-t)+3*c2*t*t*(1-t)+p2*t*t*t;
	}

	/// t-->[0,1]
	public static Vector3 GetHermitePoint_S(float t, YamlScaleData preData, YamlScaleData nexData, LerpType lerpType)
	{
		Vector3 scale;
		if(lerpType==LerpType.LineLerp)
		{
			scale = Vector3.Lerp(preData.value,nexData.value, t);
		}
		else
		{
			scale = CalculateHermitePoint(t,nexData.time-preData.time,preData.value,nexData.value,preData.outSlope,nexData.inSlope);
		}
		return scale;
	}

	/// t-->[0,1]
	public static Quaternion GetHermitePoint_Q(float t, YamlRotationData preData, YamlRotationData nexData, LerpType lerpType)
	{
		Quaternion rotation = new Quaternion();
		if(lerpType==LerpType.LineLerp)
		{
			rotation = Quaternion.Slerp(preData.value,nexData.value, t);
		}
		else
		{
			rotation.x = CalculateHermiteFloat(t,nexData.time-preData.time,preData.value.x,nexData.value.x,preData.outSlope.x,nexData.inSlope.x);
			rotation.y = CalculateHermiteFloat(t,nexData.time-preData.time,preData.value.y,nexData.value.y,preData.outSlope.y,nexData.inSlope.y);
			rotation.z = CalculateHermiteFloat(t,nexData.time-preData.time,preData.value.z,nexData.value.z,preData.outSlope.z,nexData.inSlope.z);
			rotation.w = CalculateHermiteFloat(t,nexData.time-preData.time,preData.value.w,nexData.value.w,preData.outSlope.w,nexData.inSlope.w);
		}
		return rotation;
	}

	/// t-->[0,1]
	public static Vector3 GetHermitePoint_T(float t, YamlPositionData preData, YamlPositionData nexData, LerpType lerpType)
	{
		Vector3 position;
		if(lerpType==LerpType.LineLerp)
		{
			position = Vector3.Lerp(preData.value,nexData.value, t);
		}
		else
		{
			position = CalculateHermitePoint(t,nexData.time-preData.time,preData.value,nexData.value,preData.outSlope,nexData.inSlope);
		}
		return position;
	}

	/// t-->[0,1]
	static Vector3 CalculateHermitePoint(float t, float dt, Vector3 preVector, Vector3 nexVector, Vector3 preOutSlope, Vector3 nexInSlope)
	{
		Vector3 m0 = preOutSlope * dt;
		Vector3 m1 = nexInSlope * dt;
		float t2 = t * t;
		float t3 = t2 * t;
		float a = 2*t3-3*t2+1;
		float b = t3-2*t2+t;
		float c = t3-t2;
		float d = -2*t3+3*t2;
		return a*preVector + b*m0+c*m1+d*nexVector;
	}

	/// t-->[0,1]
	static float CalculateHermiteFloat(float t, float dt, float preValue, float nexValue, float outSlope, float inSlope)
	{
		float m0 = outSlope * dt;
		float m1 = inSlope * dt;
		float t2 = t * t;
		float t3 = t2 * t;
		float a = 2*t3-3*t2+1;
		float b = t3-2*t2+t;
		float c = t3-t2;
		float d = -2*t3+3*t2;
		return a*preValue + b*m0+c*m1+d*nexValue;
	}
}
