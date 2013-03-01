using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
	// // Crossbow, Shotgun, Flame Thrower, Football, Machine Gun, Grenades, Revolver, Rocket Launcher
	BB,
	Crossbow,
	PulseShotGun,
	Flamethrower,
	Football,
	MachineGun,
	Grenade,
	Revolver,
	Rocket,
	AlienBlaster,
	Sniper,
	Zapper,
	Microwave,
	None
};

public class Store : MonoBehaviour {
	
	public static int countLevel = 5;
	public static int countWeapons = 10;
	
	/*public static Weapon[][] weaponsForLevel = new Weapon[][]
	{
		new Weapon[] {Weapon.BB,Weapon.Flamethrower,Weapon.Crossbow},
		new Weapon[] {Weapon.BB,Weapon.Flamethrower,Weapon.Crossbow,Weapon.Grenade},
		new Weapon[] {Weapon.BB,Weapon.Flamethrower,Weapon.Crossbow,Weapon.Football,Weapon.MachineGun},
		new Weapon[] {Weapon.BB,Weapon.Flamethrower,Weapon.Crossbow,Weapon.Grenade,Weapon.MachineGun,Weapon.Rocket},
		new Weapon[] {Weapon.BB,Weapon.Flamethrower,Weapon.Crossbow,Weapon.Grenade,Weapon.MachineGun,Weapon.Rocket,Weapon.Football,Weapon.PulseShotGun,Weapon.Revolver}
	};*/
	public static Weapon[][] weaponsForLevel = new Weapon[][]
	{
		new Weapon[] {Weapon.Flamethrower,Weapon.Crossbow,Weapon.AlienBlaster},
		new Weapon[] {Weapon.Grenade},
		new Weapon[] {Weapon.Football,Weapon.MachineGun},
		new Weapon[] {Weapon.Rocket},
		new Weapon[] {Weapon.PulseShotGun,Weapon.Revolver}
	};
	
	public Texture2D[] weaponIcon;
	
	#region Player Prefs
	
	private static int _playerprefs_zombieHeads=0;
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
	
	private static int _firsttimeplay=0;
	public static bool FirstTimePlay
	{
		get
		{
			return _firsttimeplay==0;
		}
		set
		{
			_firsttimeplay = value?0:1;
			PlayerPrefs.SetInt("zbfirsttimeplay",_firsttimeplay);		
		}
	}
	
	
	private static int[] _playerprefs_unlockweapon = new int[countWeapons];
	private static int[] _playerprefs_unlocklevel = new int[countLevel];
	private static int[] _playerprefs_highestwavecompleted = new int[countLevel];
		
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
	public static int HighestWaveCompleted(int level)
	{
		return _playerprefs_highestwavecompleted[level];
	}
	public static void SetHighestWaveCompleted(int level,int newscore)
	{
		if( newscore > _playerprefs_highestwavecompleted[level] )
		{
			_playerprefs_highestwavecompleted[level] = newscore;
			PlayerPrefs.SetInt("highestwavecompleted"+level,_playerprefs_highestwavecompleted[level]);
		}
	}	
	
	public static bool CanBuyWeapon(int weapon)
	{
		return !WeaponUnlocked(weapon)&&GameEnvironment.storeGun[weapon].price<=zombieHeads;
	}
	
	public void RestorePlayerPrefs()
	{
		// heads
		_playerprefs_zombieHeads = PlayerPrefs.GetInt("zombieHeads",0);
		
		// forst time play
		_firsttimeplay = PlayerPrefs.GetInt("zbfirsttimeplay",0);
		
		// weapons
		Store._playerprefs_unlockweapon[0] = 1;
		for(int i=1;i<_playerprefs_unlockweapon.Length;i++)
		{
			Store._playerprefs_unlockweapon[i] = PlayerPrefs.GetInt("weapon"+i,0);
		}	
		
		// levels
		for(int i=0;i<_playerprefs_unlocklevel.Length;i++)
		{
			Store._playerprefs_unlocklevel[i] = PlayerPrefs.GetInt("level"+i,0);
		}
		
		// highestwavecompleted
		for(int i=0;i<_playerprefs_highestwavecompleted.Length;i++)
		{
			Store._playerprefs_highestwavecompleted[i] = PlayerPrefs.GetInt("highestwavecompleted"+i,0);
		}		
		
	}
	
	void Awake()
	{
		DontDestroyOnLoad(this.gameObject);
		RestorePlayerPrefs();
		Application.targetFrameRate = 60;
	}
	
	#endregion
	
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
		
		//By Mak Kaloliya on 07022012
		#if UNITY_IPHONE
		TapjoyManager.fullscreenAdDidLoadEvent += fullscreenAdDidLoadEventIOS;
		TapjoyManager.tapPointsReceivedEvent += tapPointsReceived;
		TapjoyManager.receiveTapPointsFailedEvent += receiveTapPointsFailed;
		StoreKitManager.purchaseSuccessfulEvent += zombieStoreKitPurchaseSuccessful;
		StoreKitManager.purchaseCancelledEvent += zombieStoreKitPurchaseCancelled;
		StoreKitManager.purchaseFailedEvent += zombieStoreKitPurchaseFailed;
		#endif
		//End
		
	}
	
	void OnDisable()
	{
		#if UNITY_ANDROID
		IABAndroidManager.purchaseSucceededEvent -= HandleIABAndroidManagerpurchaseSucceededEvent;
		TapjoyAndroidManager.fullScreenAdDidLoadEvent -= fullScreenAdDidLoadEvent;
		#endif
		
		//By Mak Kaloliya on 07022013
		#if UNITY_IPHONE
		TapjoyManager.fullscreenAdDidLoadEvent -= fullscreenAdDidLoadEventIOS;
		TapjoyManager.tapPointsReceivedEvent -= tapPointsReceived;
		TapjoyManager.receiveTapPointsFailedEvent -= receiveTapPointsFailed;
		StoreKitManager.purchaseSuccessfulEvent -= zombieStoreKitPurchaseSuccessful;
		StoreKitManager.purchaseCancelledEvent -= zombieStoreKitPurchaseCancelled;
		StoreKitManager.purchaseFailedEvent -= zombieStoreKitPurchaseFailed;
		#endif
		//End
	}
	
	void HandleIABAndroidManagerpurchaseSucceededEvent (string obj)
	{
		Store.zombieHeads = Store.zombieHeads + 1000;
	}
	
	#if UNITY_IPHONE
	void zombieStoreKitPurchaseSuccessful( StoreKitTransaction transaction )
	{
		Debug.Log( "purchased product: " + transaction );
		Store.zombieHeads = Store.zombieHeads + 1000;
	}
	#endif
	
	void zombieStoreKitPurchaseFailed( string error )
	{
		Debug.Log( "purchase failed with error: " + error );
	}
	
	void zombieStoreKitPurchaseCancelled( string error )
	{
		Debug.Log( "purchase cancelled with error: " + error );
	}

	void fullScreenAdDidLoadEvent( bool didLoad )
	{
		tapjoyConnected = didLoad;
	}
	
	//By Mak Kaloliya on 07022013
	void fullscreenAdDidLoadEventIOS()
	{
		tapjoyConnected = true;
	}
	
	private void tapPointsReceived( int totalPoints )
	{
		Debug.Log( "tapPointsReceived: " + totalPoints );
	}
	
	
	private void receiveTapPointsFailed()
	{
		Debug.Log( "receiveTapPointsFailed" );
	}
	//End
	#if UNITY_IPHONE
	private List<StoreKitProduct> _products;
	#endif
	void Start()
	{
		#if UNITY_ANDROID
		IABAndroid.init( storePublicKey );
		TapjoyAndroid.init( "6f8b509b-f292-4dd3-b440-eab33f211089", "7TYeZbZ6GTqRncoALV3W", false );//old
		//TapjoyAndroid.init( "b1f6ad92-1ff9-47ca-a962-a4b7ecddebd2", "wNZUPjewwCeVRkgpCCZQ", false );//new
		#endif 
		
		//By Mak Kaloliya on 07022013
		#if UNITY_IPHONE
		TapjoyBinding.init( "b6b895c2-5c52-4eac-b102-4ae2a6bcaf84", "IaaQuo03VctIM6m3Qr5Z", true );
		StoreKitManager.productListReceivedEvent += allProducts =>
		{
			Debug.Log( "received total products: " + allProducts.Count );
			_products = allProducts;
		};
		var productIdentifiers = new string[] {"ZH1000"};
		StoreKitBinding.requestProductData( productIdentifiers );
		#endif
		//End
		
		// Clear Prefabs
		//PlayerPrefs.SetInt("zombieHeads",10000);
		//for(int i=1;i<_playerprefs_unlockweapon.Length;i++)
		//	PlayerPrefs.SetInt("weapon"+i,0);
		//for(int i=0;i<_playerprefs_unlocklevel.Length;i++)
		//	PlayerPrefs.SetInt("level"+i,0);
		//PlayerPrefs.SetInt("zbfirsttimeplay",0);		
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
	
	public AudioClip audioScrolling;
	public AudioClip clipBuy;
	public AudioClip clipBack;
	public AudioClip clipShowStore;
	public AudioClip clipQuitPlayGame;
	
	public Texture2D[] textureWeapons;
	public GameObject[] objectWeapons;
	public AudioClip[] clipWeaponInfo;
	
	public Font blackFont,redFont,greenFont;
	
	public GameObject StoreGUI;
	public GameObject LoadingGUI;
	public ButtonBase buttonMainMenu;
	public ButtonBase buttonGet1000Heads;
	public ButtonBase buttonGAME;
	public ButtonBase buttonTrash;
	public GUIText zombieHeadText;
	public Texture2D popupTexture;
	public UpDownAnimate getHeadsAnimation;
	
	private bool wantToExit = false;
	
	public GUIText ShopWeaponName;
	public GUIText ShopWeaponBuyText;
	public ButtonBase shotItemBuy;
	public ButtonBase shopInfoButton;
	private int currentshopitem = -2;
	public GUITexture shopItemTexture;
	
	public GUIText StashWeaponName;
	public GUIText StashWeaponBuyText;
	public ButtonBase StashItemBuy;
	public ButtonBase StashInfoButton;
	private int currentStashitem = -2;
	public GUITexture stashItemTexture;
	
	public GUITexture[] scrollingInfo;
	private float scrollingTime = 0.0f;
	
	public GUITexture arrowUpShow;
	public GUITexture arrowDownShow;
	public GUITexture arrowUpStash;
	public GUITexture arrowDownStash;
	
	private bool spwchannel = false;
	
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
			
			StoreGUI.SetActive(_showStore);
			
			if( _showStore )
			{
				audio.PlayOneShot(clipShowStore);
				
				UpdateWeaponAvailable();
				
				for(int i=0;i<countWeapons;i++) showFillIn[i]=false;
				if( IsLevelGamePlay )
					for(int j=0;j<weaponsForLevel[LevelInfo.Environments.control.currentLevel].Length;j++)
						showFillIn[(int)weaponsForLevel[LevelInfo.Environments.control.currentLevel][j]] = true;
		
				if( IsLevelOption )
				{
					MainMenu mainmenu = (MainMenu)GameObject.FindObjectOfType(typeof(MainMenu));
					if(!mainmenu.mapToStore)
						buttonGAME.gameObject.SetActive(false);
				}
				showZombieHeads = zombieHeads;
				
				LoadingGUI.SetActive(false);
				wantToExit = false;
				//FirstShopItem();
				//FirstStashItem();
				
				getHeadsAnimation.ResetDown();
				scrollingTime = 0f;
				Update ();
			}
			else
			{
				audio.Stop();
			}
		}
	}
	
	public void UpdateWeaponAvailable()// Show Weapon
	{
		for(int i=0;i<countWeapons;i++) showWeapon[i]=false;
		bool alllevelsunlocked=true;
		for(int i=0;i<countLevel;i++)
			if( LevelUnlocked(i) )
				for(int j=0;j<weaponsForLevel[i].Length;j++)
					showWeapon[(int)weaponsForLevel[i][j]]=true;
			else
				alllevelsunlocked = false;
		showWeapon[(int)Weapon.AlienBlaster]=alllevelsunlocked;
	}
	
	/// <summary>
	/// Weapon available for buy in store (caution: call UpdateWeaponAvailable() before this
	/// </summary>
	/// <returns>
	/// availablity
	/// </returns>
	/// <param name='index'>
	/// index of the weapon
	/// </param>
	public bool WeaponAvailable(int index)
	{
		return showWeapon[index];
	}
	
	public bool IsLevelGamePlay { get { return Application.loadedLevelName == "playgame"; } }
	public bool IsLevelOption { get { return Application.loadedLevelName == "mainmenu"; } }
	
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
	
	public void Get1000HeadsEvent()
	{
		#if UNITY_ANDROID
		IABAndroid.purchaseProduct("android.test.purchased");
		#endif
		
		#if UNITY_IPHONE
		if(_products.Count > 0)
			StoreKitBinding.purchaseProduct( "ZH1000", 1 );
		#endif
			
		#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
		Store.zombieHeads = Store.zombieHeads + 1000;
		#endif
	}
	
	private bool exitVerified = false;
	
	private void ScrollingUpdate()
	{
		int cco=0;
		for(int i=0;i<countWeapons;i++)
				if( WeaponUnlocked(i) ) cco++;
		scrollingInfo[0].enabled = scrollingTime<3&&countWeapons-cco>1;
		scrollingInfo[1].enabled = scrollingTime<3&&cco>1;
		foreach(var c in scrollingInfo)
		{
			if( scrollingTime < 3f )
			{
				scrollingTime += 0.0166666f;
				Vector3 v = c.transform.position;
				v.y = 0.5f + 0.1f*Mathf.Sin(2*scrollingTime);
				c.transform.position = v;
			}
		}		
	}
	
	void Update()
	{
		if(exitVerified)
		{
			exitVerified=false;
			Application.LoadLevel("mainmenu");
			return;
		}
		
		if(!_showStore) return;

		ScrollingUpdate();
		
		if(Fade.InProcess) return;
		if( wantToExit ) return;
		
		UpdateZombieHeads();
		
		if( buttonGAME.PressedUp ) 
		{
			if( IsLevelGamePlay ) LevelInfo.Environments.control.state = GameState.Play;
			if( IsLevelOption ) 
			{
				MainMenu mainmenu = (MainMenu)GameObject.FindObjectOfType(typeof(MainMenu));
				mainmenu.mapToStore = false;
				mainmenu.GoState(MainMenu.MenuState.AreaMap);			
			}
			showStore = false;
		}
		
		if( buttonMainMenu.PressedUp )
		{
			if( IsLevelGamePlay ) wantToExit = true;
			if( IsLevelOption )
			{
				MainMenu mainmenu = (MainMenu)GameObject.FindObjectOfType(typeof(MainMenu));
				mainmenu.mapToStore = false;
				mainmenu.GoState(MainMenu.MenuState.MainMenu);
				showStore = false;
			}
		}
		
		if( buttonGet1000Heads.PressedUp )
		{
			Get1000HeadsEvent();
		}
		
		if( currentshopitem == -2 )
			currentshopitem = FirstWeapon(false);
	
		if( currentStashitem == -2 )
			currentStashitem = FirstWeapon(true);
		
		bool enableshowshopitems = currentshopitem != -1 && shopitemslidecount==0;
		
		ShopWeaponName.enabled = enableshowshopitems;
		ShopWeaponBuyText.enabled = enableshowshopitems&&showWeapon[currentshopitem];;
		shopInfoButton.gameObject.SetActive(enableshowshopitems&&showWeapon[currentshopitem]);
		shotItemBuy.gameObject.SetActive(enableshowshopitems&&showWeapon[currentshopitem]);
		shotItemBuy.enabled = enableshowshopitems&&showWeapon[currentshopitem]&&wooi==-1;
		shopItemTexture.enabled = enableshowshopitems;
		if( shopItemTexture.enabled ) shopItemTexture.texture = weaponIcon[currentshopitem];
		
		for(int i=0;i<countWeapons;i++)
			SetLayer(objectWeapons[i],WeaponUnlocked(i)?9:8);
		
		Vector2 swp = GameEnvironment.AbsoluteSwipe;
			if( Mathf.Abs(swp.x) > Mathf.Abs(swp.y) || spwchannel)
			{
				swp = Vector2.zero;
				spwchannel = false;
			}
		
		arrowUpShow.enabled = arrowDownShow.enabled = arrowUpStash.enabled = arrowDownStash.enabled = false;
		// shop
		
		for(int i=2;i<countWeapons;i++)
			if(showWeapon[i])
				SetMaterial(objectWeapons[i],Color.white);
			else
				SetMaterial(objectWeapons[i],Color.black);
		
		if( enableshowshopitems && wooi==-1)
		{
			int nx = NextWeapon(currentshopitem,false),pv = PrevWeapon(currentshopitem,false);
			arrowUpShow.enabled   = nx!=-1&&nx!=currentshopitem;
			arrowDownShow.enabled = pv!=-1&&pv!=currentshopitem;
			
			for(int i=0;i<countWeapons;i++)
				if( !WeaponUnlocked(i) )
					objectWeapons[i].transform.localPosition = new Vector3(0,1f,0);
			////objectsWeaponsUnknown1.transform.localPosition = new Vector3(0,1f,0);
			////objectsWeaponsUnknown2.transform.localPosition = new Vector3(0,1f,0);
			
			////GameObject currentWeaponobj = showWeapon[currentshopitem]?objectWeapons[currentshopitem]:objectsWeaponsUnknown1;
			GameObject currentWeaponobj = objectWeapons[currentshopitem];
			
			currentWeaponobj.transform.localPosition = new Vector3(0f,0f,0f);
			ShopWeaponName.text = showWeapon[currentshopitem]?GameEnvironment.storeGun[currentshopitem].name:"Not Available";
			
			ShopWeaponName.font = showWeapon[currentshopitem]?blackFont:redFont;
			
			ShopWeaponBuyText.text = "COST: " + GameEnvironment.storeGun[currentshopitem].price;
			ShopWeaponBuyText.font = zombieHeads>=GameEnvironment.storeGun[currentshopitem].price?greenFont:blackFont;
			
			if( shopInfoButton.PressedDown )
			{
				wooi = currentshopitem;
				weapondescription = true;
				audio.PlayOneShot(clipWeaponInfo[wooi]);
				spwchannel = true;
			}
			
			if( shotItemBuy.PressedDown )
			{
				audio.PlayOneShot(clipBuy);
				wooi = currentshopitem;
				spwchannel = true;
			}	
			
			Rect shopRect = new Rect(0.01f*Screen.width,0.1f*Screen.height,0.487f*Screen.width,0.9f*Screen.height);
			//Rect shopRect = new Rect(0.01f*Screen.width,0.273f*Screen.height,0.487f*Screen.width,0.421f*Screen.height);
			
			if( RectContainPoint(shopRect,GameEnvironment.AbsoluteSwipeBegin) && RectContainPoint(shopRect,GameEnvironment.AbsoluteSwipeEnd) && swp.y > 0 )
			{
				int olditem = currentshopitem;
				currentshopitem = NextWeapon(currentshopitem,false);
				if( olditem != currentshopitem )
				{
					//GameObject newWeaponobj = showWeapon[currentshopitem]?objectWeapons[currentshopitem]:objectsWeaponsUnknown2;
					GameObject newWeaponobj = objectWeapons[currentshopitem];
					StartCoroutine(ShopItemsSlide(currentWeaponobj,0f,1f));
					StartCoroutine(ShopItemsSlide(newWeaponobj,-1f,0f));
				}
			}
			if( RectContainPoint(shopRect,GameEnvironment.AbsoluteSwipeBegin) && RectContainPoint(shopRect,GameEnvironment.AbsoluteSwipeEnd) && swp.y < 0 )
			{
				int olditem = currentshopitem;
				currentshopitem = PrevWeapon(currentshopitem,false);
				if( olditem != currentshopitem )
				{
					//GameObject newWeaponobj = showWeapon[currentshopitem]?objectWeapons[currentshopitem]:objectsWeaponsUnknown2;
					GameObject newWeaponobj = objectWeapons[currentshopitem];
					StartCoroutine(ShopItemsSlide(currentWeaponobj,0f,-1f));
					StartCoroutine(ShopItemsSlide(newWeaponobj,1f,0f));
				}
			}
		}
		
		if( currentshopitem == -1 )
		{
			//objectsWeaponsUnknown1.transform.localPosition = new Vector3(0,1f,0);
			//objectsWeaponsUnknown2.transform.localPosition = new Vector3(0,1f,0);		
		}
		
		bool enableshowstashitems = currentStashitem != -1 && stashitemslidecount==0;
		
		StashWeaponName.enabled = enableshowstashitems;
		StashWeaponBuyText.enabled = enableshowstashitems&&IsLevelGamePlay&&showFillIn[currentStashitem];
		StashInfoButton.gameObject.SetActive(enableshowstashitems);
		StashItemBuy.gameObject.SetActive(enableshowstashitems&&IsLevelGamePlay&&showFillIn[currentStashitem]);
		StashItemBuy.enabled = enableshowstashitems&&wooi==-1&&IsLevelGamePlay&&showFillIn[currentStashitem];
		stashItemTexture.enabled = enableshowstashitems;
		if( stashItemTexture.enabled ) stashItemTexture.texture = weaponIcon[currentStashitem];
		

		// Stash
		if( enableshowstashitems && wooi==-1)
		{	
			int nx = NextWeapon(currentStashitem,true),pv = PrevWeapon(currentStashitem,true);
			arrowUpStash.enabled   = nx!=-1&&nx!=currentStashitem;
			arrowDownStash.enabled = pv!=-1&&pv!=currentStashitem;
			
			for(int i=0;i<countWeapons;i++)
				if( WeaponUnlocked(i) )
					objectWeapons[i].transform.localPosition = new Vector3(0,1f,0);
			
			objectWeapons[currentStashitem].transform.localPosition = new Vector3(0f,0f,0f);
			
			StashWeaponName.text = GameEnvironment.storeGun[currentStashitem].name;
			
			if( StashWeaponBuyText.enabled )
				StashWeaponBuyText.text = LevelInfo.Environments.guns.gun[currentStashitem].AmmoInformation;
			/*StashWeaponBuyText.text = 
				"Dmg: " + GameEnvironment.storeGun[currentStashitem].AmmoInformationFormal +
				"\nAmmo: " + GameEnvironment.storeGun[currentStashitem].pocketsize + 
				"\nReload: " + GameEnvironment.storeGun[currentStashitem].reloadTime + 
				"\nSpeed: " + GameEnvironment.storeGun[currentStashitem].speed;	*/		
			if( StashInfoButton.PressedDown )
			{
				wooi = currentStashitem;
				weapondescription = true;
				audio.PlayOneShot(clipWeaponInfo[wooi]);
			}
			
			if( StashItemBuy.enabled && StashItemBuy.PressedDown )
			{
				audio.PlayOneShot(clipBuy);
				wooi = currentStashitem;
				fillin = true;
			}	
			
			Rect StashRect = new Rect(0.51f*Screen.width,0.25f*Screen.height,0.487f*Screen.width,0.75f*Screen.height);
			//Rect StashRect = new Rect(0.51f*Screen.width,0.273f*Screen.height,0.487f*Screen.width,0.421f*Screen.height);
			
			if( RectContainPoint(StashRect,GameEnvironment.AbsoluteSwipeBegin) && RectContainPoint(StashRect,GameEnvironment.AbsoluteSwipeEnd) && swp.y > 0 )
			{
				int olditem = currentStashitem;
				currentStashitem = NextWeapon(currentStashitem,true);
				if( olditem != currentStashitem )
				{
					StartCoroutine(StashItemsSlide(objectWeapons[olditem],0f,1f));
					StartCoroutine(StashItemsSlide(objectWeapons[currentStashitem],-1f,0f));
				}
			}
			if( RectContainPoint(StashRect,GameEnvironment.AbsoluteSwipeBegin) && RectContainPoint(StashRect,GameEnvironment.AbsoluteSwipeEnd) && swp.y < 0 )
			{
				int olditem = currentStashitem;
				currentStashitem = PrevWeapon(currentStashitem,true);
				if( olditem != currentStashitem )
				{
					StartCoroutine(StashItemsSlide(objectWeapons[olditem],0f,-1f));
					StartCoroutine(StashItemsSlide(objectWeapons[currentStashitem],1f,0f));
				}
			}
		}
	}
	
	public void DisableStoreButtons()
	{
		buttonMainMenu.DisableButtonForUse();
		buttonGet1000Heads.DisableButtonForUse();
		buttonGAME.DisableButtonForUse();
	}
	
	public void EnableStoreButtons()
	{
		buttonMainMenu.EnableButtonForUse();
		buttonGet1000Heads.EnableButtonForUse();
		buttonGAME.EnableButtonForUse();
	}
	
	private int shopitemslidecount=0;
	private IEnumerator ShopItemsSlide(GameObject obj,float y1,float y2)
	{
		shopitemslidecount++;
		float time = 0.33f;
		while(time>0)
		{
			time -= 0.016f;
			if( time < 0f ) time = 0.0f;
			Vector3 v = obj.transform.localPosition;
			v.y = y1+(y2-y1)*(1.0f-2*time);
			obj.transform.localPosition = v;
			yield return new WaitForEndOfFrame();
		}
		shopitemslidecount--;
		audio.PlayOneShot(audioScrolling);
	}
	
	private int stashitemslidecount=0;
	private IEnumerator StashItemsSlide(GameObject obj,float y1,float y2)
	{
		stashitemslidecount++;
		float time = 0.33f;
		while(time>0)
		{
			time -= 0.016f;
			if( time < 0f ) time = 0.0f;
			Vector3 v = obj.transform.localPosition;
			v.y = y1+(y2-y1)*(1.0f-2*time);
			obj.transform.localPosition = v;
			yield return new WaitForEndOfFrame();
		}
		stashitemslidecount--;
		audio.PlayOneShot(audioScrolling);
	}
	
	
	
	public GUIStyle myStyle;
	public GUIStyle buttonStyle;
	
	//private readonly Rect shopRect = new Rect(0.01f*Screen.width,0.273f*Screen.height,0.487f*Screen.width,0.421f*Screen.height);
	//private readonly Rect stashRect = new Rect(0.51f*Screen.width,0.273f*Screen.height,0.487f*Screen.width,0.421f*Screen.height);
	
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
	
	private void SetLayer(GameObject g,int n)
	{
		g.layer = n;
		Transform[] gg = g.GetComponentsInChildren<Transform>();
		foreach(Transform c in gg)
			c.gameObject.layer = n;
	}
	
	private void SetMaterial(GameObject g,Color col)
	{
		Transform[] gg = g.GetComponentsInChildren<Transform>();
		foreach(Transform c in gg)
			if( c.gameObject.renderer != null )
				c.gameObject.renderer.material.color = col;
	}
	
	private int wooi = -1;
	private bool fillin = false;
	private bool weapondescription = false;
	
	int FirstWeapon(bool unlocked)
	{
		for(int i=0;i<countWeapons;i++)
			if( WeaponUnlocked(i) == unlocked ) 
				return i;
		return -1;
	}
	int NextWeapon(int current,bool unlocked)
	{
		if(current==-1) return -1;
		for(int i=current+1;i<countWeapons;i++)
			if( WeaponUnlocked(i) == unlocked ) 
				return i;	
		for(int i=current;i>=0;i--)
			if( WeaponUnlocked(i) == unlocked ) 
				return i;
		return -1;
	}
	int PrevWeapon(int current,bool unlocked)
	{
		if(current==-1) return -1;
		for(int i=current-1;i>=0;i--)
			if( WeaponUnlocked(i) == unlocked ) 
				return i;
		for(int i=current;i<countWeapons;i++)
			if( WeaponUnlocked(i) == unlocked ) 
				return i;	
		return -1;
	}
	
	public Texture2D[] screen;
	public string[] tip;
	public GUITexture guiFullscreen;
	public GUIText guiTip;
	
	private IEnumerator GoMainMenuThread()
	{
		audio.PlayOneShot(clipQuitPlayGame);
		DisableStoreButtons();
		Fade.InProcess = true;
		LevelInfo.Environments.fade.Show(3f);
		while( !LevelInfo.Environments.fade.Finished )
		{
			yield return new WaitForEndOfFrame();
		}
		
		LevelInfo.Environments.control.guiPlayGame.SetActive(false);
		LevelInfo.Environments.control.allowShowGUI = false;
		showStore = false;
		LevelInfo.Audio.StopAll();
		Destroy(GameObject.Find("Audio Wind Loop"));
		LoadingGUI.SetActive(true);
		guiFullscreen.texture = screen[Random.Range(0,screen.Length)];
		guiTip.text = tip[Random.Range(0,tip.Length)];
		LevelInfo.Environments.fade.Disable();
		yield return new WaitForEndOfFrame();
		EnableStoreButtons();
		Fade.InProcess = false;
		
		GameObject[] z;
		z = GameObject.FindGameObjectsWithTag("Zombie");
		foreach(var g in z) Destroy(g);	
		z = GameObject.FindGameObjectsWithTag("ZombieRagdoll");
		foreach(var g in z) Destroy(g);	
		
		int lev = LevelInfo.Environments.control.currentLevel;
		for(int i=0;i<LevelInfo.Environments.generator.levelZombies[lev].zombie.Length;i++)
			for(int j=1;j<=LevelInfo.Environments.generator.levelZombies[lev].zombie[i].count;j++)
				LevelInfo.Environments.generator.levelZombies[lev].zombie[i].obj = null;
		for(int i=0;i<LevelInfo.Environments.generator.levelZombies[lev].scoobyZombies.Length;i++)
			LevelInfo.Environments.generator.levelZombies[lev].scoobyZombies[i].obj = null;
		Destroy(LevelInfo.Environments.generator.objLevel);
		yield return null;
		
		System.GC.Collect();
		AsyncOperation unload = Resources.UnloadUnusedAssets();
		while(unload.isDone) yield return null;
		
		exitVerified = true;		
		
	}
	
	public void GoMainMenuFromGamePlay()
	{
		/*if(!IsLevelGamePlay) Debug.LogError("Error at Store->GoMainMenuFromGamePlay()");
		LevelInfo.Environments.control.guiPlayGame.SetActive(false);
		LevelInfo.Environments.control.allowShowGUI = false;
		showStore = false;
		LevelInfo.Audio.StopAll();
		audio.PlayOneShot(clipQuitPlayGame);
		Destroy(GameObject.Find("Audio Wind Loop"));
		LoadingGUI.SetActive(true);
		guiFullscreen.texture = screen[Random.Range(0,screen.Length)];
		guiTip.text = tip[Random.Range(0,tip.Length)];
		LevelInfo.Environments.fade.Disable();
		Application.LoadLevel("mainmenu");		*/
		StartCoroutine(GoMainMenuThread());
		
	}
	
	void OnGUI()
	{
		if(Fade.InProcess) return;
		
		if(!_showStore) return;
		
		if( wantToExit ) // Only game play event
		{
			GUI.DrawTexture(new Rect(0.2f*Screen.width,0.15f*Screen.height,0.6f*Screen.width,0.6f*Screen.height),popupTexture);
			GUI.Label(new Rect(0.35f*Screen.width,0.305f*Screen.height,0.3f*Screen.width,0.2f*Screen.height),"Leave Game?",myStyle);
			//GUI.Box(new Rect(0.25f*Screen.width,0.25f*Screen.height,0.5f*Screen.width,0.5f*Screen.height),"Leave Game?");
			
			if(GUI.Button(new Rect(0.33f*Screen.width,0.5f*Screen.height,0.16f*Screen.width,0.1f*Screen.height), "Quit", buttonStyle ) )	
			{
				GoMainMenuFromGamePlay();
			}
			if( GUI.Button(new Rect(0.52f*Screen.width,0.5f*Screen.height,0.16f*Screen.width,0.1f*Screen.height), "Back", buttonStyle ) )
			{
				audio.PlayOneShot(clipBack);
				wantToExit = false;
			}	
			return;
		}

		
		// Shop
		/*Rect shopRect = new Rect(0.01f*Screen.width,0.273f*Screen.height,0.487f*Screen.width,0.421f*Screen.height);
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
		
		GUI.EndScrollView();*/
	
		
		// Stash
		/*Rect stashRect = new Rect(0.51f*Screen.width,0.273f*Screen.height,0.487f*Screen.width,0.421f*Screen.height);
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
		*/
		if( wooi != -1 )
		{
			if(fillin)
				ShowFillInDialog();
			else if(weapondescription)
				ShowWeaponDescription();
			else
				ShowWeaponBuyDialog();
		}
	}
	
	private void ShowWeaponBuyDialog()
	{
		GUI.DrawTexture(new Rect(0.2f*Screen.width,0.15f*Screen.height,0.6f*Screen.width,0.6f*Screen.height),popupTexture);
		if( Store.zombieHeads >= GameEnvironment.storeGun[wooi].price )
		{
			//GUI.Box(new Rect(0.25f*Screen.width,0.25f*Screen.height,0.5f*Screen.width,0.5f*Screen.height),"Do you want to buy this item?");
			
			GUI.Label(new Rect(0.35f*Screen.width,0.305f*Screen.height,0.3f*Screen.width,0.2f*Screen.height),"Do you want to buy this item?",myStyle);
			
			if(GUI.Button(new Rect(0.33f*Screen.width,0.5f*Screen.height,0.16f*Screen.width,0.1f*Screen.height), "BUY", buttonStyle ) )	
			{
				Store.UnlockWeapon(wooi);
				showWeapon[wooi] = true;
				currentStashitem = wooi;
				audio.Play();
				Store.zombieHeads -= GameEnvironment.storeGun[wooi].price;
				if( IsLevelGamePlay && LevelInfo.Environments.control.ExistGunInCurrentLevel((Weapon)wooi))
				{
					//GameEnvironment.storeGun[wooi].store += 5*GameEnvironment.storeGun[wooi].pocketsize;
					//GameEnvironment.storeGun[wooi].enabled = true;
					LevelInfo.Environments.guns.GetWeaponWithMAX((Weapon)wooi);
				}
				wooi = -1;
				spwchannel = true;
				currentshopitem = NextWeapon(currentshopitem,false);
			}
			if( GUI.Button(new Rect(0.52f*Screen.width,0.5f*Screen.height,0.16f*Screen.width,0.1f*Screen.height), "BACK", buttonStyle ) )
			{
				audio.PlayOneShot(clipBack);
				wooi = -1; 
				spwchannel = true;
			}
		}
		else
		{
			GUI.Label(new Rect(0.35f*Screen.width,0.305f*Screen.height,0.3f*Screen.width,0.3f*Screen.height),"You have not enough heads to buy this item.",myStyle);
			
			if(GUI.Button(new Rect(0.33f*Screen.width,0.5f*Screen.height,0.16f*Screen.width,0.1f*Screen.height), "MORE HEADS", buttonStyle ) )	
			{
				Get1000HeadsEvent();
			}
			
			if( GUI.Button(new Rect(0.52f*Screen.width,0.5f*Screen.height,0.16f*Screen.width,0.1f*Screen.height), "BACK", buttonStyle) )
			{
				audio.PlayOneShot(clipBack);
				wooi = -1;
				spwchannel = true;
			}
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
				//GameEnvironment.storeGun[wooi].store += 5*GameEnvironment.storeGun[wooi].pocketsize;
				LevelInfo.Environments.guns.GetWeaponWithMAX((Weapon)wooi);
				
				wooi = -1;
				fillin = false;
				spwchannel = true;
			}
			if( GUI.Button(new Rect(0.35f*Screen.width,0.6f*Screen.height,0.3f*Screen.width,0.1f*Screen.height), "Back" ) )
			{
				audio.PlayOneShot(clipBack);
				wooi = -1;
				fillin = false;
				spwchannel = true;
			}
		}
		else
		{
			GUI.Box(new Rect(0.25f*Screen.width,0.25f*Screen.height,0.5f*Screen.width,0.5f*Screen.height),"You have not enough heads.");
			if( GUI.Button(new Rect(0.35f*Screen.width,0.5f*Screen.height,0.3f*Screen.width,0.1f*Screen.height), "Back") )
			{
				audio.PlayOneShot(clipBack);
				wooi = -1;
				fillin = false;
				spwchannel = true;
			}
		}
		

	}
	
	private void ShowWeaponDescription()
	{
		GUI.DrawTexture(new Rect(0.2f*Screen.width,0.15f*Screen.height,0.6f*Screen.width,0.6f*Screen.height),popupTexture);

		GUI.Label(new Rect(0.4f*Screen.width,0.28f*Screen.height,0.3f*Screen.width,0.2f*Screen.height),"Weapon Description",myStyle);
		
		string s = 	
			"Power:" +
			"\nAccuracy:" + 
		    "\nClip Size:" +
			"\nMax Ammo:" +
			"\nSpeed:" +
			"\nReload Delay:";
		
		if( !WeaponUnlocked(wooi) )
			s += "\nPrice:";
		
		GUI.Label(new Rect(0.35f*Screen.width,0.34f*Screen.height,0.3f*Screen.width,0.25f*Screen.height),s,myStyle);
		
		s = 	
			"" + GameEnvironment.storeGun[wooi].damage +
			"\n" + GameEnvironment.storeGun[wooi].accuracy + "%" +
			"\n" + GameEnvironment.storeGun[wooi].pocketsize +
			"\n" + (5*GameEnvironment.storeGun[wooi].pocketsize) + 
			"\n" + GameEnvironment.storeGun[wooi].speed + " m/s" +
			"\n" + GameEnvironment.storeGun[wooi].reloadTime + " sec";
		
		if( !WeaponUnlocked(wooi) ) 
			s += "\n" + GameEnvironment.storeGun[wooi].price;
		
		GUI.Label(new Rect(0.5f*Screen.width,0.34f*Screen.height,0.3f*Screen.width,0.25f*Screen.height),s,myStyle);
		
		if( GUI.Button(new Rect(0.58f*Screen.width,0.5f*Screen.height,0.1f*Screen.width,0.1f*Screen.height), "OK", buttonStyle ) )
		{
			audio.Stop();
			audio.PlayOneShot(clipBack);
			wooi = -1; 
			weapondescription = false;
			spwchannel = true;
		}
		/*else
		{
			GUI.Label(new Rect(0.35f*Screen.width,0.305f*Screen.height,0.3f*Screen.width,0.3f*Screen.height),"You have not enough heads to buy this item.",myStyle);
			if( GUI.Button(new Rect(0.42f*Screen.width,0.5f*Screen.height,0.16f*Screen.width,0.1f*Screen.height), "Back") )
			{
				wooi = -1;
				spwchannel = true;
			}
		}*/
	}
	
	private bool RectContainPoint(Rect rect,Vector2 pos)
	{
		return rect.xMin <= pos.x && pos.x <= rect.xMax && rect.yMin <= pos.y && pos.y <= rect.yMax;
	}
	
	#endregion
}
