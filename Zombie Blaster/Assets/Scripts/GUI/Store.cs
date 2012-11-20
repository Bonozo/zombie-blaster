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
	
	/*public static int _playerprefs_unlockCity=0;
	public static int unlockCity{
		get
		{
			return _playerprefs_unlockCity;
		}
		set
		{
			_playerprefs_unlockCity = value;
			PlayerPrefs.SetInt("unlockCity",_playerprefs_unlockCity);
		}
	}
	
	public static int _playerprefs_unlockStadiums=0;
	public static int unlockStadiums{
		get
		{
			return _playerprefs_unlockStadiums;
		}
		set
		{
			_playerprefs_unlockStadiums = value;
			PlayerPrefs.SetInt("unlockStadiums",_playerprefs_unlockStadiums);
		}
	}
	
	public static int _playerprefs_unlockFrat=0;
	public static int unlockFrat{
		get
		{
			return _playerprefs_unlockFrat;
		}
		set
		{
			_playerprefs_unlockFrat = value;
			PlayerPrefs.SetInt("unlockFrat",_playerprefs_unlockFrat);
		}
	}
	
	public static int _playerprefs_unlockCemetery=0;
	public static int unlockCemetery{
		get
		{
			return _playerprefs_unlockCemetery;
		}
		set
		{
			_playerprefs_unlockCemetery = value;
			PlayerPrefs.SetInt("unlockCemetery",_playerprefs_unlockCemetery);
		}
	}
	*/
	private static int[] _playerprefs_unlockweapon = new int[countWeapons];
	private static int[] _playerprefs_unlocklevel = new int[countLevel];
	
	/// <summary>
	/// Unlocks the level.
	/// </summary>
	/// <param name='index'>
	/// current city code
	/// </param>
	
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
	
	#endregion
	
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

	void Awake()
	{
		DontDestroyOnLoad(this.gameObject);
		RestorePlayerPrefs();
	}
	
	private bool IsLevelGamePlay { get { return Application.loadedLevel == 2; } }
	private bool IsLevelOption { get { return Application.loadedLevel == 1; } }
	
	private bool ExistGunInCurrentLevel(Weapon w)
	{
		if( !IsLevelGamePlay ) return false;
			foreach(Weapon c in weaponsForLevel[GameEnvironment.StartLevel] )
				if( c == w )
				return true;
		return false;
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
				scrollposition,new Rect(0,0,Screen.width*0.48f,(GameEnvironment.storeGun.Length-1)*updownbuttonheight),false,true);
		
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
	
	#region Google

	//--------------- store purchase code ------------------//
	
	public string storePublicKey;
	
	void OnEnable()
	{
		IABAndroidManager.purchaseSucceededEvent += HandleIABAndroidManagerpurchaseSucceededEvent;
	}
	
	void OnDisable()
	{
		IABAndroidManager.purchaseSucceededEvent -= HandleIABAndroidManagerpurchaseSucceededEvent;
	}
	
	void HandleIABAndroidManagerpurchaseSucceededEvent (string obj)
	{
		Store.zombieHeads = Store.zombieHeads + 1000;
	}
	
	void Start()
	{
		IABAndroid.init( storePublicKey );
	}
	
	public void OnApplicationQuit()
	{
		IABAndroid.stopBillingService();
	}
	
	//--------------- store purchase code end ------------------//
	
	#endregion
}
