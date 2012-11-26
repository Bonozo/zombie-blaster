using UnityEngine;
using System.Collections;

public class ZBTapjoy : MonoBehaviour {
	
	private string message = "";
	
	// Use this for initialization
	void Start () {
		message = SystemInfo.deviceUniqueIdentifier;
	}
	
	// Update is called once per frame
	void Update () {
		if( Input.GetKey(KeyCode.Escape) )
			Application.Quit();
	}
	
	void OnGUI()
	{
		float yPos = 5.0f;
		float xPos = 5.0f;
		float width = /*( Screen.width >= 800 || Screen.height >= 800 ) ? 320 :*/ 160;
		float height = /*( Screen.width >= 800 || Screen.height >= 800 ) ? 70 :*/ 35;
		float heightPlus = height + 10.0f;
	
	
		if( GUI.Button( new Rect( xPos, yPos, width, height ), "Init" ) )
		{
			TapjoyAndroid.init( "6f8b509b-f292-4dd3-b440-eab33f211089", "7TYeZbZ6GTqRncoALV3W", true );
		}
	
	
		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Load Ad Banner" ) )
		{
			TapjoyAndroid.getDisplayAd( TapjoyAd.Size640x100, true );
		}
	
	
		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Show Ad Banner" ) )
		{
			TapjoyAndroid.showDisplayAd( TapjoyAdPlacement.BottomCenter );
		}
	
	
		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Destroy Banner" ) )
		{
			TapjoyAndroid.destroyBanner();
		}
		
		
		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Action Complete" ) )
		{
			TapjoyAndroid.actionComplete( "some_action" );
		}
		
		
		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Get Daily Reward Ad" ) )
		{
			TapjoyAndroid.getDailyRewardAd();
		}
		
		
		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Show Daily Reward Ad" ) )
		{
			TapjoyAndroid.showDailyRewardAd();
		}




		xPos = Screen.width - width - 5.0f;
		yPos = 5.0f;
		
		if( GUI.Button( new Rect( xPos, yPos, width, height ), "Show Full Screen Ad" ) )
		{
			TapjoyAndroid.showFullScreenAd();
		}
		
		
		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Show Offers" ) )
		{
			TapjoyAndroid.showOffers();
		}
		
		
		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Get Tap Points" ) )
		{
			TapjoyAndroid.getTapPoints();
		}
		
		
		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Spend Tap Points" ) )
		{
			TapjoyAndroid.spendTapPoints( 11 );
		}
		
		
		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Set Video Cache Count" ) )
		{
			TapjoyAndroid.setVideoCacheCount( 4 );
		}
		
		GUI.Label(new Rect(0f,0.9f*Screen.height,Screen.width,0.1f*Screen.height),"tapjoy : " + message);
	}

	void OnEnable()
	{
		// Listen to all events for illustration purposes
		TapjoyAndroidManager.fullScreenAdDidLoadEvent += fullScreenAdDidLoadEvent;
		TapjoyAndroidManager.dailyRewardAdLoadedEvent += dailyRewardAdLoadedEvent;
		TapjoyAndroidManager.getUpdatePointsEvent += getUpdatePointsEvent;
		TapjoyAndroidManager.getUpdatePointsFailedEvent += getUpdatePointsFailedEvent;
		TapjoyAndroidManager.getSpendPointsResponseEvent += getSpendPointsResponseEvent;
		TapjoyAndroidManager.getSpendPointsResponseFailedEvent += getSpendPointsResponseFailedEvent;
		TapjoyAndroidManager.getAwardPointsResponseEvent += getAwardPointsResponseEvent;
		TapjoyAndroidManager.getAwardPointsResponseFailedEvent += getAwardPointsResponseFailedEvent;
		TapjoyAndroidManager.earnedTapPointsEvent += earnedTapPointsEvent;
		TapjoyAndroidManager.purchasedItemsDidLoadEvent += purchasedItemsDidLoadEvent;
		TapjoyAndroidManager.getDisplayAdResponseEvent += getDisplayAdResponseEvent;
		TapjoyAndroidManager.getDisplayAdResponseFailedEvent += getDisplayAdResponseFailedEvent;
		TapjoyAndroidManager.videoReadyEvent += videoReadyEvent;
		TapjoyAndroidManager.videoErrorEvent += videoErrorEvent;
		TapjoyAndroidManager.videoCompleteEvent += videoCompleteEvent;
	}


	void OnDisable()
	{
		// Remove all event handlers
		TapjoyAndroidManager.fullScreenAdDidLoadEvent -= fullScreenAdDidLoadEvent;
		TapjoyAndroidManager.dailyRewardAdLoadedEvent -= dailyRewardAdLoadedEvent;
		TapjoyAndroidManager.getUpdatePointsEvent -= getUpdatePointsEvent;
		TapjoyAndroidManager.getUpdatePointsFailedEvent -= getUpdatePointsFailedEvent;
		TapjoyAndroidManager.getSpendPointsResponseEvent -= getSpendPointsResponseEvent;
		TapjoyAndroidManager.getSpendPointsResponseFailedEvent -= getSpendPointsResponseFailedEvent;
		TapjoyAndroidManager.getAwardPointsResponseEvent -= getAwardPointsResponseEvent;
		TapjoyAndroidManager.getAwardPointsResponseFailedEvent -= getAwardPointsResponseFailedEvent;
		TapjoyAndroidManager.earnedTapPointsEvent -= earnedTapPointsEvent;
		TapjoyAndroidManager.purchasedItemsDidLoadEvent -= purchasedItemsDidLoadEvent;
		TapjoyAndroidManager.getDisplayAdResponseEvent -= getDisplayAdResponseEvent;
		TapjoyAndroidManager.getDisplayAdResponseFailedEvent -= getDisplayAdResponseFailedEvent;
		TapjoyAndroidManager.videoReadyEvent -= videoReadyEvent;
		TapjoyAndroidManager.videoErrorEvent -= videoErrorEvent;
		TapjoyAndroidManager.videoCompleteEvent -= videoCompleteEvent;
	}



	void fullScreenAdDidLoadEvent( bool didLoad )
	{
		message =  "fullScreenAdDidLoadEvent. didLoad: " + didLoad ;
	}
	
	
	void featuredAppFailedToLoadEvent()
	{
		message =  "featuredAppFailedToLoadEvent" ;
	}
	
	
	void dailyRewardAdLoadedEvent( bool didLoad )
	{
		message =  "dailyRewardAdLoadedEvent. didLoad: " + didLoad ;
	}


	void getUpdatePointsEvent( int points )
	{
		message =  "getUpdatePointsEvent: " + points ;
	}


	void getUpdatePointsFailedEvent( string error )
	{
		message =  "getUpdatePointsFailedEvent: " + error ;
	}


	void getSpendPointsResponseEvent( string param )
	{
		message =  "getSpendPointsResponseEvent: " + param ;
	}


	void getSpendPointsResponseFailedEvent( string error )
	{
		message =  "getSpendPointsResponseFailedEvent: " + error ;
	}


	void getAwardPointsResponseEvent( string param )
	{
		message =  "getAwardPointsResponseEvent: " + param ;
	}


	void getAwardPointsResponseFailedEvent( string error )
	{
		message =  "getAwardPointsResponseFailedEvent: " + error ;
	}


	void earnedTapPointsEvent( string param )
	{
		message =  "earnedTapPointsEvent: " + param ;
	}
	
	
	void purchasedItemsDidLoadEvent( ArrayList items )
	{
		message =  "purchasedItemsDidLoad. total items: " + items.Count ;
	}


	void getDisplayAdResponseEvent()
	{
		message =  "getDisplayAdResponseEvent" ;
	}


	void getDisplayAdResponseFailedEvent( string error )
	{
		message =  "getDisplayAdResponseFailedEvent: " + error ;
	}
	
	
	void videoReadyEvent()
	{
		message =  "videoReadyEvent" ;
	}
	
	
	void videoErrorEvent( string errorCode )
	{
		message =  "videoErrorEvent. errorCode: " + errorCode ;
	}
	
	
	void videoCompleteEvent()
	{
		message =  "videoCompleteEvent" ;
	}	
	
}
