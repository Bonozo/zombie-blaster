using UnityEngine;
using System.Collections;

public class BulletGunSniper : MonoBehaviour {
	
	public GameObject ParticleSpark;
	public GameObject Reticle;
	
	public float DestroyTime = 4f;
	public float Speed = 5f;
	public float Y = 2.5f;
	
	// Use this for initialization
	void Start () {
		transform.position = new Vector3(transform.position.x,Y,transform.position.z);
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate(Time.deltaTime*Speed*Vector3.forward);
		if( Reticle != null )
		Reticle.transform.Translate(-Time.deltaTime*Speed*Vector3.forward);
		DestroyTime -= Time.deltaTime;
		if( DestroyTime < 3.0f && Reticle != null )
		{
			Destroy(Reticle);
		}
		if( DestroyTime <= 0 )
			Destroy(this.gameObject);
	}
	
	void OnCollisionEnter(Collision col)
	{
		if( col.gameObject.tag == "Zombie" )
			col.gameObject.SendMessage("GetHitDamaged",5);
		
		if( col.gameObject.tag == "ZombieHead" )
			col.gameObject.SendMessage("DieNormal");
		
		Instantiate(ParticleSpark,transform.position,Quaternion.identity);
		Destroy(this.gameObject);
	}
	
	// message reticle terget
	void SetReticleTarget(Vector3 target)
	{
		target.y -= 0.2f;
		Reticle.transform.position = target;
	}
	
	void DestroyReticle()
	{
		Destroy(Reticle);
	}
}
