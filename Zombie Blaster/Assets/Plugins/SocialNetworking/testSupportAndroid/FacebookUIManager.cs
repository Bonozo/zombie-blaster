using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// trick when using both the iOS and Android version of the plugin in the same project. Add this block to the
// top of the file you are calling the Facebook methods from so they can share code
/*
#if UNITY_ANDROID
using FacebookAccess = FacebookAndroid;
#elif UNITY_IPHONE
using FacebookAccess = FacebookBinding;
#endif
*/


public class FacebookUIManager : MonoBehaviour
{
#if UNITY_ANDROID
	public static string screenshotFilename = "someScreenshot.png";
	
	
	
	// common event handler used for all Facebook graph requests that logs the data to the console
	void completionHandler( string error, object result )
	{
		if( error != null )
			Debug.LogError( error );
		else
			Prime31.Utils.logObject( result );
	}
	
	
	void Start()
	{
		// grab a screenshot for later use
		Application.CaptureScreenshot( screenshotFilename );
		
		// optionally enable logging of all requests that go through the Facebook class
		//Facebook.instance.debugRequests = true;
	}
	
	
	void OnGUI()
	{
		float yPos = 5.0f;
		float xPos = 5.0f;
		float width = ( Screen.width >= 800 || Screen.height >= 800 ) ? 320 : 160;
		float height = ( Screen.width >= 800 || Screen.height >= 800 ) ? 70 : 30;
		float heightPlus = height + 10.0f;


		if( GUI.Button( new Rect( xPos, yPos, width, height ), "Initialize Facebook" ) )
		{
			FacebookAndroid.init( "MY_ID_HERE" );
		}
	
	
		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Login" ) )
		{
			FacebookAndroid.loginWithRequestedPermissions( new string[] { "publish_stream", "email", "user_birthday" } );
		}
	
	
		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Login with SSO" ) )
		{
			FacebookAndroid.loginWithSingleSignOn( new string[] { "publish_stream", "email", "user_birthday" } );
		}
	
	
		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Get Session Token" ) )
		{
			var token = FacebookAndroid.getAccessToken();
			Debug.Log( "session token: " + token );
		}
	
	
		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Is Session Valid?" ) )
		{
			var isSessionValid = FacebookAndroid.isSessionValid();
			Debug.Log( "Is session valid?: " + isSessionValid );
		}


		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Graph Request" ) )
		{
			Facebook.instance.graphRequest( "me", completionHandler );
		}
		
		
		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Twitter Scene" ) )
		{
			Application.LoadLevel( "TwitterTestScene" );
		}

	
		xPos = Screen.width - width - 5.0f;
		yPos = 5.0f;
		
		if( GUI.Button( new Rect( xPos, yPos, width, height ), "Logout" ) )
		{
			FacebookAndroid.logout();
		}
		
		
		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Show Post Dialog" ) )
		{
			FacebookAndroid.showPostMessageDialog();
		}


		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Show Post Dialog++" ) )
		{
			FacebookAndroid.showPostMessageDialogWithOptions( "http://prime31.com", "prime31 studios", "http://prime31.com/assets/images/banners/tweetsBannerLogo.png", "image caption here" );
		}


		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Post Image" ) )
		{
			var pathToImage = Application.persistentDataPath + "/" + screenshotFilename;
			var bytes = System.IO.File.ReadAllBytes( pathToImage );
			
			Facebook.instance.postImage( bytes, "im an image posted from Android", completionHandler );
		}


		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Custom Dialog" ) )
		{
			var parameters = new Dictionary<string,string>();
			parameters.Add( "message", "check out my cool app" );
			FacebookAndroid.showDialog( "apprequests", parameters );
		}


		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Rest Request" ) )
		{
			var parameters = new Dictionary<string,string>();
			parameters.Add( "query", "SELECT uid,name FROM user WHERE uid=4" );
			FacebookAndroid.restRequest( "fql.query", "POST", parameters );
		}
	}
#endif
}
