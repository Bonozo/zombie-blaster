using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Prime31;

public class ZombieBlasterFacebook : MonoBehaviourGUI {

	public GameObject screenshotMessage;
	public static ZombieBlasterFacebook Instance;
	
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
		#elif UNITY_IPHONE
		FacebookBinding.init();
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
		Ready = true;
		Logging = false;
	}
	
	void facebookLoginFailed( string error )
	{
		Debug.Log("Facebook login failed: " + error);
		Logging = false;
	}
	
	void facebookDidLogoutEvent()
	{
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
		{
			
		}
		else
		{
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
	
	#if UNITY_IPHONE || UNITY_ANDROID
	// Listens to all the events.  All event listeners MUST be removed before this object is disposed!
	void OnEnable()
	{
		FacebookManager.sessionOpenedEvent += sessionOpenedEvent;
		FacebookManager.loginFailedEvent += loginFailedEvent;
		
		FacebookManager.dialogCompletedWithUrlEvent += dialogCompletedEvent;
		FacebookManager.dialogFailedEvent += dialogFailedEvent;
		//FacebookManager.dialogCompletedEvent += facebokDialogCompleted;
		//FacebookManager.dialogDidNotCompleteEvent += dialogDidNotCompleteEvent;
		
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

		//FacebookManager.dialogCompletedEvent -= facebokDialogCompleted;
		FacebookManager.dialogCompletedWithUrlEvent -= dialogCompletedEvent;
		//FacebookManager.dialogDidNotCompleteEvent -= dialogDidNotCompleteEvent;
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
