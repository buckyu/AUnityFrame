using UnityEngine;
using System.Collections;

public enum EAniTrigger
{
	RunToJump = 0,
	JumpToJump = 1,
}

public class AMecanimTest : MonoBehaviour {

	AAnimator MyAnimator;
	// Use this for initialization
	void Start () {
		MyAnimator = GetComponent<AAnimator>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI()
	{
		if(Input.GetKeyDown(KeyCode.J))
		{
			MyAnimator.SetTrigger(EAniTrigger.RunToJump);
		}
	}
}
