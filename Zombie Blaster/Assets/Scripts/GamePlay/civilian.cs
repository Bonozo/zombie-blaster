using UnityEngine;
using System.Collections;

[AddComponentMenu("GamePlay/Civilian")]
public class civilian : MonoBehaviour {
	
	public GameObject ZombieRagdoll;
	
	public float CollisionToDownLenght = 0.4f;
	
	public float Speed = 20f;
	public float DestroyTime = 30f;
	
	private Vector3 lastpos;
	
	#region Moving, Life
	
	void Start () 
	{
		// setup height and spawn back to the player
		transform.position = new Vector3(transform.position.x,1.35f,transform.position.z);
		if(LevelInfo.Environments.mainCamera.WorldToScreenPoint(transform.position).z > 0)
		{
			Vector3 v = LevelInfo.Environments.control.transform.position;
			v.y = transform.position.y;
			v -= transform.position;
			transform.position += 2*v;
		}
		lastpos = transform.position;
	}
	
	void Update () 
	{
		if(Time.deltaTime==0) return;
		animation.Play("running");
		
		DestroyTime -= Time.deltaTime;
		if( DestroyTime <= 0f )
		{
			// civilian disapear in back of the player.
			if( LevelInfo.Environments.mainCamera.WorldToScreenPoint(transform.position).z > 0 )
			{
				DestroyTime = 3f;
			}
			else
			{
				Destroy(this.gameObject);
				return;
			}
		}
		
		NormalizeHeight();
		if(CanMoveForward())
			transform.Translate(Time.deltaTime*Speed/13f*Vector3.forward);
		else
		{
			transform.Rotate(0f,Random.Range(100f,250f),0f);
		}
		
		float olddist = GameEnvironment.DistXZ(LevelInfo.Environments.control.transform.position,lastpos);
		float newdist = GameEnvironment.DistXZ(LevelInfo.Environments.control.transform.position,transform.position);
		lastpos = transform.position;
		
		if(!rotating)
		{

			if( newdist > 10f )
			{
				if(!destinated)
				{
					Vector3 v = LevelInfo.Environments.control.transform.position; v.y = transform.position.y;
					
					float x = Random.Range(-7f,7f); if(Mathf.Abs(x)<1f) x=x>0?1:-1;
					float z = Random.Range(-7f,7f); if(Mathf.Abs(z)<1f) z=z>0?1:-1;
					
					v.x += x;
					v.z += z;
					transform.rotation = Quaternion.LookRotation(v-transform.position,Vector3.up);
					destinated = true;
				}
			}
			else 
			{
				if( newdist > olddist && newdist > Random.Range(4f,6f))
					StartCoroutine(Rotate(Random.Range(1f,2f)));
				
				if(!rotating && Random.Range(0,150)==1)
					StartCoroutine(Rotate(Random.Range(1f,2f)));
				destinated = false;
			}
		}
	}
	
	bool destinated = false;
	bool rotating = false;
	private IEnumerator Rotate(float time)
	{
		bool right = Random.Range(0,2)==1;
		rotating = true;
		while(time>0f)
		{
			time -= Time.deltaTime;
			transform.Rotate(0f,30f*Time.deltaTime*(right?1:-1),0f);
			yield return new WaitForEndOfFrame();
		}
		rotating = false;
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
	
	//private float cmftd = 0;
	private bool _canmoveforward = true;
	private bool CanMoveForward()
	{
		/*cmftd -= Time.deltaTime;
		if(cmftd > 0 ) return _canmoveforward;
		cmftd = 1.1f;*/
		_canmoveforward = CanMoveForwardHelper();
		return _canmoveforward;
	
	}
	private bool CanMoveForwardHelper()
	{
		float forwarddist = 2f;
		float leftrightdist = 1f;
		float diagonaldist = 2f;
		RaycastHit hit;    
		Vector3 pos = transform.position; pos.y += 0.4f;
		Debug.DrawRay(pos, forwarddist*transform.forward, Color.green);
		Debug.DrawRay(pos, leftrightdist*transform.right, Color.green);
		Debug.DrawRay(pos, -leftrightdist*transform.right, Color.green);
		Debug.DrawRay(pos, diagonaldist*0.5f*(transform.right+transform.forward), Color.green);
		Debug.DrawRay(pos, diagonaldist*0.5f*(-transform.right+transform.forward), Color.green);
		if(Physics.Raycast(pos, transform.forward, out hit, forwarddist) && hit.collider.gameObject.tag == "Zombie")
			return false;
		if(Physics.Raycast(pos, transform.right, out hit, leftrightdist) && hit.collider.gameObject.tag == "Zombie")
			if( transform.position.magnitude > hit.collider.gameObject.transform.position.magnitude )
				return false;
		if(Physics.Raycast(pos, -transform.right, out hit, leftrightdist) && hit.collider.gameObject.tag == "Zombie")
			if( transform.position.magnitude > hit.collider.gameObject.transform.position.magnitude )
				return false;
		if(Physics.Raycast(pos, 0.5f*(transform.right+transform.forward), out hit, diagonaldist) && hit.collider.gameObject.tag == "Zombie")
			if( transform.position.magnitude > hit.collider.gameObject.transform.position.magnitude )
				return false;
		if(Physics.Raycast(pos, 0.5f*(-transform.right+transform.forward), out hit, diagonaldist) && hit.collider.gameObject.tag == "Zombie")
			if( transform.position.magnitude > hit.collider.gameObject.transform.position.magnitude )
				return false;
		return true;
	}
	
	#endregion
	
	#region Die
	
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
		ActionForDie();
		//LevelInfo.Environments.hubZombiesLeft.SetNumber(LevelInfo.Environments.hubZombiesLeft.GetNumber()+1);
		GameObject g = (GameObject)Instantiate(ZombieRagdoll,transform.position,transform.rotation);
		g.SendMessage("IsCivilian");
		var rigidbodies = g.GetComponentsInChildren(typeof(Rigidbody));
		Vector3 dir = transform.position - LevelInfo.Environments.control.transform.position; dir.Normalize();
        foreach (Rigidbody child in rigidbodies) 
			child.AddForce(160f*dir);
		
		Destroy(this.gameObject);
	}
	
	public void DieWithFootball()
	{
		if(died) return; died=true;
		ActionForDie();
		//LevelInfo.Environments.hubZombiesLeft.SetNumber(LevelInfo.Environments.hubZombiesLeft.GetNumber()+1);
		GameObject g = (GameObject)Instantiate(ZombieRagdoll,transform.position,transform.rotation);
		g.SendMessage("IsCivilian");
		var rigidbodies = g.GetComponentsInChildren(typeof(Rigidbody));
		Vector3 dir = transform.position - LevelInfo.Environments.control.transform.position; dir.Normalize(); dir.y = 0.5f;
        foreach (Rigidbody child in rigidbodies) 
			child.AddForce(200f*dir);
		
		Destroy(this.gameObject);
	}
	
	public void DieWithElectricity()
	{
		if(died) return; died=true;
		ActionForDie();
		//LevelInfo.Environments.hubZombiesLeft.SetNumber(LevelInfo.Environments.hubZombiesLeft.GetNumber()+1);
		GameObject g = (GameObject)Instantiate(ZombieRagdoll,transform.position,transform.rotation);
		g.SendMessage("IsCivilian");
		g.SendMessage("SetMaterialToLighning");
		var rigidbodies = g.GetComponentsInChildren(typeof(Rigidbody));
		Vector3 dir = transform.position - LevelInfo.Environments.control.transform.position; dir.Normalize();
        foreach (Rigidbody child in rigidbodies) 
			child.AddForce(20f*dir);
		
		Destroy(this.gameObject);
	}
	
	
	public void DieWithJump()
	{
		if(died) return; died=true;
		ActionForDie();
		//LevelInfo.Environments.hubZombiesLeft.SetNumber(LevelInfo.Environments.hubZombiesLeft.GetNumber()+1);
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
		ActionForDie();
		//LevelInfo.Environments.hubZombiesLeft.SetNumber(LevelInfo.Environments.hubZombiesLeft.GetNumber()+1);
		GameObject g = (GameObject)Instantiate(ZombieRagdoll,transform.position,transform.rotation);
		g.SendMessage("IsCivilian");
		g.SendMessage("SetFireSize",1f);
		g.SendMessage("SetSmokeSize",1f);
		var rigidbodies = g.GetComponentsInChildren(typeof(Rigidbody));
		Vector3 dir = transform.position - LevelInfo.Environments.control.transform.position; dir.Normalize();
        foreach (Rigidbody child in rigidbodies) 
			child.AddForce(20f*dir);
		
		Destroy(this.gameObject);
	}

	void ActionForDie()
	{
		Store.zombieHeads = Mathf.Max(0,Store.zombieHeads-LevelInfo.Environments.control.currentWave);
		//LevelInfo.Environments.control.GetHealth(PlayerScoreForDie);	
	}
	
	#endregion
}
