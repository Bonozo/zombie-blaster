using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Prime31;

public class ZombieBlasterFacebook : MonoBehaviourGUI {

	#if UNITY_ANDROID
	public GameObject screenshotMessage;
	public static ZombieBlasterFacebook Instance;
	
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
	
	private string screenshotFilename = "ingamerandomscreenshot.png";
	private float time=5f;
	private int sc=1;
	
	void Awake() {
		DontDestroyOnLoad(this.gameObject);
		Instance = this;
	}
	
	void Start()
	{
		// grab a screenshot for later use
		//Application.CaptureScreenshot( screenshotFilename );
		StartCoroutine(CaptureScreenShoot());
		Init();
		// optionally enable logging of all requests that go through the Facebook class
		//Facebook.instance.debugRequests = true;
	}
	
	#region Random Screenshot 
	
	IEnumerator CaptureScreenShoot()
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
		Debug.Log("ZombieBlasterFacebook  init called");
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
		Debug.Log("ZombieBlasterFacebook  login called");
		#if UNITY_ANDROID
		FacebookAndroid.loginWithReadPermissions( new string[] { "email", "user_birthday" } );
		#elif UNITY_IPHONE
		FacebookBinding.loginWithReadPermissions(new string[] { "email", "user_birthday" });
		#endif
	}

	void completionHandler( string error, object result )
	{
		Debug.Log("ZombieBlasterFacebook  me request successfull");
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
		beginColumn();


		if( GUILayout.Button( "Initialize Facebook" ) )
		{
			FacebookAndroid.init();
		}
	
	
		if( GUILayout.Button( "Login" ) )
		{
			FacebookAndroid.loginWithReadPermissions( new string[] { "email", "user_birthday" } );
		}
		
		
		if( GUILayout.Button( "Reauthorize with Publish Permissions" ) )
		{
			FacebookAndroid.reauthorizeWithPublishPermissions( new string[] { "publish_actions", "manage_friendlists" }, FacebookSessionDefaultAudience.EVERYONE );
		}

		
		if( GUILayout.Button( "Logout" ) )
		{
			FacebookAndroid.logout();
		}
	
	
		if( GUILayout.Button( "Is Session Valid?" ) )
		{
			var isSessionValid = FacebookAndroid.isSessionValid();
			Debug.Log( "Is session valid?: " + isSessionValid );
		}
		
	
		if( GUILayout.Button( "Get Session Token" ) )
		{
			var token = FacebookAndroid.getAccessToken();
			Debug.Log( "session token: " + token );
		}

		
		if( GUILayout.Button( "Get Granted Permissions" ) )
		{
			var permissions = FacebookAndroid.getSessionPermissions();
			Debug.Log( "granted permissions: " + permissions.Count );
			Prime31.Utils.logObject( permissions );
		}

	
		endColumn( true );
		

		if( GUILayout.Button( "Post Image" ) )
		{
			var pathToImage = Application.persistentDataPath;// + "/" + screenshotFilename;
			var bytes = System.IO.File.ReadAllBytes( pathToImage );
			
			Facebook.instance.postImage( bytes, "im an image posted from Android", completionHandler );
		}


		if( GUILayout.Button( "Graph Request (me)" ) )
		{
			Facebook.instance.graphRequest( "me", completionHandler );
		}


		if( GUILayout.Button( "Post Message" ) )
		{
			Facebook.instance.postMessage( "im posting this from Unity: " + Time.deltaTime, completionHandler );
		}
		
		
		if( GUILayout.Button( "Post Message & Extras" ) )
		{
			Facebook.instance.postMessageWithLinkAndLinkToImage( "link post from Unity: " + Time.deltaTime, "http://prime31.com", "Prime31 Studios", "http://prime31.com/assets/images/prime31logo.png", "Prime31 Logo", completionHandler );
		}


		if( GUILayout.Button( "Show Post Dialog" ) )
		{
			// parameters are optional. See Facebook's documentation for all the dialogs and paramters that they support
			var parameters = new Dictionary<string,string>
			{
				{ "link", "http://prime31.com" },
				{ "name", "link name goes here" },
				{ "picture", "http://prime31.com/assets/images/prime31logo.png" },
				{ "caption", "the caption for the image is here" }
			};
			FacebookAndroid.showDialog( "stream.publish", parameters );
		}


		if( GUILayout.Button( "Get Friends" ) )
		{
			Facebook.instance.getFriends( completionHandler );
		}

		
		endColumn();
		
		
		if( bottomLeftButton( "Twitter Scene" ) )
		{
			Application.LoadLevel( "TwitterTestScene" );
		}
	}*/

#endif
	
	#if UNITY_IPHONE || UNITY_ANDROID
	// Listens to all the events.  All event listeners MUST be removed before this object is disposed!
	void OnEnable()
	{
		FacebookManager.sessionOpenedEvent += sessionOpenedEvent;
		FacebookManager.loginFailedEvent += loginFailedEvent;
		
		FacebookManager.dialogCompletedWithUrlEvent += dialogCompletedEvent;
		FacebookManager.dialogFailedEvent += dialogFailedEvent;
		FacebookManager.dialogCompletedEvent += facebokDialogCompleted;
		FacebookManager.dialogDidNotCompleteEvent += dialogDidNotCompleteEvent;
		
		FacebookManager.graphRequestCompletedEvent += graphRequestCompletedEvent;
		FacebookManager.graphRequestFailedEvent += facebookCustomRequestFailed;
		FacebookManager.restRequestCompletedEvent += restRequestCompletedEvent;
		FacebookManager.restRequestFailedEvent += restRequestFailedEvent;
		FacebookManager.facebookComposerCompletedEvent += facebookComposerCompletedEvent;
		
		FacebookManager.reauthorizationFailedEvent += reauthorizationFailedEvent;
		FacebookManager.reauthorizationSucceededEvent += reauthorizationSucceededEvent;
	}

	
	void OnDisable()
	{
		// Remove all the event handlers when disabled
		FacebookManager.sessionOpenedEvent -= sessionOpenedEvent;
		FacebookManager.loginFailedEvent -= loginFailedEvent;

		FacebookManager.dialogCompletedEvent -= facebokDialogCompleted;
		FacebookManager.dialogCompletedWithUrlEvent -= dialogCompletedEvent;
		FacebookManager.dialogDidNotCompleteEvent -= dialogDidNotCompleteEvent;
		FacebookManager.dialogFailedEvent -= dialogFailedEvent;
		
		FacebookManager.graphRequestCompletedEvent -= graphRequestCompletedEvent;
		FacebookManager.graphRequestFailedEvent -= facebookCustomRequestFailed;
		FacebookManager.restRequestCompletedEvent -= restRequestCompletedEvent;
		FacebookManager.restRequestFailedEvent -= restRequestFailedEvent;
		FacebookManager.facebookComposerCompletedEvent -= facebookComposerCompletedEvent;
		
		FacebookManager.reauthorizationFailedEvent -= reauthorizationFailedEvent;
		FacebookManager.reauthorizationSucceededEvent -= reauthorizationSucceededEvent;
	}

	

	void sessionOpenedEvent()
	{
		Debug.Log( "Successfully logged in to Facebook" );
		
		Debug.Log("ZombieBlasterFacebook  login sucess");
		Logging = true;
		#if UNITY_ANDROID
		Facebook.instance.graphRequest( "me", completionHandler );
		#elif UNITY_IPHONE
		Facebook.instance.graphRequest( "me", completionHandler );
		#endif
	}
	
	
	void loginFailedEvent( string error )
	{
		Debug.Log( "Facebook login failed: " + error );
		lastmessage = "Facebook login failed: " + error;
		Logging = false;
	}


	void dialogCompletedEvent( string url )
	{
		Debug.Log( "dialogCompletedEvent: " + url );
	}
	
	
	void dialogFailedEvent( string error )
	{
		Debug.Log( "dialogFailedEvent: " + error );
	}
	
	
	void facebokDialogCompleted()
	{
		Debug.Log( "facebokDialogCompleted" );
	}

	
	void dialogDidNotCompleteEvent()
	{
		Debug.Log( "facebookDialogDidntComplete" );
	}

	
	void graphRequestCompletedEvent( object obj )
	{
		Debug.Log( "graphRequestCompletedEvent" );
		Prime31.Utils.logObject( obj );
	}
	
	
	void facebookCustomRequestFailed( string error )
	{
		Debug.Log( "facebookCustomRequestFailed failed: " + error );
	}
	
	
	void restRequestCompletedEvent( object obj )
	{
		Debug.Log( "restRequestCompletedEvent" );
		Prime31.Utils.logObject( obj );
	}
	
	
	void restRequestFailedEvent( string error )
	{
		Debug.Log( "restRequestFailedEvent failed: " + error );
	}
	
	
	void facebookComposerCompletedEvent( bool didSucceed )
	{
		Debug.Log( "facebookComposerCompletedEvent did succeed: " + didSucceed );
	}


	void reauthorizationSucceededEvent()
	{
		Debug.Log( "reauthorizationSucceededEvent" );
	}
	
	
	void reauthorizationFailedEvent( string error )
	{
		Debug.Log( "reauthorizationFailedEvent: " + error );
	}
	
#endif
}
