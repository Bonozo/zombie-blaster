using UnityEngine;
using System.Collections;

public class BulletGrenade : MonoBehaviour {
	
	public GameObject ParticleExplosion;
	
	public float DestroyTime = 4f;
	public float ExplosionRadius = 3f;
	
	// Use this for initialization
	void Start () {
		Vector3 throwto = transform.forward; throwto.y+=0.3f;
		throwto.x *= 500f; throwto.y *= 500f; throwto.z *= 500f;
		rigidbody.AddForce(throwto);
		rigidbody.angularVelocity = Random.rotation.eulerAngles;
	}
	
	// Update is called once per frame
	void Update () {
		DestroyTime -= Time.deltaTime;
		if( DestroyTime <= 0 )
			Destroy(this.gameObject);
	}
	
	void OnCollisionEnter(Collision col)
	{
		GameObject[] zomb = GameObject.FindGameObjectsWithTag("Zombie");
		foreach( GameObject zombi in zomb )
		{
			if( GameEnvironment.DistXZ(zombi.transform.position,transform.position ) <= ExplosionRadius )
				zombi.SendMessage("DieWithFireAndSmoke");
		}
		
		GameObject[] barr = GameObject.FindGameObjectsWithTag("Explosible");
		foreach( GameObject zombi in barr )
		{
			if( GameEnvironment.DistXZ(zombi.transform.position,transform.position ) <= ExplosionRadius )
				zombi.SendMessage("Explode");
		}
		
		if( GameEnvironment.DistXZ(LevelInfo.Environments.control.transform.position,transform.position ) <= ExplosionRadius )
			LevelInfo.Environments.control.GetHealth(-0.1f);
		
		if( col.gameObject.tag == "Ufo" )
			col.gameObject.SendMessage("GetHitDamaged",10);
		
		LevelInfo.Environments.control.Shake();
		Instantiate(ParticleExplosion,transform.position,Quaternion.identity);
		Destroy(this.gameObject);
	}
	
}
