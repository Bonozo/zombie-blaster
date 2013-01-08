using UnityEngine;
using System.Collections;

public class Option : MonoBehaviour {
	
	public static float Health = 1f;
	public static float hSlideVolume = 1f;
	//public static bool WeaponsUnlocked = false;
	public static bool UnlimitedHealth = false;
	public static bool UnlimitedAmmo = false;
	public static bool Vibration = true;
	public static bool SpotLight = true;
	public static bool Fog = false;
	public static float SpotLightAngle = 65f;
	public static float BackgroundAmbient = 101f;
	public static bool ShowFPS = false;
	public static bool ShoveHelper = true;
	public static float Peturb = 1f;
	public static bool AutoTargeting = true;
	public static float Sensitivity = 0.01f;
	public static int FlameWaitingFrames=10;
	//public static int Quality = QualitySettings.GetQualityLevel();
	
	bool showdebug = true;
	bool debugScreen = false;
	public GUIText title;
	public GUIText version;
	public GUIStyle myStyle1;
	public GUIStyle myStyle2;
	
	// Update is called once per frame
	void Update () 
	{
		AudioListener.volume = hSlideVolume;
		version.enabled = debugScreen;
		if( GameEnvironment.AbsoluteSwipe.x < -0.5f )
			showdebug = true;
	}
	
	void OnEnable()
	{
		debugScreen = false;
		showdebug = false;
		Update();
	}
	
	private Vector2 textSize = new Vector2(Screen.width*0.2f,30);
	private Vector2 buttonSize = new Vector2(Screen.width*0.2f,30);

	private Rect textRect(float index)
	{
		index++; if(index>8) index++;
		float w = 0f;
		if( index > 8 ) { w = Screen.width*0.5f; index-=8; }
		return new Rect(w + Screen.width*0.05f,index*40f,textSize.x,textSize.y);
	}
	private Rect buttonRect(float index)
	{
		index++; if(index>8) index++;
		float w = 0f;
		if( index > 8 ) { w = Screen.width*0.5f; index-=8; }
		return new Rect(w+Screen.width*0.1f+textSize.x,index*40f,buttonSize.x,buttonSize.y);
	}
	
	void OnGUI()
	{
		if( debugScreen ){
			
		GUI.Label(textRect(1),"Unlimited Health",myStyle1);
		if( GUI.Button(buttonRect(1),UnlimitedHealth?"ON":"OFF" ,myStyle2) )
			UnlimitedHealth = !UnlimitedHealth;
		
		GUI.Label(textRect(2),"Tapjoy Connected",myStyle1);
		GUI.Box(buttonRect(2),"" + Store.tapjoyConnected,myStyle2 );
			
		GUI.Label(textRect(3),"Health",myStyle1);
		Health = GUI.HorizontalSlider(buttonRect(3),Health,0.05f,1f);

		GUI.Label(textRect(4),"Ambient (" + (int)BackgroundAmbient + ")",myStyle1);
		BackgroundAmbient = GUI.HorizontalSlider(buttonRect(4),BackgroundAmbient,0f,255f);
		
			
		GUI.Label(textRect(5),"Spot Light Angle (" + (int)SpotLightAngle + ")",myStyle1);
		SpotLightAngle = GUI.HorizontalSlider(buttonRect(5),SpotLightAngle,0f,100f);
	
	
		GUI.Label(textRect(6),"Unlimited Ammo",myStyle1);
		if( GUI.Button(buttonRect(6),UnlimitedAmmo?"ON":"OFF" ,myStyle2) )
			UnlimitedAmmo = !UnlimitedAmmo;			
	
		GUI.Label(textRect(7),"Show FPS",myStyle1);
		if( GUI.Button(buttonRect(7),ShowFPS?"ON":"OFF" ,myStyle2) )
			ShowFPS = !ShowFPS;
		
		GUI.Label(textRect(8),"Airsoft Peturb (" + ( ((float)((int)(100f*Peturb)))/100f/*:)*/ )+ "%)",myStyle1);
		Peturb = GUI.HorizontalSlider(buttonRect(8),Peturb,0f,10f);
	
		GUI.Label(textRect(9),"FT wait frames (" + FlameWaitingFrames + ")",myStyle1);
		FlameWaitingFrames = Mathf.RoundToInt(GUI.HorizontalSlider(buttonRect(9),(float)FlameWaitingFrames,0f,60f));
			
		if( GUI.Button( new Rect(Screen.width-100,Screen.height-60,80,40),"Options",myStyle2))
		{
			debugScreen = false;
			title.text = "Options";
		}
			

		}
		
		else{
			
		
		GUI.Label(textRect(1),"Volume",myStyle1);
		hSlideVolume = GUI.HorizontalSlider(buttonRect(1),hSlideVolume,0f,1f);
		
		GUI.Label(textRect(2),"Vibration",myStyle1);
		if( GUI.Button(buttonRect(2),Vibration?"ON":"OFF" ,myStyle2) )
			Vibration = !Vibration;
		
		GUI.Label(textRect(3),"Quality",myStyle1);
		
		if( GUI.Button(buttonRect(3),QualitySettings.names[QualitySettings.GetQualityLevel()],myStyle2 ) )
		{
			if( QualitySettings.GetQualityLevel() == QualitySettings.names.Length-1 )
				QualitySettings.SetQualityLevel(0);
			else
				QualitySettings.IncreaseLevel();
		}
		
		GUI.Label(textRect(4),"Light",myStyle1);
		if( GUI.Button(buttonRect(4),SpotLight?"FlashLight":"Directional",myStyle2 ) )
			SpotLight = !SpotLight;
		
		GUI.Label(textRect(5),"Fog",myStyle1);
		if( GUI.Button(buttonRect(5),Fog?"ON":"OFF",myStyle2 ) )
			Fog = !Fog;
			
		GUI.Label(textRect(6),"Shove Helper",myStyle1);
		if( GUI.Button(buttonRect(6),ShoveHelper?"ON":"OFF",myStyle2 ) )
			ShoveHelper = !ShoveHelper;
		
		GUI.Label(textRect(8),"Auto Targeting",myStyle1);
		if( GUI.Button(buttonRect(8),AutoTargeting?"ON":"OFF" ,myStyle2) )
			AutoTargeting = !AutoTargeting;
			
		GUI.Label(textRect(9),"Sensitivity(" + Mathf.Round(10000*Sensitivity)/100f + "%)",myStyle1);
		Sensitivity = GUI.HorizontalSlider(buttonRect(9),(float)Sensitivity,0f,0.2f);
			
		if(showdebug && GUI.Button( new Rect(Screen.width-200,Screen.height-60,80,40),"Debug",myStyle2))
		{
			debugScreen = true;
			title.text = "Options Debug";
		}
			
		if( GUI.Button( new Rect(Screen.width-100,Screen.height-60,80,40),"Credits",myStyle2))
			{
				MainMenu mainmenu = (MainMenu)GameObject.FindObjectOfType(typeof(MainMenu));
				mainmenu.GoState(MainMenu.MenuState.Credits);
			}
		}
		

			
	}
}
