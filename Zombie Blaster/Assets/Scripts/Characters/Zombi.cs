using UnityEngine;
using System.Collections;

public class Zombi : MonoBehaviour {
	
	#region Parameters
	
	public AudioClip audioAttackwalk;
	
	// Placement
	public float Speed = 0.5f;
	public float ZombieHeight = 1.35f;
	public float WaitDistance = 1.5f;
	public float CollisionToDownLenght = 0.35f;
	
	// Parts
	public GameObject ZombieRagdoll;
	public GameObject ZombieFire;
	public GameObject ZombieSmoke;
	public HeadHit headHit;
	public GameObject head;

	// Scores
	public float BitePoint = 0.01f;
		
	// Helemt
	public bool haveHelmet = false;
	public GameObject HelmetPrefab;
	
	// Jump
	public bool canJump;
	public AudioClip jumpAudio;	
	
	// Shield Up
	public bool canShieldUp;
	
	// Shoot
	public bool canShoot;
	public Transform shootTransform;
	public GameObject shootBullet;
	public float shootTime;
	public AudioClip shootAudio;
	
	
	#endregion
	
	#region Variables

	private SwipeUpCaution swipeUpcaution;
	
	private float flaming = 1f;
	private float smoking = 1f;
	private float damage = 10;
	private bool audioswaonelayed = false;
	private bool spawning = true;
	
	private float RunningTime = 3f;
	private float runningTime = 0.0f;
	
	private GameObject particleDirtClod;
	
	#endregion
	
	#region Start, Update, OnGUI
	
	// Use this for initialization
	void Start () {
		gameObject.AddComponent<AudioSource>();
		swipeUpcaution = (SwipeUpCaution)GameObject.FindObjectOfType(typeof(SwipeUpCaution));
		
		headHit.tag = "ZombieHead";
		headHit.HeadContainer = this;
		
		haveHelmet = false;
		if( HelmetPrefab != null )
		{
			haveHelmet = Random.Range(0,2)==1;
			HelmetPrefab.SetActiveRecursively(haveHelmet);
		}
		
		transform.position = new Vector3(transform.position.x,ZombieHeight,transform.position.z);
		
		// Playing Spawn Animation
		particleDirtClod = LevelInfo.Environments.control.currentLevel % 2 == 1? LevelInfo.Environments.dirtyClodCityPrefab: LevelInfo.Environments.dirtyClodPrefab;
		particleDirtClod = (GameObject)Instantiate(particleDirtClod,new Vector3(transform.position.x,1f,transform.position.z),Quaternion.identity);
		if(spawning)
		{
			animation.Play("spawn");
			LevelInfo.Audio.PlayZombieSpawn();
		}
		
		if( audioAttackwalk == null )
			audioAttackwalk = LevelInfo.Audio.AudioZombieAttackWalk;
	}
	
	private float shotDeltaTime=0.0f;
	// Update is called once per frame
	void Update () {
		
		// look to player.
		Vector3 np = LevelInfo.Environments.control.transform.position-transform.position; np.y = 0;
		transform.rotation = Quaternion.LookRotation(np,Vector3.up);
		
		if( transform.position.y < -10f )
		{
			LevelInfo.Environments.control.zombiesLeftForThisWave--;
			Destroy(this.gameObject);
		}
		
		if( animation.IsPlaying("spawn") ) return;
		if( animation.IsPlaying("jump") ) return;
		if( animation.IsPlaying("shoot") ) 
		{
			if( animation["shoot"].time >= shootTime )
			{
				shotDeltaTime -= Time.deltaTime;
				if( shotDeltaTime <= 0 )
				{
					Instantiate(shootBullet,shootTransform.position,Quaternion.identity);
					shotDeltaTime = animation["shoot"].length-animation["shoot"].time+0.05f;
				}
			}
			return;
		}
		if(!audioswaonelayed) { audio.Stop(); audioswaonelayed=true; }
		
		// Head Hited End
		
		if( animation.IsPlaying("zap") ) return;
		if( animation.IsPlaying("get hit") ) return;
		
		if( LevelInfo.Environments.control.Died ) return;

		// If Near Updates
		if( NearPlayer() )
		{
			swipeUpcaution.Activate(LevelInfo.Environments.mainCamera.WorldToScreenPoint(transform.position));
				
			
			if( !animation.IsPlaying("attack01") && !animation.IsPlaying("attack02") )
			{
				animation.Play(Random.Range(1,3)==1?"attack01":"attack02");
				if( flaming <= 0.75f )
					LevelInfo.Environments.control.GetBite(2*BitePoint);
				else
					LevelInfo.Environments.control.GetBite(BitePoint);
			}
			
		}
		// If Far Update
		else
		{
			/// Shoot
			if( runningTime <= 0f &&  canShoot && Random.Range(0,300)==1)
			{
				if( shootAudio != null ) LevelInfo.Audio.audioSourceZombies.PlayOneShot(shootAudio);
				animation.Play("shoot");
				return;
			}
			// Jump
			if( runningTime <= 0f && canJump && Random.Range(0,300)==1)
			{
				if( jumpAudio != null ) LevelInfo.Audio.audioSourceZombies.PlayOneShot(jumpAudio);
				animation.Play("jump");
				return;
			}
			
			// Shield Up
			if( runningTime <= 0f && canShieldUp && Random.Range(0,300)==1)
			{
				//if( jumpAudio != null ) LevelInfo.Audio.audioSourceZombies.PlayOneShot(jumpAudio);
				animation.Play("shieldup");
				return;
			}
			
			// ?? // Running
			if( runningTime <= 0f && HitWithName(gameObject.name,"FootballPlayer") && Random.Range(0,300)==1 )
			{
				runningTime = RunningTime;
			}
			
			if( runningTime > 0f )
			{
				runningTime -= Time.deltaTime;
				animation.Play("run");
			}
			else
			{
				if( !animation.IsPlaying("shieldup") )
					animation.Play("walk");
			}
			// Move Forward if no zombie in front of
			
			NormalizeHeight();
			float spd = Speed; if( runningTime > 0f ) spd *= 2f;
			if(CanMoveForward()) transform.Translate(Time.deltaTime*spd*Vector3.forward);
			if( Random.Range(0,LevelInfo.Audio.zombieAudioAttackWalkRate)==1 )
				LevelInfo.Audio.audioSourceZombies.PlayOneShot(audioAttackwalk);
		}
		
	}
	
	private float nhdt = 0f;
	private void NormalizeHeight()
	{
		nhdt -= Time.deltaTime;
		if( nhdt > 0f ) return;
		nhdt = 1.3f;
		RaycastHit hit;    
		
		Vector3 pos = transform.position;
		// ?? // 
		if( HitWithName(gameObject.name,"Fatkid") || HitWithName(gameObject.name,"Farmer2") ||
			HitWithName(gameObject.name,"Ballerina") || HitWithName(gameObject.name,"FootballPlayer"))
			pos.y += 0.4f;
		
		pos.y += 1f;
		
		/*if(Physics.Raycast(pos, -transform.up, out hit, CollisionToDownLenght+0.01f+1f) && 
			hit.collider.gameObject.tag != "Flamethrower" )
			transform.Translate(0,0.02f,0);
		if( !Physics.Raycast(pos, -transform.up, out hit, CollisionToDownLenght-0.03f+1f) )
			transform.Translate(0,-0.02f,0);
		*/
		if(Physics.Raycast(pos, -transform.up, out hit, CollisionToDownLenght+10f) && hit.collider.gameObject.tag != "Flamethrower" )
		{
			transform.Translate(0f,-hit.distance+CollisionToDownLenght+1f,0f);
		}
		Debug.DrawRay(pos, -CollisionToDownLenght*transform.up, Color.green);
	}
	
	private float cmftd = 0;
	private bool _canmoveforward = true;
	private bool CanMoveForward()
	{
		cmftd -= Time.deltaTime;
		if(cmftd > 0 ) return _canmoveforward;
		cmftd = 1.1f;
		_canmoveforward = CanMoveForwardHelper();
		return _canmoveforward;
	
	}
	private bool CanMoveForwardHelper()
	{
		float forwarddist = 0.5f;
		float leftrightdist = 0.3f;
		float diagonaldist = 0.7f;
		RaycastHit hit;    
		Vector3 pos = transform.position; pos.y += 0.4f;
		Debug.DrawRay(pos, forwarddist*transform.forward, Color.green);
		Debug.DrawRay(pos, leftrightdist*transform.right, Color.green);
		Debug.DrawRay(pos, -leftrightdist*transform.right, Color.green);
		Debug.DrawRay(pos, diagonaldist*0.5f*(transform.right+transform.forward), Color.green);
		Debug.DrawRay(pos, diagonaldist*0.5f*(-transform.right+transform.forward), Color.green);
		if(Physics.Raycast(pos, transform.forward, out hit, forwarddist))
			return false;
		if(Physics.Raycast(pos, transform.right, out hit, leftrightdist))
			if( transform.position.magnitude > hit.collider.gameObject.transform.position.magnitude )
				return false;
		if(Physics.Raycast(pos, -transform.right, out hit, leftrightdist))
			if( transform.position.magnitude > hit.collider.gameObject.transform.position.magnitude )
				return false;
		if(Physics.Raycast(pos, 0.5f*(transform.right+transform.forward), out hit, diagonaldist))
			if( transform.position.magnitude > hit.collider.gameObject.transform.position.magnitude )
				return false;
		if(Physics.Raycast(pos, 0.5f*(-transform.right+transform.forward), out hit, diagonaldist))
			if( transform.position.magnitude > hit.collider.gameObject.transform.position.magnitude )
				return false;
		return true;
	}
	
	void OnGUI()
	{
		// showing healt bar
		
		if( Time.timeScale == 0.0f ) return;
		
		Vector3 head = headHit.transform.position; head.y += 0.5f;
		Vector3 pos = LevelInfo.Environments.mainCamera.WorldToScreenPoint(head);
		if( pos.z > 0 )
		{
			pos.y = Screen.height - pos.y;
			pos.z = 1f;
		
			GUI.DrawTexture(new Rect(pos.x-20,pos.y,40,5),LevelInfo.Environments.ProgressBarZombieEmpty);
			GUI.DrawTexture(new Rect(pos.x-20,pos.y,damage*4,5),LevelInfo.Environments.ProgressBarZombieFull);
		}
		//GUI.Label(new Rect(0,100,200,200),"Dist " + transform.position.magnitude );
	}
	
	#endregion
	
	#region Zombie Get Hit Setup
	
	void OnTriggerStay(Collider col)
	{	
		// Zapper Attack
		if( HitWithName(col.gameObject.name,"Zapper") )
		{
			smoking -= Time.deltaTime;
			ZombieSmoke.particleEmitter.minSize = ZombieSmoke.particleEmitter.maxSize = (1-smoking)*1f;
			animation.Play("zap");
			if( smoking <= 0 )
			{
				DieNormal();
				return;
			}
		}
		
		// Flame Attack
		if( HitWithName(col.gameObject.name,"Flame") )
		{
			flaming -= Time.deltaTime;
			ZombieFire.particleEmitter.minSize = ZombieFire.particleEmitter.maxSize = (1-flaming)*1f;
			if( flaming <= 0 )
			{
				DieNormal();
				return;
			}
		}	
	}
	
	public void GetHitDamaged(int hitpoints)
	{
		if( LevelInfo.Environments.control.DamageMultiplied ) hitpoints *= 4;
		damage -= hitpoints;
		animation.Play("get hit");
		animation["get hit"].time = 0.0f;
		if( damage <= 0 )
		{
			if( haveHelmet )
			{
				haveHelmet = false;
				
				GameObject g = (GameObject)Instantiate(HelmetPrefab,HelmetPrefab.transform.position,HelmetPrefab.transform.rotation);
				g.transform.localScale = HelmetPrefab.transform.lossyScale;
				g.AddComponent<ThrowingOut>();
				
				HelmetPrefab.SetActiveRecursively(false);
				
				damage = 10;
			}
			else
				DieNormal();
		}
	}
	
	public void GetHit()
	{
		animation.Play("get hit");
		animation["get hit"].time = 0.0f;
	}
	
	#endregion
	
	#region Die
	
	private bool died = false;
	
	public void DieNormal()
	{
		if( died ) return;
		died = true;
		LevelInfo.Environments.control.GetZombie();
		GameObject g = (GameObject)Instantiate(ZombieRagdoll,transform.position,transform.rotation);
		ZombieSmoke.particleEmitter.minSize = ZombieSmoke.particleEmitter.maxSize = (1-smoking)*1f;
		ZombieSmoke.particleEmitter.minSize = ZombieSmoke.particleEmitter.maxSize = (1-smoking)*1f;
		g.SendMessage("SetFireSize",ZombieFire.particleEmitter.maxSize);
		g.SendMessage("SetSmokeSize",ZombieSmoke.particleEmitter.maxSize);
		
		var rigidbodies = g.GetComponentsInChildren(typeof(Rigidbody));
		Vector3 dir = transform.position - LevelInfo.Environments.control.transform.position; dir.Normalize();
        foreach (Rigidbody child in rigidbodies) 
			child.AddForce(200f*dir);
		
		Destroy(this.gameObject);
	}
	
	public void DieWithJump()
	{
		if( died ) return;
		died = true;
		LevelInfo.Environments.control.GetZombie();
		
		// Adding Force
		GameObject ragdoll = (GameObject)Instantiate(ZombieRagdoll,transform.position,transform.rotation);
		ragdoll.SendMessage("SetFireSize",ZombieFire.particleEmitter.maxSize);
		ragdoll.SendMessage("SetSmokeSize",ZombieSmoke.particleEmitter.maxSize);
		var rigidbodies = ragdoll.GetComponentsInChildren(typeof(Rigidbody));
        foreach (Rigidbody child in rigidbodies) 
			child.AddForce(new Vector3(Random.Range(-320f,320f),320f,Random.Range(-320f,320f)));
		
		Destroy(this.gameObject);
	}
	
	public void DieWithFootball()
	{
		if( died ) return;
		died = true;
		LevelInfo.Environments.control.GetZombie();
		GameObject g = (GameObject)Instantiate(ZombieRagdoll,transform.position,transform.rotation);
		ZombieSmoke.particleEmitter.minSize = ZombieSmoke.particleEmitter.maxSize = (1-smoking)*1f;
		ZombieSmoke.particleEmitter.minSize = ZombieSmoke.particleEmitter.maxSize = (1-smoking)*1f;
		g.SendMessage("SetFireSize",ZombieFire.particleEmitter.maxSize);
		g.SendMessage("SetSmokeSize",ZombieSmoke.particleEmitter.maxSize);
		
		var rigidbodies = g.GetComponentsInChildren(typeof(Rigidbody));
		Vector3 dir = transform.position - LevelInfo.Environments.control.transform.position; dir.Normalize(); dir.y =0.5f;
        foreach (Rigidbody child in rigidbodies) 
			child.AddForce(400f*dir);
		
		Destroy(this.gameObject);	
	}
	
	public void ThrowOut()
	{
		swipeUpcaution.Deactivate();
		
		GameObject ragdoll = (GameObject)Instantiate(ZombieRagdoll,transform.position,transform.rotation);
		ragdoll.SendMessage("SetFireSize",ZombieFire.particleEmitter.maxSize);
		ragdoll.SendMessage("SetSmokeSize",ZombieSmoke.particleEmitter.maxSize);
		var rigidbodies = ragdoll.GetComponentsInChildren(typeof(Rigidbody));
		Vector3 dir = transform.position - LevelInfo.Environments.control.transform.position; dir.Normalize(); dir.y = 0.5f;
        foreach (Rigidbody child in rigidbodies) 
			child.AddForce(300f*dir);
		ragdoll.SendMessage("ThrowedOut");
		Destroy(this.gameObject);
	}
	
	public void DieWithFireAndSmoke()
	{
		flaming = smoking = 0;
		DieNormal();
	}
	
	void OnDestroy()
	{
		Destroy(particleDirtClod);
	}
	
	#endregion
	
	#region Helpful, Local Properties
	
	private bool HitWithName(string name,string comparewith)
	{
		return name.Length >= comparewith.Length && name.Substring(0,comparewith.Length) == comparewith;
	}
	
	public float XZDistFromPlayer()
	{
		return Vector3.Distance(transform.position,new Vector3(LevelInfo.Environments.control.transform.position.x,transform.position.y,LevelInfo.Environments.control.transform.position.z));
	}
	
	public bool NearPlayer()
	{
		return XZDistFromPlayer() <= WaitDistance;
	}
	
	#endregion
	
	#region Messages
	
	public void SetFireSize(float f)
	{
		flaming = 1-f;
		ZombieFire.particleEmitter.minSize = f;
		ZombieFire.particleEmitter.maxSize = f;
	}	
	
	public void SetSmokeSize(float f)
	{
		smoking = 1-f;
		ZombieSmoke.particleEmitter.minSize = f;
		ZombieSmoke.particleEmitter.maxSize = f;
	}
	
	public void DontSpawn()
	{
		spawning = false;
	}
	
	#endregion
}
