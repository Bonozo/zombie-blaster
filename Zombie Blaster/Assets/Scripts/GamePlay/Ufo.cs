using UnityEngine;
using System.Collections;

public class Ufo : MonoBehaviour {
	
	public float Speed = 10f;
	public float ExplosionRadius = 3f;
	public GameObject particleExplode;
	public ParticleEmitter smoke;
	
	private bool died = false;
	private float health = 10;
	private int toplayer = 0;
	
	// Use this for initialization
	void Start () {
		smoke.minSize = smoke.maxSize = 0f;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 pos = LevelInfo.Environments.control.transform.position;
		pos.y = transform.position.y;
		transform.RotateAround(pos,Vector3.up,Time.deltaTime*Speed);
		
		var player = LevelInfo.Environments.control.transform.position;
		
		if(Random.Range(0,70)==1) toplayer = Random.Range(-1,2);
		float dist = GameEnvironment.DistXZ(transform.position,player);
		if( dist >= 9f ) toplayer = -1;
		if( dist <= 4f ) toplayer = 1;
		pos = (transform.position-player).normalized;
		transform.position += Time.deltaTime*pos*toplayer;
		
	}
	
	void OnCollisionEnter(Collision col)
	{
		if( col.gameObject.tag == "Bullet" ) return;
		
		Vector3 exppos = col.contacts[0].point;
		Instantiate(particleExplode,exppos,Quaternion.identity);
				GameObject[] zomb = GameObject.FindGameObjectsWithTag("Zombie");
	
		foreach( GameObject zombi in zomb )
			if( GameEnvironment.DistXZ(zombi.transform.position,transform.position ) <= ExplosionRadius )
				zombi.SendMessage("DieWithFireAndSmoke");
	
		GameObject[] barr = GameObject.FindGameObjectsWithTag("Explosible");
		foreach( GameObject zombi in barr )
			if( GameEnvironment.DistXZ(zombi.transform.position,transform.position ) <= ExplosionRadius )
				zombi.SendMessage("Explode");
			
		LevelInfo.Environments.control.Shake();
		Destroy(this.gameObject);
	}
	
	public void GetFlame(float hitpoints)
	{
		if(LevelInfo.Environments.control.DamageMultiplied) hitpoints*=4;
		health -= hitpoints;
		TryExplode();
	}
	
	public void GetHitDamaged(float hitpoints)
	{
		if(LevelInfo.Environments.control.DamageMultiplied) hitpoints*=4;
		health -= hitpoints;
		TryExplode();
	}
	
	private void TryExplode()
	{
		if(died) return;
		
		smoke.minSize = 0.025f*(10f-health);
		smoke.maxSize = smoke.minSize+0.2f;
		
		if(health <= 0f )
		{
			died = true;
			gameObject.AddComponent("Rigidbody");
			
			// Give scores and heads the player
			LevelInfo.Environments.control.GetScore(LevelInfo.State.scoreForUFO,true);	
			Store.zombieHeads += 20;
		}
	}
	
}
