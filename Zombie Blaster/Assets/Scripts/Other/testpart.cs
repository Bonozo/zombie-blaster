using UnityEngine;
using System.Collections;

public class testpart : MonoBehaviour {

	float  scrollspeed = .02f;

	private Vector2 offset;

	void Update () 
	{
    	offset.x += scrollspeed * Time.deltaTime;
		renderer.material.SetTextureOffset ("_MainTex", offset);
	}

}
