using UnityEngine;
using System.Collections;
using System.Xml;
using System;

[AddComponentMenu("GamePlay/Control")]
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
	
	//*commented for warning*//int noOfTimesScoreDisplay;
	
	bool isGet;
	bool isPost;
	//*commented for warning*//bool isScoreDisplayed;
	bool isName;
	bool isConnectionError1;
	bool isLeaderBoard;
	
	const float yPosition = 0.03f;
	
	//*commented for warning*//string scoreLB;
	string nameLB;
	
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
	
	
	#endregion
	
	#region Parameters
	
	public GameObject guiPlayGame;
	public GameObject guiMap;
	public GUIStyle myGUIStyle; 	
	public GUIStyle buttonGUIStyle;
	public Texture texturePopup;
	
	public float Speed = 3f;
	
	public Vector3[] VantagePoints;
	
	#endregion
	
	#region Variables
	
	private int currentZombiesInWave = 0;
	public int currentHeadshotsInWave = 0;
	
	private float health = 1f;
	private float healthshow = 1f;
	private float armor = 0f;
	private float armorshow = 0f;
	
	
	private float showWaveCompleteTime = 4f;
	private float waitfornewwave = 0f;
	
	private bool restartLevel = false;
	private int lastZombieHeads;
	
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
			}
			
			if( _state == GameState.Store && value != GameState.Store)
			{
				guiPlayGame.SetActive(true);
			}	
			
			if( _state == GameState.Map && value != GameState.Map)
			{
				guiPlayGame.SetActive(true);
				guiMap.SetActive(false);
			}	
			if(_state == GameState.Options && value != GameState.Options)
			{
				InitOptions();
				LevelInfo.Audio.InitVolume();
				guiPlayGame.SetActive(true);
				LevelInfo.Environments.guiOptions.SetActive(false);
			}
			
			GameState laststate = state;
			_state = value;
			
			switch(_state)
			{
			case GameState.Lose:
				Time.timeScale = 0f;
				LevelInfo.Audio.StopAll();
				LevelInfo.Audio.PlayGameOver();
				LevelInfo.Environments.hubLives.SetNumberWithFlash(LevelInfo.Environments.hubLives.GetNumber()-1);
				ZBFacebook.Instance.Init();
				break;
			case GameState.Play:
				Time.timeScale = 1f;
				LevelInfo.Environments.guns.Update();
				
				if(laststate == GameState.Paused)
					LevelInfo.Audio.UnPauseAll();
				
				break;
			case GameState.Paused:
				Time.timeScale = 0f;
				LevelInfo.Audio.PauseAll();
				
				if(laststate == GameState.Play)
					LevelInfo.Audio.PlayAudioPause(true);
				
				System.GC.Collect();
				break;
			case GameState.Store:
				LevelInfo.Audio.StopEffects();
				Time.timeScale = 0f;
				guiPlayGame.SetActive(false);
				LevelInfo.Environments.store.GetComponent<Store>().showStore = true;	
				break;
			case GameState.Map:
				LevelInfo.Audio.StopEffects();
				Time.timeScale = 0f;
				guiPlayGame.SetActive(false);
				guiMap.SetActive(true);
				break;
			case GameState.Options:
				guiPlayGame.SetActive(false);
				LevelInfo.Environments.guiOptions.SetActive(true);
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
		
		/*Instantiate((GameObject)Resources.Load("Environments/"+LevelInfo.State.level[currentLevel].name));
		yield return new WaitForSeconds(0.5f);
		Resources.UnloadUnusedAssets();*/
		
		/*GameObject level = (GameObject)Instantiate(LevelInfo.State.level[currentLevel].hierarchyPlace);
		level.SetActive(true);
		level.transform.parent = GameObject.Find("Environments").transform;*/
		
		/*foreach(var c in LevelInfo.State.level)
			c.hierarchyPlace.SetActiveRecursively(false);
		LevelInfo.State.level[currentLevel].hierarchyPlace.SetActiveRecursively(true);*/
	}
	
	#endregion
	
	#region Awake, Start , Update
	
	void Awake()
	{		
		LevelInfo.Environments.store.showLoadingScreen = false;
		InitOptions();
		
		if( Store.FirstTimePlay )
		{
			LevelInfo.Environments.buttonMap.isEnabled=false;
			LevelInfo.Environments.buttonStore.isEnabled=false;
			LevelInfo.Environments.buttonPauseStore.isEnabled=false;
			LevelInfo.Environments.buttonPauseMap.isEnabled=false;
		}
	}
	
	void InitOptions()
	{
		LevelInfo.Environments.lightSpot.SetActive(Option.SpotLight);
		LevelInfo.Environments.lightDirectional.SetActive(!Option.SpotLight);
		LevelInfo.Environments.lightSpot.GetComponent<Light>().spotAngle = Option.SpotLightAngle;		
		RenderSettings.fog = Option.Fog;
		RenderSettings.ambientLight = new Color(Option.BackgroundAmbient/256f,Option.BackgroundAmbient/256f,Option.BackgroundAmbient/256f);
	}
	
	// Use this for initialization
	void Start ()
	{	
		#region LeaderBoardDemo
		Time.timeScale = 1;
		//*commented for warning*//noOfTimesScoreDisplay = 0;
		
		isGet = false;
		isPost = false;
		//*commented for warning*//isScoreDisplayed = false;
		isName = false;
		isConnectionError1 = false;
		isLeaderBoard = false;
		
		getScoreResponse = "";
		postScoreResponse = "";
		
		nameLB = "";
		//*commented for warning*//scoreLB = "";
		
		#endregion
	
		int vi = GameEnvironment.StartWave % VantagePoints.Length;
		transform.position = new Vector3(VantagePoints[vi].x,transform.position.y,VantagePoints[vi].z);
		
		ForceLevel(GameEnvironment.StartLevel,GameEnvironment.StartWave);
		
		CreateNewZombieWave();
		
		LevelInfo.Environments.hubLives.SetNumber(startLives);
		LevelInfo.Environments.hubScores.SetNumber(startScore);
		
		LevelInfo.Environments.powerupTimeDamageMultiplier.gameObject.SetActive(false);
		LevelInfo.Environments.powerupTimeUnlimitedAmmo.gameObject.SetActive(false);
		LevelInfo.Environments.powerupTimeShilded.gameObject.SetActive(false);
		

		//if( !restartLevel )
		//	StartCoroutine(ShowHints());
		
		LevelInfo.Environments.fpsGUI.SetActive(Option.ShowFPS);
		
		lastZombieHeads = -1;
		LevelInfo.Environments.hubZombieHeads.SetNumber(Store.zombieHeads);
		
		LevelInfo.Environments.fade.Hide(3f);
		
		System.GC.Collect();
	}
	
	void Update () 
	{
		// Testing
		
		if(Input.GetKeyUp(KeyCode.PageUp))
			DamageMultiply();
		if(Input.GetKeyUp(KeyCode.PageDown))
			Shield();
		if(Input.GetKeyUp(KeyCode.Home))
			Rampage();

		if( Input.GetKeyUp(KeyCode.H) )
			health -= 0.1f;
		if( Input.GetKeyUp(KeyCode.J) )
			health += 0.1f;	
		if( Input.GetKeyUp(KeyCode.N) )
			armor -= 0.1f;
		if( Input.GetKeyUp(KeyCode.M) )
			armor += 0.1f;
		if( Input.GetKeyUp(KeyCode.L) )
			GetBite(0.1f);
		
		
		// Update Zombie Heads Number On Screen
		if( ScreenShowed )
			LevelInfo.Environments.hubZombieHeads.SetNumberWithFlash(Store.zombieHeads);
		
		if( Fade.InProcess || prologuecomplete) return;
		
		LevelInfo.Environments.guiPaused.SetActive(state == GameState.Paused);
		
		
		if(lastZombieHeads != Store.zombieHeads && !Store.FirstTimePlay && LevelInfo.Environments.fade.Finished)	
		{
			ShowStoreNotifiaction();
			ShowMapNotification();
			lastZombieHeads = Store.zombieHeads;
		}
		LevelInfo.Environments.notificationStore.gameObject.SetActive(storeNotificationTime&&ScreenShowed);
		LevelInfo.Environments.notificationMap.gameObject.SetActive(mapNotificationTime&&ScreenShowed);
		
		ShakeUpdates();
		
		if( state != GameState.Play) return;
		/*if(Input.GetKey(KeyCode.Escape) )
		{
			//LevelInfo.Environments.control.state = GameState.Paused;
			return;
		}*/
		
		if( Option.UnlimitedHealth ) health = 1.0f;
		
		// Health 
		UpdateHealthBar();
		if( healthshow != health )
		{
			if( healthshow < health ) healthshow += Mathf.Min(Time.deltaTime*1f,health-healthshow);
			if( healthshow > health ) healthshow -= Mathf.Min(Time.deltaTime*1f,healthshow-health);
			if( healthshow <= 0 )
			{
				if( Store.FirstTimePlay )
				{
					if(!prologuecomplete) PrologueComplete();
				}
				else
					state = GameState.Lose; 
				return;
			}
		}
		
		if( armorshow != armor)
		{
			if( armorshow < armor ) armorshow += Math.Min(Time.deltaTime*1f,armor-armorshow);
			if( armorshow > armor ) armorshow -= Math.Min(Time.deltaTime*1f,armorshow-armor);
		}
		
		
		float swipe = GameEnvironment.Swipe;
		if (Moving ) // Moving to next vantage point
		{
			MovingUpdate();
			return;
		}
		
		if( AutoTargeting ) return;
		
		Vector3 abswipe = GameEnvironment.AbsoluteSwipe;
		if( abswipe.y>Mathf.Abs(abswipe.x) && abswipe.y>=0.1f )
		{
			GameObject[] zombies = GameObject.FindGameObjectsWithTag("Zombie");
			bool nearzombie = false;
			
			foreach(var z in zombies )
				if( GameEnvironment.DistXZ(z.transform.position,transform.position) <= 1.5f && camera.WorldToScreenPoint(z.transform.position).z > 0 )
				{
					z.SendMessage("ThrowOut");
					nearzombie = true;
				}
			if(nearzombie) 
				LevelInfo.Audio.PlayPlayerShove();
		}
		
		// Auto Targeting Updates
		if( Option.AutoTargeting )
		{
			bool targetted = false;
			
			if( swipe < 0 )
			{
				if(LeftAttackingZombie != null )
				{
					targetted = true;
					AutoTargetToPoint(LeftAttackingZombie.transform.position,false);
				}
				LeftAttackingZombie = RightAttackingZombie = null;
				if(targetted) return;
			}
			
			if( swipe > 0 )
			{
				if(RightAttackingZombie != null )
				{
					targetted = true;
					AutoTargetToPoint(RightAttackingZombie.transform.position,true);
				}
				LeftAttackingZombie = RightAttackingZombie = null;
				if(targetted) return;
			}
			
			if(leftAutoTargetTime<0f) LeftAttackingZombie = null;
			else leftAutoTargetTime-=Time.deltaTime;
			if(rightAutoTargetTime<0f) RightAttackingZombie = null;
			else rightAutoTargetTime-=Time.deltaTime;
		}
		// Rotate Updates
		if( !(currentLevel==0&&currentWave==1) && !Store.FirstTimePlay )
		{
			Vector3 rot = transform.rotation.eulerAngles;
			if(rot.y >= 180.0f ) rot.y -= 360f;
		
			if(Option.XInversion)
				rot.y -= Speed*Time.deltaTime*swipe;
			else
				rot.y += Speed*Time.deltaTime*swipe;
			if(Option.TiltingMove)
			{
				float tilt = GameEnvironment.InputAxis.y;
				if(Mathf.Abs(tilt)>0.075f) 
				{
					if(tilt>0) tilt-=0.075f; else tilt+=0.075f;
					tilt*=0.5f;
					
					if(Option.XInversion)
						rot.y -= Speed*Time.deltaTime*tilt;
					else
						rot.y += Speed*Time.deltaTime*tilt;
				}
			}
			transform.rotation = Quaternion.Euler(rot);
		}
	}
	
	#endregion

	#region Notifications
	
	public void ShowStoreNotifiaction()
	{
		bool old = storeNotificationTime;
		storeNotificationTime=false;
		LevelInfo.Environments.store.UpdateWeaponAvailable();
		for(int i=0;i<Store.countWeapons;i++)
			if(LevelInfo.Environments.store.WeaponAvailable(i) && Store.CanBuyWeapon(i) )
				storeNotificationTime=true;
		if(!old&&storeNotificationTime)
			LevelInfo.Audio.audioSourcePlayer.PlayOneShot(LevelInfo.Audio.clipUIStar);
	}
	
	public void ShowMapNotification()
	{
		bool old = mapNotificationTime;
		mapNotificationTime=false;
		for(int i=0;i<Store.countLevel;i++)
			if( !Store.LevelUnlocked(i) && Store.zombieHeads >= GameEnvironment.levelPrice[i] )
				mapNotificationTime=true;

		if(!old&&mapNotificationTime) 
			LevelInfo.Audio.audioSourcePlayer.PlayOneShot(LevelInfo.Audio.clipUIStar);
	}
	
	private bool storeNotificationTime=false;
	private bool mapNotificationTime=false;
	
	#endregion

	#region GUI
	
	private bool postedscore = false;
	private bool sharedonfacebook = false;
	
	public bool allowShowGUI = true;
	void OnGUI()
	{
		if( Fade.InProcess) return;
		if( !allowShowGUI) return;
		switch(state)
		{
		case GameState.Lose:
			GUI.DrawTexture(new Rect(0.1f*Screen.width,0.1f*Screen.height,0.8f*Screen.width,0.8f*Screen.height),texturePopup);
			if( (LevelInfo.Environments.hubLives.GetNumber() > 0) && (isLeaderBoard == false))
			{
				//GUI.Box(new Rect(0.25f*Screen.width,0.25f*Screen.height,0.5f*Screen.width,0.5f*Screen.height),"CONTINUE?");
				GUI.Label(new Rect(0.3f*Screen.width,0.28f*Screen.height,0.5f*Screen.width,0.2f*Screen.height),"CONTINUE?",myGUIStyle);
				if( GUI.Button(new Rect(0.27f*Screen.width,0.62f*Screen.height,0.15f*Screen.width,0.1f*Screen.height), "YES", buttonGUIStyle ) )	
				{
					isLeaderBoard = false;
					GameEnvironment.StartWave = currentWave-1;
					startLives = LevelInfo.Environments.hubLives.GetNumber();
					startScore = LevelInfo.Environments.hubScores.GetNumber();
					
					restartLevel = true;
					
					Time.timeScale = 1.0f;
					Application.LoadLevel(Application.loadedLevel);
				}
				if( GUI.Button(new Rect(0.58f*Screen.width,0.62f*Screen.height,0.15f*Screen.width,0.1f*Screen.height), "NO", buttonGUIStyle) )
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
				if( GUI.Button(new Rect(0.3f*Screen.width,0.62f*Screen.height,0.15f*Screen.width,0.1f*Screen.height), "Post Score", buttonGUIStyle ) )
				{			
					isGet = false;
					isPost = true;
										
					//*commented for warning*//isScoreDisplayed = false;					
				}
				
				if(isGet)
				{
					if(isConnectionError1)
						getScoreResponse = "Error in Network Connection!";
					else
						getScoreResponse = "Waiting for Response..";
					
					if(nameArray[0] != null && isConnectionError1 == false )
					{	
						GUI.Label(new Rect(0.38f*Screen.width,0.32f*Screen.height,0.15f*Screen.width,0.03f*Screen.height), "" + "NAME",myGUIStyle);				
						
						GUI.Label(new Rect(0.50f*Screen.width,0.32f*Screen.height,0.15f*Screen.width,0.03f*Screen.height), "" + "SCORE",myGUIStyle);					
						
						for(int i = 0; i < 10; i++)
						{	
							GUI.Label(new Rect(0.38f*Screen.width, (0.35f + yPosition*i)*Screen.height, 0.15f*Screen.width,0.03f*Screen.height), "" + nameArray[i].ToString(),myGUIStyle);
						}	
							//GUI.Label(new Rect(400,40, 200, 70), "" + scoreArray[i].ToString());
							
							//GUI.Label(new Rect(200,40 + yPosition*(i+1), 200, 70), "" + nameArray[i].ToString());
						for(int i = 0; i < 10; i++)
						{			
							GUI.Label(new Rect(0.50f*Screen.width, (0.35f + yPosition*i)*Screen.height, 0.15f*Screen.width,0.03f*Screen.height), "" + scoreArray[i].ToString(),myGUIStyle);
						}
					}
					else
					{
						GUI.Label(new Rect(0.35f*Screen.width,0.35f*Screen.height,0.30f*Screen.width,0.032f*Screen.height), getScoreResponse.ToString(),myGUIStyle);
					}
					
					//isGet = false;				
				}
				
				if(isPost)
				{
					//GUI.Label(new Rect(100, 60, 200, 70), getScoreResponse.ToString());
					GUI.Label(new Rect(0.35f*Screen.width,0.355f*Screen.height, 0.1f*Screen.width,0.05f*Screen.height), "NAME: ",myGUIStyle);
					GUI.Label(new Rect(0.35f*Screen.width,0.42f*Screen.height, 0.1f*Screen.width,0.05f*Screen.height), "SCORE: ",myGUIStyle);
					
					
					#if UNITY_ANDROID || UNITY_IPHONE
					if( ZBFacebook.Instance.Ready)
					{
						nameLB = ZBFacebook.Instance.fbname;
						GUI.Label(new Rect(0.48f*Screen.width,0.35f*Screen.height, 0.2f*Screen.width,0.05f*Screen.height), nameLB,myGUIStyle);
					}
					else
					{
						if( GUI.Button(new Rect(0.48f*Screen.width,0.35f*Screen.height, 0.2f*Screen.width,0.05f*Screen.height), "Log in ", buttonGUIStyle) && !ZBFacebook.Instance.Logging)
						{
							ZBFacebook.Instance.Login();
						}
					}
					#else
					nameLB = GUI.TextField(new Rect(0.48f*Screen.width,0.35f*Screen.height, 0.2f*Screen.width,0.05f*Screen.height), nameLB,12,myGUIStyle);
					#endif
					
					GUI.Label(new Rect(0.48f*Screen.width,0.42f*Screen.height, 0.2f*Screen.width,0.05f*Screen.height), (LevelInfo.Environments.hubScores.GetNumber()).ToString(),myGUIStyle);
					
					//scoreLB = GUI.TextField(new Rect(0.45f*Screen.width,0.40f*Screen.height, 0.15f*Screen.width,0.035f*Screen.height), scoreLB);

					if(GUI.Button(new Rect(0.45f*Screen.width,0.475f*Screen.height, 0.10f*Screen.width,0.075f*Screen.height), "Post", buttonGUIStyle) )
					{
						if(postedscore)
						{
							postScoreResponse = "You already have posted.";
						}
						else if(nameLB.Trim().Equals(""))	
						{
							isName = false;
							postScoreResponse = "Please log in to facebook.";//"Please Enter Name!";
							ZBFacebook.Instance.Init();
						}
						else
						{	
							isName = true;
							string url = "http://therealmattharmon.com/Zombie/addscoreresp.php?snm=" + nameLB.Replace(' ','_') + "&score=" + (LevelInfo.Environments.hubScores.GetNumber()).ToString() + "&format=xml";
			        		WWW www = new WWW(url);
			        		StartCoroutine(WaitForRequest(www));
							postedscore = true;
						}						
					}
			
					#if UNITY_ANDROID || UNITY_IPHONE
					
					if(ZBFacebook.Instance.Posted)
					{
						sharedonfacebook = true;
						ZBFacebook.Instance.Posted = false;
						postScoreResponse = "Shared on facebook!";
					}
					
					if(GUI.Button(new Rect(0.585f*Screen.width,0.475f*Screen.height, 0.10f*Screen.width,0.075f*Screen.height), "Share", buttonGUIStyle))
					{
						if( !sharedonfacebook )
						{
							postScoreResponse = "Already shared on facebook!";
						}
						else
						{
							ZBFacebook.Instance.PostOnWall(LevelInfo.Environments.hubScores.GetNumber());		
						}
					}
					#endif
					
					
					
					if(!isName || (isPost == true))
						GUI.Label(new Rect(0.38f*Screen.width,0.57f*Screen.height,0.30f*Screen.width,0.032f*Screen.height), postScoreResponse.ToString(),myGUIStyle);					
				}
				#endregion
	
				if( GUI.Button(new Rect(0.58f*Screen.width,0.62f*Screen.height,0.15f*Screen.width,0.1f*Screen.height), "EXIT", buttonGUIStyle) )
				{
					isLeaderBoard = false;		
					GameEnvironment.ToMap = true;
					LevelInfo.Environments.store.GoMainMenuFromGamePlay();
				}
				 
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
					// Write To PlayerPrefs
					Store.SetHighestWaveCompleted(currentLevel,currentWave);
					
					float headshotsinwave = (float)currentHeadshotsInWave/(float)currentZombiesInWave;
					int stars = 0;
					if( headshotsinwave >= 0.9f ) stars = 3;
					else if( headshotsinwave >= 0.75f ) stars = 2;
					else stars = 1;
					rewardforwavecomplete = (UnityEngine.Random.Range(0,2)==0?HealthPackType.BonusHeads:HealthPackType.XtraLife);
					LevelInfo.Environments.waveInfo.ShowWaveComplete(rewardforwavecomplete,stars,0.3f,0.2f,0.2f);
					
					bonusForWaveComplete = 1000;
				}
			}
			break;
		}
	}
	
	#endregion
	
	#region AutoTargeting
	
	[System.NonSerializedAttribute]
	public Zombi LeftAttackingZombie;
	[System.NonSerializedAttribute]
	public Zombi RightAttackingZombie;
	
	
	public float AutoTargetingSpeed = 1f;
	private bool AutoTargeting = false;
	
	private float leftAutoTargetTime=0f;
	private float rightAutoTargetTime=0f;
	
	private bool autoTargetTurnDirectionRight;
	
	public void ZombieStartsAttacking(Zombi zombie)
	{
		if( !Option.AutoTargeting || AutoTargeting) return;
		Vector3 scp = LevelInfo.Environments.mainCamera.WorldToScreenPoint(zombie.transform.position);
		if(scp.z > 0 && scp.x>=0 && scp.x <= Screen.width) return;
		if(scp.z<0) scp.x = -scp.x;
		if(scp.x < -Screen.width*0.5f && scp.z<0 || scp.x<0f && scp.z>=0)
		{
			LeftAttackingZombie=zombie;
			leftAutoTargetTime=3f;
		}
		else
		{
			RightAttackingZombie=zombie;
			rightAutoTargetTime=3f;
		}
	}
	
	private IEnumerator AutoTargetingUpdate()
	{
		while( AutoTargeting )
		{	
			Vector3 c = transform.rotation.eulerAngles;
			bool ok=false;
			
			if( autoTargetTurnDirectionRight )
			{
				if(c.y > angleY ) c.y -= 360f;
				c.y += AutoTargetingSpeed*Time.deltaTime;
				if( c.y >= angleY ) ok=true;
			}
			else
			{
				if(c.y < angleY ) c.y += 360f;
				c.y -= AutoTargetingSpeed*Time.deltaTime;
				if( c.y <= angleY ) ok=true;				
			}

			if(ok)
			{ 
				c.y = angleY;
				angling = false;
				AutoTargeting = false;
			}
			transform.rotation = Quaternion.Euler(c);
			yield return new WaitForEndOfFrame();
		}		
	}
	
	public void AutoTargetToPoint(Vector3 dest,bool right)
	{
		if(!AutoTargeting)
		{
			autoTargetTurnDirectionRight = right;
			AutoTargeting = true;
			angleY = Quaternion.LookRotation(dest-transform.position,Vector3.up).eulerAngles.y;
			StartCoroutine(AutoTargetingUpdate());
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
	
	#region Moving to Vantage Point
	
	public float MovingSpeed = 1f;
	public float MovingRotateSpeed = 10f;
	private bool Moving = false;

	private Vector3 destination;
	private float angleY;
	private bool angling = false;
	
		
	private void MoveTo(Vector3 dest)
	{
		Moving = true;
		angling = true;
		destination = dest;
		angleY = Quaternion.LookRotation(destination-transform.position,Vector3.up).eulerAngles.y;
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
			if( Mathf.Abs(c.y - angleY) <= 1f )	{ c.y = angleY; angling = false;  waitfornewwave = Time.time + showWaveCompleteTime; }
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
				
				//Give reward to the player
				if( rewardforwavecomplete == HealthPackType.XtraLife) LevelInfo.Environments.hubLives.SetNumberWithFlash(LevelInfo.Environments.hubLives.GetNumber()+1);
				else if(rewardforwavecomplete == HealthPackType.BonusHeads) Store.zombieHeads = Store.zombieHeads + 100;
				else Debug.LogError("Rewards must be heads or xtralife, but now is " + rewardforwavecomplete); 
				
				CreateNewZombieWave();
			}
		}		
		else
			transform.Translate(dir*MovingSpeed*Time.deltaTime,Space.World);
	}
	
	#endregion

	#region Health Bar Update
	
	void UpdateHealthBar()
	{
		var v = LevelInfo.Environments.healthbarHealth.transform.localScale;
		v.x = 146.4f*Mathf.Clamp01(healthshow);
		v.y = health!=healthshow&&healthshow<=1?21:18;
		LevelInfo.Environments.healthbarHealth.color = 
			health!=healthshow&&healthshow<=1?new Color(1f,1f,1f,0.6f):Color.white;
		LevelInfo.Environments.healthbarHealth.transform.localScale = v;
		
		v = LevelInfo.Environments.healthbarArmor.transform.localScale;
		v.x = 146.4f*Mathf.Clamp01(armorshow);
		v.y = armor!=armorshow?21:18;
		LevelInfo.Environments.healthbarArmor.color = 
			armor!=armorshow&&armorshow<=1?new Color(1f,1f,1f,0.6f):Color.red;
		LevelInfo.Environments.healthbarArmor.transform.localScale = v;
		
		// flash/pulse healthbar icons
		if( health!=healthshow )
		{
			LevelInfo.Environments.healthbarTombstone.StartFlash();
			LevelInfo.Environments.healthbarBoxes.StartFlash();
		}
		else
		{
			LevelInfo.Environments.healthbarTombstone.EndFlash();
			LevelInfo.Environments.healthbarBoxes.EndFlash();	
		}
		
		
		LevelInfo.Environments.powerupTimeDamageMultiplier.gameObject.SetActive(DamageMultiplied);
		LevelInfo.Environments.powerupTimeUnlimitedAmmo.gameObject.SetActive(UnlimitedAmmo);
		LevelInfo.Environments.powerupTimeShilded.gameObject.SetActive(Shielded);
	}
	
	
	#endregion
	
	#region Properties
	
	public bool GamePaused {
		get
		{
			return state != GameState.Play;
		}
	}
	
	public bool ScreenShowed
	{
		get
		{
			return state == GameState.Play || state == GameState.Paused || state == GameState.Lose;
		}
	}
	
	public bool Died { get { return state == GameState.Lose; }}
	
	public float Health { get { return healthshow; } set { health = value; } }
	public float Armor { get { return armorshow; } set { armor = value; } }
	
	public int ZombiesLeft { get { return LevelInfo.Environments.hubZombiesLeft.GetNumber(); } }
	
	private bool isInWave = false;
	public bool IsInWave { get { return isInWave; }}
	
	public int AliveZombieCount { get { return ZombiesLeft - LevelInfo.Environments.generator.zombiesLeft; }}
	
	#endregion

	#region Other Methods
	
	public IEnumerator ShowTip(string message,float wait)
	{
		while(wait > 0 )
		{
			wait -= Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
		LevelInfo.Environments.generator.GenerateMessageText(new Vector3(0.5f,0.2f,-1),message,false,4f);
	}
	
	public void GetBite(float lostHealth)
	{
		LevelInfo.Audio.PlayPlayerGetHit();
		
		if(armor==0)
			GetHealth(-lostHealth);
		else
		{
			GetHealth(-lostHealth*0.5f);
			GetArmor(-lostHealth*0.5f);
		}
	}
	
	public void GetZombie()
	{
		GetScore(LevelInfo.State.scoreForZombie,true);
		//zombiesLeftForThisWave--;
		LevelInfo.Environments.hubZombiesLeft.SetNumberWithFlash(LevelInfo.Environments.hubZombiesLeft.GetNumber()-1);
		if( LevelInfo.Environments.hubZombiesLeft.GetNumber() <= 0 && GameObject.FindGameObjectsWithTag("ZombieHead").Length == 1)
		{
			StartCoroutine(WaveComplete());
			if( Store.FirstTimePlay )
				PrologueComplete();
		}
	}
	
	public void GetScore(int scores,bool allowMultiply)
	{
		LevelInfo.Environments.hubScores.SetNumberWithFlash(LevelInfo.Environments.hubScores.GetNumber()+scores);
	}
	
	public void GetHealth(float h)
	{
		if( Option.UnlimitedHealth ) return;
		if( h<0 && Shielded ) return;
		#if UNITY_ANDROID || UNITY_IPHONE
		if( h<0f && Option.Vibration)
				Handheld.Vibrate();
		#endif
		if( h<0f)
			LevelInfo.Environments.screenBlood.Pulse();
		
		health = Mathf.Clamp01(health+h);
	}
	
	public void GetArmor(float h)
	{
		if( Option.UnlimitedHealth ) return;
		if( h<0 && Shielded ) return;
		armor = Mathf.Clamp01(armor+h);	
	}
	
	private HealthPackType rewardforwavecomplete;
	private int bonusForWaveComplete = -1;
	public IEnumerator WaveComplete()
	{
		isInWave = false;
		
		float time = Time.time + 6f;
		while( Time.time < time )
			yield return new WaitForEndOfFrame();
		
		DestroyAllRemainedWithTag("Zombie");
		
		LevelInfo.Audio.audioSourcePlayer.PlayOneShot(LevelInfo.Audio.AudioWaveComplete);
		bonusForWaveComplete = UnityEngine.Random.Range(0,2);
		//state = GameState.WaveCompleted;
		PrepareAndCreateNewWave();
	}
	
	public void PrepareAndCreateNewWave()
	{
		//DestroyAllRemainedWithTag("Zombie");
		//DestroyAllRemainedWithTag("ZombieRagdoll");
		
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
		System.GC.Collect();
		
		isInWave = true;
		currentWave++;
		
		currentZombiesInWave = LevelInfo.State.zombiesCountFactor*currentWave;
		currentHeadshotsInWave = 0;

		LevelInfo.Environments.hubZombiesLeft.SetNumber(currentZombiesInWave);
		LevelInfo.Environments.generator.StartNewWave(LevelInfo.Environments.hubZombiesLeft.GetNumber());
		
		LevelInfo.Environments.waveInfo.ShowWave(currentWave,LevelInfo.Environments.hubZombiesLeft.GetNumber());
		LevelInfo.Audio.PlayLevel(currentWave);
		LevelInfo.Audio.audioSourcePlayer.PlayOneShot(LevelInfo.Audio.AudioWaveComplete);
		
		DestroyAllRemainedWithTag("Ufo");
		GameObject g = (GameObject)Instantiate(LevelInfo.Environments.ufoSpaceship,new Vector3(transform.position.x+UnityEngine.Random.Range(4f,6f),4f,0f),Quaternion.identity);
		g.transform.parent = LevelInfo.Environments.environmentsTransform;
		
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
		if( Store.FirstTimePlay )
			StartCoroutine(ShowTip("TIP: TAP TO SHOOT",4f));
	}
	
	public bool prologuecomplete = false;
	private void PrologueComplete()
	{
		StartCoroutine(PrologueCompleteThread());		
	}
	
	IEnumerator PrologueCompleteThread()
	{
		prologuecomplete=true;
		//LevelInfo.Environments.buttonPause.isEnabled=false;
		
		yield return new WaitForSeconds(2f);
		LevelInfo.Environments.waveInfo.ShowPrologueComplete();
		yield return new WaitForSeconds(5f);
		Store.zombieHeads += 500;
		yield return new WaitForSeconds(1f);
		LevelInfo.Environments.waveInfo.HideWaveComplete();
		yield return new WaitForSeconds(2f);
		
		Store.FirstTimePlay=false;
		GameEnvironment.ToMap = true;
		LevelInfo.Environments.store.GoMainMenuFromGamePlay();
	}
	
	#endregion
	
	#region Powerups
	
	public bool DamageMultiplied = false;
	private float damageMultiplieTime = 0f;
	public bool Shielded = false;
	private float shieldTime = 0f;
	public bool UnlimitedAmmo = false;
	private float unlimitedAmmoTime = 0f;
	
	private IEnumerator DamageMultiplyThread()
	{
		damageMultiplieTime = Time.time + 15f;
		
		if( !DamageMultiplied )
		{
			DamageMultiplied = true;
		
			while( Time.time < damageMultiplieTime )
			{
				LevelInfo.Environments.powerupTimeDamageMultiplier.text = "4x damage " + (int)(damageMultiplieTime-Time.time+1);
				yield return new WaitForEndOfFrame();
			}
			DamageMultiplied = false;
		}
	}
	
	private IEnumerator ShieldThread()
	{
		LevelInfo.Audio.PlayShield(true);
		shieldTime = Time.time + 15f;
		
		if( !Shielded )
		{
			LevelInfo.Environments.shield.Show();
			
			Shielded = true;
		
			while( Time.time < shieldTime )
			{
				LevelInfo.Environments.shield.SetActive(true);
				LevelInfo.Environments.powerupTimeShilded.text = "Invincibility " + (int)(shieldTime-Time.time+1);
				yield return new WaitForEndOfFrame();
			}
			Shielded = false;
			
			if(shieldTime==0f)
			{
				LevelInfo.Environments.shield.HideImmediately();
			}
			else
			{
				LevelInfo.Environments.shield.Hide();
				LevelInfo.Audio.PlayShield(false);
			}
		}
	}
	
	private IEnumerator UnlimitedAmmoThread()
	{
		unlimitedAmmoTime = Time.time + 15f;
		
		if( !UnlimitedAmmo )
		{
			UnlimitedAmmo = true;
		
			while( Time.time < unlimitedAmmoTime )
			{
				LevelInfo.Environments.powerupTimeUnlimitedAmmo.text = "Rampage " + (int)(unlimitedAmmoTime-Time.time+1);
				yield return new WaitForEndOfFrame();
			}
			UnlimitedAmmo = false;
		}
	}
	
	public void DamageMultiply()
	{
		//shieldTime=0f;
		//unlimitedAmmoTime=0f;
		StartCoroutine(DamageMultiplyThread());
	}
	
	public void Shield()
	{
		//damageMultiplieTime=0f;
		//unlimitedAmmoTime=0f;
		StartCoroutine(ShieldThread());
	}
	
	public void Rampage()
	{
		//shieldTime=0f;
		//damageMultiplieTime=0f;
		StartCoroutine(UnlimitedAmmoThread());
	}
	
	#endregion
}
