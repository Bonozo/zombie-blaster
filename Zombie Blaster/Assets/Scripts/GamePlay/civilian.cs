using UnityEngine;
using System.Collections;

[AddComponentMenu("GamePlay/Civilian")]
public class civilian : MonoBehaviour {
	
	public GameObject ZombieRagdoll;
	
	public float CollisionToDownLenght = 0.4f;
	
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
	}
	
	// Update is called once per frame
	void Update () {
		
		if(Time.deltaTime==0) return;
		
		DestroyTime -= Time.deltaTime;
		if( DestroyTime <= 0f )
		{
			Destroy(this.gameObject);
			return;
		}
		
		NormalizeHeight();
		
		Vector3 lastpos = transform.position;
		Vector3 v = control.transform.position; v.y = transform.position.y;
		if( Vector3.Distance(transform.position,v ) > 8f )
			transform.Translate(Time.deltaTime * Speed / 13 * (v - transform.position).normalized,Space.World );
		else
			transform.RotateAround(v,Vector3.up,Time.deltaTime*Speed);
		transform.rotation = Quaternion.LookRotation(transform.position-lastpos,Vector3.up);
		animation.Play("running");
	}
	
	private float nhdt = 0f;
	private void NormalizeHeight()
	{
		nhdt -= Time.deltaTime;
		if( nhdt > 0f ) return;
		nhdt = 0.291f;
		RaycastHit hit;    
		
		Vector3 pos = transform.position;
		pos.y += 1f;

		if(Physics.Raycast(pos, -transform.up, out hit, CollisionToDownLenght+10f) && hit.collider.gameObject.name != "Flame" )
		{
			transform.Translate(0f,-hit.distance+CollisionToDownLenght+1f,0f);
		}
		Debug.DrawRay(pos, -CollisionToDownLenght*transform.up, Color.green);
	}
	
	/*private void NormalizeHeight()
	{
		if( Random.Range(0,30) != 1 ) return;
		
		RaycastHit hit;    
		
		Vector3 pos = transform.position;
		// ?? // 
		if( HitWithName(gameObject.name,"Fatkid") || HitWithName(gameObject.name,"Farmer2") ||
			HitWithName(gameObject.name,"Ballerina") || HitWithName(gameObject.name,"FootballPlayer"))
			pos.y += 0.4f;
		
		pos.y += 1f;
		
		if(Physics.Raycast(pos, -transform.up, out hit, CollisionToDownLenght+0.01f+1f) /*&& 
			hit.collider.gameObject.name == "ground" *//*)
		/*	transform.Translate(0,0.02f,0);
		if( !Physics.Raycast(pos, -transform.up, out hit, CollisionToDownLenght-0.03f+1f) )
			transform.Translate(0,-0.02f,0);
		
		Debug.DrawRay(pos, -CollisionToDownLenght*transform.up, Color.green);
	}*/
	
	public void GetFlame()
	{
		DieNormal();
	}
	
	public void GetHit()
	{
		DieNormal();
	}
	
	public void GetHitDamaged(int hitpoints)
	{
		DieNormal();
	}
	
	private bool died = false;
	public void DieNormal()
	{
		if(died) return; died=true;
		control.GetHealth(PlayerScoreForDie);
		LevelInfo.Environments.hubZombiesLeft.SetNumber(LevelInfo.Environments.hubZombiesLeft.GetNumber()+1);
		GameObject g = (GameObject)Instantiate(ZombieRagdoll,transform.position,transform.rotation);
		g.SendMessage("IsCivilian");
		var rigidbodies = g.GetComponentsInChildren(typeof(Rigidbody));
		Vector3 dir = transform.position - control.transform.position; dir.Normalize();
        foreach (Rigidbody child in rigidbodies) 
			child.AddForce(160f*dir);
		
		Destroy(this.gameObject);
	}
	
	public void DieWithFootball()
	{
		if(died) return; died=true;
		control.GetHealth(PlayerScoreForDie);
		LevelInfo.Environments.hubZombiesLeft.SetNumber(LevelInfo.Environments.hubZombiesLeft.GetNumber()+1);
		GameObject g = (GameObject)Instantiate(ZombieRagdoll,transform.position,transform.rotation);
		g.SendMessage("IsCivilian");
		var rigidbodies = g.GetComponentsInChildren(typeof(Rigidbody));
		Vector3 dir = transform.position - control.transform.position; dir.Normalize(); dir.y = 0.5f;
        foreach (Rigidbody child in rigidbodies) 
			child.AddForce(200f*dir);
		
		Destroy(this.gameObject);
	}
	
	public void DieWithJump()
	{
		if(died) return; died=true;
		control.GetHealth(PlayerScoreForDie);
		LevelInfo.Environments.hubZombiesLeft.SetNumber(LevelInfo.Environments.hubZombiesLeft.GetNumber()+1);
		GameObject ragdoll = (GameObject)Instantiate(ZombieRagdoll,transform.position,transform.rotation);
		ragdoll.SendMessage("IsCivilian");
		var rigidbodies = ragdoll.GetComponentsInChildren(typeof(Rigidbody));
        foreach (Rigidbody child in rigidbodies) 
			child.AddForce(new Vector3(Random.Range(-160f,160f),160f,Random.Range(-160f,160f)));
		Destroy(this.gameObject);
	}
	
	public void DieWithFireAndSmoke()
	{
		if(died) return; died=true;
		control.GetHealth(PlayerScoreForDie);
		LevelInfo.Environments.hubZombiesLeft.SetNumber(LevelInfo.Environments.hubZombiesLeft.GetNumber()+1);
		GameObject g = (GameObject)Instantiate(ZombieRagdoll,transform.position,transform.rotation);
		g.SendMessage("IsCivilian");
		g.SendMessage("SetFireSize",1f);
		g.SendMessage("SetSmokeSize",1f);
		var rigidbodies = g.GetComponentsInChildren(typeof(Rigidbody));
		Vector3 dir = transform.position - control.transform.position; dir.Normalize();
        foreach (Rigidbody child in rigidbodies) 
			child.AddForce(20f*dir);
		
		Destroy(this.gameObject);
	}
}
