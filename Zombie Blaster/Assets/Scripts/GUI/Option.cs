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
		title.text = "";
		Update();
	}
	
	private Rect textRect(float index)
	{
		index++;
		return new Rect(ScreenWidth*0.05f,index*0.075f*ScreenHeight,ScreenWidth*0.3f,ScreenHeight*0.06f);
	}
	private Rect buttonRect(float index)
	{
		index++;
		return new Rect(ScreenWidth*0.35f,index*0.075f*ScreenHeight,ScreenWidth*0.4f,ScreenHeight*0.06f);
	}
	
	[System.NonSerializedAttribute]
	public int ScreenWidth = 1200;
	[System.NonSerializedAttribute]
	public int ScreenHeight = 800;
	
	void OnGUI()
	{
		float horizRatio = (float)Screen.width / (float)ScreenWidth;
		float vertRatio = (float)Screen.height / (float)ScreenHeight;
		GUI.matrix = Matrix4x4.TRS (Vector3.zero, Quaternion.identity, new Vector3 (horizRatio, vertRatio, 1));
		
		if(wanttoresetdefault)
		{
			GUI.DrawTexture(new Rect(0.2f*ScreenWidth,0.15f*ScreenHeight,0.6f*ScreenWidth,0.6f*ScreenHeight),texturePopup);
			GUI.Label(new Rect(0.35f*ScreenWidth,0.305f*ScreenHeight,0.3f*ScreenWidth,0.2f*ScreenHeight),"Restore Default Option Settings?",myStyle3);
			if(GUI.Button(new Rect(0.33f*ScreenWidth,0.5f*ScreenHeight,0.16f*ScreenWidth,0.1f*ScreenHeight), "YES", buttonGUIStyle ) )	
			{
				wanttoresetdefault = false;
				RestoreDefault();
				PlayTapAudio();
			}
			if( GUI.Button(new Rect(0.52f*ScreenWidth,0.5f*ScreenHeight,0.16f*ScreenWidth,0.1f*ScreenHeight), "NO", buttonGUIStyle ) )
			{
				wanttoresetdefault = false;
				PlayTapAudio();
			}	
			return;
		}
		
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
			
			if( GUI.Button( new Rect(0.84f*ScreenWidth,0.01f*ScreenHeight,0.15f*ScreenWidth,0.09f*ScreenHeight),"Options",myStyle2))
			{
				debugScreen = false;
				title.text = "";
				PlayTapAudio();
			}
			
			if( GUI.Button( new Rect(0.84f*ScreenWidth,0.11f*ScreenHeight,0.15f*ScreenWidth,0.09f*ScreenHeight),"+1000 ZH",myStyle2))
			{
				Store.zombieHeads += 1000;
				PlayTapAudio();
			}
			
			//By Mak Kaloliya on 07022013
			#if UNITY_IPHONE
			if( GUI.Button( new Rect(0.84f*ScreenWidth,0.21f*ScreenHeight,0.15f*ScreenWidth,0.09f*ScreenHeight),"Show TapJoy Offers",myStyle2))
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
			
			if( GUI.Button(buttonRect(11),"Restore Defaults",myStyle2 ) )
			{
				wanttoresetdefault = true;
				PlayTapAudio();
			}
			
			if(showdebug && GUI.Button( new Rect(0.84f*ScreenWidth,0.01f*ScreenHeight,0.15f*ScreenWidth,0.09f*ScreenHeight),"Debug",myStyle2))
			{
				debugScreen = true;
				title.text = "Debug";
				PlayTapAudio();
			}
		}
		

			
	}
	
	void PlayTapAudio()
	{
		audio.Play();
	}
}
