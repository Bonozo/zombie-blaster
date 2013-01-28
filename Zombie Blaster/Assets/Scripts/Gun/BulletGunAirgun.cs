using UnityEngine;
using System.Collections;

public class BulletGunAirgun : MonoBehaviour {
	
	public GameObject sparks;
	public float DestroyTime = 4f;
	public float Speed = 5f;
	public float Y = 2.5f;
	public int damage = 2;
	
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
		//	col.gameObject.GetComponent<Zombi>().GetHitFinished(Weapon.BB,col.contacts[0].point,damage);
			col.gameObject.SendMessage("GetHitDamaged",damage);
		
		if( col.gameObject.tag == "ZombieHead" )
			col.gameObject.SendMessage("DieWithAirsoft");
		
		Destroy(this.gameObject);
	}
}
