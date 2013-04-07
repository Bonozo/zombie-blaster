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
	BB,
	Revolver,
	PulseShotGun,
	Flamethrower,
	Football,
	MachineGun,
	Grenade,
	Crossbow,
	Rocket,
	AlienBlaster,
	Spade,
	None
};

public class Store : MonoBehaviour {
	
	#region Environments
	
	public static int countLevel = 5;
	public static int countWeapons = 11;
	
	public static Weapon[][] weaponsForLevel = new Weapon[][]
	{
		new Weapon[] {Weapon.Flamethrower,Weapon.Revolver,Weapon.AlienBlaster},
		new Weapon[] {Weapon.Grenade,Weapon.Spade},
		new Weapon[] {Weapon.Football,Weapon.MachineGun},
		new Weapon[] {Weapon.Rocket},
		new Weapon[] {Weapon.PulseShotGun,Weapon.Crossbow}
	};
	
	#endregion
	
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
		
		// first time play
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
	
	public static void ClearGameStats()
	{
		// Clear Prefabs
		//PlayerPrefs.SetInt("zombieHeads",0);
		for(int i=1;i<_playerprefs_unlockweapon.Length;i++)
			PlayerPrefs.SetInt("weapon"+i,0);
		for(int i=0;i<_playerprefs_unlocklevel.Length;i++)
			PlayerPrefs.SetInt("level"+i,0);
		for(int i=0;i<_playerprefs_highestwavecompleted.Length;i++)
			PlayerPrefs.SetInt("highestwavecompleted"+i,0);
		PlayerPrefs.SetInt("zbfirsttimeplay",0);	
		Store instance = GameObject.Find("Store").GetComponent<Store>();
		instance.RestorePlayerPrefs();
		instance.currentshopitem = instance.currentStashitem = -2;
		
	}
	
	void Awake()
	{
		DontDestroyOnLoad(this.gameObject);
		RestorePlayerPrefs();
		Application.targetFrameRate = 60;
	}
	
	#endregion
	
	#region Google, Tapjoy, Amazon

	//--------------- store purchase code ------------------//
	
	private string androidKey = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAnTAvl2vK4KXRCx7UnvmLgVxZCsRUUivSb6iIvHP2aVpNPK3BBJvOK1uxYumXNiSr5OPJBxGQ+Tx4nmNpx9sIjZQy06BUUtvYDOgSuakMCdF1LfWZXQ5II7L5IzZnYenTc+aWTZaSTJy4z1e/oifsz0BltZ3b31DhZBb1wYjZeynd3mJA4DZs2bGpCvGRrf1QeF9oLtBvyq1NAQ2+R2zSgQPPzEarn/9Lqq/oHqx7wCrTxiX8/I7ButnJpWHoCkDIszC4SncTqMmZlCtl2hlfRzvEWgMozAlZJYMU9IC9hVJKbkYpf3vvJ8QPyE4RTPoDzGRoKeI5lHB8bU0PLaUcjwIDAQAB";
	public static bool tapjoyConnected = false;
	
	void OnEnable()
	{
		#if UNITY_ANDROID
		IABAndroidManager.billingSupportedEvent += HandleIABAndroidManagerbillingSupportedEvent;
		IABAndroidManager.purchaseSucceededEvent += HandleIABAndroidManagerpurchaseSucceededEvent;
		IABAndroidManager.purchaseCancelledEvent += HandleIABAndroidManagerpurchaseCancelledEvent;
		IABAndroidManager.purchaseFailedEvent += HandleIABAndroidManagerpurchaseFailedEvent;
		
		AmazonIAPManager.onSdkAvailableEvent += HandleAmazonIAPManageronSdkAvailableEvent;
		AmazonIAPManager.itemDataRequestFinishedEvent += HandleAmazonIAPManageritemDataRequestFinishedEvent;
		AmazonIAPManager.itemDataRequestFailedEvent += HandleAmazonIAPManageritemDataRequestFailedEvent;
		AmazonIAPManager.purchaseSuccessfulEvent += HandleAmazonIAPManagerpurchaseSuccessfulEvent;
		AmazonIAPManager.purchaseFailedEvent += HandleAmazonIAPManagerpurchaseFailedEvent;
		
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
		IABAndroidManager.billingSupportedEvent -= HandleIABAndroidManagerbillingSupportedEvent;
		IABAndroidManager.purchaseSucceededEvent -= HandleIABAndroidManagerpurchaseSucceededEvent;
		IABAndroidManager.purchaseCancelledEvent -= HandleIABAndroidManagerpurchaseCancelledEvent;
		IABAndroidManager.purchaseFailedEvent -= HandleIABAndroidManagerpurchaseFailedEvent;
		
		AmazonIAPManager.onSdkAvailableEvent -= HandleAmazonIAPManageronSdkAvailableEvent;
		AmazonIAPManager.itemDataRequestFinishedEvent -= HandleAmazonIAPManageritemDataRequestFinishedEvent;
		AmazonIAPManager.itemDataRequestFailedEvent -= HandleAmazonIAPManageritemDataRequestFailedEvent;
		AmazonIAPManager.purchaseSuccessfulEvent -= HandleAmazonIAPManagerpurchaseSuccessfulEvent;
		AmazonIAPManager.purchaseFailedEvent -= HandleAmazonIAPManagerpurchaseFailedEvent;
		
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
	
	#if UNITY_ANDROID
	
	[System.NonSerializedAttribute]
	public bool canPay = false;
	
	//=================Google Play Store============================
	void HandleIABAndroidManagerbillingSupportedEvent (bool obj)
	{
		Debug.Log("Android Billing Supported: " + obj);
		canPay = obj;
	}
	
	void HandleIABAndroidManagerpurchaseSucceededEvent (string obj)
	{
		Debug.Log( "purchased product: " + obj );
		Store.zombieHeads = Store.zombieHeads + 1000;
	}
	
	void HandleIABAndroidManagerpurchaseFailedEvent (string obj)
	{
		Debug.Log( "purchase failed with error: " + obj );
	}

	void HandleIABAndroidManagerpurchaseCancelledEvent (string obj)
	{
		Debug.Log( "purchase cancelled with error: " + obj );
	}
	//===============================================================
	
	//===================Amazon App Store============================
	void HandleAmazonIAPManageronSdkAvailableEvent (bool obj)
	{
		Debug.Log( "Amazon onSdkAvailableEvent. isTestMode: " + obj );
	}

	void HandleAmazonIAPManagerpurchaseFailedEvent (string obj)
	{
		Debug.Log( "Amazon purchaseFailedEvent: " + obj );
	}

	void HandleAmazonIAPManagerpurchaseSuccessfulEvent (AmazonReceipt obj)
	{
		Debug.Log( "purchaseSuccessfulEvent: " + obj );
		Store.zombieHeads = Store.zombieHeads + 1000;
	}
	
	void HandleAmazonIAPManageritemDataRequestFailedEvent()
	{
		Debug.Log( "Amazon dataRequestFailed" );
	}
	
	void HandleAmazonIAPManageritemDataRequestFinishedEvent (List<string> arg1, List<AmazonItem> arg2)
	{
		Debug.Log( "itemDataRequestFinishedEvent. unavailable skus: " + arg1.Count + ", avaiable items: " + arg2.Count );
	}
	//===============================================================
	
	#endif
	
	#if UNITY_IPHONE
	void zombieStoreKitPurchaseSuccessful( StoreKitTransaction transaction )
	{
		Debug.Log( "purchased product: " + transaction );
		Store.zombieHeads = Store.zombieHeads + 1000;
	}
	
	void zombieStoreKitPurchaseFailed( string error )
	{
		Debug.Log( "purchase failed with error: " + error );
	}
	
	void zombieStoreKitPurchaseCancelled( string error )
	{
		Debug.Log( "purchase cancelled with error: " + error );
	}
	
	#endif

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
		IABAndroid.init( androidKey );
		
		//TapjoyAndroid.init( "6f8b509b-f292-4dd3-b440-eab33f211089", "7TYeZbZ6GTqRncoALV3W", false );//old
		TapjoyAndroid.init( "b1f6ad92-1ff9-47ca-a962-a4b7ecddebd2", "wNZUPjewwCeVRkgpCCZQ", false );//new
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
		
		// Clear Prefabs
		//PlayerPrefs.SetInt("zombieHeads",100000);
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
	
	public void Get1000HeadsEvent()
	{
		#if UNITY_ANDROID
		
		if(GlobalPersistentParameters.AmazonBuild)
		{
			AmazonIAP.initiateItemDataRequest( new string[] { "1000heads" } );
			AmazonIAP.initiatePurchaseRequest( "1000heads" );
		}
		else
		{
			if( !canPay )
			{
				Debug.Log("Can't pay. Initializing!");
				IABAndroid.init(androidKey );
				IABAndroid.startCheckBillingAvailableRequest();
			}
			else
			{
				IABAndroid.purchaseProduct("1000heads");
			}
		}
		#endif
		
		#if UNITY_IPHONE
		if(_products.Count > 0)
			StoreKitBinding.purchaseProduct( "ZH1000", 1 );
		#endif
			
		#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
		Store.zombieHeads = Store.zombieHeads + 1000;
		#endif
	}
	
	
	#endregion
	
	#region GUI
	
	public AudioClip audioScrolling;
	public AudioClip clipBuy;
	public AudioClip clipBack;
	public AudioClip clipShowStore;
	public AudioClip clipQuitPlayGame;
	
	public GameObject[] objectWeapons;
	public Texture2D[] weaponIcon;
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
	
	//public ButtonBase arrowUpShowButton;
	//public ButtonBase arrowDownShowButton;
	//public ButtonBase arrowUpStashButton;
	//public ButtonBase arrowDownStashButton;
	
	private bool spwchannel = false;
	
	private int showZombieHeads = -1; 
	private bool[] showWeapon = new bool[countWeapons];
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
				buttonGAME.gameObject.SetActive(true);
				
				if( IsLevelOption )
				{
					MainMenu mainmenu = (MainMenu)GameObject.FindObjectOfType(typeof(MainMenu));
					if(!mainmenu.mapToStore)
						buttonGAME.gameObject.SetActive(false);
				}
				showZombieHeads = zombieHeads;
				
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
			if( IsLevelGamePlay ) LevelInfo.Environments.control.state = ButtonStore.lastState;
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
		
		UpdateItems();
	}
	
	void UpdateItems()
	{
		if( currentshopitem == -2 )
			currentshopitem = FirstWeapon(false);
	
		if( currentStashitem == -2 )
			currentStashitem = FirstWeapon(true);
		
		bool enableshowshopitems = currentshopitem != -1 && shopitemslidecount==0;
		
		ShopWeaponName.enabled = enableshowshopitems;
		ShopWeaponBuyText.enabled = enableshowshopitems&&showWeapon[currentshopitem];
		shopInfoButton.gameObject.SetActive(enableshowshopitems);
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
		for(int i=1;i<countWeapons;i++)
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

			if( (RectContainPoint(shopRect,GameEnvironment.AbsoluteSwipeBegin) && RectContainPoint(shopRect,GameEnvironment.AbsoluteSwipeEnd) && swp.y > 0 )
				/*|| (arrowUpShow.enabled && arrowUpShowButton.PressedUp)*/ )
			{
				int olditem = currentshopitem;
				currentshopitem = NextWeapon(currentshopitem,false);
				if( olditem != currentshopitem )
				{
					GameObject newWeaponobj = objectWeapons[currentshopitem];
					StartCoroutine(ShopItemsSlide(currentWeaponobj,0f,1f));
					StartCoroutine(ShopItemsSlide(newWeaponobj,-1f,0f));
				}
			}
			if( (RectContainPoint(shopRect,GameEnvironment.AbsoluteSwipeBegin) && RectContainPoint(shopRect,GameEnvironment.AbsoluteSwipeEnd) && swp.y < 0 )
				/*|| (arrowDownShow.enabled && arrowDownShowButton.PressedUp)*/ )
			{
				int olditem = currentshopitem;
				currentshopitem = PrevWeapon(currentshopitem,false);
				if( olditem != currentshopitem )
				{
					GameObject newWeaponobj = objectWeapons[currentshopitem];
					StartCoroutine(ShopItemsSlide(currentWeaponobj,0f,-1f));
					StartCoroutine(ShopItemsSlide(newWeaponobj,1f,0f));
				}
			}
		}
		
		bool enableshowstashitems = currentStashitem != -1 && stashitemslidecount==0;
		
		StashWeaponName.enabled = enableshowstashitems;
		StashWeaponBuyText.enabled = enableshowstashitems&&IsLevelGamePlay&&showWeapon[currentStashitem]&&currentStashitem!=(int)Weapon.Spade;
		StashInfoButton.gameObject.SetActive(enableshowstashitems);
		StashItemBuy.gameObject.SetActive(enableshowstashitems&&IsLevelGamePlay&&showWeapon[currentStashitem]&&currentStashitem != (int)Weapon.Spade);
		StashItemBuy.enabled = enableshowstashitems&&wooi==-1&&IsLevelGamePlay&&showWeapon[currentStashitem]&&currentStashitem != (int)Weapon.Spade;
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
		
			if( (RectContainPoint(StashRect,GameEnvironment.AbsoluteSwipeBegin) && RectContainPoint(StashRect,GameEnvironment.AbsoluteSwipeEnd) && swp.y > 0 )
				/*|| (arrowUpStash.enabled && arrowUpStashButton.PressedUp)*/ )
			{
				int olditem = currentStashitem;
				currentStashitem = NextWeapon(currentStashitem,true);
				if( olditem != currentStashitem )
				{
					StartCoroutine(StashItemsSlide(objectWeapons[olditem],0f,1f));
					StartCoroutine(StashItemsSlide(objectWeapons[currentStashitem],-1f,0f));
				}
			}
			if( (RectContainPoint(StashRect,GameEnvironment.AbsoluteSwipeBegin) && RectContainPoint(StashRect,GameEnvironment.AbsoluteSwipeEnd) && swp.y < 0 )
				/*|| (arrowDownStash.enabled && arrowDownStashButton.PressedUp)*/ )	
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
	
	private bool _showLoadingScreen = false;
	public bool showLoadingScreen
	{
		get
		{
			return _showLoadingScreen;
		}
		set
		{
			_showLoadingScreen = value;
			if(_showLoadingScreen)
			{
				int index = Random.Range(0,screen.Length);
				guiFullscreen.texture = screen[index];
	
				if(index==1&&Random.Range(0,tip.Length+1)==0)// special code
				{
					guiTip.text = "FUN FACT: MOST JERSEY NUMBERS ARE YEARS OF ROMERO MOVIES";
				}
				else
					guiTip.text = tip[Random.Range(0,tip.Length)];
			}
			LoadingGUI.SetActive(_showLoadingScreen);
		}
	}
	
	private IEnumerator GoMainMenuThread()
	{
		audio.PlayOneShot(clipQuitPlayGame);
		DisableStoreButtons();
		Fade.InProcess = true;
		
		if(GameEnvironment.ToMap)
			LevelInfo.Environments.fade.Show(3f);
		else
			LevelInfo.Environments.fade.Show(1.1f);
		
		while( !LevelInfo.Environments.fade.Finished )
		{
			yield return new WaitForEndOfFrame();
		}
		
		LevelInfo.Environments.control.guiPlayGame.SetActive(false);
		LevelInfo.Environments.control.allowShowGUI = false;
		showStore = false;
		LevelInfo.Audio.StopAll();
		Destroy(GameObject.Find("Audio Wind Loop"));
		showLoadingScreen = true;
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
		StartCoroutine(GoMainMenuThread());
		
	}
	
	void OnGUI()
	{
		GUI.matrix = GameEnvironment.GetGameGUIMatrix();
		
		if(Fade.InProcess) return;
		
		if(!_showStore) return;
		
		if( wantToExit ) // Only game play event
		{
			GUI.DrawTexture(new Rect(0.2f*GameEnvironment.GUIWidth,0.15f*GameEnvironment.GUIHeight,0.6f*GameEnvironment.GUIWidth,0.6f*GameEnvironment.GUIHeight),popupTexture);
			GUI.Label(new Rect(0.35f*GameEnvironment.GUIWidth,0.305f*GameEnvironment.GUIHeight,0.3f*GameEnvironment.GUIWidth,0.2f*GameEnvironment.GUIHeight),"Leave Game?",myStyle);

			if(GUI.Button(new Rect(0.33f*GameEnvironment.GUIWidth,0.5f*GameEnvironment.GUIHeight,0.16f*GameEnvironment.GUIWidth,0.1f*GameEnvironment.GUIHeight), "Quit", buttonStyle ) )	
			{
				GoMainMenuFromGamePlay();
			}
			if( GUI.Button(new Rect(0.52f*GameEnvironment.GUIWidth,0.5f*GameEnvironment.GUIHeight,0.16f*GameEnvironment.GUIWidth,0.1f*GameEnvironment.GUIHeight), "Back", buttonStyle ) )
			{
				audio.PlayOneShot(clipBack);
				wantToExit = false;
			}	
			return;
		}

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
		GUI.DrawTexture(new Rect(0.2f*GameEnvironment.GUIWidth,0.15f*GameEnvironment.GUIHeight,0.6f*GameEnvironment.GUIWidth,0.6f*GameEnvironment.GUIHeight),popupTexture);
		if( Store.zombieHeads >= GameEnvironment.storeGun[wooi].price )
		{
		
			GUI.Label(new Rect(0.35f*GameEnvironment.GUIWidth,0.305f*GameEnvironment.GUIHeight,0.3f*GameEnvironment.GUIWidth,0.2f*GameEnvironment.GUIHeight),"Do you want to buy this item?",myStyle);
			
			if(GUI.Button(new Rect(0.33f*GameEnvironment.GUIWidth,0.5f*GameEnvironment.GUIHeight,0.16f*GameEnvironment.GUIWidth,0.1f*GameEnvironment.GUIHeight), "BUY", buttonStyle ) )	
			{
				Store.UnlockWeapon(wooi);
				showWeapon[wooi] = true;
				currentStashitem = wooi;
				audio.Play();
				Store.zombieHeads -= GameEnvironment.storeGun[wooi].price;
				if( IsLevelGamePlay && wooi != (int)Weapon.Spade)
				{
					LevelInfo.Environments.guns.GetWeaponWithMAX((Weapon)wooi);
				}
				wooi = -1;
				spwchannel = true;
				currentshopitem = NextWeapon(currentshopitem,false);
			}
			if( GUI.Button(new Rect(0.52f*GameEnvironment.GUIWidth,0.5f*GameEnvironment.GUIHeight,0.16f*GameEnvironment.GUIWidth,0.1f*GameEnvironment.GUIHeight), "BACK", buttonStyle ) )
			{
				audio.PlayOneShot(clipBack);
				wooi = -1; 
				spwchannel = true;
			}
		}
		else
		{
			GUI.Label(new Rect(0.35f*GameEnvironment.GUIWidth,0.305f*GameEnvironment.GUIHeight,0.3f*GameEnvironment.GUIWidth,0.3f*GameEnvironment.GUIHeight),"You have not enough heads to buy this item.",myStyle);
			
			if(GUI.Button(new Rect(0.33f*GameEnvironment.GUIWidth,0.5f*GameEnvironment.GUIHeight,0.16f*GameEnvironment.GUIWidth,0.1f*GameEnvironment.GUIHeight), "MORE HEADS", buttonStyle ) )	
			{
				Get1000HeadsEvent();
			}
			
			if( GUI.Button(new Rect(0.52f*GameEnvironment.GUIWidth,0.5f*GameEnvironment.GUIHeight,0.16f*GameEnvironment.GUIWidth,0.1f*GameEnvironment.GUIHeight), "BACK", buttonStyle) )
			{
				audio.PlayOneShot(clipBack);
				wooi = -1;
				spwchannel = true;
			}
		}
		

	}
	
	private void ShowFillInDialog()
	{
		int allammo = GameEnvironment.storeGun[wooi].maxammo+GameEnvironment.storeGun[wooi].pocketsize;
		int currentammo = GameEnvironment.storeGun[wooi].current+GameEnvironment.storeGun[wooi].store;
		float pc = 1f-(float)currentammo/(float)allammo;
		int price = Mathf.CeilToInt(100f*pc);
		
		GUI.DrawTexture(new Rect(0.2f*GameEnvironment.GUIWidth,0.15f*GameEnvironment.GUIHeight,0.6f*GameEnvironment.GUIWidth,0.6f*GameEnvironment.GUIHeight),popupTexture);
	
		if( GameEnvironment.storeGun[wooi].current == GameEnvironment.storeGun[wooi].pocketsize && GameEnvironment.storeGun[wooi].store == GameEnvironment.storeGun[wooi].maxammo )
		{
			GUI.Label(new Rect(0.35f*GameEnvironment.GUIWidth,0.305f*GameEnvironment.GUIHeight,0.3f*GameEnvironment.GUIWidth,0.2f*GameEnvironment.GUIHeight),"Your \"" + GameEnvironment.storeGun[wooi].name + " \" is already fully loaded!",myStyle);
			if( GUI.Button(new Rect(0.42f*GameEnvironment.GUIWidth,0.5f*GameEnvironment.GUIHeight,0.16f*GameEnvironment.GUIWidth,0.1f*GameEnvironment.GUIHeight), "BACK", buttonStyle) )
			{
				audio.PlayOneShot(clipBack);
				wooi = -1;
				fillin = false;
				spwchannel = true;
			}		
		}
		else if( Store.zombieHeads >= price )
		{
			GUI.Label(new Rect(0.35f*GameEnvironment.GUIWidth,0.305f*GameEnvironment.GUIHeight,0.3f*GameEnvironment.GUIWidth,0.2f*GameEnvironment.GUIHeight),"Refill the \"" + GameEnvironment.storeGun[wooi].name + "\" ammo for " + price + " Heads?",myStyle);
			if(GUI.Button(new Rect(0.33f*GameEnvironment.GUIWidth,0.5f*GameEnvironment.GUIHeight,0.16f*GameEnvironment.GUIWidth,0.1f*GameEnvironment.GUIHeight), "REFILL", buttonStyle ) )	
			{
				audio.Play();
				Store.zombieHeads -= price;
				LevelInfo.Environments.guns.GetWeaponWithMAX((Weapon)wooi);
				wooi = -1;
				fillin = false;
				spwchannel = true;			
			}
			if( GUI.Button(new Rect(0.52f*GameEnvironment.GUIWidth,0.5f*GameEnvironment.GUIHeight,0.16f*GameEnvironment.GUIWidth,0.1f*GameEnvironment.GUIHeight), "BACK", buttonStyle ) )
			{
				audio.PlayOneShot(clipBack);
				wooi = -1;
				fillin = false;
				spwchannel = true;			
			}
		}
		else
		{
			GUI.Label(new Rect(0.35f*GameEnvironment.GUIWidth,0.305f*GameEnvironment.GUIHeight,0.3f*GameEnvironment.GUIWidth,0.3f*GameEnvironment.GUIHeight),"You do not have enough heads.",myStyle);
			
			if(GUI.Button(new Rect(0.33f*GameEnvironment.GUIWidth,0.5f*GameEnvironment.GUIHeight,0.16f*GameEnvironment.GUIWidth,0.1f*GameEnvironment.GUIHeight), "MORE HEADS", buttonStyle ) )	
			{
				Get1000HeadsEvent();
			}
			
			if( GUI.Button(new Rect(0.52f*GameEnvironment.GUIWidth,0.5f*GameEnvironment.GUIHeight,0.16f*GameEnvironment.GUIWidth,0.1f*GameEnvironment.GUIHeight), "BACK", buttonStyle) )
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
		GUI.DrawTexture(new Rect(0.1f*GameEnvironment.GUIWidth,0.1f*GameEnvironment.GUIHeight,0.8f*GameEnvironment.GUIWidth,0.8f*GameEnvironment.GUIHeight),popupTexture);
		
		if(showWeapon[wooi]||wooi==0)
		{
			GUI.Label(new Rect(0.4f*GameEnvironment.GUIWidth,0.25f*GameEnvironment.GUIHeight,0.3f*GameEnvironment.GUIWidth,0.2f*GameEnvironment.GUIHeight),GameEnvironment.storeGun[wooi].name,myStyle);
			
			GUI.Label(new Rect(0.25f*GameEnvironment.GUIWidth,0.32f*GameEnvironment.GUIHeight,0.5f*GameEnvironment.GUIWidth,0.25f*GameEnvironment.GUIHeight),
				GameEnvironment.storeGun[wooi].description,myStyle);
			
			if(wooi != (int)Weapon.Spade)
			{
				string s = 
					"Power:" +
					"\nAccuracy:" + 
				    "\nClip Size:" +
					"\nMax Ammo:" +
					"\nSpeed:" +
					"\nReload Delay:";
				
				if( !WeaponUnlocked(wooi) )
					s += "\nPrice:";
				
				GUI.Label(new Rect(0.3f*GameEnvironment.GUIWidth,0.44f*GameEnvironment.GUIHeight,0.3f*GameEnvironment.GUIWidth,0.25f*GameEnvironment.GUIHeight),s,myStyle);
				
				s = 	
					"" + GameEnvironment.storeGun[wooi].damage +
					"\n" + GameEnvironment.storeGun[wooi].accuracy + "%" +
					"\n" + GameEnvironment.storeGun[wooi].pocketsize +
					"\n" + (GameEnvironment.storeGun[wooi].maxammo) + 
					"\n" + GameEnvironment.storeGun[wooi].speed + " m/s" +
					"\n" + GameEnvironment.storeGun[wooi].reloadTime + " sec";
				
				if( !WeaponUnlocked(wooi) ) 
					s += "\n" + GameEnvironment.storeGun[wooi].price;
				
				GUI.Label(new Rect(0.45f*GameEnvironment.GUIWidth,0.44f*GameEnvironment.GUIHeight,0.3f*GameEnvironment.GUIWidth,0.25f*GameEnvironment.GUIHeight),s,myStyle);
			}
		}
		else
		{
			int levestounlock=0;
			for(int i=0;i<countLevel;i++)
				for(int j=0;j<weaponsForLevel[i].Length;j++)
					if( (int)weaponsForLevel[i][j] == wooi )
						levestounlock = i;
			
			string message;
			if( wooi == (int)Weapon.AlienBlaster)
				message = "UNLOCK ALL AREAS ON THE MAP TO GET THE \"Alien Blaster\"";
			else
				message = "UNLOCK \"" + GameEnvironment.levelName[levestounlock] + "\" ON THE MAP TO GET THE \"" 
				+ GameEnvironment.storeGun[wooi].name + "\"";
			
			GUI.Label(new Rect(0.25f*GameEnvironment.GUIWidth,0.32f*GameEnvironment.GUIHeight,0.5f*GameEnvironment.GUIWidth,0.25f*GameEnvironment.GUIHeight),
				message,myStyle);
		}
		
		if( GUI.Button(new Rect(0.58f*GameEnvironment.GUIWidth,0.585f*GameEnvironment.GUIHeight,0.15f*GameEnvironment.GUIWidth,0.1f*GameEnvironment.GUIHeight), "OK", buttonStyle ) )
		{
			audio.Stop();
			audio.PlayOneShot(clipBack);
			wooi = -1; 
			weapondescription = false;
			spwchannel = true;
		}
	}
	
	
	
	private bool RectContainPoint(Rect rect,Vector2 pos)
	{
		return rect.xMin <= pos.x && pos.x <= rect.xMax && rect.yMin <= pos.y && pos.y <= rect.yMax;
	}
	
	#endregion
	
	#region safe
	// Multithreaded Safe Singleton Pattern
    // URL: http://msdn.microsoft.com/en-us/library/ms998558.aspx
    private static readonly object _syncRoot = new Object();
    private static volatile Store _staticInstance;	
    public static Store Instance 
	{
        get {
            if (_staticInstance == null) {				
                lock (_syncRoot) {
                    _staticInstance = FindObjectOfType (typeof(Store)) as Store;
                    if (_staticInstance == null) {
                       Debug.LogError("The Store instance was unable to be found, if this error persists please contact support.");						
                    }
                }
            }
            return _staticInstance;
        }
    }
	
	#endregion
}
