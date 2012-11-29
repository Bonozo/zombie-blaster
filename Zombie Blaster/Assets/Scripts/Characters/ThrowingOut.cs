using UnityEngine;
using System.Collections;

public class ThrowingOut : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
		gameObject.AddComponent("Rigidbody");
		
		Control control = (Control)GameObject.FindObjectOfType(typeof(Control));
		Vector3 dir = transform.position - control.transform.position; dir.Normalize();
		rigidbody.AddForce(800f*dir);
		
		Destroy(this.gameObject,4f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
