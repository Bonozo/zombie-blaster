using UnityEngine;
using System.Collections;

public class BulletAlienBlaster : MonoBehaviour {
	
	// THESE CLASS ARE NOT USED. ALIEN BLASTER BULLET IS DETERMINED BY "BulletGunRevolver.cs"
	
	public GameObject sparks;
	public float DestroyTime = 4f;
	public float Speed = 5f;
	public float Y = 2.5f;
	
	// Use this for initialization
	void Start () {
		transform.position = new Vector3(transform.position.x,Y,transform.position.z);
		Instantiate(sparks,transform.position,transform.rotation);
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate(Time.deltaTime*Speed*Vector3.forward);
		DestroyTime -= Time.deltaTime;
		if( DestroyTime <= 0 )
			Destroy(this.gameObject);
	}
	
	void OnCollisionEnter(Collision col)
	{
		bool blooded = false;
		if( col.gameObject.tag == "Zombie" && (col.gameObject.GetComponent<Zombi>() == null || !col.gameObject.GetComponent<Zombi>().haveHelmet) ) blooded = true;
		if( col.gameObject.tag == "ZombieHead" && !col.gameObject.GetComponent<HeadHit>().HeadContainer.haveHelmet ) blooded = true;
		Instantiate(blooded?LevelInfo.Environments.particleBlood:LevelInfo.Environments.particleSpark,transform.position,Quaternion.identity);
		
		
		if( col.gameObject.tag == "Zombie" )
			col.gameObject.SendMessage("GetHitDamaged",10);
		
		if( col.gameObject.tag == "ZombieHead" )
			col.gameObject.SendMessage("DieDamaged");
		
		Destroy(this.gameObject);
	}
}
