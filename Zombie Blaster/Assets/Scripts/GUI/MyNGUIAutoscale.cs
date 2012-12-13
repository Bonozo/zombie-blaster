using UnityEngine;
using System.Collections;

public class MyNGUIAutoscale : MonoBehaviour {
	
	public Rect rect01 = new Rect(0.5f,0.5f,1f,1f);
	
	void Start()
	{
		rect01.y = 1-rect01.y;
		float x = rect01.x-0.5f*rect01.width, y = -rect01.y+0.5f*rect01.height;
		transform.localPosition = new Vector3(x*Screen.width,y*Screen.height,transform.localPosition.z);
		transform.localScale = new Vector3(rect01.width*Screen.width,rect01.height*Screen.height,transform.localScale.z);

	}
}
