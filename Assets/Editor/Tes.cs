using UnityEngine;
using System.Collections;
using UnityEditor;
public class Tes : MonoBehaviour {

	private Animator animator;
	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetKeyDown(KeyCode.A))
		{
			animator.SetBool("New Bool", true);
			animator.SetTrigger("New Trigger");
			Invoke("Test", 0.05f);
			UnityEditor.EditorApplication.isPaused = true;
		}
	}

	void Test()
	{
		animator.SetTrigger("New Trigger 0");

	}

	public void AnimationStartCallBack(string name)
	{
		Debug.Log("Start:"+name);
	}

	public void AnimationEndCallBack(string name)
	{
		Debug.Log("End:"+name);
	}

}
