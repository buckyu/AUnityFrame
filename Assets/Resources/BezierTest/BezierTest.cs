using UnityEngine;
using System.Collections;

public class BezierTest : MonoBehaviour {
	public GameObject yelloLine;
	public LineRenderer yellowLineRenderer;

	void Start () {
		yellowLineRenderer = GetComponent<LineRenderer>();
		yellowLineRenderer.SetVertexCount(100);
	}
	
	void Update () 
	{
		for(int i=1;i<=100;i++)
		{
			Vector3 vec = BezierTool.Get3DPoint(i*0.01f,new Vector3(0.0122665102f,0,0.8491714f),new Vector3(-0.00136441702f,0,0.734538555f),
			                                    new Vector3(-0.408927709f,-0.000241587477f,-3.4390018f), new Vector3(-0.246247232f,-0.761205673f,-1.37065136f));
			yellowLineRenderer.SetPosition(i-1, vec);
		}
	}
}
