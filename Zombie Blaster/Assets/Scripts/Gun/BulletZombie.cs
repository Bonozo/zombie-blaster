using UnityEngine;
using System.Collections;

public class BulletZombie : MonoBehaviour {
	
	public float DestroyTime = 3f;
	public float Speed = 5f;
	public float Y = 2.5f;
	
	private Control control;
	
	// Use this for initialization
	void Start () {
		
		control = (Control)GameObject.FindObjectOfType(typeof(Control));
		
		transform.position = new Vector3(transform.position.x,Y,transform.position.z);
		transform.LookAt(GameObject.Find("GunAirgun").transform.position,Vector3.up);
		transform.Translate(0.5f*Vector3.forward);
		transform.Translate(0.2f*Vector3.right);
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate(Time.deltaTime*Speed*Vector3.forward);
		
		Vector3 v = transform.position;
		v.y = 0;
		if( v.magnitude <= 0.5f )
		{
			control.GetHealth(-0.01f);
			Destroy(this.gameObject);
		}
		
		DestroyTime -= Time.deltaTime;
		if( DestroyTime <= 0 )
			Destroy(this.gameObject);
	}
}
