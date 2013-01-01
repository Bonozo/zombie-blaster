using UnityEngine;
using System.Collections;

public class BulletRocket : MonoBehaviour {
	
	public GameObject ParticleExplosion;
	public GameObject Exhaust;
	
	public float DestroyTime = 4f;
	public float Speed = 5f;
	public float Y = 2.5f;
	public float ExplosionRadius = 3f;
	
	// Use this for initialization
	void Start () {
		transform.position = new Vector3(transform.position.x,Y,transform.position.z);
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
	
	void OnCollisionEnter()
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
		
		Control control = (Control)GameObject.FindObjectOfType(typeof(Control));
		if( GameEnvironment.DistXZ(control.transform.position,transform.position ) <= 2*ExplosionRadius )
			control.GetHealth(-0.2f);
		
		LevelInfo.Environments.control.Shake();
		Instantiate(ParticleExplosion,transform.position,Quaternion.identity);
		Destroy(this.gameObject);
	}
	
}
