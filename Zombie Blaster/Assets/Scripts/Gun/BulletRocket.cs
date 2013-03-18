using UnityEngine;
using System.Collections;

public class BulletRocket : MonoBehaviour {
	
	public GameObject ParticleExplosion;
	public GameObject Exhaust;
	
	public float DestroyTime = 4f;
	public float Speed = 5f;
	public float ExplosionRadius = 3f;
	
	// Use this for initialization
	void Start () {
		Exhaust = (GameObject)Instantiate(Exhaust,transform.position,Quaternion.identity);
		Destroy(Exhaust,DestroyTime);
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate(Time.deltaTime*Speed*Vector3.forward);
		if(Exhaust != null) Exhaust.transform.position = transform.position;
		DestroyTime -= Time.deltaTime;
		if( DestroyTime <= 0 )
			Destroy(this.gameObject);
	}
	
	void OnDestroy()
	{
		if( Exhaust != null )
			Exhaust.particleEmitter.emit=false;
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
