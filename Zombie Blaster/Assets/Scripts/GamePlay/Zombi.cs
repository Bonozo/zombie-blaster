using UnityEngine;
using System.Collections;

[AddComponentMenu("GamePlay/Zombie")]
public class Zombi : MonoBehaviour {
	
	#region Parameters
	
	public bool scooby;
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
	public Transform head;

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
	public bool sparkWhenShot;
	public GameObject materialGameObject;
	
	
	#endregion
	
	#region Variables

	
	private float flaming = 1f;
	private float smoking = 1f;
	private float damage = 10;
	private bool audioswaonelayed = false;
	private bool spawning = true;
	
	private float RunningTime = 3f;
	private float runningTime = 0.0f;
	
	private HealthBar healthBar;
	private float aliveTime = 0.0f;
	private bool toattack = false;
	private bool startedAttacking = false;
	
	#endregion
	
	#region Start, Update, OnGUI
	
	// Use this for initialization
	void Start () {
		gameObject.AddComponent<AudioSource>();
		
		headHit.tag = "ZombieHead";
		headHit.HeadContainer = this;
		
		haveHelmet = false;
		if( HelmetPrefab != null )
		{
			haveHelmet = Random.Range(0,2)==1;
			HelmetPrefab.SetActiveRecursively(haveHelmet);
		}
		
		transform.position = new Vector3(transform.position.x,ZombieHeight,transform.position.z);
		
		// Institates
		Destroy(Instantiate(LevelInfo.Environments.control.currentLevel % 2 == 1? LevelInfo.Environments.dirtyClodCityPrefab: LevelInfo.Environments.dirtyClodPrefab,
			new Vector3(transform.position.x,1f,transform.position.z),Quaternion.identity),3f);
		
		// Healthbar :))))))))))
		healthBar = ((GameObject)UICamera.Instantiate(LevelInfo.Environments.zombieHealthBar)).GetComponent<HealthBar>();
		healthBar.gameObject.name = name + " health bar";
		healthBar.scoobyArrow.gameObject.SetActive(scooby);
		healthBar.gameObject.transform.parent = LevelInfo.Environments.nGUITopLeftTransform;
		healthBar.gameObject.transform.localPosition = new Vector3(0,0,0);
		healthBar.gameObject.transform.localScale = new Vector3(1,1,1);
		
		// Playing Spawn Animation
		if(spawning)
		{
			animation.Play("spawn");
			LevelInfo.Audio.PlayZombieSpawn();
		}
		
		if( audioAttackwalk == null )
			audioAttackwalk = LevelInfo.Audio.AudioZombieAttackWalk;
		
		NormalizeHeight();
	}
	
	private float shotDeltaTime=0.0f;
	// Update is called once per frame
	void Update () {
		UpdateHealthBar();
		
		if( Time.timeScale==0f ) return;
		
		// look to player.
		Vector3 np = LevelInfo.Environments.control.transform.position-transform.position; np.y = 0;
		transform.rotation = Quaternion.LookRotation(np,Vector3.up);
		
		if( transform.position.y < -10f )
		{
			LevelInfo.Environments.hubZombiesLeft.SetNumber(LevelInfo.Environments.hubZombiesLeft.GetNumber()-1);
			Destroy(this.gameObject);
		}
		
		if( animation.IsPlaying("spawn") ) return;
		if( animation.IsPlaying("jump") ) return;
		if( animation.IsPlaying("shoot") ) 
		{
			if( animation["shoot"].time >= shootTime )
			{
				float alltime = animation["shoot"].length-shootTime;
				float curtime = animation["shoot"].length-animation["shoot"].time;
				alltime*=0.5f; curtime -= alltime; if(curtime<0) curtime = 0;
				float cp = 0.75f+0.25f*curtime/alltime;
				
				shotDeltaTime -= Time.deltaTime;
				
				if( sparkWhenShot && animation["shoot"].time >= shootTime+0.1f )
					materialGameObject.renderer.material.color = new Color(cp,cp,cp,1f);
				
				if( shotDeltaTime<=0 )
				{
					if( shootAudio != null ) LevelInfo.Audio.audioSourceZombies.PlayOneShot(shootAudio);
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
			LevelInfo.Environments.swipeUpCaution.Activate(transform.position);
				
			if(!startedAttacking) { LevelInfo.Environments.control.ZombieStartsAttacking(this); startedAttacking=true; }
			
			if( !animation.IsPlaying("attack01") && !animation.IsPlaying("attack02") )
			{
				animation.Play(Random.Range(1,3)==1?"attack01":"attack02");
				toattack = true;
				/*if( flaming <= 0.75f )
					LevelInfo.Environments.control.GetBite(2*BitePoint);
				else
					LevelInfo.Environments.control.GetBite(BitePoint);*/
			}
			else 
			{
				if(toattack && (animation["attack01"].time >=0.3f || animation["attack02"].time >= 0.3f) )
				{
					LevelInfo.Environments.control.GetBite(BitePoint*(flaming<=0.75f?2:1));
					toattack = false;
				}
			}
			
		}
		// If Far Update
		else
		{
			/// Shoot
			if( runningTime <= 0f &&  canShoot && Random.Range(0,300)==1 )
			{
				animation.Play("shoot");
				return;
			}
			// Jump
			if( runningTime <= 0f && canJump && Random.Range(0,300)==1)
			{
				if( jumpAudio != null ) LevelInfo.Audio.audioSourceZombies.PlayOneShot(jumpAudio,0.5f);
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
		nhdt = 0.291f;
		RaycastHit hit;    
		
		Vector3 pos = transform.position;
		pos.y += 1f;
		
		/*if(Physics.Raycast(pos, -transform.up, out hit, CollisionToDownLenght+0.01f+1f) && 
			hit.collider.gameObject.tag != "Flamethrower" )
			transform.Translate(0,0.02f,0);
		if( !Physics.Raycast(pos, -transform.up, out hit, CollisionToDownLenght-0.03f+1f) )
			transform.Translate(0,-0.02f,0);
		*/
		if(Physics.Raycast(pos, -transform.up, out hit, CollisionToDownLenght+10f) && hit.collider.gameObject.name != "Flame" )
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

	
	void UpdateHealthBar()
	{
		if( healthBar == null ) return;
		Vector3 headpos = head!=null?head.transform.position:headHit.transform.position;
		
		aliveTime += Time.deltaTime;
		bool show = false;
		if( LevelInfo.Environments.control.state != GameState.Store ) 
		{
			headpos.y += 0.5f;
			Vector3 pos = LevelInfo.Environments.mainCamera.WorldToScreenPoint(headpos);
			//Debug.Log(pos);
			if( pos.z > 0 )
			{
				show = true;
				
				pos.y = Screen.height - pos.y;
				pos.z = 1f;		
				 
				healthBar.back.transform.localPosition = new Vector3(pos.x-20,-pos.y,0f);
				healthBar.front.transform.localPosition = new Vector3(pos.x-20,-pos.y,0f);
				healthBar.front.transform.localScale = new Vector3(damage*4,healthBar.front.transform.localScale.y,healthBar.front.transform.localScale.z);
				  
				if( scooby )
				{
					//float delta = aliveTime%2f; if( delta > 1f) delta = 2-delta; delta *= 25f;
					float delta = 12.5f+Mathf.Sin(4*aliveTime)*12.5f;
					healthBar.scoobyArrow.transform.localPosition = new Vector3(pos.x-15,-pos.y+50+delta,0f);
				}
			}
		}
		
		healthBar.gameObject.SetActive(show);
		healthBar.scoobyArrow.gameObject.SetActive(scooby);
	}
	
	/*void OnGUI()
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
	}*/
	
	#endregion
	
	#region Zombie Get Hit Setup
	
	void OnTriggerEnter(Collider col)
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
	
	/*public void GetHitFinished(Weapon weapon,Vector3 contactpoint,int hitpoints)
	{
		bool headhit = contactpoint.y>head.position.y-0.3f;
		if( animation.IsPlaying("spawn") ) headhit = Mathf.Abs(contactpoint.y-head.position.y) <= 0.3f;
		
		if( headhit )
			LevelInfo.Environments.generator.GenerateMessageText(head.position + new Vector3(0f,0.75f,0),"Headshot");
		
		if( LevelInfo.Environments.control.DamageMultiplied ) hitpoints *= 4;
		damage -= hitpoints;
		if( headhit && !haveHelmet ) damage = 0;
		animation.Play("get hit");
		animation["get hit"].time = 0.0f;
		
		if( damage <= 0 )
		{
			if(haveHelmet && ( weapon == Weapon.BB || weapon == Weapon.Crossbow || weapon == Weapon.MachineGun ) )
			{
				haveHelmet = false;
				
				GameObject g = (GameObject)Instantiate(HelmetPrefab,HelmetPrefab.transform.position,HelmetPrefab.transform.rotation);
				g.transform.localScale = HelmetPrefab.transform.lossyScale;
				g.AddComponent<ThrowingOut>();
				
				HelmetPrefab.SetActiveRecursively(false);
				
				damage = 10;		
			}
			else
			{
				if(NearPlayer()) GameObject.Find("Goo").SendMessage("Show");
				Store.zombieHeads++;
				LevelInfo.Environments.control.score += LevelInfo.State.scoreForHeadShot - LevelInfo.State.scoreForZombie;
				LevelInfo.Audio.PlayZombieHeadShot();
				switch(weapon)
				{
				case Weapon.BB:
					DieNormal();
					break;
				}
			}
		}

	}*/
	
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
			child.AddForce(new Vector3(Random.Range(-160f,160f),160f,Random.Range(-160f,160f)));
		
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
			child.AddForce(200f*dir);
		
		Destroy(this.gameObject);	
	}
	
	public void ThrowOut()
	{
		LevelInfo.Environments.swipeUpCaution.Deactivate();
		
		GameObject ragdoll = (GameObject)Instantiate(ZombieRagdoll,transform.position,transform.rotation);
		ragdoll.SendMessage("SetFireSize",ZombieFire.particleEmitter.maxSize);
		ragdoll.SendMessage("SetSmokeSize",ZombieSmoke.particleEmitter.maxSize);
		var rigidbodies = ragdoll.GetComponentsInChildren(typeof(Rigidbody));
		Vector3 dir = transform.position - LevelInfo.Environments.control.transform.position; dir.Normalize(); dir.y = 0.5f;
        foreach (Rigidbody child in rigidbodies) 
			child.AddForce(200f*dir);
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
		if( healthBar != null )
			Destroy(healthBar.gameObject);
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
		scooby = false;
	}
	
	#endregion
}
