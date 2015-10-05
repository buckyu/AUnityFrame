using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct SQT
{
	public float	  Weight;
	public Vector3 	  Scale;
	public Vector3    Position;
	public Quaternion Rotation;
}

/// <summary>
/// 先用局部时间来维护动作播放进度.
/// </summary>
public class AAnimation : AAnimationBase 
{
	private AAnimator aAnimator;
	bool playing = false;
	/// 局部时间线.
	float localCurrentTime;
	SQT CurentSQT = new SQT();

	YamlSQTData preFrameData;
	YamlSQTData nexFrameData;
	string currentJointName;
	float frameLerp;

	public AAnimation Init(string name, AAnimator animator)
	{
		this.aAnimator = animator;
		this.name = name;
		ReadAnimationFile(name);
		localCurrentTime = startTime;
		return this;
	}

	public void Update(float dealtTime)
	{
		localCurrentTime += dealtTime;
		if(localCurrentTime>=stopTime)
		{
			localCurrentTime = stopTime;
			if(loop)
			{
				localCurrentTime = startTime;
			}
			else
			{
				//
			}
		}
	}
	
	public SQT UpdateSQT(string jointName)
	{
		currentJointName = jointName;
		if(PositionsList.ContainsKey(currentJointName))
		{
			CurentSQT.Weight = 1;
			UpdatePosition();
			UpdateRotation();
			UpdateScale();
		}
		else //如果找不到该关节，则把权重设为0，不再参与接下来的计算(除了容错以外还可用于遮罩功能).
		{
			CurentSQT.Weight = 0;
		}
		return CurentSQT;
	}

	void UpdatePosition()
	{
		//更新每一根关节的位置.
		List<YamlSQTData> pList = PositionsList[currentJointName];
		if(FindKeyFrames(pList))
		{
			CurentSQT.Position = BezierTool.GetBezierPoint_T(frameLerp,(YamlPositionData)preFrameData,(YamlPositionData)nexFrameData,aAnimator.LerpType);
		}
	}
	
	void UpdateRotation()
	{
		//更新每一根关节的旋转信息.
		List<YamlSQTData> pList = RotationList[currentJointName];
		if(FindKeyFrames(pList))
		{
			CurentSQT.Rotation = BezierTool.GetBezierPoint_Q(frameLerp,(YamlRotationData)preFrameData,(YamlRotationData)nexFrameData,aAnimator.LerpType);
		}
	}

	void UpdateScale()
	{
		//更新每一根关节的缩放信息.
		List<YamlSQTData> pList = ScaleList[currentJointName];
		if(FindKeyFrames(pList))
		{
			CurentSQT.Scale = BezierTool.GetBezierPoint_S(frameLerp,(YamlScaleData)preFrameData,(YamlScaleData)nexFrameData,aAnimator.LerpType);
		}
	}

	///找到前后两个关键帧的信息，如果找不到，则保持原有动作.
	bool FindKeyFrames(List<YamlSQTData> sqtList)
	{
		int listCount = sqtList.Count;
		for(int j=0;j<listCount;j++)
		{
			if(localCurrentTime<=sqtList[j].time)
			{
				nexFrameData = sqtList[j];
				if(j==0)
				{
					preFrameData = nexFrameData;
				}
				else
				{
					preFrameData = sqtList[j-1];
				}
				if(nexFrameData.time-preFrameData.time==0)
				{
					frameLerp = 1;
				}
				else
				{
					frameLerp = (localCurrentTime-preFrameData.time)/(nexFrameData.time-preFrameData.time);
				}
				return true;
			}
		}
		return false;
	}
}
