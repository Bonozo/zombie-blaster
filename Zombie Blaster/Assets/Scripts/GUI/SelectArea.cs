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
	
	public AudioSource audioUnlocked;
	public AudioSource audioLocked;
	public AudioSource audioUnlockLevel;
	
	public GameObject guiLoading;
	public Texture2D[] screen;
	public string[] tip;
	public GUITexture guiFullscreen;
	public GUIText guiTip;
	
	private bool[] unlocked = new bool[5];
	private int[] unlock_heads = new int [5] {0,500,1000,1500,2000};
	private int unlock_index = -1;
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
				(unlock_heads[i] <= Store.zombieHeads?Color.yellow:Color.white);
			levelButton[i].audioPressed = unlocked[i]?audioUnlocked:audioLocked;
		}
	}
	
	private Store store;
	private Weapon[][] newWeapos = new Weapon[][]
	{
		new Weapon[] {Weapon.Flamethrower,Weapon.Crossbow},
		new Weapon[] {Weapon.Flamethrower,Weapon.Crossbow,Weapon.Grenade},
		new Weapon[] {Weapon.Football,Weapon.MachineGun},
		new Weapon[] {Weapon.Grenade,Weapon.Rocket},
		new Weapon[] {Weapon.PulseShotGun,Weapon.Revolver}
	};
	
	void OnEnable()
	{
		guiLoading.SetActive(false);
	}
	
	// Use this for initialization
	void Start () {
		
		store = (Store)GameObject.FindObjectOfType(typeof(Store));
		
		GameEnvironment.StartLevel = 0;
		for(int i=0;i<Store.countLevel;i++)
		{
			unlocked[i] = Store.LevelUnlocked(i);
			if(unlocked[i]) countUnlocked++;
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
			MainMenu mainmenu = (MainMenu)GameObject.FindObjectOfType(typeof(MainMenu));
			mainmenu.mapToStore = true;
			mainmenu.GoState(MainMenu.MenuState.Store);
			return;
		}
		
		if( unlock_index != -1 ) return;
		
		
		for(int i=0;i<levelButton.Length;i++)
			if( !unlocked[i] && levelButton[i].PressedUp )
			{
				unlock_index = i;
				return;
			}
		
		for(int i=0;i<levelButton.Length;i++)
			if( unlocked[i] && levelButton[i].PressedUp )
			{
				GameEnvironment.StartLevel = i;
				GameEnvironment.StartWave = 0;
				
				levelButton[i].SetAsPressed();
				
				Destroy(GameObject.Find("Sound Background"));
				Destroy(GameObject.Find("Sound Wind"));
				guiLoading.SetActive(true);
				guiFullscreen.texture = screen[Random.Range(0,screen.Length)];
				guiTip.text = tip[Random.Range(0,tip.Length)];
				Application.LoadLevel("playgame");	
			}
	}
	
	public GUIStyle myStyle;
	void OnGUI ()
	{ 
		popupTexture.enabled = unlock_index != -1;
		if( unlock_index != -1 )
		{
			bool can_bay = Store.zombieHeads >= unlock_heads[unlock_index];

			//GUI.Box(new Rect(0.25f*Screen.width,0.25f*Screen.height,0.5f*Screen.width,0.5f*Screen.height),"");
			GUI.Label(new Rect(0.45f*Screen.width,0.305f*Screen.height,0.1f*Screen.width,0.1f*Screen.height),"" + unlock_heads[unlock_index],myStyle);
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
					Store.zombieHeads = Store.zombieHeads - unlock_heads[unlock_index];
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
				unlock_index = -1;
				for(int i=0;i<levelButton.Length;i++)
					levelButton[i].Ignore();
			}
		}

	}
}
