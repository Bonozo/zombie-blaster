using UnityEngine;
using System.Collections;

public class BulletGunPulseShotGun : MonoBehaviour {
	
	public GameObject sparks;
	
	public float DestroyTime = 3f;
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
		bool iscivil = col.gameObject.GetComponent<civilian>() != null;
		if( col.gameObject.tag == "Zombie" && (col.gameObject.GetComponent<Zombi>() == null || !col.gameObject.GetComponent<Zombi>().haveHelmet) ) blooded = true;
		if( col.gameObject.tag == "ZombieHead" && !col.gameObject.GetComponent<HeadHit>().HeadContainer.haveHelmet ) blooded = true;

		if(!blooded || iscivil)
			Instantiate(LevelInfo.Environments.particleSpark,transform.position,Quaternion.identity);
		else
		{	
			ParticleSystem p = ((GameObject)Instantiate(LevelInfo.Environments.particleBlood,transform.position,Quaternion.identity)).GetComponent<ParticleSystem>();
			p.startSpeed = 7f;
			p.Emit(200);
		}
		
		
		if( col.gameObject.tag == "Zombie" )
			col.gameObject.SendMessage("DieWithJump");	
		else if( col.gameObject.tag == "ZombieHead" )
			col.gameObject.SendMessage("DieWithJump");
		if( col.gameObject.tag == "Ufo" )
			col.gameObject.SendMessage("GetHitDamaged",10);
		Destroy(this.gameObject);
	}
}
