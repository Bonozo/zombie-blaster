using UnityEngine;
using System.Collections;

public class Flashlight : MonoBehaviour {
	
	public float Speed = 10f;
	public float maxHitch = 6;
	
	float cr = 0f,cr2 = 0f;
	private float Speed2;
	
	void Start()
	{
		Speed2 = Speed;
	}
	
	// Update is called once per frame
	void Update () {
		Speed *= Random.Range(0,2)==1?1:-1;
		Speed2 *= Random.Range(0,2)==1?1:-1;
		
		if( cr > maxHitch ) Speed = -Mathf.Abs(Speed);
		if( cr < -maxHitch ) Speed = Mathf.Abs(Speed);
		
		if( cr2 > maxHitch ) Speed2 = -Mathf.Abs(Speed2);
		if( cr2 < -maxHitch ) Speed2 = Mathf.Abs(Speed2);
		
		transform.Rotate(Speed2*Time.deltaTime,Speed*Time.deltaTime,0f);
		cr += Speed*Time.deltaTime;
		cr2 += Speed2*Time.deltaTime;

	}
}
