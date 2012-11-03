using UnityEngine;
using System.Collections;

public class GunCylinderBase : MonoBehaviour {
	
	public float StreamSpeed;
	
	// Use this for initialization
	void Start () {
		Disappear();
	}
	
	public void Stream(float deltaTime)
	{
		gameObject.collider.enabled = true;
		Vector3 sc = transform.localScale;
		sc.y += StreamSpeed*deltaTime;
		transform.localScale = sc;
	}
	
	public void Disappear()
	{
		gameObject.collider.enabled = false;
		Vector3 sc = transform.localScale;
		sc.y = 0.0f;
		transform.localScale = sc;	
	}
	
	public void Clamp(float maxscale)
	{
		if( transform.localScale.y > maxscale)
		{
			Vector3 sc = transform.localScale;
			sc.y = maxscale;
			transform.localScale = sc;
		}
	}
	
	public bool IsDisappear { get { return transform.localScale.y == 0.0f; }}
	
	public void ChangeRotation(Quaternion cameraRot)
	{
		transform.rotation = cameraRot;
		transform.Rotate(90f,0f,0f);
	}
}
