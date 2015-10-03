using UnityEngine;
using System.Collections;
using System.Collections.Generic;
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
	AAnimation currentAAnimation;
	void Start () {
		foreach(AnimationClip clip in Animations)
		{
			AAnimations.Add(clip.name, new AAnimation().Init(clip.name,this));
		}
		Test();
	}

	void Test()
	{
		currentAAnimation = AAnimations[DeafultAnimationClip.name];
		currentAAnimation.Play();
	}

	// Update is called once per frame
	void Update () {
		if(currentAAnimation!=null)
			currentAAnimation.Update(Time.deltaTime);
	}

	public void SetTrigger(string value)
	{

	}

	public Transform GetJointsByName(string pathName)
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
