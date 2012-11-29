using UnityEngine;
using System.Collections;

public class BulletFootball : MonoBehaviour {
	
	public float DestroyTime = 4f;
	public float Y = 2.5f;
	public float ExplosionRadius = 3f;
	
	// Use this for initialization
	void Start () {
		transform.position = new Vector3(transform.position.x,Y,transform.position.z);
		rigidbody.AddForce(2000*transform.forward);
	}
	
	// Update is called once per frame
	void Update () {
		DestroyTime -= Time.deltaTime;
		if( DestroyTime <= 0 )
			Destroy(this.gameObject);
	}
	
	void OnCollisionEnter(Collision col)
	{
		if( col.gameObject.tag == "Zombie" )
			col.gameObject.SendMessage("DieWithFootball");
			
		if( col.gameObject.tag == "ZombieHead" )
			col.gameObject.SendMessage("DieWithFootball");
		
		
		Destroy(this.gameObject);
	}
	
}
