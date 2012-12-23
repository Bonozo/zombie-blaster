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
	//public static int Quality = QualitySettings.GetQualityLevel();
	
	bool debugScreen = false;
	public GUIText title;
	public GUIText version;
	
	// Update is called once per frame
	void Update () 
	{
		AudioListener.volume = hSlideVolume;
		version.enabled = debugScreen;
	}
	
	private Vector2 textSize = new Vector2(Screen.width*0.2f,30);
	private Vector2 buttonSize = new Vector2(Screen.width*0.2f,30);

	private Rect textRect(float index)
	{
		index++;
		float w = 0f;
		if( index > 8 ) { w = Screen.width*0.5f; index-=8; }
		return new Rect(w + Screen.width*0.05f,index*40f,textSize.x,textSize.y);
	}
	private Rect buttonRect(float index)
	{
		index++;
		float w = 0f;
		if( index > 8 ) { w = Screen.width*0.5f; index-=8; }
		return new Rect(w+Screen.width*0.1f+textSize.x,index*40f,buttonSize.x,buttonSize.y);
	}
	
	void OnGUI()
	{
		if( debugScreen ){
			
		GUI.Label(textRect(1),"Unlimited Health");
		if( GUI.Button(buttonRect(1),UnlimitedHealth?"ON":"OFF" ) )
			UnlimitedHealth = !UnlimitedHealth;
		
		GUI.Label(textRect(2),"Tapjoy Connected Status");
		GUI.Box(buttonRect(2),"" + Store.tapjoyConnected );
			
		GUI.Label(textRect(3),"Health");
		Health = GUI.HorizontalSlider(buttonRect(3),Health,0.05f,1f);

		GUI.Label(textRect(4),"Background Ambient (" + (int)BackgroundAmbient + ")");
		BackgroundAmbient = GUI.HorizontalSlider(buttonRect(4),BackgroundAmbient,0f,255f);
		
			
		GUI.Label(textRect(5),"Spot Light Angle (" + (int)SpotLightAngle + ")");
		SpotLightAngle = GUI.HorizontalSlider(buttonRect(5),SpotLightAngle,0f,100f);
	
	
		GUI.Label(textRect(6),"Unlimited Ammo");
		if( GUI.Button(buttonRect(6),UnlimitedAmmo?"ON":"OFF" ) )
			UnlimitedAmmo = !UnlimitedAmmo;			
	
		GUI.Label(textRect(7),"Show FPS");
		if( GUI.Button(buttonRect(7),ShowFPS?"ON":"OFF" ) )
			ShowFPS = !ShowFPS;
			
		if( GUI.Button( new Rect(Screen.width*0.45f,Screen.height-100,0.1f*Screen.width,40),"Options"))
		{
			debugScreen = false;
			title.text = "Options";
		}
		}
		
		else{
			
		
		GUI.Label(textRect(1),"Volume");
		hSlideVolume = GUI.HorizontalSlider(buttonRect(1),hSlideVolume,0f,1f);
		
		GUI.Label(textRect(2),"Vibration");
		if( GUI.Button(buttonRect(2),Vibration?"ON":"OFF" ) )
			Vibration = !Vibration;
		
		GUI.Label(textRect(3),"Quality");
		
		if( GUI.Button(buttonRect(3),QualitySettings.names[QualitySettings.GetQualityLevel()] ) )
		{
			if( QualitySettings.GetQualityLevel() == QualitySettings.names.Length-1 )
				QualitySettings.SetQualityLevel(0);
			else
				QualitySettings.IncreaseLevel();
		}
		
		GUI.Label(textRect(4),"Light");
		if( GUI.Button(buttonRect(4),SpotLight?"FlashLight":"Directional" ) )
			SpotLight = !SpotLight;
		
		GUI.Label(textRect(5),"Fog");
		if( GUI.Button(buttonRect(5),Fog?"ON":"OFF" ) )
			Fog = !Fog;
			
		if( GUI.Button( new Rect(Screen.width*0.45f,Screen.height-100,0.1f*Screen.width,40),"Debug"))
		{
			debugScreen = true;
			title.text = "Options Debug";
		}
		}
		

			
	}
}
