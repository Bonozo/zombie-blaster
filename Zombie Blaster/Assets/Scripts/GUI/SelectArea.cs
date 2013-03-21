using UnityEngine;
using System.Collections;

public class SelectArea : MonoBehaviour {
	
	public Fade fade;
	
	public ButtonBase[] levelButton;
	public GUITexture[] lockIcon;
	public Texture headSack;
	
	public Texture2D lockedTexture;
	public GUITexture[] bloods;
	public GUIText headsText;
	public GUITexture popupTexture;
	public GUIText helpMessage;
	
	public ButtonBase storeButton;
	public ButtonBase backToGameForGamePlay;
	public GUITexture notification;
	
	public AudioSource audioUnlocked;
	public AudioSource audioLocked;
	public AudioSource audioUnlockLevel;
	public AudioSource audioBack;
	public AudioSource audioOpenMap;
	
	private bool[] unlocked = new bool[5];
	private int unlock_index = -1;
	private int play_index = -1;
	private int countUnlocked=0;
	
	private Color colorGreen = new Color(0f,1f,0f,0.5f);
	private Color colorWhite = new Color(1f,1f,1f,0.5f);
	
	private Store store;
	
	private void UpdateSelectAreaScreen()
	{
		foreach(var g in bloods )
		{
			Color c = g.color;
			c.a = 0.09f*(countUnlocked);
			g.color = c;
		}
		for(int i=0;i<5;i++)
		{
			lockIcon[i].enabled = !unlocked[i];
			lockIcon[i].gameObject.GetComponent<ColorPlay>().colmax = 
				(GameEnvironment.levelPrice[i] <= Store.zombieHeads?colorGreen:colorWhite);		
			lockIcon[i].gameObject.GetComponent<ColorPlay>().speed = 
				(GameEnvironment.levelPrice[i] <= Store.zombieHeads?2:1);
			lockIcon[i].gameObject.GetComponent<ColorPlay>().pause = 
				(GameEnvironment.levelPrice[i] <= Store.zombieHeads?0:2);
			levelButton[i].audioPressed = unlocked[i]?audioUnlocked:audioLocked;
		}
	}
	
	private Weapon[][] newWeapos = new Weapon[][] //??// Same as Store
	{
		new Weapon[] {Weapon.Flamethrower,Weapon.Revolver},
		new Weapon[] {Weapon.Grenade,Weapon.Spade},
		new Weapon[] {Weapon.Football,Weapon.MachineGun},
		new Weapon[] {Weapon.Rocket},
		new Weapon[] {Weapon.PulseShotGun,Weapon.Crossbow}
	};
	
	void OnEnable()
	{
		if( store.IsLevelOption && Store.FirstTimePlay )
		{
			// same code
			GameEnvironment.StartLevel = 4; // Cemetary
			GameEnvironment.StartWave = 0;
			Destroy(GameObject.Find("Sound Background"));
			Destroy(GameObject.Find("Sound Wind"));
			store.showLoadingScreen = true;
			System.GC.Collect();
			Application.LoadLevel("playgame");
			return;
		}
		
		UpdateSelectAreaScreen();
		
		play_index = unlock_index = -1;
		GameEnvironment.StartWave = 0;
		
		audioOpenMap.Play();
		for(int i=0;i<5;i++)
		{
			lockIcon[i].gameObject.GetComponent<ColorPlay>().pauseInStart = 
				(GameEnvironment.levelPrice[i] <= Store.zombieHeads?false:true);
		}
	}
	
	void Awake()
	{
		store = (Store)GameObject.FindObjectOfType(typeof(Store));
		GameEnvironment.StartWave = 0;
		
		GameEnvironment.StartLevel = 0;
		for(int i=0;i<Store.countLevel;i++)
		{
			unlocked[i] = Store.LevelUnlocked(i);
			if(unlocked[i]) 
			{
				countUnlocked++;
				levelButton[i].standartTexture = levelButton[i].pressedTexture;
			}
		}
		if( countUnlocked == 5 && helpMessage != null)
			Destroy(helpMessage);
		
		for(int i=0;i<levelButton.Length;i++)
			levelButton[i].canPressed = unlocked[i];	
	}
	
	void UpdateNotification()
	{
		bool show = false;
		if( fade.Finished )
		{
			store.UpdateWeaponAvailable();
			for(int i=0;i<Store.countWeapons;i++)
				if( store.WeaponAvailable(i) && Store.CanBuyWeapon(i) )
				{
					show = true;
				}
		}
		notification.enabled = show;
	}
	
	void Update()
	{
		UpdateNotification();
		
		if(Fade.InProcess) return;
		
		headsText.text = "" + Store.zombieHeads;
		
		if( storeButton.PressedUp )
		{
			if( store.IsLevelOption )
			{
				MainMenu mainmenu = (MainMenu)GameObject.FindObjectOfType(typeof(MainMenu));
				mainmenu.mapToStore = true;
				mainmenu.GoState(MainMenu.MenuState.Store);
			}
			
			if( store.IsLevelGamePlay )
			{
				LevelInfo.Environments.control.state = GameState.Store;
			}
			return;
		}
		
		if( backToGameForGamePlay!=null && backToGameForGamePlay.PressedUp )
		{
			LevelInfo.Environments.control.state = ButtonMap.lastState;
			return;
		}
		
		if( unlock_index != -1 || play_index != -1) return;
		
		
		for(int i=0;i<levelButton.Length;i++)
			if( !unlocked[i] && levelButton[i].PressedUp )
			{
				unlock_index = i;
				return;
			}
		
		for(int i=0;i<levelButton.Length;i++)
			if( unlocked[i] && levelButton[i].PressedUp )
			{
				play_index = i;	
				GameEnvironment.StartWave = Store.HighestWaveCompleted(i);
			}
	}
	
	public IEnumerator StartGameThread()
	{
		Fade.InProcess = true;
		if( store.IsLevelOption )
		{
			MainMenu mainmenu = (MainMenu)GameObject.FindObjectOfType(typeof(MainMenu));
			mainmenu.DisableMenuButtons();
		}
		else
			backToGameForGamePlay.DisableButtonForUse();
		fade.Show(1.5f);
		while( !fade.Finished )
		{
			yield return new WaitForEndOfFrame();
		}
		
		Destroy(GameObject.Find("Sound Background"));
		Destroy(GameObject.Find("Sound Wind"));
		/*guiLoading.SetActive(true);
		guiFullscreen.texture = screen[Random.Range(0,screen.Length)];
		guiTip.text = tip[Random.Range(0,tip.Length)];*/
		store.showLoadingScreen = true;
		play_index = -1;
		fade.Disable();
		yield return new WaitForEndOfFrame();
		Fade.InProcess=false;
		System.GC.Collect();
		Application.LoadLevel("playgame");
	}
	
	public void StartGame(int level,int wave)
	{
		audioUnlocked.Play();
		GameEnvironment.StartLevel = level;
		GameEnvironment.StartWave = wave;
		popupTexture.enabled = false;
		StartCoroutine(StartGameThread());
	}
	
	public GUIStyle myStyle;
	public GUIStyle buttonStyle;
	void OnGUI ()
	{ 
		GUI.matrix = GameEnvironment.GetGameGUIMatrix();
		
		if(Fade.InProcess) return;
		
		popupTexture.enabled = unlock_index != -1 || play_index != -1;
		if( unlock_index != -1 )
		{
			bool can_bay = Store.zombieHeads >= GameEnvironment.levelPrice[unlock_index];

			//GUI.Box(new Rect(0.25f*GameEnvironment.GUIWidth,0.25f*GameEnvironment.GUIHeight,0.5f*GameEnvironment.GUIWidth,0.5f*GameEnvironment.GUIHeight),"");
			GUI.Label(new Rect(0.425f*GameEnvironment.GUIWidth,0.32f*GameEnvironment.GUIHeight,0.1f*GameEnvironment.GUIWidth,0.1f*GameEnvironment.GUIHeight),"COST: " + GameEnvironment.levelPrice[unlock_index],myStyle);
			GUI.DrawTexture(new Rect(0.52f*GameEnvironment.GUIWidth,0.29f*GameEnvironment.GUIHeight,0.08f*GameEnvironment.GUIWidth,0.08f*GameEnvironment.GUIHeight),headSack);
			
			/*int cc = 0;
			for(int i=0; i<newWeapos[unlock_index] .Length; i++)
					cc++;
			for(int i=0,j=0; i<newWeapos[unlock_index].Length; i++)
			{
				float sz = 0.08f*GameEnvironment.GUIHeight;
				if( j < 5 )
					GUI.DrawTexture(new Rect( (0.5f-0.045f*Mathf.Min(cc,5)+0.09f*j)*GameEnvironment.GUIWidth,0.41f*GameEnvironment.GUIHeight,sz,sz),store.weaponIcon[(int)newWeapos[unlock_index][i]]);
				else
					GUI.DrawTexture(new Rect( (0.5f-0.045f*(cc-5)+0.09f*(j-5))*GameEnvironment.GUIWidth,0.51f*GameEnvironment.GUIHeight,sz,sz),store.weaponIcon[(int)newWeapos[unlock_index][i]]);
				j++;
			}*/
			float sz = 0.08f*GameEnvironment.GUIHeight;
			
			if(newWeapos[unlock_index].Length==1)
			{
				GUI.Label(new Rect(0.42f*GameEnvironment.GUIWidth,0.41f*GameEnvironment.GUIHeight+sz*0.5f-8,0.1f*GameEnvironment.GUIWidth,0.1f*GameEnvironment.GUIHeight),"UNLOCKS: ",myStyle);
				GUI.DrawTexture(new Rect( 0.52f*GameEnvironment.GUIWidth,0.41f*GameEnvironment.GUIHeight,sz,sz),store.weaponIcon[(int)newWeapos[unlock_index][0]]);
			}
			else if(newWeapos[unlock_index].Length==2)
			{
				GUI.Label(new Rect(0.4f*GameEnvironment.GUIWidth,0.41f*GameEnvironment.GUIHeight+sz*0.5f-8,0.1f*GameEnvironment.GUIWidth,0.1f*GameEnvironment.GUIHeight),"UNLOCKS: ",myStyle);
				GUI.DrawTexture(new Rect( 0.5f*GameEnvironment.GUIWidth,0.41f*GameEnvironment.GUIHeight,sz,sz),store.weaponIcon[(int)newWeapos[unlock_index][0]]);
				GUI.DrawTexture(new Rect( 0.51f*GameEnvironment.GUIWidth+sz,0.41f*GameEnvironment.GUIHeight,sz,sz),store.weaponIcon[(int)newWeapos[unlock_index][1]]);
			}
			else
				Debug.LogError("ZB: Weapons unlocked for level is not implemented for grather that 2");
			
			if( can_bay)
			{
				if(GUI.Button(new Rect(0.33f*GameEnvironment.GUIWidth,0.55f*GameEnvironment.GUIHeight,0.16f*GameEnvironment.GUIWidth,0.1f*GameEnvironment.GUIHeight), "UNLOCK", buttonStyle ) )	
				{
					Store.UnlockLevel(unlock_index);
					unlocked[unlock_index] = true;
					countUnlocked++;
					UpdateSelectAreaScreen();
					Store.zombieHeads = Store.zombieHeads - GameEnvironment.levelPrice[unlock_index];
					levelButton[unlock_index].canPressed = unlocked[unlock_index];
					audioUnlockLevel.Play();
					levelButton[unlock_index].standartTexture = levelButton[unlock_index].pressedTexture;
					unlock_index = -1;
				}
			}
			else
			{
				if( GUI.Button(new Rect(0.33f*GameEnvironment.GUIWidth,0.55f*GameEnvironment.GUIHeight,0.16f*GameEnvironment.GUIWidth,0.1f*GameEnvironment.GUIHeight),"MORE HEADS", buttonStyle))
					store.Get1000HeadsEvent();
			}
	
			if( GUI.Button(new Rect(0.52f*GameEnvironment.GUIWidth,0.55f*GameEnvironment.GUIHeight,0.16f*GameEnvironment.GUIWidth,0.1f*GameEnvironment.GUIHeight), "CANCEL", buttonStyle ) )
			{
				audioLocked.Stop();
				audioBack.Play();
				unlock_index = -1;
				for(int i=0;i<levelButton.Length;i++)
					levelButton[i].Ignore();
			}
		}
		
		if( play_index != -1 )
		{
			GUI.Label(new Rect(0.35f*GameEnvironment.GUIWidth,0.33f*GameEnvironment.GUIHeight,0.1f*GameEnvironment.GUIWidth,0.1f*GameEnvironment.GUIHeight),"HIGHEST WAVE COMPLETED: " + Store.HighestWaveCompleted(play_index),myStyle);
			if(GUI.Button(new Rect(0.33f*GameEnvironment.GUIWidth,0.55f*GameEnvironment.GUIHeight,0.16f*GameEnvironment.GUIWidth,0.1f*GameEnvironment.GUIHeight), "PLAY", buttonStyle))
			{
				/*audioUnlocked.Play();
				GameEnvironment.StartLevel = play_index;
				
				Destroy(GameObject.Find("Sound Background"));
				Destroy(GameObject.Find("Sound Wind"));
				guiLoading.SetActive(true);
				guiFullscreen.texture = screen[Random.Range(0,screen.Length)];
				guiTip.text = tip[Random.Range(0,tip.Length)];
				play_index = -1;
				Application.LoadLevel("playgame");	*/
				StartGame(play_index,GameEnvironment.StartWave);
				return;
			}
			
			if( Store.HighestWaveCompleted(play_index)>0 )
			{		
				GUI.Label(new Rect(0.35f*GameEnvironment.GUIWidth,0.43f*GameEnvironment.GUIHeight,0.1f*GameEnvironment.GUIWidth,0.1f*GameEnvironment.GUIHeight),"Start Wave ",myStyle);
				if( GUI.Button(new Rect(0.485f*GameEnvironment.GUIWidth,0.405f*GameEnvironment.GUIHeight, 0.08f*GameEnvironment.GUIWidth,0.08f*GameEnvironment.GUIHeight), "-", buttonStyle) && GameEnvironment.StartWave > 0 )
					GameEnvironment.StartWave--;
				
				myStyle.alignment = TextAnchor.MiddleCenter;	
				GUI.Label(new Rect(0.57f*GameEnvironment.GUIWidth,0.42f*GameEnvironment.GUIHeight, 0.03f*GameEnvironment.GUIWidth,0.05f*GameEnvironment.GUIHeight), (GameEnvironment.StartWave+1).ToString(),myStyle);
				myStyle.alignment = TextAnchor.UpperLeft;
				
				if( GUI.Button(new Rect(0.605f*GameEnvironment.GUIWidth,0.405f*GameEnvironment.GUIHeight, 0.08f*GameEnvironment.GUIWidth,0.08f*GameEnvironment.GUIHeight), "+", buttonStyle) && GameEnvironment.StartWave < Store.HighestWaveCompleted(play_index) )
					GameEnvironment.StartWave++;	
			}
			
			
			
			if( GUI.Button(new Rect(0.52f*GameEnvironment.GUIWidth,0.55f*GameEnvironment.GUIHeight,0.16f*GameEnvironment.GUIWidth,0.1f*GameEnvironment.GUIHeight), "BACK", buttonStyle ))
			{
				audioUnlocked.Stop();
				audioBack.Play();
				play_index = -1;
				GameEnvironment.StartWave = 0;
				for(int i=0;i<levelButton.Length;i++)
					levelButton[i].Ignore();
			}
		}

	}
}
