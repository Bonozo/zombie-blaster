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
			#if UNITY_ANDROID
			TapjoyAndroid.init( "6f8b509b-f292-4dd3-b440-eab33f211089", "7TYeZbZ6GTqRncoALV3W", true );
			#endif
			
		}
	
	
		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Load Ad Banner" ) )
		{
			#if UNITY_ANDROID
			TapjoyAndroid.getDisplayAd( TapjoyAd.Size640x100, true );
			#endif
			
		}
	
	
		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Show Ad Banner" ) )
		{
			#if UNITY_ANDROID
			TapjoyAndroid.showDisplayAd( TapjoyAdPlacement.BottomCenter );
			#endif
			
		}
	
	
		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Destroy Banner" ) )
		{
			#if UNITY_ANDROID
			TapjoyAndroid.destroyBanner();
			#endif
			
		}
		
		
		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Action Complete" ) )
		{
			#if UNITY_ANDROID
			TapjoyAndroid.actionComplete( "some_action" );
			#endif
			
		}
		
		
		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Get Daily Reward Ad" ) )
		{
			#if UNITY_ANDROID
			TapjoyAndroid.getDailyRewardAd();
			#endif
			
		}
		
		
		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Show Daily Reward Ad" ) )
		{
			#if UNITY_ANDROID
			TapjoyAndroid.showDailyRewardAd();
			#endif
			
		}




		xPos = Screen.width - width - 5.0f;
		yPos = 5.0f;
		
		if( GUI.Button( new Rect( xPos, yPos, width, height ), "Show Full Screen Ad" ) )
		{
			#if UNITY_ANDROID
			TapjoyAndroid.showFullScreenAd();
			#endif
			
		}
		
		
		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Show Offers" ) )
		{
			#if UNITY_ANDROID
			TapjoyAndroid.showOffers();
			#endif
			
		}
		
		
		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Get Tap Points" ) )
		{
			#if UNITY_ANDROID
			TapjoyAndroid.getTapPoints();
			#endif
			
		}
		
		
		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Spend Tap Points" ) )
		{
			#if UNITY_ANDROID
			TapjoyAndroid.spendTapPoints( 11 );
			#endif
		}
		
		
		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Set Video Cache Count" ) )
		{
			#if UNITY_ANDROID
			TapjoyAndroid.setVideoCacheCount( 4 );
			#endif
		}
		
		GUI.Label(new Rect(0f,0.9f*Screen.height,Screen.width,0.1f*Screen.height),"tapjoy : " + message);
	}

	void OnEnable()
	{
		// Listen to all events for illustration purposes
		#if UNITY_ANDROID
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
		#endif
		
	}


	void OnDisable()
	{
		// Remove all event handlers
		#if UNITY_ANDROID
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
		#endif
		#if UNITY_ANDROID
		
		#endif
		
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
