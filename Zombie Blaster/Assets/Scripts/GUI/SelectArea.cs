using UnityEngine;
using System.Collections;

public class SelectArea : MonoBehaviour {
	
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
	
	public AudioSource audioUnlocked;
	public AudioSource audioLocked;
	public AudioSource audioUnlockLevel;
	public AudioSource audioBack;
	public AudioSource audioOpenMap;
	
	public GameObject guiLoading;
	public Texture2D[] screen;
	public string[] tip;
	public GUITexture guiFullscreen;
	public GUIText guiTip;
	
	private bool[] unlocked = new bool[5];
	private int unlock_index = -1;
	private int play_index = -1;
	private int countUnlocked=0;

	private void UpdateSelectAreaScreen()
	{
		foreach(var g in bloods )
		{
			Color c = g.color;
			c.a = 0.115f*(countUnlocked-1);
			g.color = c;
		}
		for(int i=0;i<5;i++)
		{
			lockIcon[i].enabled = !unlocked[i];
			lockIcon[i].gameObject.GetComponent<ColorPlay>().colmax = 
				(GameEnvironment.levelPrice[i] <= Store.zombieHeads?Color.green:Color.white);		
			lockIcon[i].gameObject.GetComponent<ColorPlay>().speed = 
				(GameEnvironment.levelPrice[i] <= Store.zombieHeads?2:1);
			lockIcon[i].gameObject.GetComponent<ColorPlay>().pause = 
				(GameEnvironment.levelPrice[i] <= Store.zombieHeads?0:2);
			levelButton[i].audioPressed = unlocked[i]?audioUnlocked:audioLocked;
		}
	}
	
	private Store store;
	private Weapon[][] newWeapos = new Weapon[][]
	{
		new Weapon[] {Weapon.BB,Weapon.Flamethrower,Weapon.Crossbow},
		new Weapon[] {Weapon.Grenade},
		new Weapon[] {Weapon.Football,Weapon.MachineGun},
		new Weapon[] {Weapon.Rocket},
		new Weapon[] {Weapon.PulseShotGun,Weapon.Revolver}
	};
	
	void OnEnable()
	{
		play_index = unlock_index = -1;
		GameEnvironment.StartWave = 0;
		
		audioOpenMap.Play();
		guiLoading.SetActive(false);
		for(int i=0;i<5;i++)
		{
			lockIcon[i].gameObject.GetComponent<ColorPlay>().pauseInStart = 
				(GameEnvironment.levelPrice[i] <= Store.zombieHeads?false:true);
		}
	}
	
	// Use this for initialization
	void Start () {
		
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
		UpdateSelectAreaScreen();

		
		for(int i=0;i<levelButton.Length;i++)
			levelButton[i].canPressed = unlocked[i];
	}
	
	void Update()
	{
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
			LevelInfo.Environments.control.state = GameState.Play;
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
	
	public GUIStyle myStyle;
	void OnGUI ()
	{ 
		popupTexture.enabled = unlock_index != -1 || play_index != -1;
		if( unlock_index != -1 )
		{
			bool can_bay = Store.zombieHeads >= GameEnvironment.levelPrice[unlock_index];

			//GUI.Box(new Rect(0.25f*Screen.width,0.25f*Screen.height,0.5f*Screen.width,0.5f*Screen.height),"");
			GUI.Label(new Rect(0.45f*Screen.width,0.305f*Screen.height,0.1f*Screen.width,0.1f*Screen.height),"" + GameEnvironment.levelPrice[unlock_index],myStyle);
			GUI.DrawTexture(new Rect(0.5f*Screen.width,0.29f*Screen.height,0.08f*Screen.width,0.08f*Screen.height),headSack);
			
			int cc = 0;
			for(int i=0; i<newWeapos[unlock_index] .Length; i++)
					cc++;
			for(int i=0,j=0; i<newWeapos[unlock_index].Length; i++)
				{
				if( j < 5 )
					GUI.DrawTexture(new Rect( (0.5f-0.045f*Mathf.Min(cc,5)+0.09f*j)*Screen.width,0.41f*Screen.height,0.08f*Screen.width,0.08f*Screen.height),store.weaponIcon[(int)newWeapos[unlock_index][i]]);
				else
					GUI.DrawTexture(new Rect( (0.5f-0.045f*(cc-5)+0.09f*(j-5))*Screen.width,0.51f*Screen.height,0.08f*Screen.width,0.08f*Screen.height),store.weaponIcon[(int)newWeapos[unlock_index][i]]);
					j++;
				}
			
			if( can_bay)
			{
				if(GUI.Button(new Rect(0.33f*Screen.width,0.55f*Screen.height,0.16f*Screen.width,0.1f*Screen.height), "UNLOCK" ) )	
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
				if( GUI.Button(new Rect(0.33f*Screen.width,0.55f*Screen.height,0.16f*Screen.width,0.1f*Screen.height),"GET MORE HEADS"))
					store.Get1000HeadsEvent();
			}
	
			if( GUI.Button(new Rect(0.52f*Screen.width,0.55f*Screen.height,0.16f*Screen.width,0.1f*Screen.height), "CANCEL" ) )
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
			GUI.Label(new Rect(0.35f*Screen.width,0.33f*Screen.height,0.1f*Screen.width,0.1f*Screen.height),"Highest Wave Completed " + Store.HighestWaveCompleted(play_index),myStyle);
			if(GUI.Button(new Rect(0.33f*Screen.width,0.55f*Screen.height,0.16f*Screen.width,0.1f*Screen.height), "PLAY" ) )
			{
				audioUnlocked.Play();
				GameEnvironment.StartLevel = play_index;
				
				Destroy(GameObject.Find("Sound Background"));
				Destroy(GameObject.Find("Sound Wind"));
				guiLoading.SetActive(true);
				guiFullscreen.texture = screen[Random.Range(0,screen.Length)];
				guiTip.text = tip[Random.Range(0,tip.Length)];
				play_index = -1;
				Application.LoadLevel("playgame");	
				return;
			}
			
			if( Store.HighestWaveCompleted(play_index)>0 )
			{		
				GUI.Label(new Rect(0.35f*Screen.width,0.43f*Screen.height,0.1f*Screen.width,0.1f*Screen.height),"Start Wave ",myStyle);
				if( GUI.Button(new Rect(0.5f*Screen.width,0.42f*Screen.height, 0.05f*Screen.width,0.05f*Screen.height), "-") && GameEnvironment.StartWave > 0 )
					GameEnvironment.StartWave--;
				
				myStyle.alignment = TextAnchor.MiddleCenter;	
				GUI.Label(new Rect(0.57f*Screen.width,0.42f*Screen.height, 0.03f*Screen.width,0.05f*Screen.height), (GameEnvironment.StartWave+1).ToString(),myStyle);
				myStyle.alignment = TextAnchor.UpperLeft;
				
				if( GUI.Button(new Rect(0.62f*Screen.width,0.42f*Screen.height, 0.05f*Screen.width,0.05f*Screen.height), "+") && GameEnvironment.StartWave < Store.HighestWaveCompleted(play_index) )
					GameEnvironment.StartWave++;	
			}
			
			
			
			if( GUI.Button(new Rect(0.52f*Screen.width,0.55f*Screen.height,0.16f*Screen.width,0.1f*Screen.height), "BACK" ) )
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
