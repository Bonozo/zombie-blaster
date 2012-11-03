using UnityEngine;
using System.Collections;

public class test : MonoBehaviour {
	
	public string animname = "Take 001";
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		animation.Play(animname);
		Debug.Log(animation[animname].time);
	}
}
