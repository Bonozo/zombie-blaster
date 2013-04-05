using UnityEngine;
using System.Collections;

public class YRotate : MonoBehaviour {
	
	public float Speed = 1f;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		gameObject.transform.Rotate(0,Speed,0);
	}
}
