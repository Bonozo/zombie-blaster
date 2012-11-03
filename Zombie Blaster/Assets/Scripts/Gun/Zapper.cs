using UnityEngine;
using System.Collections;

public class Zapper : MonoBehaviour {
	
	public float StreamSpeed = 10f;
	
	// Use this for initialization
	void Start () {
		Disappear();
	}
	
	public void Stream(float deltaTime)
	{
		if(!audio.isPlaying) audio.Play();
		Vector3 sc = transform.localScale;
		sc.y += StreamSpeed*deltaTime;
		transform.localScale = sc;
	}
	
	public void Disappear()
	{
		audio.Stop();
		Vector3 sc = transform.localScale;
		sc.y = 0.0f;
		transform.localScale = sc;	
	}
	
	public bool IsDisappear { get { return transform.localScale.y == 0.0f; }}
	
	public void ChangeRotation(Quaternion cameraRot)
	{
		transform.rotation = cameraRot;
		transform.Rotate(90f,0f,0f);
	}
}
