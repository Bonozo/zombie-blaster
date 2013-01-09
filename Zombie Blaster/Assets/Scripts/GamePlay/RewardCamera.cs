using UnityEngine;
using System.Collections;

public class RewardCamera : MonoBehaviour {

	// Use this for initialization
	void Start () {
		/*
		 * pos 110,-70,0
		 * scale 100,100
		 * */
		Rect rect = new Rect(Screen.width*0.5f+55,Screen.height*0.5f-135f,100f,100f);
		rect.x /= Screen.width;
		rect.y /= Screen.height;
		rect.width /= Screen.width;
		rect.height /= Screen.height;
		camera.rect = rect;
	}
}
