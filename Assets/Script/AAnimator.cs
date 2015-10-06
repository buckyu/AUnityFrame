using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 1.动画播放.(done 2015/10/03)
/// 2.动画融合.(done 2015/10/06)
/// 3.状态机解析和实现.
/// 4.混合树.
/// 5.分层动画.
/// 6.动画事件.
/// </summary>
public class AAnimator : MonoBehaviour {
	public AnimationClip[] Animations;
	public AnimationClip DeafultAnimationClip;
	public LerpType LerpType;
	private Dictionary<string, AAnimation> AAnimations = new Dictionary<string, AAnimation>();
	private Dictionary<string, Transform>  ABones	   = new Dictionary<string, Transform>();
	string jointName;
	AAnimation currentAAnimation;
	SQT currentSQT = new SQT();
	SQT targetSQT = new SQT();
	SQT reallySQT = new SQT();
	AAnimation targetAAnimation;
	Transform currentJointTransform;
	public AnimationClip   sourceClip;
	public AnimationClip   targetClip;
	public EAniTrigger trigger;

	private float transitionWight = 0;
	private List<string> JointsList = new List<string>();
	void Start () {
		foreach(AnimationClip clip in Animations)
		{
			AAnimations.Add(clip.name, new AAnimation().Init(clip.name,this));
		}
		JointsList = ToolTraversal.Traversal(transform.Find("000"),"");
		foreach(string name in JointsList)
		{
			GetJointsByName(name);
		}
		Test();
	}

	void Test()
	{
		currentAAnimation = AAnimations[DeafultAnimationClip.name];
	}

	// Update is called once per frame
	void Update () {
		if(currentAAnimation!=null)
		{
			currentAAnimation.Update(Time.deltaTime);
			if(targetAAnimation!=null)
			{
				targetAAnimation.Update(Time.deltaTime);
				transitionWight += Time.deltaTime*5;
				//过渡结束.
				if(transitionWight>=1)
				{
					transitionWight = 0;
					currentAAnimation = targetAAnimation;
					targetAAnimation = null;
					reallySQT = targetSQT;
				}
			}
			int JointsCount = JointsList.Count;
			for(int i=0;i<JointsCount;i++)
			{
				jointName = JointsList[i];
				currentJointTransform = GetJointsByName(jointName);
				if(currentJointTransform==null)
				{
					continue;
				}
				currentSQT = currentAAnimation.UpdateSQT(jointName);
				if(targetAAnimation!=null)
				{
					targetSQT = targetAAnimation.UpdateSQT(jointName);
				}
				else
				{
					targetSQT.SetAllWeight(0);
				}
				reallySQT = ToolSQT.Lerp(currentSQT,targetSQT,transitionWight);
				if(reallySQT.Position_Weight!=0)
					currentJointTransform.localPosition = reallySQT.Position;
				if(reallySQT.Rotation_Weight!=0)
					currentJointTransform.localRotation = reallySQT.Rotation;
				if(reallySQT.Scale_Weight!=0)
					currentJointTransform.localScale    = reallySQT.Scale;
			}
		}
	}

	public void SetTrigger(EAniTrigger value)
	{
		targetAAnimation = AAnimations[targetClip.name];
		transitionWight = 0;
	}


	private Transform GetJointsByName(string pathName)
	{
		if(ABones.ContainsKey(pathName))
		{
			return ABones[pathName];
		}
		Transform result = transform.Find(pathName);
		ABones.Add(pathName,result);
		return result;
	}
}
