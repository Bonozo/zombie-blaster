using UnityEngine;
using System.Collections.Generic;


public class TwitterUIManager : MonoBehaviour
{
#if UNITY_ANDROID
	void OnGUI()
	{
		float yPos = 5.0f;
		float xPos = 5.0f;
		float width = ( Screen.width >= 800 || Screen.height >= 800 ) ? 320 : 160;
		float height = ( Screen.width >= 800 || Screen.height >= 800 ) ? 70 : 30;
		float heightPlus = height + 10.0f;
	
	
		if( GUI.Button( new Rect( xPos, yPos, width, height ), "Initialize Twitter" ) )
		{
			// Replace these with your own credentials!!!
			TwitterAndroid.init( "INSERT_YOUR_INFO_HERE", "INSERT_YOUR_INFO_HERE" );
		}
	
	
		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Login" ) )
		{
			TwitterAndroid.showLoginDialog();
		}

	
		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Is Logged In?" ) )
		{
			var isLoggedIn = TwitterAndroid.isLoggedIn();
			Debug.Log( "Is logged in?: " + isLoggedIn );
		}

		
		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Post Update with Image" ) )
		{
			var pathToImage = Application.persistentDataPath + "/" + FacebookUIManager.screenshotFilename;
			var bytes = System.IO.File.ReadAllBytes( pathToImage );
			
			TwitterAndroid.postUpdateWithImage( "test update from Unity!", bytes );
		}
		
		
		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Facebook Scene" ) )
		{
			Application.LoadLevel( "FacebookTestScene" );
		}

	
		xPos = Screen.width - width - 5.0f;
		yPos = 5.0f;
		
		if( GUI.Button( new Rect( xPos, yPos, width, height ), "Logout" ) )
		{
			TwitterAndroid.logout();
		}
		
		
		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Post Update" ) )
		{
			TwitterAndroid.postUpdate( "im an update from the Twitter Android Plugin" );
		}


		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Get Home Timeline" ) )
		{
			TwitterAndroid.getHomeTimeline();
		}


		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Get Followers" ) )
		{
			TwitterAndroid.getFollowers();
		}


		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Custom Request" ) )
		{
			var dict = new Dictionary<string, string>();
			dict.Add( "screen_name", "prime_31" );
			dict.Add( "test", "paramters" );
			dict.Add( "test2", "asdf" );
			dict.Add( "test3", "wer" );
			dict.Add( "test4", "vbn" );
			
			TwitterAndroid.performRequest( "get", "/1/users/show.json", dict );
		}

	}
#endif
}
