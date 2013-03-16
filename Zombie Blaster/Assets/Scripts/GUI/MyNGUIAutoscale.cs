using UnityEngine;
using System.Collections;

public class MyNGUIAutoscale : MonoBehaviour {
	
	public Rect rect01 = new Rect(0.5f,0.5f,1f,1f);
	
	void Start()
	{
		float width = Screen.width;
		float height = Screen.height;
		rect01.y = 1-rect01.y;
		float x = rect01.x-0.5f*rect01.width, y = -rect01.y+0.5f*rect01.height;
		transform.localPosition = new Vector3(x*width,y*Screen.height,transform.localPosition.z);
		transform.localScale = new Vector3(rect01.width*width,rect01.height*height,transform.localScale.z);
	}
}
