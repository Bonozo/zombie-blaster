using UnityEngine;
using System.Collections;

public class Option : MonoBehaviour {
	
	public static float Health = 1f;
	public static float hSlideVolume = 1f;
	public static bool WeaponsUnlocked = false;
	public static float SpawnDistanceMin=10f,SpawnDistanceMax=13f;
	public static bool UnlimitedHealth = false;
	public static bool UnlimitedAmmo = false;
	public static bool UnlockAreas = false;
	public static bool Vibration = true;
	//public static int Quality = QualitySettings.GetQualityLevel();
	
	// Use this for initialization
	void Start ()
	{
	}
	
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
		
		GUI.Label(textRect(3),"Weapons Unlocked");
		if( GUI.Button(buttonRect(3),WeaponsUnlocked?"ON":"OFF" ))
		{
			WeaponsUnlocked = !WeaponsUnlocked;
			if( WeaponsUnlocked )
				for(int i=1;i<GameEnvironment.storeGun.Length;i++)
					GameEnvironment.storeGun[i].SetEnabled(Option.WeaponsUnlocked);
		}
		
		GUI.Label(textRect(4),"Spawn Distance Min");
		SpawnDistanceMin = GUI.HorizontalSlider(buttonRect(4),SpawnDistanceMin,5f,13f);
		if(SpawnDistanceMin>SpawnDistanceMax) SpawnDistanceMin=SpawnDistanceMax;
		
				
		GUI.Label(textRect(5),"Spawn Distance Max");
		SpawnDistanceMax = GUI.HorizontalSlider(buttonRect(5),SpawnDistanceMax,5f,13f);
		if( SpawnDistanceMax<SpawnDistanceMin) SpawnDistanceMax=SpawnDistanceMin;
		
		GUI.Label(textRect(6),"Unlimited Health");
		if( GUI.Button(buttonRect(6),UnlimitedHealth?"ON":"OFF" ) )
			UnlimitedHealth = !UnlimitedHealth;
		
		GUI.Label(textRect(7),"Unlimited Ammo");
		if( GUI.Button(buttonRect(7),UnlimitedAmmo?"ON":"OFF" ) )
			UnlimitedAmmo = !UnlimitedAmmo;
		
		GUI.Label(textRect(8),"Unlock Areas");
		if( GUI.Button(buttonRect(8),UnlockAreas?"ON":"OFF" ) )
			UnlockAreas = !UnlockAreas;
		
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
	}
}
