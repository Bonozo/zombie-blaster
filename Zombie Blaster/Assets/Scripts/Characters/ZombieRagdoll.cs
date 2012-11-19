using UnityEngine;
using System.Collections;

public class ZombieRagdoll : MonoBehaviour {
	
	public bool scooby = false;
	
	public GameObject Fire;
	public GameObject Smoke;
	public GameObject Zombie;
	
	private float DestroyTime = 8f;
	private bool throwedout = false;
	private bool iscivilian = false;
	
	private bool rd = false;
	//private float rootbeginposy;
	//private float forcey = 0;
	
	public GameObject head;
	private CollisionSender headcol;
	private bool falledsoundplayed = false;
	
	// Use this for initialization
	void Start () {
		
		Fire.particleEmitter.emit = true;
		Smoke.particleEmitter.emit = true;
		
		//rootbeginposy = head.transform.position.y*0.9f;
		
		headcol = head.AddComponent<CollisionSender>();
		
		LevelInfo.Audio.PlayZombieGetsAttcked();
	}


	
	// Update is called once per frame
	void Update () 
	{	
		DestroyTime -= Time.deltaTime;
		
	/*	if( throwedout )
		{
			if( DestroyTime <= 46f )
			{
				head.rigidbody.AddForce( new Vector3(0,forcey,0));
				forcey += 60f*Time.deltaTime;
				if( head.transform.position.y >= rootbeginposy )
				{
					GameObject zomb = (GameObject)Instantiate(Zombie,head.transform.position,Quaternion.identity);
					zomb.SendMessage("DontSpawn");
					zomb.SendMessage("SetFireSize",Fire.particleEmitter.maxSize);
					zomb.SendMessage("SetSmokeSize",Smoke.particleEmitter.maxSize);
					Destroy(this.gameObject);
				}
			}
		}
		else*/
		{
			if( DestroyTime <= 4f )
			{
				if( !rd )
				{
					var joints = GetComponentsInChildren(typeof(CharacterJoint));
       					foreach (var child in joints) 
				    		Destroy(child);
					var rigidbodies = GetComponentsInChildren(typeof(Rigidbody));
    	   				foreach (var child in rigidbodies) 
								Destroy(child);
					var colliders = GetComponentsInChildren(typeof(Collider));
       					foreach (var child in colliders) 
				    		Destroy(child);
					rd = true;
				}
			if(!throwedout) transform.Translate(0,-0.16f*Time.deltaTime,0);
			}
		}
		
		if( !falledsoundplayed && headcol.entered )
		{
			falledsoundplayed = true;
			LevelInfo.Audio.PlayZombieFalls();
		}
		
		if( DestroyTime <= 0f )
		{
			if( throwedout )
			{
				if( iscivilian ) LevelInfo.Environments.control.zombiesLeftForThisWave++;
				GameObject zomb = (GameObject)Instantiate(Zombie,head.transform.position,Quaternion.identity);
				zomb.SendMessage("DontSpawn");
				zomb.SendMessage("SetFireSize",Fire.particleEmitter.maxSize);
				zomb.SendMessage("SetSmokeSize",Smoke.particleEmitter.maxSize);
				Destroy(this.gameObject);		
			}
			else
			{
				// other logic for cemetary level
				bool cemetary = LevelInfo.Environments.control.currentLevel == 4;
				if( cemetary ) // Cemetary
				{
					scooby = LevelInfo.Environments.control.currentWave > 1;
					if( scooby && Random.Range(0,2)==0 ) scooby = false;
				}
				
				if(scooby || Random.Range(0,2)==1)
				{
					HealthPack er = (HealthPack)Instantiate(LevelInfo.Environments.healthPack,transform.position,transform.rotation);
					er.scooby = scooby;
				}
				Destroy(this.gameObject);
			}
		}
	}
	
	public void SetFireSize(float f)
	{
		Fire.particleEmitter.minSize = f;
		Fire.particleEmitter.maxSize = f;
	}	
	
	public void SetSmokeSize(float f)
	{
		Smoke.particleEmitter.minSize = f;
		Smoke.particleEmitter.maxSize = f;
	}
	
	public void ThrowedOut()
	{
		//DestroyTime = 50;
		throwedout = true;
	}

	public void IsCivilian()
	{
		//DestroyTime = 50;
		throwedout = true;
		iscivilian = true;
	}
}
