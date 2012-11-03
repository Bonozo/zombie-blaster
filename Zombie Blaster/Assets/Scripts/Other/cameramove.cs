using UnityEngine;
using System.Collections;

public class cameramove : MonoBehaviour {
	
	public float Speed = 10f;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(0,Input.GetAxis("Horizontal")*Time.deltaTime*Speed,0);
	}
}
