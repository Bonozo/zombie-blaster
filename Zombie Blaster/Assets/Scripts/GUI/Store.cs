using UnityEngine;
using System.Collections;

public enum Levels
{
	Farm=0,
	College=1,
	Stadium=2,
	Town=3,
	Cemetery=4
}

public enum Weapon
{
	BB,
	Flamethrower,
	Rocket,
	PulseShotGun,
	Grenade,
	MachineGun,
	Crossbow,
	Football,
	Revolver,
	Sniper,
	Zapper,
	Microwave,
	None
};

public class Store : MonoBehaviour {
	
	public static int countLevel = 5;
	public static int countWeapons = 9;
	
	public static Weapon[][] weaponsForLevel = new Weapon[][]
	{
		new Weapon[] {Weapon.BB,Weapon.Flamethrower,Weapon.Crossbow},
		new Weapon[] {Weapon.BB,Weapon.Flamethrower,Weapon.Crossbow,Weapon.Grenade},
		new Weapon[] {Weapon.BB,Weapon.Flamethrower,Weapon.Crossbow,Weapon.Football,Weapon.MachineGun},
		new Weapon[] {Weapon.BB,Weapon.Flamethrower,Weapon.Crossbow,Weapon.Grenade,Weapon.MachineGun,Weapon.Rocket},
		new Weapon[] {Weapon.BB,Weapon.Flamethrower,Weapon.Crossbow,Weapon.Grenade,Weapon.MachineGun,Weapon.Rocket,Weapon.Football,Weapon.PulseShotGun,Weapon.Revolver}
	};
	
	#region Player Prefs
	
	public static int _playerprefs_zombieHeads=0;
	public static int zombieHeads{
		get
		{
			return _playerprefs_zombieHeads;
		}
		set
		{
			_playerprefs_zombieHeads = value;
			PlayerPrefs.SetInt("zombieHeads",_playerprefs_zombieHeads);
		}
	}
	
	private static int[] _playerprefs_unlockweapon = new int[countWeapons];
	private static int[] _playerprefs_unlocklevel = new int[countLevel];
		
	public static bool WeaponUnlocked(int weapon)
	{
		return _playerprefs_unlockweapon[weapon] == 1;
	}
	public static void UnlockWeapon(int weapon)
	{
		_playerprefs_unlockweapon[weapon] = 1;
		PlayerPrefs.SetInt("weapon"+weapon,_playerprefs_unlockweapon[weapon]);
	}
	public static bool LevelUnlocked(int level)
	{
		return _playerprefs_unlocklevel[level] == 1;
	}
	public static void UnlockLevel(int level)
	{
		_playerprefs_unlocklevel[level] = 1;
		PlayerPrefs.SetInt("level"+level,_playerprefs_unlocklevel[level]);
	}
	
	public void RestorePlayerPrefs()
	{
		// heads
		_playerprefs_zombieHeads = PlayerPrefs.GetInt("zombieHeads",0);
		
		// weapons
		Store._playerprefs_unlockweapon[0] = 1;
		for(int i=1;i<_playerprefs_unlockweapon.Length;i++)
		{
			Store._playerprefs_unlockweapon[i] = PlayerPrefs.GetInt("weapon"+i,0);
		}	
		
		// levels
		Store._playerprefs_unlocklevel[0] = 1;
		for(int i=1;i<_playerprefs_unlocklevel.Length;i++)
		{
			Store._playerprefs_unlocklevel[i] = PlayerPrefs.GetInt("level"+i,0);
		}
		
	}
	
	void Awake()
	{
		DontDestroyOnLoad(this.gameObject);
		RestorePlayerPrefs();
		Application.targetFrameRate = 60;
	}
	
	#endregion
	/*
	#region Game Store
	
	public Texture2D textureBackground;
	public Texture2D textureScreenShot;
	public Texture2D textureZombieHead;
	
	private readonly float updownbuttonheight = 0.1f*Screen.height;
	private readonly float armorydown = 0.25f*Screen.height;
	
	private static Vector2 scrollposition = Vector2.zero;
	private static int wooi = -1;
	
	private static int cost = 100;
	
	public bool isPlayGame = false;
	
	private bool[] showWeapon = new bool[countWeapons];
	private bool _showStore = false;
	public bool showStore
	{
		get
		{
			return _showStore;
		}
		set
		{
			_showStore = value;
			if( _showStore )
			{
				for(int i=0;i<countWeapons;i++) showWeapon[i]=WeaponUnlocked(i);
				for(int i=0;i<countLevel;i++)
					if( LevelUnlocked(i) )
						for(int j=0;j<weaponsForLevel[i].Length;j++)
							showWeapon[(int)weaponsForLevel[i][j]] = true;
			}
		}
	}
	
	
	public GUIStyle myStyle;
	
	public void DrawStore()
	{
		// Background
		GUI.DrawTexture(new Rect(0f,0f,Screen.width,Screen.height),textureBackground);
		
		// Current Zombie Head Amount
		GUI.DrawTexture(new Rect(0.1f*Screen.width,0.005f*Screen.height,Screen.width*0.1f,Screen.height*0.09f),textureZombieHead);
		GUI.Box(new Rect(0.25f*Screen.width,0,0.25f*Screen.width,updownbuttonheight),"" + Store.zombieHeads,myStyle);
		
		// Get 1000 Head
		if( GUI.Button(new Rect(Screen.width*0.5f,0,Screen.width*0.5f,updownbuttonheight),"Get 1000 Heads",myStyle) )
		{
			IABAndroid.purchaseProduct("android.test.purchased");
		}
		
		// Return to Game
		if( GUI.Button(new Rect(0,Screen.height-updownbuttonheight,Screen.width*0.5f,updownbuttonheight),"Return to Game",myStyle) )
		{
			wooi = -1;
			showStore = false;
			GameEnvironment.IgnoreButtons();
			if( IsLevelOption )
			{
				MainMenu mainmenu = (MainMenu)GameObject.FindObjectOfType(typeof(MainMenu));
				mainmenu.GoMainState();
			}
			else if( IsLevelGamePlay )
				LevelInfo.Environments.control.state = GameState.Play;
	
		}
		
		if( GUI.Button(new Rect(Screen.width*0.5f,Screen.height-updownbuttonheight,Screen.width*0.5f,updownbuttonheight),"Main Menu",myStyle) )
		{
			Application.LoadLevel("mainmenu");

		}	
		if( wooi == -1 )
		{		
			// Screen Shot
			GUI.DrawTexture(new Rect(Screen.width*0.5f,updownbuttonheight,Screen.width*0.5f,Screen.height-2*updownbuttonheight),textureScreenShot);
		
			// Armory
			GUI.color = Color.white;
			GUI.Box(new Rect(0,updownbuttonheight,Screen.width*0.5f,armorydown-updownbuttonheight),"Armory",myStyle);
		
			// Weapon List
			scrollposition = GUI.BeginScrollView(new Rect(0,armorydown,Screen.width*0.5f,Screen.height-armorydown-updownbuttonheight),
				scrollposition,new Rect(0,0,Screen.width*0.48f,(GameEnvironment.storeGun.Length-1)*updownbuttonheight),false,false);
		
			for(int i=1;i<GameEnvironment.storeGun.Length;i++)
			{
				string showname = showWeapon[i]?GameEnvironment.storeGun[i].name:"????????????";
				if( GUI.Button(new Rect(0f,(i-1)*updownbuttonheight,Screen.width*0.24f,updownbuttonheight),showname,myStyle ) )
					wooi = i;
				GUI.Box(new Rect(Screen.width*0.24f,(i-1)*updownbuttonheight,Screen.width*0.24f,updownbuttonheight),GameEnvironment.storeGun[i].AmmoInformation,myStyle );
			}
		
			GUI.EndScrollView();
		}
		else
		{
			// Weapon Name
			string showname = showWeapon[wooi]?GameEnvironment.storeGun[wooi].name:"????????????";
			GUI.Box(new Rect(0,updownbuttonheight,Screen.width*0.5f,updownbuttonheight),showname,myStyle);
			
			// Cost of Ammo
			GUI.Box(new Rect(Screen.width*0.5f,updownbuttonheight,Screen.width*0.5f,updownbuttonheight),"Cost : " + cost,myStyle);
			
			// Description
			GUI.Box(new Rect(0,2*updownbuttonheight,Screen.width*0.5f,Screen.height-3*updownbuttonheight),"Current Ammo" + GameEnvironment.storeGun[wooi].AmmoInformation,myStyle);
		
			// Purchases
			if( Store.WeaponUnlocked(wooi) )
			{
				bool ew = ExistGunInCurrentLevel((Weapon)wooi);
				if( IsLevelOption || !ew )
					GUI.Box(new Rect(Screen.width*0.5f,Screen.height-2*updownbuttonheight,Screen.width*0.25f,updownbuttonheight),"Owned",myStyle); 
				if(ew && GUI.Button(new Rect(Screen.width*0.5f,Screen.height-2*updownbuttonheight,Screen.width*0.25f,updownbuttonheight),"Fill Ammo",myStyle) ) 
				{
					if( Store.zombieHeads >= cost )
					{
						audio.Play();
						Store.zombieHeads -= cost;
						GameEnvironment.storeGun[wooi].store += 5*GameEnvironment.storeGun[wooi].pocketsize;
					}
				}
			}
			else
			{
				if( GUI.Button(new Rect(Screen.width*0.5f,Screen.height-2*updownbuttonheight,Screen.width*0.25f,updownbuttonheight),"Purchase",myStyle) ) 
				{
					if( Store.zombieHeads >= cost )
					{
						Store.UnlockWeapon(wooi);
						showWeapon[wooi] = true;
						audio.Play();
						Store.zombieHeads -= cost;
						if( IsLevelGamePlay && ExistGunInCurrentLevel((Weapon)wooi))
						{
							GameEnvironment.storeGun[wooi].store += 5*GameEnvironment.storeGun[wooi].pocketsize;
							GameEnvironment.storeGun[wooi].enabled = true;
						}
					}
				}
			}
			if( GUI.Button(new Rect(Screen.width*0.75f,Screen.height-2*updownbuttonheight,Screen.width*0.25f,updownbuttonheight),"Back",myStyle) ) 
			{
				wooi = -1;
			}
		}
	}
	
	void OnGUI()
	{
		if( showStore ) DrawStore();
	}
	
	#endregion
	*/
	
	#region Google, Tapjoy

	//--------------- store purchase code ------------------//
	
	public string storePublicKey;
	public static bool tapjoyConnected = false;
	
	void OnEnable()
	{
		#if UNITY_ANDROID
		IABAndroidManager.purchaseSucceededEvent += HandleIABAndroidManagerpurchaseSucceededEvent;
		TapjoyAndroidManager.fullScreenAdDidLoadEvent += fullScreenAdDidLoadEvent;
		#endif
		
	}
	
	void OnDisable()
	{
		#if UNITY_ANDROID
		IABAndroidManager.purchaseSucceededEvent -= HandleIABAndroidManagerpurchaseSucceededEvent;
		TapjoyAndroidManager.fullScreenAdDidLoadEvent -= fullScreenAdDidLoadEvent;
		#endif
		
	}
	
	void HandleIABAndroidManagerpurchaseSucceededEvent (string obj)
	{
		Store.zombieHeads = Store.zombieHeads + 1000;
	}

	void fullScreenAdDidLoadEvent( bool didLoad )
	{
		tapjoyConnected = didLoad;
	}

	void Start()
	{
		#if UNITY_ANDROID
		IABAndroid.init( storePublicKey );
		TapjoyAndroid.init( "6f8b509b-f292-4dd3-b440-eab33f211089", "7TYeZbZ6GTqRncoALV3W", false );
		#endif
	}
	
	public void OnApplicationQuit()
	{
		#if UNITY_ANDROID
		IABAndroid.stopBillingService();
		#endif
		
	}
	
	//--------------- store purchase code end ------------------//
	
	#endregion


	#region GUI
	
	public Texture2D[] textureWeapons;
	public Texture2D textureWeaponUnknown;
	public GameObject[] objectWeapons;
	
	public GameObject StoreGUI;
	public ButtonBase buttonMainMenu;
	public ButtonBase buttonGet1000Heads;
	public ButtonBase buttonGAME;
	public ButtonBase buttonTrash;
	public GUIText zombieHeadText;
	
	private int showZombieHeads = -1; 
	private bool[] showWeapon = new bool[countWeapons];
	private bool[] showFillIn = new bool[countWeapons];
	private bool _showStore = false;
	public bool showStore{
		get
		{
			return _showStore;
		}
		set
		{
			_showStore = value;
			
			StoreGUI.SetActiveRecursively(_showStore);
			
			if( _showStore )
			{
				for(int i=0;i<countWeapons;i++) showWeapon[i]=WeaponUnlocked(i);
				for(int i=0;i<countWeapons;i++) showFillIn[i]=false;
				if( IsLevelGamePlay )
					for(int j=0;j<weaponsForLevel[LevelInfo.Environments.control.currentLevel].Length;j++)
						showFillIn[(int)weaponsForLevel[LevelInfo.Environments.control.currentLevel][j]] = true;
				if( IsLevelOption ) buttonGAME.gameObject.SetActiveRecursively(false);
				showZombieHeads = zombieHeads;
				//FirstShopItem();
				//FirstStashItem();
			}
		}
	}
	
	private bool IsLevelGamePlay { get { return Application.loadedLevel == 2; } }
	private bool IsLevelOption { get { return Application.loadedLevel == 1; } }
	
	private void UpdateZombieHeads()
	{
		int delta = Mathf.Abs(showZombieHeads-zombieHeads);
		if(delta<10) delta=1;
		else if(delta<100) delta=10;
		else delta=100;
		if( showZombieHeads < zombieHeads ) showZombieHeads+=delta;
		if( showZombieHeads > zombieHeads ) showZombieHeads-=delta;
		zombieHeadText.text = "" + showZombieHeads;
		
	}
	
	void Update()
	{
		UpdateZombieHeads();
		
		if( buttonGAME.PressedUp ) 
		{
			if( IsLevelGamePlay ) LevelInfo.Environments.control.state = GameState.Play;
			if( IsLevelOption ) Debug.LogError("Game button should be disabled in MainMenu screen");
			showStore = false;
		}
		
		if( buttonMainMenu.PressedUp )
		{
			if( IsLevelGamePlay ) Application.LoadLevel("mainmenu");
			if( IsLevelOption )
			{
				MainMenu mainmenu = (MainMenu)GameObject.FindObjectOfType(typeof(MainMenu));
				mainmenu.GoState(MainMenu.MenuState.MainMenu);
				showStore = false;
			}
		}
		
		if( buttonGet1000Heads.PressedUp )
		{
			#if UNITY_ANDROID
			IABAndroid.purchaseProduct("android.test.purchased");
			#endif
			
			#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
			Store.zombieHeads = Store.zombieHeads + 1000;
			#endif
		}
	}
	
	public GUIStyle myStyle;
	
	//private readonly Rect shopRect = new Rect(0.01f*Screen.width,0.273f*Screen.height,0.487f*Screen.width,0.421f*Screen.height);
	//private readonly Rect stashRect = new Rect(0.51f*Screen.width,0.273f*Screen.height,0.487f*Screen.width,0.421f*Screen.height);

	private readonly float itemHeight = 0.42f*Screen.height;
	
	
	private float itemdist = 0.6f;
	private void PrepareShopItems()
	{
		for(int i=0,j1=0,j2=0;i<countWeapons;i++)
		{
			if( !WeaponUnlocked(i) )
			{
				objectWeapons[i].transform.position = new Vector3(0,-j1++*itemdist,0);
				objectWeapons[i].layer = 8;
			}
			else
			{
				objectWeapons[i].transform.position = new Vector3(0,-j2++*itemdist,0);
				objectWeapons[i].layer = 9;
			}
		}
	}
	
	private int _current_shop_item = -1;
	
	private void FirstShopItem()
	{
		_current_shop_item=-1;
		NextShopItem();
	}
	
	private void NextShopItem()
	{
		for(int i=0;i<countWeapons;i++)
			objectWeapons[i].SetActiveRecursively(false);	
		
		bool allunl = true;
		for(int i=0;i<countWeapons;i++)
			if( !WeaponUnlocked(i) )
				allunl = false;
		if( allunl ) {_current_shop_item = -1; return; }
		
		do { _current_shop_item = (_current_shop_item+1)%countWeapons; }
		while( WeaponUnlocked(_current_shop_item) ); 
			
		ShowItems();
	}
	
	private void PrevShopItem()
	{
		for(int i=0;i<countWeapons;i++)
			objectWeapons[i].SetActiveRecursively(false);	
		
		bool allunl = true;
		for(int i=0;i<countWeapons;i++)
			if( !WeaponUnlocked(i) )
				allunl = false;
		if( allunl ) {_current_shop_item = -1; return; }
		
		do { _current_shop_item = (_current_shop_item-1+countWeapons)%countWeapons; }
		while( WeaponUnlocked(_current_shop_item) );
		ShowItems();
	}	
	
	private int _current_stash_item = -1;
	
	private void FirstStashItem()
	{
		_current_stash_item=-1;
		NextStashItem();
	}
	
	private void NextStashItem()
	{
		
		for(int i=0;i<countWeapons;i++)
			objectWeapons[i].SetActiveRecursively(false);	
		
		bool allunl = true;
		for(int i=0;i<countWeapons;i++)
			if( WeaponUnlocked(i) )
				allunl = false;
		if( allunl ) { _current_stash_item = -1; return; }
		
		do { _current_stash_item = (_current_stash_item+1)%countWeapons; }
		while( !WeaponUnlocked(_current_stash_item) ); 
			
		ShowItems();
		
	}
	
	private void PrevStashItem()
	{
		for(int i=0;i<countWeapons;i++)
			objectWeapons[i].SetActiveRecursively(false);	
		
		bool allunl = true;
		for(int i=0;i<countWeapons;i++)
			if( WeaponUnlocked(i) )
				allunl = false;
		if( allunl ) {_current_stash_item = -1; return; }
		
		do { _current_stash_item = (_current_stash_item-1+countWeapons)%countWeapons; }
		while( !WeaponUnlocked(_current_stash_item) );
		ShowItems();
	}	
	
	private void ShowItems()
	{
		if( _current_shop_item != -1 )
		{
			objectWeapons[_current_shop_item].SetActiveRecursively(true);
			objectWeapons[_current_shop_item].transform.position = new Vector3(-0.28f,-0.05f,0.59f);
		}
		if( _current_stash_item != -1 )
		{
			objectWeapons[_current_stash_item].SetActiveRecursively(true);
			objectWeapons[_current_stash_item].transform.position = new Vector3(0.18f,-0.05f,0.59f);		
		}
	}
	
	private void SetLayer(GameObject g,int n)
	{
		g.layer = n;
		Transform[] gg = g.GetComponentsInChildren<Transform>();
		foreach(Transform c in gg)
			c.gameObject.layer = n;
	}
	
	
	private int wooi = -1;
	private bool fillin = false;
	private Vector2 scrollposition = Vector2.zero;
	private Vector2 scrollposition2 = Vector2.zero;
	void OnGUI()
	{
		if(!_showStore) return;
		
		
		// Shop
		Rect shopRect = new Rect(0.01f*Screen.width,0.273f*Screen.height,0.487f*Screen.width,0.421f*Screen.height);
		scrollposition = GUI.BeginScrollView(new Rect(0.01f*Screen.width,0.273f*Screen.height,0.487f*Screen.width,0.421f*Screen.height),scrollposition,new Rect(0f,0f,0.457f*Screen.width,8*itemHeight),false,true);
		foreach(Touch touch in Input.touches)
			if( RectContainPoint(shopRect,touch.position))
				scrollposition.y += 0.5f*touch.deltaPosition.y;
		
		for(int i=0,j=0;i<GameEnvironment.storeGun.Length;i++)
			if(!WeaponUnlocked(i) )
			{
				objectWeapons[i].transform.localPosition = new Vector3(0,-j + 1/0.42f*scrollposition.y/Screen.height,0);
				SetLayer(objectWeapons[i],8);
				//Texture2D texture = showWeapon[i]?textureWeapons[i]:textureWeaponUnknown;
				//GUI.DrawTexture(new Rect(0.035f*Screen.width,j*itemHeight,0.246f*Screen.width,itemHeight),textureWeapons[i]);
				GUI.Box(new Rect(0.02f*Screen.width,j*itemHeight,0.3f*Screen.width,0.08f*Screen.height),GameEnvironment.storeGun[i].name,myStyle);	
				if(wooi==-1)
				{			
					if( GUI.Button(new Rect(0.335f*Screen.width,(j+0.5f)*itemHeight,0.14f*Screen.width,0.08f*Screen.height),"" + (25*i),myStyle) )
						wooi = i;
				}
				else
					GUI.Box(new Rect(0.335f*Screen.width,(j+0.5f)*itemHeight,0.14f*Screen.width,0.08f*Screen.height),"" + (25*i),myStyle);
				j++;
			}
		
		GUI.EndScrollView();
	
		
		// Stash
		Rect stashRect = new Rect(0.51f*Screen.width,0.273f*Screen.height,0.487f*Screen.width,0.421f*Screen.height);
		scrollposition2 = GUI.BeginScrollView(new Rect(0.51f*Screen.width,0.273f*Screen.height,0.487f*Screen.width,0.421f*Screen.height),scrollposition2,new Rect(0f,0f,0.457f*Screen.width,8*itemHeight),false,true);
		foreach(Touch touch in Input.touches)
			if( RectContainPoint(stashRect,touch.position))
				scrollposition2.y += 0.5f*touch.deltaPosition.y;
		
		for(int i=0,j=0;i<GameEnvironment.storeGun.Length;i++)
			if( WeaponUnlocked(i) )
			{
				objectWeapons[i].transform.localPosition = new Vector3(0,-j + 1/0.42f*scrollposition2.y/Screen.height,0);
				SetLayer(objectWeapons[i],9);	
				
				GUI.Box(new Rect(0.02f*Screen.width,j*itemHeight,0.3f*Screen.width,0.08f*Screen.height),GameEnvironment.storeGun[i].name,myStyle);	
				if( IsLevelGamePlay && showFillIn[i])
				{
					if(wooi==-1 && GUI.Button(new Rect(0.335f*Screen.width,(j+0.5f)*itemHeight,0.14f*Screen.width,0.08f*Screen.height),GameEnvironment.storeGun[i].AmmoInformation,myStyle) )
					{
						wooi = i;
						fillin = true;
					}
					else
						GUI.Box(new Rect(0.335f*Screen.width,(j+0.5f)*itemHeight,0.14f*Screen.width,0.08f*Screen.height),GameEnvironment.storeGun[i].AmmoInformation,myStyle);
				}
				j++;
			}
		
		
		GUI.EndScrollView();
				
		
		/*if( _current_shop_item != -1 )
		{
			GUI.Box(new Rect(0.03f*Screen.width,0.3f*Screen.height,0.3f*Screen.width,0.1f*Screen.height),GameEnvironment.storeGun[_current_shop_item].name,myStyle);
			
			if( wooi == -1 )
			{
				if( GameEnvironment.AbsoluteSwipe.y > 0f )
					PrevShopItem();
				if( GameEnvironment.AbsoluteSwipe.y < 0f )
					NextShopItem();
				
				
				if( GUI.Button(new Rect(0.36f*Screen.width,0.4f*Screen.height,0.13f*Screen.width,0.08f*Screen.height),"Prev",myStyle) )
					PrevShopItem();
				if( GUI.Button(new Rect(0.36f*Screen.width,0.5f*Screen.height,0.13f*Screen.width,0.08f*Screen.height),"Next",myStyle) )
					NextShopItem();
				if( GUI.Button(new Rect(0.36f*Screen.width,0.6f*Screen.height,0.13f*Screen.width,0.08f*Screen.height),"Buy",myStyle) )
					wooi = _current_shop_item;
			}
		}		
		
		if( _current_stash_item != -1 )
		{
			GUI.Box(new Rect(0.53f*Screen.width,0.3f*Screen.height,0.3f*Screen.width,0.1f*Screen.height),GameEnvironment.storeGun[_current_stash_item].name,myStyle);
			
			if( wooi == -1 )
			{
				if( GUI.Button(new Rect(0.86f*Screen.width,0.4f*Screen.height,0.13f*Screen.width,0.08f*Screen.height),"Prev",myStyle) )
					PrevStashItem();
				if( GUI.Button(new Rect(0.86f*Screen.width,0.5f*Screen.height,0.13f*Screen.width,0.08f*Screen.height),"Next",myStyle) )
					NextStashItem();
				if(IsLevelGamePlay && GUI.Button(new Rect(0.86f*Screen.width,0.6f*Screen.height,0.13f*Screen.width,0.08f*Screen.height),"Fill in",myStyle) )
				{
					wooi = _current_stash_item;
					fillin = true;
				}
			}
		}*/
		
		if( wooi != -1 )
		{
			if(fillin)
				ShowFillInDialog();
			else
				ShowWeaponBuyDialog();
		}
	}
	
	private void ShowWeaponBuyDialog()
	{
		if( Store.zombieHeads >= 25*wooi )
		{
			GUI.Box(new Rect(0.25f*Screen.width,0.25f*Screen.height,0.5f*Screen.width,0.5f*Screen.height),"Do you want to buy this item?");
			
			if(GUI.Button(new Rect(0.35f*Screen.width,0.4f*Screen.height,0.3f*Screen.width,0.1f*Screen.height), "Buy" ) )	
			{
				Store.UnlockWeapon(wooi);
				showWeapon[wooi] = true;
				audio.Play();
				Store.zombieHeads -= 25*wooi;
				if( IsLevelGamePlay && ExistGunInCurrentLevel((Weapon)wooi))
				{
					GameEnvironment.storeGun[wooi].store += 5*GameEnvironment.storeGun[wooi].pocketsize;
					GameEnvironment.storeGun[wooi].enabled = true;
				}
				wooi = -1;
			}
			if( GUI.Button(new Rect(0.35f*Screen.width,0.6f*Screen.height,0.3f*Screen.width,0.1f*Screen.height), "Back" ) )
				wooi = -1;			
		}
		else
		{
			GUI.Box(new Rect(0.25f*Screen.width,0.25f*Screen.height,0.5f*Screen.width,0.5f*Screen.height),"You have not enough heads to buy this item.");
			if( GUI.Button(new Rect(0.35f*Screen.width,0.5f*Screen.height,0.3f*Screen.width,0.1f*Screen.height), "Back") )
				wooi = -1;
		}
		

	}
	
	private void ShowFillInDialog()
	{
		if( Store.zombieHeads >= 100 )
		{
			GUI.Box(new Rect(0.25f*Screen.width,0.25f*Screen.height,0.5f*Screen.width,0.5f*Screen.height),"Do you want to Fill in ammo?");
			
			if(GUI.Button(new Rect(0.35f*Screen.width,0.4f*Screen.height,0.3f*Screen.width,0.1f*Screen.height), "Fill in" ) )	
			{
				audio.Play();
				Store.zombieHeads -= 100;
				GameEnvironment.storeGun[wooi].store += 5*GameEnvironment.storeGun[wooi].pocketsize;
				wooi = -1;
				fillin = false;
			}
			if( GUI.Button(new Rect(0.35f*Screen.width,0.6f*Screen.height,0.3f*Screen.width,0.1f*Screen.height), "Back" ) )
			{
				wooi = -1;
				fillin = false;
			}
		}
		else
		{
			GUI.Box(new Rect(0.25f*Screen.width,0.25f*Screen.height,0.5f*Screen.width,0.5f*Screen.height),"You have not enough heads.");
			if( GUI.Button(new Rect(0.35f*Screen.width,0.5f*Screen.height,0.3f*Screen.width,0.1f*Screen.height), "Back") )
			{
				wooi = -1;
				fillin = false;
			}
		}
		

	}
	
	private bool ExistGunInCurrentLevel(Weapon w)
	{
		if( !IsLevelGamePlay ) return false;
			foreach(Weapon c in weaponsForLevel[GameEnvironment.StartLevel] )
				if( c == w )
				return true;
		return false;
	}
	
	private bool RectContainPoint(Rect rect,Vector2 pos)
	{
		return rect.xMin <= pos.x && pos.x <= rect.xMax && rect.yMin <= pos.y && pos.y <= rect.yMax;
	}
	
	#endregion
}
