using UnityEngine;
using System.Collections;

public class Control : MonoBehaviour {
	
	#region Parameters
	
	public GameObject guiPlayGame;
	
	public float Speed = 3f;
	
	public Vector3[] VantagePoints;
	
	public bool DamageMultiplied = false;
	private float damageMultiplieTime = 0f;
	
	#endregion
	
	#region Variables
	
	private float health = 1f;
	private float healthshow = 1f;
	private float waitfornewwave = 3f;
	
	private bool restartLevel = false;
	
	#endregion
	
	#region State
	
	public GameState _state = GameState.Play;
	public GameState state
	{
		get 
		{
			return _state;
		}
		set
		{
			if( _state == GameState.Paused && value != GameState.Paused)
			{
				LevelInfo.Audio.PlayAudioPause(false);
				guiPlayGame.SetActiveRecursively(true);
			}
			
			_state = value;
			
			switch(_state)
			{
			case GameState.Lose:
				Time.timeScale = 0f;
				LevelInfo.Environments.hubLives.SetNumberWithFlash(LevelInfo.Environments.hubLives.GetNumber()-1);
				break;
			case GameState.Play:
				Time.timeScale = 1f;
				break;
			case GameState.Paused:
				LevelInfo.Audio.StopEffects();
				guiPlayGame.SetActiveRecursively(false);
				LevelInfo.Audio.PlayAudioPause(true);
				Time.timeScale = 0f;
				GameObject.Find("Store").GetComponent<Store>().showStore = true;
				break;
			case GameState.WaveCompleted:
				//Time.timeScale = 0;
				//waitfornewwave = Time.realtimeSinceStartup + 3f;
				break;
			}
		}
	}
	
	public int currentLevel = 0;
	
	public int currentWave = 0;
		
	private static int startScore = 0;
	private static int startLives = 1;	
	public void ForceLevel(int levelnumber,int currentwave)
	{
		currentLevel = levelnumber;
		currentWave = currentwave;
		foreach(var c in LevelInfo.State.level)
			c.hierarchyPlace.SetActiveRecursively(false);
		LevelInfo.State.level[currentLevel].hierarchyPlace.SetActiveRecursively(true);
	}
	
	#endregion
	
	#region Start, Update
	
	// Use this for initialization
	void Start ()
	{
		health = Option.Health;
	
		int vi = GameEnvironment.StartWave % VantagePoints.Length;
		transform.position = new Vector3(VantagePoints[vi].x,transform.position.y,VantagePoints[vi].z);
		
		ForceLevel(GameEnvironment.StartLevel,GameEnvironment.StartWave);
		
		CreateNewZombieWave();
		
		LevelInfo.Environments.hubLives.SetNumber(startLives);
		LevelInfo.Environments.hubScore.SetNumber(startScore);
		
		LevelInfo.Environments.lightSpot.SetActive(Option.SpotLight);
		LevelInfo.Environments.lightDirectional.SetActive(!Option.SpotLight);
		LevelInfo.Environments.lightSpot.GetComponent<Light>().spotAngle = Option.SpotLightAngle;
		
		LevelInfo.Environments.guiDamageMultiplier.gameObject.SetActive(false);
		
		RenderSettings.fog = Option.Fog;
		RenderSettings.ambientLight = new Color(Option.BackgroundAmbient/256f,Option.BackgroundAmbient/256f,Option.BackgroundAmbient/256f);
		
		//if( !restartLevel )
		//	StartCoroutine(ShowHints());
		
		LevelInfo.Environments.fpsGUI.SetActive(Option.ShowFPS);
		
		LevelInfo.Environments.hubZombieHeads.SetNumber(Store.zombieHeads);
	}
	
	public IEnumerator ShowTip(string message,float wait)
	{
		while(wait > 0 )
		{
			wait -= Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
		LevelInfo.Environments.generator.GenerateMessageText(new Vector3(0.5f,0.2f,10),message,false,4f);
	}
	
	// Update is called once per frame
	void Update () 
	{
		//??//
		if( Input.GetKeyUp(KeyCode.H) )
			health -= 0.1f;
		if( Input.GetKeyUp(KeyCode.J) )
			health += 0.1f;
	
		ShakeUpdates();
		
		if( state != GameState.Play) return;//expect
		if(Input.GetKey(KeyCode.Escape) )
		{
			LevelInfo.Environments.control.state = GameState.Paused;
			return;
		}
		////
		
		if( Option.UnlimitedHealth ) health = 1.0f;
		
		if( healthshow != health )
		{
			if( healthshow < health ) healthshow += Mathf.Min(Time.deltaTime*1f,health-healthshow);
			if( healthshow > health ) healthshow -= Mathf.Min(Time.deltaTime*1f,healthshow-health);
			if( healthshow <= 0 ) { ToLose(); return; }
		}
		
		if (Moving )
		{
			MovingUpdate();
			return;
		}
		
		if( GameEnvironment.AbsoluteSwipe.y >= 0.3f )
		{
			GameObject[] zombies = GameObject.FindGameObjectsWithTag("Zombie");
			foreach(var z in zombies )
			{
				if( GameEnvironment.DistXZ(z.transform.position,transform.position) <= 1.5f &&
					camera.WorldToScreenPoint(z.transform.position).z > 0 )
				{
					z.SendMessage("ThrowOut");
				}
			}
		}
		
		
		// Rotate Updates
		if( !(currentLevel==0&&currentWave==1) )
		{
			Vector3 rot = transform.rotation.eulerAngles;
			if(rot.y >= 180.0f ) rot.y -= 360f;
		
			rot.y += Speed*Time.deltaTime*GameEnvironment.Swipe;
			rot.y += Speed*Time.deltaTime*Input.GetAxis("Horizontal"); // for PC version test.
		
			transform.rotation = Quaternion.Euler(rot);
		}
		
		UpdateHealthBar();
		LevelInfo.Environments.hubZombieHeads.SetNumberWithFlash(Store.zombieHeads);
	}
	
	void OnGUI()
	{
		switch(state)
		{
		case GameState.Lose:
			if( LevelInfo.Environments.hubLives.GetNumber() > 0 )
			{
				GUI.Box(new Rect(0.25f*Screen.width,0.25f*Screen.height,0.5f*Screen.width,0.5f*Screen.height),"CONTINUE?");
				if( GUI.Button(new Rect(0.35f*Screen.width,0.4f*Screen.height,0.3f*Screen.width,0.1f*Screen.height), "YES" ) )	
				{
					GameEnvironment.StartWave = currentWave-1;
					startLives = LevelInfo.Environments.hubLives.GetNumber();
					startScore = LevelInfo.Environments.hubScore.GetNumber();
					
					restartLevel = true;
					
					Time.timeScale = 1.0f;
					Application.LoadLevel(Application.loadedLevel);
				}
				if( GUI.Button(new Rect(0.35f*Screen.width,0.6f*Screen.height,0.3f*Screen.width,0.1f*Screen.height), "NO") )
				{
					Time.timeScale = 1.0f;
					Application.LoadLevel("mainmenu");
				}			
			}
			else
			{
				GUI.Box(new Rect(0.25f*Screen.width,0.25f*Screen.height,0.5f*Screen.width,0.5f*Screen.height),"YOU ARE DEAD.");
				if( GUI.Button(new Rect(0.35f*Screen.width,0.5f*Screen.height,0.3f*Screen.width,0.1f*Screen.height), "MENU") )
				{
					Time.timeScale = 1.0f;
					Application.LoadLevel("mainmenu");
				}
			}
			break;
		case GameState.Play:
			if( !Moving || angling || Time.realtimeSinceStartup > waitfornewwave ) break;
			GUI.Box(new Rect(0.25f*Screen.width,0.25f*Screen.height,0.5f*Screen.width,0.5f*Screen.height),"WAVE COMPLETE.");
			
			GUI.Label(new Rect(0.35f*Screen.width,0.5f*Screen.height,0.3f*Screen.width,0.1f*Screen.height), "REWARD" ) ;

			GUI.DrawTexture(new Rect(0.525f*Screen.width,0.47f*Screen.height,0.1f*Screen.width,0.1f*Screen.height),(bonusForWaveComplete==0?LevelInfo.Environments.texturePickUpXtraLife:LevelInfo.Environments.texturePickUpBonusHeads) );
			
			//if( Time.realtimeSinceStartup > waitfornewwave )
			//	PrepareAndCreateNewWave();
			break;
		}
	}
	
	private void MovingUpdate()
	{
		if( angling )
		{
			Vector3 c = transform.rotation.eulerAngles;
			if( c.y < angleY ) c.y += 360f;
			// c.y > angleY
			if( c.y - angleY <= 180.0f ) c.y -= MovingRotateSpeed*Time.deltaTime;
			else c.y += MovingRotateSpeed*Time.deltaTime;
			if( Mathf.Abs(c.y - angleY) <= 1f )	{ c.y = angleY; angling = false;  waitfornewwave = Time.realtimeSinceStartup + 3f; }
			transform.rotation = Quaternion.Euler(c);
			return;
		}
		
		Vector3 dir = destination-GameEnvironment.ProjectionXZ(transform.position); dir.Normalize();
		transform.Translate(dir*MovingSpeed*Time.deltaTime,Space.World);
		if( GameEnvironment.DistXZ(transform.position,destination) <= 0.5f )
		{
			Moving = false;
			CreateNewZombieWave();
		}		
	}
	
	#endregion
	
	#region Shake
	
	float shake_decay;
	float shake_intensity;
	
	private void ShakeUpdates()
	{
		transform.rotation =  new Quaternion(0f,transform.rotation.y,0f,transform.rotation.w);	
		if(shake_intensity > 0)
		{
    	    transform.rotation =  new Quaternion(
        	            transform.rotation.x + Random.Range(-shake_intensity,shake_intensity)*.2f,
                        transform.rotation.y + Random.Range(-shake_intensity,shake_intensity)*.05f,
                        transform.rotation.z + Random.Range(-shake_intensity,shake_intensity)*.2f,
                        transform.rotation.w + Random.Range(-shake_intensity,shake_intensity)*.2f);
       		shake_intensity -= shake_decay;
    	}
	}
	
	public void Shake()
	{
    	shake_intensity = 0.07f;
    	shake_decay = 0.001f;
	}
	
	#endregion
	
	#region Moving
	
	public float MovingSpeed = 1f;
	public float MovingRotateSpeed = 10f;
	private bool Moving = false;

	private Vector3 destination;
	private float angleY;
	private bool angling = false;
	
		
	private void MoveTo(Vector3 dest)
	{
		//GameObject[] g = GameObject.FindGameObjectsWithTag("HealthPack");
		//foreach(GameObject gg in g) Destroy(gg);
		
		Moving = true;
		angling = true;
		destination = dest;
		angleY = Quaternion.LookRotation(destination-transform.position,Vector3.up).eulerAngles.y;
	}
	
	#endregion

	#region On GUI
	
	void UpdateHealthBar()
	{
		var v = LevelInfo.Environments.healthbarHealth.transform.localScale;
		v.x = 146.4f*Mathf.Clamp01(healthshow);
		LevelInfo.Environments.healthbarHealth.transform.localScale = v;
		
		v = LevelInfo.Environments.healthbarArmor.transform.localScale;
		v.x = 79f*Mathf.Clamp01(healthshow-1);
		LevelInfo.Environments.healthbarArmor.transform.localScale = v;
	}
	
	
	#endregion
	
	#region Properties
	
	public bool Died { get { return healthshow <= 0; }}
	
	public float Health { get { return healthshow; } set { health = value; } }
	
	public int ZombiesLeft { get { return LevelInfo.Environments.hubZombiesLeft.GetNumber(); } }
	
	#endregion

	#region Other Methods
	
	public void GetBite(float lostHealth)
	{
		LevelInfo.Audio.PlayPlayerGetHit();
		GetHealth(-lostHealth);
	}
	
	public void GetZombie()
	{
		LevelInfo.Environments.hubScore.SetNumberWithFlash(LevelInfo.Environments.hubScore.GetNumber()+LevelInfo.State.scoreForZombie);
		//zombiesLeftForThisWave--;
		LevelInfo.Environments.hubZombiesLeft.SetNumberWithFlash(LevelInfo.Environments.hubZombiesLeft.GetNumber()-1);
		if( LevelInfo.Environments.hubZombiesLeft.GetNumber() <= 0 && GameObject.FindGameObjectsWithTag("ZombieHead").Length == 1)
			StartCoroutine(WaveComplete());
	}
	
	public void GetHealth(float h)
	{
		if( Option.UnlimitedHealth ) return;
		#if UNITY_ANDROID || UNITY_IPHONE
		if( h<0f && Option.Vibration)
				Handheld.Vibrate();
		#endif
		
		health += h;
		if( health > LevelInfo.State.playerMaxHealth ) health = LevelInfo.State.playerMaxHealth;
		if( health <= 0.0f)
		{
			health = 0.0f;
			//ToLose();
		}
	}
	
	private int bonusForWaveComplete = 0;
	public IEnumerator WaveComplete()
	{
		float time = Time.time + 6f;
		while( Time.time < time )
			yield return new WaitForEndOfFrame();
		
		LevelInfo.Audio.audioSourcePlayer.PlayOneShot(LevelInfo.Audio.AudioWaveComplete);
		bonusForWaveComplete = Random.Range(0,2);
		state = GameState.WaveCompleted;
		waitfornewwave = Time.realtimeSinceStartup + 3f;
		PrepareAndCreateNewWave();
	}
	
	public void PrepareAndCreateNewWave()
	{
		LevelInfo.Audio.audioSourceZombies.Stop();
		if( bonusForWaveComplete==0) LevelInfo.Environments.hubLives.SetNumberWithFlash(LevelInfo.Environments.hubLives.GetNumber()+1);
		else Store.zombieHeads= Store.zombieHeads + 100;
		state = GameState.Play;
		MoveTo(VantagePoints[currentWave%VantagePoints.Length]);
	}
	
	private void DestroyAllRemainedWithTag(string tag)
	{
		var c = GameObject.FindGameObjectsWithTag(tag);
		foreach(var d in c) Destroy (d);
	}
	
	public void CreateNewZombieWave()
	{	
		DestroyAllRemainedWithTag("Zombie");
		DestroyAllRemainedWithTag("ZombieRagdoll");
		
		currentWave++;

		LevelInfo.Environments.hubZombiesLeft.SetNumber(LevelInfo.State.zombiesCountFactor*currentWave);
		LevelInfo.Environments.generator.StartNewWave(LevelInfo.Environments.hubZombiesLeft.GetNumber());
		
		LevelInfo.Environments.waveInfo.ShowWave(currentWave,LevelInfo.Environments.hubZombiesLeft.GetNumber());
		LevelInfo.Audio.PlayLevel(currentWave);
		LevelInfo.Audio.audioSourcePlayer.PlayOneShot(LevelInfo.Audio.AudioWaveComplete);
		
		if( currentLevel==0 )
		{
			switch(currentWave)
			{
			case 1:
				StartCoroutine(ShowTip("TIP: TAP TO SHOOT",4f));
				break;
			case 2:
				StartCoroutine(ShowTip("TIP: SWIPE TO TURN",4f));
				break;
			case 3:
				StartCoroutine(ShowTip("TIP: SHOOT RARE ZOMBIES FOR WEAPONS",4f));
				break;
			}
		}
	}
	
	public void ToLose()
	{
		LevelInfo.Audio.StopAll();
		LevelInfo.Audio.PlayGameOver();
		audio.Stop();
		state = GameState.Lose;
		Time.timeScale = 0.0f;	
	}
	
	private IEnumerator DamageMultiplyThread()
	{
		DamageMultiplied = true;
		
		
		
		damageMultiplieTime = Time.time + 30f;
		while( Time.time < damageMultiplieTime )
		{
			LevelInfo.Environments.guiDamageMultiplier.gameObject.SetActive(true);
			LevelInfo.Environments.guiDamageMultiplier.text = "" + (int)(damageMultiplieTime-Time.time+1);
			yield return new WaitForEndOfFrame();
		}
		
		LevelInfo.Environments.guiDamageMultiplier.gameObject.SetActive(false);
		DamageMultiplied = false;
	}
	
	public void DamageMultiply()
	{
		if( DamageMultiplied ) damageMultiplieTime = Time.time + 30;
		else StartCoroutine(DamageMultiplyThread());
	}
	
	#endregion
}