using UnityEngine;
using System.Collections;

public class ZBFacebook : ButtonBase {
	
	public string appID;
	
	private float messagetime = 0f;
	private string _message = "press ZB button to show/hide facebook gui";
	private string message
	{
		get
		{
			return _message;
		}
		set
		{
			_message = value;
			messagetime = 10f;
		}
	}
	
	void Awake()
	{
		FacebookManager.loginSucceededEvent += facebookLogin;
		FacebookManager.loginFailedEvent += facebookLoginFailed;
		FacebookManager.loggedOutEvent += facebookDidLogoutEvent;
		
		FacebookAndroid.init(appID);
	}
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update();
		if(base.PressedUp )
		{
			FacebookAndroid.loginWithRequestedPermissions( new string[] { "publish_stream", "email", "user_birthday" } );
		}
	}
	
	void OnGUI()
	{
		if( messagetime > 0f )
		{
			messagetime -= Time.deltaTime;
			GUI.Label(new Rect(0,0,Screen.width,Screen.height),"facebook " + message);
		}
	}
	
	void facebookLogin()
	{
		message = "Successfully logged in to Facebook";
		GUI.Label(new Rect(0,0,Screen.width,Screen.height),"facebook " + message);
		Facebook.instance.postMessage("Zombie Blaster is here.",completionHandler);
	}
	
	
	void facebookLoginFailed( string error )
	{
		message = "Facebook login failed: " + error;
	}
	
	
	void facebookDidLogoutEvent()
	{
		message = "Logged out" ;
	}

	void completionHandler( string error, object result )
	{
		if( error != null )
		{
			if( ! FacebookAndroid.isSessionValid() )
				message = "Post Message : Please Log in first";
			else
				message = "Post Message Eroor : " + error ;
		}
		else
		{
			message = "Post Message : Succeed";
		}
	}
}
