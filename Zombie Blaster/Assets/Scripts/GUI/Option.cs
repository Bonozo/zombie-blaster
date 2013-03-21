using UnityEngine;
using System.Collections;

public class Option : MonoBehaviour {
	
	#region Options
	
	private static float _hSlideVolume = 1f;
	public static float hSlideVolume{
		get{
			return _hSlideVolume;
		}
		set{
			if(_hSlideVolume != value)
			{
				_hSlideVolume = value;
				PlayerPrefs.SetFloat("options_volume",_hSlideVolume);
				if(Store.Instance.IsLevelGamePlay)
				{
					LevelInfo.Audio.InitVolume();
				}
				else
				{
					AudioListener.volume = _hSlideVolume;
				}
			}
		}
	}
	
	private static float _sfxVolume = 1f;
	public static float sfxVolume{
		get{
			return _sfxVolume;
		}
		set{
			if(_sfxVolume != value)
			{
				_sfxVolume = value;
				PlayerPrefs.SetFloat("options_sfxvolume",_sfxVolume);
				if(Store.Instance.IsLevelGamePlay)
				{
					LevelInfo.Audio.InitVolume();
				}
			}
		}
	}
	
	private static bool _Vibration = true;
	public static bool Vibration{
		get{
			return _Vibration;
		}
		set{
			if(_Vibration != value)
			{
				_Vibration = value;
				PlayerPrefs.SetInt("options_vibration",_Vibration?1:0);
			}
		}
	}
	
	private static bool _SpotLight = true;
	public static bool SpotLight{
		get{
			return _SpotLight;
		}
		set{
			if(_SpotLight != value)
			{
				_SpotLight = value;
				PlayerPrefs.SetInt("options_spotlight",_SpotLight?1:0);
			}
		}
	}
	
	private static bool _Fog = false;
	public static bool Fog{
		get{
			return _Fog;
		}
		set{
			if(_Fog != value)
			{
				_Fog = value;
				PlayerPrefs.SetInt("options_fog",_Fog?1:0);
			}
		}
	}
	
	private static bool _ShoveHelper = true;
	public static bool ShoveHelper{
		get{
			return _ShoveHelper;
		}
		set{
			if(_ShoveHelper != value)
			{
				_ShoveHelper = value;
				PlayerPrefs.SetInt("options_shovehelper",_ShoveHelper?1:0);
			}
		}
	}
	
	private static float _Sensitivity = 0.01f;
	public static float Sensitivity{
		get{
			return _Sensitivity;
		}
		set{
			if(_Sensitivity != value)
			{
				_Sensitivity = value;
				PlayerPrefs.SetFloat("options_sensitivity",_Sensitivity);
			}
		}
	}
	
	private static bool _TiltingMove = true;
	public static bool TiltingMove{
		get{
			return _TiltingMove;
		}
		set{
			if(_TiltingMove != value)
			{
				_TiltingMove = value;
				PlayerPrefs.SetInt("options_tiltmove",_TiltingMove?1:0);
			}
		}
	}
	
	private static bool _XInversion = false;
	public static bool XInversion{
		get{
			return _XInversion;
		}
		set{
			if(_XInversion != value)
			{
				_XInversion = value;
				PlayerPrefs.SetInt("options_xinversion",_XInversion?1:0);
			}
		}
	}
	#endregion
	
	#region Debug	
	public static bool UnlimitedHealth = false;
	public static bool UnlimitedAmmo = false;
	public static float SpotLightAngle = 65f;
	public static float BackgroundAmbient = 101f;
	public static bool ShowFPS = false;
	public static bool AutoTargeting = false;
	public static int FlameWaitingFrames=5;
	#endregion
	
	#region Init
	
	public static void Init()
	{
		hSlideVolume = PlayerPrefs.GetFloat("options_volume",0.5f);
		sfxVolume = PlayerPrefs.GetFloat("options_sfxvolume",1f);
		Vibration = PlayerPrefs.GetInt("options_vibration",1)==1;
		SpotLight = PlayerPrefs.GetInt("options_spotlight",1)==1;
		Fog = PlayerPrefs.GetInt("options_fog",0)==1;
		ShoveHelper = PlayerPrefs.GetInt("options_shovehelper",1)==1;
		Sensitivity = PlayerPrefs.GetFloat("options_sensitivity",0.01f);
		TiltingMove = PlayerPrefs.GetInt("options_tiltmove",1)==1;
		XInversion = PlayerPrefs.GetInt("options_xinversion",0)==1;
		
		AudioListener.volume = _hSlideVolume;
	}
	
	private void RestoreDefault()
	{
		hSlideVolume = 0.5f;
		sfxVolume = 1f;
		Vibration = true;
		SpotLight = true;
		Fog=false;
		ShoveHelper = true;
		Sensitivity = 0.01f;
		TiltingMove = true;
		XInversion = false;
		QualitySettings.SetQualityLevel(1);
	}
	
	#endregion


	#region Variables
	
	bool showdebug = true;
	bool debugScreen = false;
	bool wanttoresetdefault;
	bool showqualityleveldescription = false;
	public ButtonBase creditsButton;
	public GUIText title;
	public GUIText version;
	public GUIStyle myStyle1;
	public GUIStyle myStyle2;
	public GUIStyle myStyle3;
	public GUIStyle buttonGUIStyle;
	public Texture texturePopup;
	public ButtonBase backToGameForGamePlay;
	
	#endregion
	
	// Update is called once per frame
	void Update () 
	{
		version.enabled = !debugScreen;
		
		if( backToGameForGamePlay!=null && backToGameForGamePlay.PressedUp )
		{
			LevelInfo.Environments.control.state = GameState.Paused;
			return;
		}
		
		if( creditsButton!=null && creditsButton.PressedUp )
		{
			MainMenu mainmenu = (MainMenu)GameObject.FindObjectOfType(typeof(MainMenu));
			mainmenu.GoState(MainMenu.MenuState.Credits);
		}
		
		if( GameEnvironment.AbsoluteSwipe.x < -0.5f )
			showdebug = true;
	}
	
	void OnEnable()
	{
		debugScreen = false;
		showdebug = false;
		wanttoresetdefault = false;
		showqualityleveldescription = false;
		title.text = "";
		Update();
	}
	
	private Rect textRect(float index)
	{
		index++;
		return new Rect(GameEnvironment.ScreenWidth*0.05f,index*0.075f*GameEnvironment.ScreenHeight,GameEnvironment.ScreenWidth*0.3f,GameEnvironment.ScreenHeight*0.06f);
	}
	private Rect buttonRect(float index)
	{
		index++;
		return new Rect(GameEnvironment.ScreenWidth*0.35f,index*0.075f*GameEnvironment.ScreenHeight,GameEnvironment.ScreenWidth*0.4f,GameEnvironment.ScreenHeight*0.06f);
	}
	
	private Rect buttonRectRight(float index)
	{
		index++;
		return new Rect(GameEnvironment.ScreenWidth*0.76f,index*0.075f*GameEnvironment.ScreenHeight,GameEnvironment.ScreenWidth*0.06f,GameEnvironment.ScreenHeight*0.06f);
	}
	
	/*[System.NonSerializedAttribute]
	public int GameEnvironment.ScreenWidth = 1200;
	[System.NonSerializedAttribute]
	public int GameEnvironment.ScreenHeight = 800;*/
	
	void OnGUI()
	{
		GUI.matrix = GameEnvironment.GetGameGUIMatrix();
		GUI.enabled = !wanttoresetdefault&&!showqualityleveldescription;
		
		if( debugScreen )
		{
			GUI.Label(textRect(1),"Unlimited Health",myStyle1);
			if( GUI.Button(buttonRect(1),UnlimitedHealth?"ON":"OFF" ,myStyle2) )
				UnlimitedHealth = !UnlimitedHealth;
			
			GUI.Label(textRect(2),"Tapjoy Connected",myStyle1);
			GUI.Box(buttonRect(2),"" + Store.tapjoyConnected,myStyle2 );
			
			GUI.Label(textRect(3),"Auto Targeting",myStyle1);
			if( GUI.Button(buttonRect(3),AutoTargeting?"ON":"OFF" ,myStyle2) )
				AutoTargeting = !AutoTargeting;
	
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
			
			GUI.Label(textRect(8),"Airsoft Accuracy (" + GameEnvironment.storeGun[0].accuracy + "%)",myStyle1);
			GameEnvironment.storeGun[0].accuracy = Mathf.RoundToInt(GUI.HorizontalSlider(buttonRect(8),GameEnvironment.storeGun[0].accuracy,0f,100f));
		
			GUI.Label(textRect(9),"FT wait frames (" + FlameWaitingFrames + ")",myStyle1);
			FlameWaitingFrames = Mathf.RoundToInt(GUI.HorizontalSlider(buttonRect(9),(float)FlameWaitingFrames,0f,60f));
			
			if( GUI.Button( new Rect(0.84f*GameEnvironment.ScreenWidth,0.01f*GameEnvironment.ScreenHeight,0.15f*GameEnvironment.ScreenWidth,0.09f*GameEnvironment.ScreenHeight),"Options",myStyle2))
			{
				debugScreen = false;
				title.text = "";
				PlayTapAudio();
			}
			
			if( GUI.Button( new Rect(0.84f*GameEnvironment.ScreenWidth,0.11f*GameEnvironment.ScreenHeight,0.15f*GameEnvironment.ScreenWidth,0.09f*GameEnvironment.ScreenHeight),"+1000 ZH",myStyle2))
			{
				Store.zombieHeads += 1000;
				PlayTapAudio();
			}
			
			//By Mak Kaloliya on 07022013
			#if UNITY_IPHONE
			if( GUI.Button( new Rect(0.84f*GameEnvironment.ScreenWidth,0.21f*GameEnvironment.ScreenHeight,0.15f*GameEnvironment.ScreenWidth,0.09f*GameEnvironment.ScreenHeight),"Show TapJoy Offers",myStyle2))
			{
				TapjoyBinding.showOffers();
			}
			#endif
			//End
		}
		
		else
		{
			GUI.Label(textRect(1),"Music ("+(int)(hSlideVolume*100)+"%)",myStyle1);
			hSlideVolume = GUI.HorizontalSlider(buttonRect(1),hSlideVolume,0f,1f);
		
			GUI.Label(textRect(2),"SFX ("+(int)(sfxVolume*100)+"%)",myStyle1);
			sfxVolume = GUI.HorizontalSlider(buttonRect(2),sfxVolume,0f,1f);
			
			GUI.Label(textRect(3),"Vibration",myStyle1);
			if( GUI.Button(buttonRect(3),Vibration?"ON":"OFF" ,myStyle2) )
			{
				Vibration = !Vibration;
				PlayTapAudio();
			}
			
			GUI.Label(textRect(4),"Light",myStyle1);
			if( GUI.Button(buttonRect(4),SpotLight?"FlashLight":"Directional",myStyle2 ) )
			{
				SpotLight = !SpotLight;
				PlayTapAudio();
			}
			
			GUI.Label(textRect(5),"Fog",myStyle1);
			if( GUI.Button(buttonRect(5),Fog?"ON":"OFF",myStyle2 ) )
			{
				Fog = !Fog;
				PlayTapAudio();
			}
			
			GUI.Label(textRect(6),"Shove Helper",myStyle1);
			if( GUI.Button(buttonRect(6),ShoveHelper?"ON":"OFF",myStyle2 ) )
			{
				ShoveHelper = !ShoveHelper;
				PlayTapAudio();
			}
			
			GUI.Label(textRect(7),"Sensitivity(" + Mathf.Round(10000*Sensitivity)/100f + "%)",myStyle1);
			Sensitivity = GUI.HorizontalSlider(buttonRect(7),(float)Sensitivity,0f,0.2f);
			
			GUI.Label(textRect(8),"Tilt Move",myStyle1);
			if( GUI.Button(buttonRect(8),TiltingMove?"ON":"OFF",myStyle2 ) )
			{
				TiltingMove = !TiltingMove;
				PlayTapAudio();
			}
			
			GUI.Label(textRect(9),"X-Inversion",myStyle1);
			if( GUI.Button(buttonRect(9),XInversion?"ON":"OFF",myStyle2 ) )
			{
				XInversion = !XInversion;
				PlayTapAudio();
			}
			
			GUI.Label(textRect(10),"Quality",myStyle1);	
			if( GUI.Button(buttonRect(10),QualitySettings.names[QualitySettings.GetQualityLevel()],myStyle2 ) )
			{
				if( QualitySettings.GetQualityLevel() == QualitySettings.names.Length-1 )
					QualitySettings.SetQualityLevel(0);
				else
					QualitySettings.IncreaseLevel();
				PlayTapAudio();
			}
			
			if( GUI.Button(buttonRectRight(10),"[?]", myStyle2 ) )
			{
				showqualityleveldescription = true;
				PlayTapAudio();
			}
			
			if( GUI.Button(buttonRect(11),"Restore Defaults",myStyle2 ) )
			{
				wanttoresetdefault = true;
				PlayTapAudio();
				return;
			}
			
			if(showdebug && GUI.Button( new Rect(0.84f*GameEnvironment.ScreenWidth,0.01f*GameEnvironment.ScreenHeight,0.15f*GameEnvironment.ScreenWidth,0.09f*GameEnvironment.ScreenHeight),"Debug",myStyle2))
			{
				debugScreen = true;
				title.text = "Debug";
				PlayTapAudio();
			}
			
			if(wanttoresetdefault)
			{
				GUI.enabled = true;
				GUI.DrawTexture(new Rect(0.2f*GameEnvironment.ScreenWidth,0.15f*GameEnvironment.ScreenHeight,0.6f*GameEnvironment.ScreenWidth,0.6f*GameEnvironment.ScreenHeight),texturePopup);
				GUI.Label(new Rect(0.35f*GameEnvironment.ScreenWidth,0.305f*GameEnvironment.ScreenHeight,0.3f*GameEnvironment.ScreenWidth,0.2f*GameEnvironment.ScreenHeight),"Restore Default Option Settings?",myStyle3);
				if(GUI.Button(new Rect(0.33f*GameEnvironment.ScreenWidth,0.5f*GameEnvironment.ScreenHeight,0.16f*GameEnvironment.ScreenWidth,0.1f*GameEnvironment.ScreenHeight), "YES", buttonGUIStyle ) )	
				{
					wanttoresetdefault = false;
					RestoreDefault();
					PlayTapAudio();
				}
				if( GUI.Button(new Rect(0.52f*GameEnvironment.ScreenWidth,0.5f*GameEnvironment.ScreenHeight,0.16f*GameEnvironment.ScreenWidth,0.1f*GameEnvironment.ScreenHeight), "NO", buttonGUIStyle ) )
				{
					wanttoresetdefault = false;
					PlayTapAudio();
				}	
			}
			
			if(showqualityleveldescription)
				ShowCurrentQualitySettings();
		}	
	}
	
	void ShowCurrentQualitySettings()
	{
		GUI.enabled = true;
		GUI.DrawTexture(new Rect(0.1f*GameEnvironment.ScreenWidth,0.1f*GameEnvironment.ScreenHeight,0.8f*GameEnvironment.ScreenWidth,0.8f*GameEnvironment.ScreenHeight),texturePopup);
		int oldfontsize = myStyle3.fontSize;
		myStyle3.fontSize = 28;
		int index = QualitySettings.GetQualityLevel();
		string str = 
			// ooops Unity Standard Settings
			"Name: " + QualitySettings.names[index] + "\n" +
			"Pixel Light Count: " + QualitySettings.pixelLightCount.ToString () + "\n" + 
			// Texture Quality = Always Full Res
			"Anisotropic Filtering: " + QualitySettings.anisotropicFiltering.ToString() + "\n" + 
			"Anti Aliasing: " + QualitySettings.antiAliasing.ToString() + "\n" + 
			// Soft Particles = Poor(false), Average(false), Best(true)
			// Shadow = Poor(Disable Shadows), Average(Hard shadows only), Best(Hard and Soft Shadows)
			// Shadow Resolution = Poor(low), Average(medium), Best(hard)
			"Shadow Projection: " + QualitySettings.shadowProjection.ToString() + "\n" + 
			"Shadow Cascades: " + QualitySettings.shadowCascades.ToString() + "\n" + 
			"Shadow Distance: " + QualitySettings.shadowDistance.ToString() + "\n" + 
			"Blend Weights: " + QualitySettings.blendWeights.ToString() + "\n" + 
			"VSync Count: " + QualitySettings.vSyncCount.ToString() + "\n" + 
			"Particle Raycast Budget: " + QualitySettings.particleRaycastBudget.ToString() + "\n";
		
		GUI.Label(new Rect(0.3f*GameEnvironment.ScreenWidth,0.25f*GameEnvironment.ScreenHeight,0.5f*GameEnvironment.ScreenWidth,0.5f*GameEnvironment.ScreenHeight),str ,myStyle3);
		
		myStyle3.fontSize = oldfontsize;
		
		if( GUI.Button(new Rect(0.42f*GameEnvironment.ScreenWidth,0.62f*GameEnvironment.ScreenHeight,0.16f*GameEnvironment.ScreenWidth,0.1f*GameEnvironment.ScreenHeight), "OK", buttonGUIStyle ) )
		{
			showqualityleveldescription = false;
			PlayTapAudio();
		}	
	}
	
	void PlayTapAudio()
	{
		audio.Play();
	}
}
