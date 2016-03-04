using UnityEngine;
using System.Collections;

public enum EAniTrigger
{
	RunToJump = 0,
	JumpToJump = 1,
}

public class AMecanimTest : MonoBehaviour {

	float alpha = 1f;
	float speed = 1f;
	AAnimator MyAnimator;
	// Use this for initialization
	void Start () {
		Shader.SetGlobalFloat("HD_DISTANCE", 450f);
		Shader.SetGlobalVector("HD_CURVE_OFFSET", new Vector4(-100f, -50f, -100f, 0f));
		MyAnimator = GetComponent<AAnimator>();
	}

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.J))
		{
			MyAnimator.SetTrigger(EAniTrigger.RunToJump);
		}
		if(Input.GetKeyDown(KeyCode.UpArrow))
		{
			alpha += 0.1f;
			alpha = alpha>=1?1:alpha;
			MyAnimator.SetAlpha(alpha);
		}
		if(Input.GetKeyDown(KeyCode.DownArrow))
		{
			alpha -= 0.1f;
			alpha = alpha<=0?0:alpha;
			MyAnimator.SetAlpha(alpha);
		}
		if(Input.GetKeyDown(KeyCode.LeftArrow))
		{
			speed -= 0.1f;
			speed = speed<=0?0:speed;
			MyAnimator.Speed = speed;
		}
		if(Input.GetKeyDown(KeyCode.RightArrow))
		{
			speed += 0.1f;
			speed = speed>=1?1:speed;
			MyAnimator.Speed = speed;
		}
		if(Input.GetKeyDown(KeyCode.S))
		{
			MyAnimator.drawJoints = !MyAnimator.drawJoints;
		}
	}
}
