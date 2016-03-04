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
	public bool drawJoints = false;
	private float transitionWight = 0;
	//这两个字段暴露出来给编辑器用.
	public static List<string> JointsList = new List<string>();
	public static Dictionary<string,bool> JointMaskDic = new Dictionary<string, bool>();
	//这两个字段暴露出来给编辑器用.
	void Start () {
		Speed = 1;
		foreach(AnimationClip clip in Animations)
		{
			AAnimations.Add(clip.name, new AAnimation().Init(clip.name,this));
		}
		JointsList = ToolTraversal.Traversal(transform.Find("000"),"");
		foreach(string name in JointsList)
		{
			GetJointsByName(name);
//			Debug.Log (name);
		}

		Test();
	}

	public Transform jointRes;
	void Test()
	{
		currentAAnimation = AAnimations[DeafultAnimationClip.name];
	}

	// Update is called once per frame
	void Update () {
		if(currentAAnimation!=null)
		{
			//更新动画的局部时间.
			currentAAnimation.Update(Time.deltaTime*Speed);
			if(targetAAnimation!=null)
			{
				targetAAnimation.Update(Time.deltaTime*Speed);
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
			//更新动画数据.
			int JointsCount = JointsList.Count;
			for(int i=0;i<JointsCount;i++)
			{
				jointName = JointsList[i];
				//如果设置了骨骼遮罩，则直接返回.
				if(JointMaskDic.ContainsKey(jointName)&&JointMaskDic[jointName])
				{
					continue;
				}
				currentJointTransform = GetJointsByName(jointName);
				if(currentJointTransform==null)
				{
					continue;
				}
				Transform joint = DrawJoints(jointName);
				if(joint!=null)
				{
					joint.parent = currentJointTransform;
					joint.localScale = new Vector3(0.1f,0.1f,0.1f);
					joint.localPosition = Vector3.zero;
					joint.localRotation = Quaternion.identity;
					joint.gameObject.SetActive(drawJoints);
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
	/*--------------------------------------------演示 begin--------------------------------------------*/
	Dictionary<string, Transform> JointsDic = new Dictionary<string, Transform>();
	List<string> ignorJoint = new List<string>(){"000","root","jiaoxia","center","toubu","zuoshou","youshou","beibu","zhongxin"};
	private Transform DrawJoints(string name)
	{
		name = GetLastName(name);
		if(ignorJoint.Contains(name))
		{
			return null;
		}
		if(JointsDic.ContainsKey(name))
		{
			return JointsDic[name];
		}
		Transform joint = Instantiate(jointRes) as Transform;
		joint.name = name+"_joint";
		JointsDic.Add(name,joint);
		return joint;
	}

	string GetLastName(string value)
	{
		string[] temp = value.Split('/');
		return temp[temp.Length-1];
	}

	SkinnedMeshRenderer skin;
	public void SetAlpha(float alpha)
	{
		if(skin==null)
			skin = transform.GetComponentInChildren<SkinnedMeshRenderer>();
		skin.material.SetFloat("_Alpha",alpha);
	}
	/*--------------------------------------------演示 end--------------------------------------------*/
	

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

	public float Speed{get;set;}
	public bool ShowJoint{set;get;}
}
