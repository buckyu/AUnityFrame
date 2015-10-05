using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Transition
{

}

/// <summary>
/// 1.动画播放.
/// 2.动画融合.
/// 3.混合树.
/// 3.分层动画.
/// 4.动画事件.
/// </summary>
public class AAnimator : MonoBehaviour {
	public AnimationClip[] Animations;
	public AnimationClip DeafultAnimationClip;
	public LerpType LerpType;
	private Dictionary<string, AAnimation> AAnimations = new Dictionary<string, AAnimation>();
	private Dictionary<string, Transform>  ABones	   = new Dictionary<string, Transform>();
	string jointName;
	AAnimation currentAAnimation;
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
				currentAAnimation.UpdateSQT(jointName);
//				currentJointTransform.localPosition = Vector3.Lerp(currentAAnimation.CurPosition,targetAAnimation.CurPosition,transitionWight);
//				currentJointTransform.localRotation = Quaternion.Slerp(currentAAnimation.CurQuaternion,targetAAnimation.CurQuaternion,transitionWight);
//				currentJointTransform.localScale    = Vector3.Lerp(currentAAnimation.CurScale,targetAAnimation.CurScale,transitionWight);
			}
		}
	}

//	SQT GetAAnimationSQT(AAnimation aAnimation)
//	{
//
//	}

	public void SetTrigger(EAniTrigger value)
	{

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
