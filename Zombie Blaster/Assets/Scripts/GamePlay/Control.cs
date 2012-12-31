using UnityEngine;
using System.Collections;
using System.Xml;
using System;

public class Control : MonoBehaviour {

	#region LeaderBoardDemo
	
	string getScoreResponse;
	string postScoreResponse;
	
	string []lines;
	
	string []splitByID;
	string []splitByNAME;
	string []splitBySCORE;
	
	string ID;
	string NAME;
	string SCORE;
	
	
	string []idArray = new string[500];
	string []nameArray = new string[500];
	string []scoreArray = new string[500];	
	
	int j = 0;
	int k = 0;
	int l = 0;
	
	int noOfTimesScoreDisplay;
	
	bool isGet;
	bool isPost;
	bool isScoreDisplayed;
	bool isName;
	bool isConnectionError1;
	bool isLeaderBoard;
	
	const float yPosition = 0.03f;
	
	string scoreLB;
	string nameLB;
	
	#endregion
	
	#region Parameters
	
	public GameObject guiPlayGame;
	public GUIStyle myGUIStyle; 	
	public GUIStyle postGUIStyle;
	public Texture texturePopup;
	
	public float Speed = 3f;
	
	public Vector3[] VantagePoints;
	
	public bool DamageMultiplied = false;
	private float damageMultiplieTime = 0f;
	
	#endregion
	
	#region Variables
	
	private float health = 1f;
	private float healthshow = 1f;
	private float showWaveCompleteTime = 3f;
	private float waitfornewwave = 0f;
	
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
				//LevelInfo.Audio.PlayAudioPause(true);
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
		#region LeaderBoardDemo
		Time.timeScale = 1;
		noOfTimesScoreDisplay = 0;
		
		isGet = false;
		isPost = false;
		isScoreDisplayed = false;
		isName = false;
		isConnectionError1 = false;
		isLeaderBoard = false;
		
		getScoreResponse = "";
		postScoreResponse = "";
		
		nameLB = "";
		scoreLB = "";
		
		#endregion
		
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

    IEnumerator WaitForRequest(WWW www)
    //void ResponseData()
	{
        yield return www;
		
        if (www.error == null && isGet == true)
        {
			isConnectionError1 = false;
			
			getScoreResponse = www.text;
			//getScoreResponse = "<gamescore><status>Top game store list</status><S_ID>2</S_ID><S_NAME>Jerry</S_NAME><S_SCORE>100</S_SCORE><S_ID>1</S_ID><S_NAME>Jam</S_NAME><S_SCORE>5</S_SCORE><RequestParam><method>Score details</method></RequestParam></gamescore>";
			//Debug.Log(getScoreResponse);
			
			splitByID = new string [] {"<S_ID>"};
			splitByNAME = new string [] {"<S_NAME>"};
			splitBySCORE = new string [] {"<S_SCORE>"};
			
			string []linesID = getScoreResponse.Split(splitByID, StringSplitOptions.None);
			string []linesNAME = getScoreResponse.Split(splitByNAME, StringSplitOptions.None);
			string []linesSCORE = getScoreResponse.Split(splitBySCORE, StringSplitOptions.None);
			
			
			#region ID
			for(int i = 1; i < linesID.Length; i++)
			{
					
				//Debug.Log(linesID[i]);
				
				foreach(char c in linesID[i])
				{
					if(c == '<')
					{
						break;
					}
					else
					{
						ID += c;
					}
				}
				
				if(ID != null)
					idArray[j] = ID;
				
				ID = null;
				j++;
			}
			
			for(int i = 0; idArray[i] == null; i++)
			{
				Debug.Log(idArray[i]);
			}
			#endregion
			
			
			#region NAME
			for(int i1 = 1; i1 < linesNAME.Length; i1++)
			{
					
				//Debug.Log(linesNAME[i1]);
				
				foreach(char c1 in linesNAME[i1])
				{
					if(c1 == '<')
					{
						break;
					}
					else
					{
						NAME += c1;
					}
				}
				
				if(NAME != null)
					nameArray[k] = NAME;
				
				NAME = null;
				k++;
			}
			
			for(int i = 0; nameArray[i] == null; i++)
			{
				Debug.Log(nameArray[i]);
			}
			#endregion
			
			
			#region SCORE			
			for(int i3 = 1; i3 < linesSCORE.Length; i3++)
			{
					
				//Debug.Log(linesSCORE[i3]);
				
				foreach(char c3 in linesSCORE[i3])
				{
					if(c3 == '<')
					{
						break;
					}
					else
					{
						SCORE += c3;
					}
				}
				
				if(SCORE != null)
					scoreArray[l] = SCORE;
				
				SCORE = null;
				l++;
			}
			
			for(int i = 0; scoreArray[i] == null; i++)
			{
				Debug.Log(scoreArray[i]);
			}
			#endregion				
			
		} 
		else if(www.error != null && isGet == true) 
		{
			isConnectionError1 = true;
            //Debug.Log("WWW Error: "+ www.error);
			getScoreResponse = "Error in Network Connection!";
        } 
		
		if(www.error == null && isPost == true)
		{
			postScoreResponse = "Score is successfully posted!";
			//Debug.Log("WWW Ok!: " + www.data);
		}	
		else if(www.error != null && isPost == true)
		{
			postScoreResponse = "Error in Network Connection!!";
	         //Debug.Log("WWW Error: "+ www.error);
		}	
    }	
	
	// Update is called once per frame
	void Update () 
	{
		//??//
		if( Input.GetKeyUp(KeyCode.H) )
			health -= 0.1f;
		if( Input.GetKeyUp(KeyCode.J) )
			health += 0.1f;
	
		// Update Zombie Heads Number On Screen
		LevelInfo.Environments.hubZombieHeads.SetNumberWithFlash(Store.zombieHeads);
		
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
	}
	
	public bool allowShowGUI = true;
	void OnGUI()
	{
		if( !allowShowGUI) return;
		switch(state)
		{
		case GameState.Lose:
			GUI.DrawTexture(new Rect(0.1f*Screen.width,0.1f*Screen.height,0.8f*Screen.width,0.8f*Screen.height),texturePopup);
			if( (LevelInfo.Environments.hubLives.GetNumber() > 0) && (isLeaderBoard == false))
			{
				//GUI.Box(new Rect(0.25f*Screen.width,0.25f*Screen.height,0.5f*Screen.width,0.5f*Screen.height),"CONTINUE?");
				GUI.Label(new Rect(0.3f*Screen.width,0.28f*Screen.height,0.5f*Screen.width,0.2f*Screen.height),"CONTINUE?",myGUIStyle);
				if( GUI.Button(new Rect(0.27f*Screen.width,0.67f*Screen.height,0.15f*Screen.width,0.05f*Screen.height), "YES" ) )	
				{
					isLeaderBoard = false;
					GameEnvironment.StartWave = currentWave-1;
					startLives = LevelInfo.Environments.hubLives.GetNumber();
					startScore = LevelInfo.Environments.hubScore.GetNumber();
					
					restartLevel = true;
					
					Time.timeScale = 1.0f;
					Application.LoadLevel(Application.loadedLevel);
				}
				if( GUI.Button(new Rect(0.58f*Screen.width,0.67f*Screen.height,0.15f*Screen.width,0.05f*Screen.height), "NO") )
				{
					isLeaderBoard = true;
					//Time.timeScale = 1.0f;
					//Application.LoadLevel("mainmenu");
				}
			}
			
			else
			{
				GUI.Label(new Rect(0.3f*Screen.width,0.28f*Screen.height,0.5f*Screen.width,0.2f*Screen.height),"YOU ARE DEAD.",myGUIStyle);
				//Debug.Log("R");
				#region Post, Get Best Score
				/*if( (GUI.Button(new Rect(0.27f*Screen.width,0.67f*Screen.height,0.15f*Screen.width,0.05f*Screen.height), "Get Top Score")) && (isScoreDisplayed == false) )
				{	
		        	string url = "http://crustdesigns.com/demo/game/gettopscore.php?top=10&format=xml";
		        	WWW www = new WWW(url);
		        	StartCoroutine(WaitForRequest(www));
				
					isPost = false;
					isGet = true;
					
					isScoreDisplayed = true;	
				}*/
				if( GUI.Button(new Rect(0.3f*Screen.width,0.67f*Screen.height,0.15f*Screen.width,0.05f*Screen.height), "Post Score") )
				{			
					isGet = false;
					isPost = true;
										
					isScoreDisplayed = false;					
				}
				
				if(isGet)
				{
					if(isConnectionError1)
						getScoreResponse = "Error in Network Connection!";
					else
						getScoreResponse = "Waiting for Response..";
					
					if(nameArray[0] != null && isConnectionError1 == false )
					{	
						GUI.Label(new Rect(0.38f*Screen.width,0.32f*Screen.height,0.15f*Screen.width,0.03f*Screen.height), "" + "NAME",postGUIStyle);				
						
						GUI.Label(new Rect(0.50f*Screen.width,0.32f*Screen.height,0.15f*Screen.width,0.03f*Screen.height), "" + "SCORE",postGUIStyle);					
						
						for(int i = 0; i < 10; i++)
						{	
							GUI.Label(new Rect(0.38f*Screen.width, (0.35f + yPosition*i)*Screen.height, 0.15f*Screen.width,0.03f*Screen.height), "" + nameArray[i].ToString(),postGUIStyle);
						}	
							//GUI.Label(new Rect(400,40, 200, 70), "" + scoreArray[i].ToString());
							
							//GUI.Label(new Rect(200,40 + yPosition*(i+1), 200, 70), "" + nameArray[i].ToString());
						for(int i = 0; i < 10; i++)
						{			
							GUI.Label(new Rect(0.50f*Screen.width, (0.35f + yPosition*i)*Screen.height, 0.15f*Screen.width,0.03f*Screen.height), "" + scoreArray[i].ToString(),postGUIStyle);
						}
					}
					else
					{
						GUI.Label(new Rect(0.35f*Screen.width,0.35f*Screen.height,0.30f*Screen.width,0.032f*Screen.height), getScoreResponse.ToString(),postGUIStyle);
					}
					
					//isGet = false;				
				}
				
				if(isPost)
				{
					//GUI.Label(new Rect(100, 60, 200, 70), getScoreResponse.ToString());
					GUI.Label(new Rect(0.35f*Screen.width,0.355f*Screen.height, 0.1f*Screen.width,0.05f*Screen.height), "NAME: ",postGUIStyle);
					GUI.Label(new Rect(0.35f*Screen.width,0.42f*Screen.height, 0.1f*Screen.width,0.05f*Screen.height), "SCORE: ",postGUIStyle);
					
					nameLB = GUI.TextField(new Rect(0.48f*Screen.width,0.35f*Screen.height, 0.2f*Screen.width,0.05f*Screen.height), nameLB,12,postGUIStyle);
					GUI.Label(new Rect(0.48f*Screen.width,0.42f*Screen.height, 0.2f*Screen.width,0.05f*Screen.height), (LevelInfo.Environments.hubScore.GetNumber()).ToString(),postGUIStyle);
					
					//scoreLB = GUI.TextField(new Rect(0.45f*Screen.width,0.40f*Screen.height, 0.15f*Screen.width,0.035f*Screen.height), scoreLB);

					if( GUI.Button(new Rect(0.45f*Screen.width,0.5f*Screen.height, 0.10f*Screen.width,0.035f*Screen.height), "Post") )
					{
						if(nameLB.Trim().Equals(""))	
						{
							isName = false;
							postScoreResponse = "Please Enter Name!";
						}
						else
						{	
							isName = true;
							string url = "http://crustdesigns.com/demo/game/addscoreresp.php?snm=" + nameLB + "&score=" + (LevelInfo.Environments.hubScore.GetNumber()).ToString() + "&format=xml";
			        		WWW www = new WWW(url);
			        		StartCoroutine(WaitForRequest(www));
						}						
					}
					
					if(!isName || (isPost == true))
						GUI.Label(new Rect(0.38f*Screen.width,0.57f*Screen.height,0.30f*Screen.width,0.032f*Screen.height), postScoreResponse.ToString(),postGUIStyle);					
				}
				#endregion
	
				if( GUI.Button(new Rect(0.58f*Screen.width,0.67f*Screen.height,0.15f*Screen.width,0.05f*Screen.height), "MENU") )
				{
					isLeaderBoard = false;
					//Debug.Log("RR");					
					GameObject.Find("Store").GetComponent<Store>().GoMainMenuFromGamePlay();
					//Application.LoadLevel("mainmenu");
				}
				 
				/*if( GUI.Button(new Rect(0.35f*Screen.width,0.5f*Screen.height,0.3f*Screen.width,0.1f*Screen.height), "MENU") )
				{
					Time.timeScale = 1.0f;
					Application.LoadLevel("mainmenu");
				}*/
			}
			break;
		case GameState.Play:
			if( !Moving || angling )
				break;
			
			
			if( bonusForWaveComplete >= 0 )
			{
				if( Time.time > waitfornewwave )
				{
					LevelInfo.Environments.waveInfo.HideWaveComplete();
					bonusForWaveComplete = -1;
				}
				else if(bonusForWaveComplete<1000 && Time.time > waitfornewwave-showWaveCompleteTime+0.5f)
				{
					// Writing Wave Complete
					//Give reward to the player
					if( bonusForWaveComplete==0) LevelInfo.Environments.hubLives.SetNumberWithFlash(LevelInfo.Environments.hubLives.GetNumber()+1);
					else Store.zombieHeads= Store.zombieHeads + 100;
					// Write To PlayerPrefs
					Store.SetHighestWaveCompleted(currentLevel,currentWave);
					
					LevelInfo.Environments.waveInfo.ShowWaveComplete(bonusForWaveComplete);
					bonusForWaveComplete = 1000;
				}
			}
			/*GUI.Box(new Rect(0.25f*Screen.width,0.25f*Screen.height,0.5f*Screen.width,0.5f*Screen.height),"WAVE COMPLETE.");
			
			GUI.Label(new Rect(0.35f*Screen.width,0.5f*Screen.height,0.3f*Screen.width,0.1f*Screen.height), "REWARD" ) ;

			GUI.DrawTexture(new Rect(0.5f*Screen.width,0.445f*Screen.height,0.15f*Screen.width,0.15f*Screen.height),(bonusForWaveComplete==0?LevelInfo.Environments.texturePickUpXtraLife:LevelInfo.Environments.texturePickUpBonusHeads),ScaleMode.ScaleToFit );
			*/
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
			if( Mathf.Abs(c.y - angleY) <= 1f )	{ c.y = angleY; angling = false;  waitfornewwave = Time.time + 3f; }
			transform.rotation = Quaternion.Euler(c);
			return;
		}
		
		Vector3 dir = destination-GameEnvironment.ProjectionXZ(transform.position); dir.Normalize();
		if( GameEnvironment.DistXZ(transform.position,destination) <= 0.5f )
		{
			if( Time.time > waitfornewwave )
			{
				LevelInfo.Environments.waveInfo.HideWaveComplete();
				Moving = false;
				CreateNewZombieWave();
			}
		}		
		else
			transform.Translate(dir*MovingSpeed*Time.deltaTime,Space.World);
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
        	            transform.rotation.x + UnityEngine.Random.Range(-shake_intensity,shake_intensity)*.2f,
                        transform.rotation.y + UnityEngine.Random.Range(-shake_intensity,shake_intensity)*.05f,
                        transform.rotation.z + UnityEngine.Random.Range(-shake_intensity,shake_intensity)*.2f,
                        transform.rotation.w + UnityEngine.Random.Range(-shake_intensity,shake_intensity)*.2f);
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

	#region Health Bar Update
	
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
	
	private int bonusForWaveComplete = -1;
	public IEnumerator WaveComplete()
	{
		float time = Time.time + 6f;
		while( Time.time < time )
			yield return new WaitForEndOfFrame();
		
		LevelInfo.Audio.audioSourcePlayer.PlayOneShot(LevelInfo.Audio.AudioWaveComplete);
		bonusForWaveComplete = UnityEngine.Random.Range(0,2);
		state = GameState.WaveCompleted;
		waitfornewwave = Time.realtimeSinceStartup + showWaveCompleteTime;
		PrepareAndCreateNewWave();
	}
	
	public void PrepareAndCreateNewWave()
	{
		DestroyAllRemainedWithTag("Zombie");
		DestroyAllRemainedWithTag("ZombieRagdoll");
		
		LevelInfo.Audio.audioSourceZombies.Stop();
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
		currentWave++;

		LevelInfo.Environments.hubZombiesLeft.SetNumber(LevelInfo.State.zombiesCountFactor*currentWave);
		LevelInfo.Environments.generator.StartNewWave(LevelInfo.Environments.hubZombiesLeft.GetNumber());
		
		LevelInfo.Environments.waveInfo.ShowWave(currentWave,LevelInfo.Environments.hubZombiesLeft.GetNumber());
		LevelInfo.Audio.PlayLevel(currentWave);
		LevelInfo.Audio.audioSourcePlayer.PlayOneShot(LevelInfo.Audio.AudioWaveComplete);
		
		if( !restartLevel && currentLevel==0 )
		{
			switch(currentWave)
			{
			case 1:
				StartCoroutine(ShowTip("TIP: TAP TO SHOOT",4f));
				break;
			case 2:
				StartCoroutine(ShowTip("TIP: SWIPE LEFT OR RIGHT TO TURN",4f));
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
