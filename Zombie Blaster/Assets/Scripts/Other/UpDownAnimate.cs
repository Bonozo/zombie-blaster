using UnityEngine;
using System.Collections;

public class UpDownAnimate : MonoBehaviour {
	
	public float ymin=-1f,ymax=0.25f,speed=0.5f;
	private Vector3 pos;
	private bool up;
	
	// Use this for initialization
	void Start () {
		pos = transform.localPosition;
		up=true;
		
		pos.y = ymin;
		transform.localPosition = pos;
	}
	
	// Update is called once per frame
	void Update () {
		float d = 0.0167f*speed;
		pos.y += up?d:-d;
		if(up && pos.y>=ymax) up=false;
		if(!up && pos.y<=ymin) up=true;
		transform.localPosition = pos;
	}
	
	public void ResetDown()
	{
		up=true;
		pos.y = ymin;
		transform.localPosition = pos;		
	}
}
