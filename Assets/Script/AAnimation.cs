using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 先用局部时间来维护动作播放进度.
/// </summary>
public class AAnimation : AAnimationBase 
{
	private AAnimator aAnimator;
	bool playing = false;
	/// 局部时间线.
	float localCurrentTime;

	Vector3 curPosition;
	Vector3 curScale;
	Quaternion curQuaternion;

	SQTData preFrameData;
	SQTData nexFrameData;
	string currentJointName;
	float frameLerp;

	public AAnimation Init(string name, AAnimator animator)
	{
		this.aAnimator = animator;
		this.name = name;
		ReadAnimationFile(name);
		return this;
	}
	
	public void Play()
	{
		playing = true;
		localCurrentTime = startTime;
	}

	public void Stop()
	{
		playing = false;
	}

	public void Update(float dealtTime)
	{
		if(playing)
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
					Stop();
				}
			}
		}
	}
	
	public void UpdateSQT(string jointName)
	{
			currentJointName = jointName;
			UpdatePosition();
			UpdateRotation();
			UpdateScale();
	}

	void UpdatePosition()
	{
		//更新每一根关节的位置.
		List<SQTData> pList = PositionsList[currentJointName];
		if(FindKeyFrames(pList))
		{
			curPosition = BezierTool.GetBezierPoint_T(frameLerp,(PositionData)preFrameData,(PositionData)nexFrameData,aAnimator.LerpType);
		}
	}
	
	void UpdateRotation()
	{
		//更新每一根关节的旋转信息.
		List<SQTData> pList = RotationList[currentJointName];
		if(FindKeyFrames(pList))
		{
			curQuaternion = BezierTool.GetBezierPoint_Q(frameLerp,(RotationData)preFrameData,(RotationData)nexFrameData,aAnimator.LerpType);
		}
	}

	void UpdateScale()
	{
		//更新每一根关节的缩放信息.
		List<SQTData> pList = ScaleList[currentJointName];
		if(FindKeyFrames(pList))
		{
			curScale = BezierTool.GetBezierPoint_S(frameLerp,(ScaleData)preFrameData,(ScaleData)nexFrameData,aAnimator.LerpType);
		}
	}

	bool FindKeyFrames(List<SQTData> sqtList)
	{
		//找到前后两个关键帧.
		int listCount = sqtList.Count;
		for(int j=0;j<listCount;j++)
		{
			if(localCurrentTime<=sqtList[j].time)
			{
				nexFrameData = sqtList[j];
				preFrameData = sqtList[j-1];
				if(nexFrameData.time-localCurrentTime==0)
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
	public Vector3 CurPosition{get{return curPosition;}}
	public Vector3 CurScale{get{return curScale;}}
	public Quaternion CurQuaternion{get{return curQuaternion;}}
}
