using UnityEngine;
using System.Collections;

public class TestCube : MonoBehaviour {
	public GameObject Sphere;
	bool test = true;
	// Use this for initialization
	void Start () {
//		string s = "";
//		for(int i=0;i<22314;i++)
//		{
//			s = string.Concat(s,"0,");
//		}
//		Debug.Log(s);
		Debug.Log(collider.gameObject.CompareTag("a"));
		Debug.Log(collider.tag);
	}
	
	// Update is called once per frame
	void Update () {
//		Debug.Log("Cube: activeSelf:"+gameObject.activeSelf+" activeInHierarchy:"+gameObject.activeInHierarchy);
//		Debug.Log("Sphere: activeSelf:"+Sphere.activeSelf+" activeInHierarchy:"+Sphere.activeInHierarchy);
//		if(Input.GetKeyDown(KeyCode.T))
//		{
//			test = !test;
//			Sphere.SetActive(test);
//		}

	}
}
