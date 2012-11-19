using UnityEngine;
using System.Collections;

public class Store : MonoBehaviour {
	
	#region Game Store
	
	public Texture2D textureBackground;
	public Texture2D textureScreenShot;
	public Texture2D textureZombieHead;
	
	private readonly float updownbuttonheight = 0.1f*Screen.height;
	private readonly float armorydown = 0.25f*Screen.height;
	
	private static Vector2 scrollposition = Vector2.zero;
	private static int wooi = -1;
	
	private static int cost = 10;
	
	public bool isPlayGame = false;
	public bool showStore = false;

	void Awake()
	{
		DontDestroyOnLoad(this.gameObject);
	}

	
	public void DrawStore()
	{	
		// Background
		GUI.DrawTexture(new Rect(0f,0f,Screen.width,Screen.height),textureBackground);
		
		// Current Zombie Head Amount
		GUI.DrawTexture(new Rect(0.125f*Screen.width,0.005f*Screen.height,Screen.width*0.09f,Screen.height*0.09f),textureZombieHead);
		GUI.Box(new Rect(0,0,0.5f*Screen.width,updownbuttonheight),"" + GameEnvironment.zombieHeads);
		
		// Get 1000 Head
		if( GUI.Button(new Rect(Screen.width*0.5f,0,Screen.width*0.5f,updownbuttonheight),"Get 1000 Heads") )
		{
			IABAndroid.purchaseProduct("android.test.purchased");
		}
		
		// Return to Game
		if( GUI.Button(new Rect(0,Screen.height-updownbuttonheight,Screen.width*0.5f,updownbuttonheight),"Return to Game") )
		{
			wooi = -1;
			showStore = false;
			GameEnvironment.IgnoreButtons();
			if( Application.loadedLevelName == "mainmenu" )
			{
				MainMenu mainmenu = (MainMenu)GameObject.FindObjectOfType(typeof(MainMenu));
				mainmenu.GoMainState();
			}
			else if( Application.loadedLevelName == "playgame" )
				LevelInfo.Environments.control.state = GameState.Play;
	
		}
		
		if( GUI.Button(new Rect(Screen.width*0.5f,Screen.height-updownbuttonheight,Screen.width*0.5f,updownbuttonheight),"Main Menu") )
		{
			Application.LoadLevel("mainmenu");

		}	
		if( wooi == -1 )
		{		
			// Screen Shot
			GUI.DrawTexture(new Rect(Screen.width*0.5f,updownbuttonheight,Screen.width*0.5f,Screen.height-2*updownbuttonheight),textureScreenShot);
		
			// Armory
			GUI.color = Color.white;
			GUI.Box(new Rect(0,updownbuttonheight,Screen.width*0.5f,armorydown-updownbuttonheight),"Armory");
		
			// Weapon List
			scrollposition = GUI.BeginScrollView(new Rect(0,armorydown,Screen.width*0.5f,Screen.height-armorydown-updownbuttonheight),
				scrollposition,new Rect(0,0,Screen.width*0.48f,GameEnvironment.storeGun.Length*updownbuttonheight),false,true);
		
			for(int i=0;i<GameEnvironment.storeGun.Length;i++)
			{
				
				if( GUI.Button(new Rect(0f,i*updownbuttonheight,Screen.width*0.24f,updownbuttonheight),GameEnvironment.storeGun[i].name ) )
					wooi = i;
				GUI.Box(new Rect(Screen.width*0.24f,i*updownbuttonheight,Screen.width*0.24f,updownbuttonheight),GameEnvironment.storeGun[i].AmmoInformation );
			}
		
			GUI.EndScrollView();
		}
		else
		{
			// Weapon Name
			GUI.Box(new Rect(0,updownbuttonheight,Screen.width*0.5f,updownbuttonheight),GameEnvironment.storeGun[wooi].name);
			
			// Cost of Ammo
			GUI.Box(new Rect(Screen.width*0.5f,updownbuttonheight,Screen.width*0.5f,updownbuttonheight),"Cost : " + cost);
			
			// Description
			GUI.Box(new Rect(0,2*updownbuttonheight,Screen.width*0.5f,Screen.height-3*updownbuttonheight),"Current Ammo" + GameEnvironment.storeGun[wooi].AmmoInformation);
		
			// Purchases
			if( GUI.Button(new Rect(Screen.width*0.5f,Screen.height-2*updownbuttonheight,Screen.width*0.25f,updownbuttonheight),"Purchases") ) 
			{
				if( GameEnvironment.zombieHeads >= cost )
				{
					audio.Play();
					GameEnvironment.zombieHeads -= cost;
					GameEnvironment.storeGun[wooi].store += GameEnvironment.storeGun[wooi].pocketsize;
					GameEnvironment.storeGun[wooi].enabled = true;
				}
			}
			if( GUI.Button(new Rect(Screen.width*0.75f,Screen.height-2*updownbuttonheight,Screen.width*0.25f,updownbuttonheight),"Back") ) 
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
		GameEnvironment.zombieHeads = GameEnvironment.zombieHeads + 1000;
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
