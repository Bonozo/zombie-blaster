using UnityEngine;
using System.Collections;

public class civilian : MonoBehaviour {
	
	public GameObject ZombieRagdoll;
	
	public float Speed = 20f;
	public float DestroyTime = 30f;
	//public GameObject ZombieRagdoll;
	//public GameObject ZombieFire;
	//public GameObject ZombieSmoke;
	
	private float PlayerScoreForDie = -0.1f;
	private Control control;
	private Vector3 center;
	
	// Use this for initialization
	void Start () {
		control = (Control)GameObject.FindObjectOfType(typeof(Control));
		transform.position = new Vector3(transform.position.x,1.35f,transform.position.z);
		center = transform.position; center.x += 4f;
		animation["walk"].speed = 4f;
	}
	
	// Update is called once per frame
	void Update () {
		
		DestroyTime -= Time.deltaTime;
		if( DestroyTime <= 0f )
		{
			Destroy(this.gameObject);
			return;
		}
		
		
		
		Vector3 lastpos = transform.position;
		Vector3 v = control.transform.position; v.y = transform.position.y;
		if( Vector3.Distance(transform.position,v ) > 8f )
			transform.Translate(Time.deltaTime * Speed / 13 * (v - transform.position).normalized,Space.World );
		else
			transform.RotateAround(v,Vector3.up,Time.deltaTime*Speed);
		transform.rotation = Quaternion.LookRotation(transform.position-lastpos,Vector3.up);
		animation.Play("walk");
	}
	
	void OnTriggerStay(Collider col)
	{	
		// Zapper Attack
		if( HitWithName(col.gameObject.name,"Zapper") )
		{
			//ZombieSmoke.particleEmitter.minSize = ZombieSmoke.particleEmitter.maxSize = 1f;
			DieNormal();
			return;
		}
		
		// Flame Attack
		if( HitWithName(col.gameObject.name,"Flame") )
		{
			//ZombieFire.particleEmitter.minSize = ZombieFire.particleEmitter.maxSize = 1f;
			DieNormal();
			return;
		}	
	}
	
	/*void OnCollisionEnter(Collision col)
	{	
		//  PulseShotGun Attack
		if( HitWithName(col.gameObject.name,"BulletPulseShotGun") )
		{
			DieWithJump();
			return;
		}
		
		// Airgun Attack
		if( HitWithName(col.gameObject.name,"BulletAirgun") )
		{
			DieNormal();
			return;
		} 
		
		// Crossbow Attack
		if( HitWithName(col.gameObject.name,"BulletCrossbow") )
		{
			DieNormal();
			return;
		} 
	}*/
	
	public void GetHit()
	{
		DieNormal();
	}
	
	public void GetHitDamaged(int hitpoints)
	{
		DieNormal();
	}
	
	public void DieNormal()
	{
		control.GetHealth(PlayerScoreForDie);
		GameObject g = (GameObject)Instantiate(ZombieRagdoll,transform.position,transform.rotation);
		g.SendMessage("IsCivilian");
		var rigidbodies = g.GetComponentsInChildren(typeof(Rigidbody));
		Vector3 dir = transform.position - control.transform.position; dir.Normalize();
        foreach (Rigidbody child in rigidbodies) 
			child.AddForce(1000f*dir);
		
		Destroy(this.gameObject);
	}
	
	public void DieWithFootball()
	{
		control.GetHealth(PlayerScoreForDie);
		GameObject g = (GameObject)Instantiate(ZombieRagdoll,transform.position,transform.rotation);
		g.SendMessage("IsCivilian");
		var rigidbodies = g.GetComponentsInChildren(typeof(Rigidbody));
		Vector3 dir = transform.position - control.transform.position; dir.Normalize(); dir.y = 0.5f;
        foreach (Rigidbody child in rigidbodies) 
			child.AddForce(2000f*dir);
		
		Destroy(this.gameObject);
	}
	
	public void DieWithJump()
	{
		control.GetHealth(PlayerScoreForDie);
		
		GameObject ragdoll = (GameObject)Instantiate(ZombieRagdoll,transform.position,transform.rotation);
		ragdoll.SendMessage("IsCivilian");
		var rigidbodies = ragdoll.GetComponentsInChildren(typeof(Rigidbody));
        foreach (Rigidbody child in rigidbodies) 
			child.AddForce(new Vector3(Random.Range(-1600f,1600f),1600f,Random.Range(-1600f,1600f)));
		Destroy(this.gameObject);
	}
	
	public void DieWithFireAndSmoke()
	{
		control.GetHealth(PlayerScoreForDie);
		
		GameObject g = (GameObject)Instantiate(ZombieRagdoll,transform.position,transform.rotation);
		g.SendMessage("IsCivilian");
		g.SendMessage("SetFireSize",1f);
		g.SendMessage("SetSmokeSize",1f);
		var rigidbodies = g.GetComponentsInChildren(typeof(Rigidbody));
		Vector3 dir = transform.position - control.transform.position; dir.Normalize();
        foreach (Rigidbody child in rigidbodies) 
			child.AddForce(100f*dir);
		
		Destroy(this.gameObject);
	}
	
	private bool HitWithName(string name,string comparewith)
	{
		return name.Length >= comparewith.Length && name.Substring(0,comparewith.Length) == comparewith;
	}
}
