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
				lives--;
				break;
			case GameState.Play:
				Time.timeScale = 1f;
				break;
			case GameState.Paused:
				guiPlayGame.SetActiveRecursively(false);
				LevelInfo.Audio.PlayAudioPause(true);
				Time.timeScale = 0f;
				GameObject.Find("Store").GetComponent<Store>().showStore = true;
				break;
			case GameState.WaveCompleted:
				Time.timeScale = 0;
				waitfornewwave = Time.realtimeSinceStartup + 3f;
				break;
			}
		}
	}
	
	public int currentLevel = 0;
	
	public int currentWave = 0;
	
	private int _zombiesLeftForThisWave = 0;
	public int zombiesLeftForThisWave { get { return _zombiesLeftForThisWave; } set {_zombiesLeftForThisWave = value; LevelInfo.Environments.guiZombiesLeft.text = "" + _zombiesLeftForThisWave; } }
		
	private static int startScore = 0;
	private int _score = 0;
	public int score { get { return _score; } set {_score = value; LevelInfo.Environments.guiScore.text = "" + _score; } }
	
	private static int startLives = 1;
	private int _lives = 0;
	public int lives { get { return _lives; } set {_lives = value; LevelInfo.Environments.guiLives.text = "" + _lives; } }
	
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
		
		lives = startLives;
		score = startScore;
		
		LevelInfo.Environments.lightSpot.SetActiveRecursively(Option.SpotLight);
		LevelInfo.Environments.lightDirectional.SetActiveRecursively(!Option.SpotLight);
		LevelInfo.Environments.lightSpot.GetComponent<Light>().spotAngle = Option.SpotLightAngle;
		
		LevelInfo.Environments.guiDamageMultiplier.gameObject.SetActiveRecursively(false);
		
		RenderSettings.fog = Option.Fog;
		RenderSettings.ambientLight = new Color(Option.BackgroundAmbient/256f,Option.BackgroundAmbient/256f,Option.BackgroundAmbient/256f);
		
		LevelInfo.Environments.generator.GenerateMessageText(new Vector3(0.2f,0.2f,10),"TAP TO SHOOT and SWIPE TO TURN",false,4f);
	}
	
	// Update is called once per frame
	void Update () 
	{
		//??//
		if( Input.GetKeyUp(KeyCode.H) )
			health -= 0.1f;
		if( Input.GetKeyUp(KeyCode.J) )
			health += 0.1f;
	
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
		
		
		Vector3 rot = transform.rotation.eulerAngles;
		if(rot.y >= 180.0f ) rot.y -= 360f;
		
		rot.y += Speed*Time.deltaTime*GameEnvironment.Swipe;
		rot.y += Speed*Time.deltaTime*Input.GetAxis("Horizontal"); // for PC version test.
		
		transform.rotation = Quaternion.Euler(rot);
		
		UpdateHealthBar();
		LevelInfo.Environments.guiZombieHeads.text = "" + Store.zombieHeads;
	}
	
	void OnGUI()
	{
		switch(state)
		{
		case GameState.Lose:
			if( lives > 0 )
			{
				GUI.Box(new Rect(0.25f*Screen.width,0.25f*Screen.height,0.5f*Screen.width,0.5f*Screen.height),"CONTINUE?");
				if( GUI.Button(new Rect(0.35f*Screen.width,0.4f*Screen.height,0.3f*Screen.width,0.1f*Screen.height), "CONTINUE" ) )	
				{
					GameEnvironment.StartWave = currentWave-1;
					startLives = lives;
					startScore = score;
					
					Time.timeScale = 1.0f;
					Application.LoadLevel(Application.loadedLevel);
				}
				if( GUI.Button(new Rect(0.35f*Screen.width,0.6f*Screen.height,0.3f*Screen.width,0.1f*Screen.height), "MENU" ) )
				{
					Time.timeScale = 1.0f;
					Application.LoadLevel("mainmenu");
				}			
			}
			else
			{
				GUI.Box(new Rect(0.25f*Screen.width,0.25f*Screen.height,0.5f*Screen.width,0.5f*Screen.height),"YOU ARE DEAD.");
				if( GUI.Button(new Rect(0.35f*Screen.width,0.5f*Screen.height,0.3f*Screen.width,0.1f*Screen.height), "MENU" ) )
				{
					Time.timeScale = 1.0f;
					Application.LoadLevel("mainmenu");
				}
			}
			break;
		case GameState.WaveCompleted:
			GUI.Box(new Rect(0.25f*Screen.width,0.25f*Screen.height,0.5f*Screen.width,0.5f*Screen.height),"WAVE COMPLETE.");
			
			GUI.Label(new Rect(0.35f*Screen.width,0.5f*Screen.height,0.3f*Screen.width,0.1f*Screen.height), "REWARD" ) ;

			GUI.DrawTexture(new Rect(0.525f*Screen.width,0.47f*Screen.height,0.1f*Screen.width,0.1f*Screen.height),(bonusForWaveComplete==0?LevelInfo.Environments.texturePickUpXtraLife:LevelInfo.Environments.texturePickUpBonusHeads) );
			
			if( Time.realtimeSinceStartup > waitfornewwave )
				PrepareAndCreateNewWave();
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
			if( Mathf.Abs(c.y - angleY) <= 1f )	{ c.y = angleY; angling = false; }
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
		GameObject h = LevelInfo.Environments.healthbarHealth;
		Vector3 v = h.transform.localScale;
		v.x = Mathf.Clamp01(healthshow/LevelInfo.State.playerMaxHealth);
		h.transform.localScale = v;
		
		v = h.transform.localPosition;
		v.x = -0.01500002f-0.5f*(1-h.transform.localScale.x);
		h.transform.localPosition = v;
		
	}
	
	
	#endregion
	
	#region Properties
	
	public bool Died { get { return healthshow <= 0; }}
	
	public float Health { get { return healthshow; } set { health = value; } }
	
	#endregion

	#region Other Methods
	
	public void GetBite(float lostHealth)
	{
		LevelInfo.Audio.PlayPlayerGetHit();
		GetHealth(-lostHealth);
	}
	
	public void GetZombie()
	{
		score += LevelInfo.State.scoreForZombie;
		zombiesLeftForThisWave--;
		if( zombiesLeftForThisWave <= 0 && GameObject.FindGameObjectsWithTag("ZombieHead").Length == 1)
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
		
		LevelInfo.Audio.audioSourceZombies.PlayOneShot(LevelInfo.Audio.AudioWaveComplete);
		bonusForWaveComplete = Random.Range(0,2);
		state = GameState.WaveCompleted;
	}
	
	public void PrepareAndCreateNewWave()
	{
		LevelInfo.Audio.audioSourceZombies.Stop();
		if( bonusForWaveComplete==0) lives++;
		else Store.zombieHeads= Store.zombieHeads + 100;
		state = GameState.Play;
		MoveTo(VantagePoints[currentWave%VantagePoints.Length]);
	}
	
	public void CreateNewZombieWave()
	{	
		currentWave++;

		zombiesLeftForThisWave = LevelInfo.State.zombiesCountFactor*currentWave;
		LevelInfo.Environments.generator.StartNewWave(zombiesLeftForThisWave);
		
		LevelInfo.Environments.waveInfo.ShowWave(currentWave,zombiesLeftForThisWave);
		LevelInfo.Audio.PlayLevel(currentWave);
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
		
		LevelInfo.Environments.guiDamageMultiplier.gameObject.SetActiveRecursively(true);
		
		damageMultiplieTime = Time.time + 30f;
		while( Time.time < damageMultiplieTime )
		{
			LevelInfo.Environments.guiDamageMultiplier.text = "" + (int)(damageMultiplieTime-Time.time+1);
			yield return new WaitForEndOfFrame();
		}
		
		LevelInfo.Environments.guiDamageMultiplier.gameObject.SetActiveRecursively(false);
		DamageMultiplied = false;
	}
	
	public void DamageMultiply()
	{
		if( DamageMultiplied ) damageMultiplieTime = Time.time + 30;
		else StartCoroutine(DamageMultiplyThread());
	}
	
	#endregion
}