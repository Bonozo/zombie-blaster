using UnityEngine;
using System.Collections;

public class Yrotate : MonoBehaviour {
	
	public float Speed = 10f;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(0,Speed*Time.deltaTime,0);
	}
}
