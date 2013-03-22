using UnityEngine;
using System.Collections;

public class ZBFacebook : MonoBehaviour {
	
	public string appID;
	public GameObject screenshotMessage;
	
	[System.NonSerialized]
	public string lastmessage = "";
	[System.NonSerialized]
	public string fbname = "";
	[System.NonSerialized]
	public string fbfirstname = "";
	[System.NonSerializedAttribute]
	public bool Ready = false;
	[System.NonSerializedAttribute]
	public bool Logging = false;	
	[System.NonSerializedAttribute]
	public bool Posted = false;
	
	void Awake()
	{
		DontDestroyOnLoad(this.gameObject);
				
		#if UNITY_ANDROID
		FacebookManager.sessionOpenedEvent += facebookLogin;
		FacebookManager.loginFailedEvent += facebookLoginFailed;
		//FacebookManager.loggedOutEvent += facebookDidLogoutEvent;
		//Mak Kaloliya On 17-03-13
		#elif UNITY_IOS
		FacebookManager.sessionOpenedEvent += facebookLogin;
		FacebookManager.loginFailedEvent += facebookLoginFailed;
		//FacebookManagerIOS.loggedOutEvent += facebookDidLogoutEvent;	
		#endif
		Init();
		// Init also is called from 1. when going to lose state and when posting with an error 0 length name
	}
	
	#region Random Screenshot 
	
	private string screenshotFilename = "ingamerandomscreenshot.png";
	private float time=5f;
	private int sc=1;
	
	IEnumerator Start()
	{
		yield return new WaitForSeconds(3.0f);
		Application.CaptureScreenshot( screenshotFilename );
	}
	
	void Update()
	{
		#if UNITY_ANDROID || UNITY_IPHONE
		if(Application.loadedLevel == 2)
		{
			time -= Time.deltaTime;
			if( time <= 0f )
			{
				if(LevelInfo.Environments.control.state == GameState.Play)
				{
					Application.CaptureScreenshot( screenshotFilename );
					StartCoroutine(ShowScreenShotMessage());
				}
				time = (++sc)*10f;
			}
		}
		#endif
	}
	
	IEnumerator ShowScreenShotMessage()
	{
		screenshotMessage.SetActive(true);
		yield return new WaitForSeconds(1.0f);
		screenshotMessage.SetActive(false);
	}
	
	#endregion
	
	public void Init()
	{
		#if UNITY_ANDROID
		FacebookAndroid.init();
		lastmessage = "Init called";
		#elif UNITY_IPHONE
		FacebookBinding.init();
		lastmessage = "Init called";
		#endif
	}
	
	public void Login()
	{
		#if UNITY_ANDROID
		FacebookAndroid.loginWithReadPermissions( new string[] { "email", "user_birthday" } );
		//FacebookAndroid.loginWithRequestedPermissions( new string[] { "publish_stream", "email", "user_birthday" } );
		#elif UNITY_IPHONE
		FacebookBinding.loginWithReadPermissions(new string[] { "email", "user_birthday" });
		#endif
	}

	void facebookLogin()
	{
		Logging = true;
		#if UNITY_ANDROID
		Facebook.instance.graphRequest( "me", completionHandler );
		#elif UNITY_IPHONE
		Facebook.instance.graphRequest( "me", completionHandler );
		#endif
		
		
	}

	void completionHandler( string error, object result )
	{
		var ht = result as Hashtable;
		fbname = ht["name"].ToString();
		fbfirstname = ht["first_name"].ToString();
		lastmessage = fbname + " successfully logged in to Facebook";
		Ready = true;
		Logging = false;
	}
	
	void facebookLoginFailed( string error )
	{
		lastmessage = "Facebook login failed: " + error;
		Logging = false;
	}
	
	void facebookDidLogoutEvent()
	{
		lastmessage = fbname + " logged out" ;
		fbname = "";
		fbfirstname = "";
		Ready = false;
	}
	
	public void PostOnWall(int scores)
	{
			#if UNITY_ANDROID
		string posttext = fbfirstname + " played Zombie Blaster! (Score: " + scores + ")";
		
		var pathToImage = Application.persistentDataPath + "/" + screenshotFilename;
		var bytes = System.IO.File.ReadAllBytes( pathToImage );
		
		Facebook.instance.postImage( bytes, posttext, completionHandler1 );
		#elif UNITY_IPHONE
		string posttext = fbfirstname + " played Zombie Blaster! (Score: " + scores + ")";
		
		var pathToImage = Application.persistentDataPath + "/" + screenshotFilename;
		var bytes = System.IO.File.ReadAllBytes( pathToImage );
		
		Facebook.instance.postImage( bytes, posttext, completionHandler1 );
		#endif
	}
	
	private void completionHandler1( string error, object result )
	{
		if(error!=null)
			lastmessage = "Error when posting on facebook: " + error;
		else
		{
			lastmessage = "Successfully posted on facebook wall";
			Posted = true;
		}
	}
	
	public bool isSessionValid()
	{
		#if UNITY_ANDROID
		return FacebookAndroid.isSessionValid();
		#elif UNITY_IPHONE
		return FacebookBinding.isSessionValid();
		#else
		return false;
		#endif
	}
	
	/*void OnGUI()
	{
		GUI.Label(new Rect(0,0,1000,1000),lastmessage);
	}*/
	
	#region Static Instance
	
	// Multithreaded Safe Singleton Pattern
    // URL: http://msdn.microsoft.com/en-us/library/ms998558.aspx
    private static readonly object _syncRoot = new Object();
    private static volatile ZBFacebook _staticInstance;	
    public static ZBFacebook Instance 
	{
        get {
            if (_staticInstance == null) {				
                lock (_syncRoot) {
                    _staticInstance = FindObjectOfType (typeof(ZBFacebook)) as ZBFacebook;
                    if (_staticInstance == null) {
                       Debug.LogError("The ZBFacebook instance was unable to be found, if this error persists please contact support.");						
                    }
                }
            }
            return _staticInstance;
        }
    }
	
	#endregion
}