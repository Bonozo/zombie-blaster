using UnityEngine;
using System.Collections;

public class ZBFacebook : MonoBehaviour {
	
	public string appID;
	
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
		
		FacebookManager.loginSucceededEvent += facebookLogin;
		FacebookManager.loginFailedEvent += facebookLoginFailed;
		FacebookManager.loggedOutEvent += facebookDidLogoutEvent;
		//Init();
		// Init also is called from 1. when going to lose state and when posting with an error 0 length name
	}
	
	#region Random Screenshot 
	
	private string screenshotFilename = "ingamerandomscreenshot.png";
	private float time=10f;
	private int sc=1;
	
	IEnumerator Start()
	{
		yield return new WaitForSeconds(4.0f);
		Application.CaptureScreenshot( screenshotFilename );
	}
	
	void Update()
	{
		if(Application.loadedLevel == 2)
		{
			time -= Time.time;
			if( time <= 0f )
			{
				if(LevelInfo.Environments.control.state == GameState.Play)
					Application.CaptureScreenshot( screenshotFilename );
				time = (++sc)*10f;
			}
		}
	}
	
	#endregion
	
	public void Init()
	{
		FacebookAndroid.init(appID);
		lastmessage = "Init called";
	}
	
	public void Login()
	{
		FacebookAndroid.loginWithRequestedPermissions( new string[] { "publish_stream", "email", "user_birthday" } );
	}

	void facebookLogin()
	{
		Logging = true;
		Facebook.instance.graphRequest( "me", completionHandler );
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
		string posttext = fbfirstname + " played Zombie Blaster! (Score: " + scores + ")";
		
		var pathToImage = Application.persistentDataPath + "/" + screenshotFilename;
		var bytes = System.IO.File.ReadAllBytes( pathToImage );
		
		Facebook.instance.postImage( bytes, posttext, completionHandler1 );
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
		return FacebookAndroid.isSessionValid();
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