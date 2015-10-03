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
				RenderAnimation();
				if(loop)
				{
					localCurrentTime = startTime;
				}
				else
				{
					Stop();
				}
			}
			else
			{
				RenderAnimation();
			}
		}
	}

	SQTData preFrameData;
	SQTData nexFrameData;
	Transform currentJointTransform;
	string currentJointName;
	float frameLerp;
	
	void RenderAnimation()
	{
		int JointsCount = JointsList.Count;
		for(int i=0;i<JointsCount;i++)
		{
			currentJointName = JointsList[i];
			currentJointTransform = aAnimator.GetJointsByName(currentJointName);
			if(currentJointTransform==null)
			{
				continue;
			}
			UpdatePosition();
			UpdateRotation();
			UpdateScale();
		}
	}

	void UpdatePosition()
	{
		//更新每一根关节的位置.
		List<SQTData> pList = PositionsList[currentJointName];
		if(FindKeyFrames(pList))
		{
			currentJointTransform.localPosition = BezierTool.GetBezierPoint_T(frameLerp,(PositionData)preFrameData,(PositionData)nexFrameData,aAnimator.LerpType);
		}
	}
	
	void UpdateRotation()
	{
		//更新每一根关节的旋转信息.
		List<SQTData> pList = RotationList[currentJointName];
		if(FindKeyFrames(pList))
		{
			currentJointTransform.localRotation = BezierTool.GetBezierPoint_Q(frameLerp,(RotationData)preFrameData,(RotationData)nexFrameData,aAnimator.LerpType);
		}
	}

	void UpdateScale()
	{
		//更新每一根关节的缩放信息.
		List<SQTData> pList = ScaleList[currentJointName];
		if(FindKeyFrames(pList))
		{
			currentJointTransform.localScale = BezierTool.GetBezierPoint_S(frameLerp,(ScaleData)preFrameData,(ScaleData)nexFrameData,aAnimator.LerpType);
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
}
