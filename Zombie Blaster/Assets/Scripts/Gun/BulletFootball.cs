using UnityEngine;
using System.Collections;

public class BulletFootball : MonoBehaviour {
	
	public float DestroyTime = 4f;
	
	// Use this for initialization
	void Start () {
		Vector3 throwto = transform.forward; throwto.y+=0.15f;
		rigidbody.AddForce(750*throwto);
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
		
		if( col.gameObject.tag == "Ufo" )
			col.gameObject.SendMessage("GetHitDamaged",10);
		
		Instantiate(LevelInfo.Environments.particleSpark,transform.position,Quaternion.identity);
		Destroy(this.gameObject);
	}
	
}
