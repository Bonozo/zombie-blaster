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
	//public static int Quality = QualitySettings.GetQualityLevel();
	
	// Update is called once per frame
	void Update () 
	{
		AudioListener.volume = hSlideVolume;
	}
	
	private Vector2 textSize = new Vector2(Screen.width*0.2f,30);
	private Vector2 buttonSize = new Vector2(Screen.width*0.2f,30);

	private Rect textRect(float index)
	{
		float w = 0f;
		if( index > 8 ) { w = Screen.width*0.5f; index-=8; }
		return new Rect(w + Screen.width*0.05f,index*40f,textSize.x,textSize.y);
	}
	private Rect buttonRect(float index)
	{
		float w = 0f;
		if( index > 8 ) { w = Screen.width*0.5f; index-=8; }
		return new Rect(w+Screen.width*0.1f+textSize.x,index*40f,buttonSize.x,buttonSize.y);
	}
	
	void OnGUI()
	{
		GUI.Label(textRect(1),"Volume");
		hSlideVolume = GUI.HorizontalSlider(buttonRect(1),hSlideVolume,0f,1f);
		
		GUI.Label(textRect(2),"Health");
		Health = GUI.HorizontalSlider(buttonRect(2),Health,0.05f,1f);

		GUI.Label(textRect(3),"Background Ambient (" + (int)BackgroundAmbient + ")");
		BackgroundAmbient = GUI.HorizontalSlider(buttonRect(3),BackgroundAmbient,0f,255f);
		
		
		GUI.Label(textRect(6),"Unlimited Health");
		if( GUI.Button(buttonRect(6),UnlimitedHealth?"ON":"OFF" ) )
			UnlimitedHealth = !UnlimitedHealth;
		
		GUI.Label(textRect(7),"Unlimited Ammo");
		if( GUI.Button(buttonRect(7),UnlimitedAmmo?"ON":"OFF" ) )
			UnlimitedAmmo = !UnlimitedAmmo;
		
		GUI.Label(textRect(9),"Vibration");
		if( GUI.Button(buttonRect(9),Vibration?"ON":"OFF" ) )
			Vibration = !Vibration;
		
		GUI.Label(textRect(10),"Quality");
		
		if( GUI.Button(buttonRect(10),QualitySettings.names[QualitySettings.GetQualityLevel()] ) )
		{
			if( QualitySettings.GetQualityLevel() == QualitySettings.names.Length-1 )
				QualitySettings.SetQualityLevel(0);
			else
				QualitySettings.IncreaseLevel();
		}
		
		GUI.Label(textRect(11),"Light");
		if( GUI.Button(buttonRect(11),SpotLight?"FlashLight":"Directional" ) )
			SpotLight = !SpotLight;
		
		GUI.Label(textRect(12),"Fog");
		if( GUI.Button(buttonRect(12),Fog?"ON":"OFF" ) )
			Fog = !Fog;
		
		GUI.Label(textRect(13),"Spot Light Angle (" + (int)SpotLightAngle + ")");
		SpotLightAngle = GUI.HorizontalSlider(buttonRect(13),SpotLightAngle,0f,100f);
		
		GUI.Label(textRect(14),"Tapjoy Connected Status");
		GUI.Box(buttonRect(14),"" + Store.tapjoyConnected );
			
	}
}
