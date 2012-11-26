using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public class TapjoyAndroidEventListener : MonoBehaviour
{
#if UNITY_ANDROID
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
		Debug.Log( "fullScreenAdDidLoadEvent. didLoad: " + didLoad );
	}
	
	
	void featuredAppFailedToLoadEvent()
	{
		Debug.Log( "featuredAppFailedToLoadEvent" );
	}
	
	
	void dailyRewardAdLoadedEvent( bool didLoad )
	{
		Debug.Log( "dailyRewardAdLoadedEvent. didLoad: " + didLoad );
	}


	void getUpdatePointsEvent( int points )
	{
		Debug.Log( "getUpdatePointsEvent: " + points );
	}


	void getUpdatePointsFailedEvent( string error )
	{
		Debug.Log( "getUpdatePointsFailedEvent: " + error );
	}


	void getSpendPointsResponseEvent( string param )
	{
		Debug.Log( "getSpendPointsResponseEvent: " + param );
	}


	void getSpendPointsResponseFailedEvent( string error )
	{
		Debug.Log( "getSpendPointsResponseFailedEvent: " + error );
	}


	void getAwardPointsResponseEvent( string param )
	{
		Debug.Log( "getAwardPointsResponseEvent: " + param );
	}


	void getAwardPointsResponseFailedEvent( string error )
	{
		Debug.Log( "getAwardPointsResponseFailedEvent: " + error );
	}


	void earnedTapPointsEvent( string param )
	{
		Debug.Log( "earnedTapPointsEvent: " + param );
	}
	
	
	void purchasedItemsDidLoadEvent( ArrayList items )
	{
		Debug.Log( "purchasedItemsDidLoad. total items: " + items.Count );
	}


	void getDisplayAdResponseEvent()
	{
		Debug.Log( "getDisplayAdResponseEvent" );
	}


	void getDisplayAdResponseFailedEvent( string error )
	{
		Debug.Log( "getDisplayAdResponseFailedEvent: " + error );
	}
	
	
	void videoReadyEvent()
	{
		Debug.Log( "videoReadyEvent" );
	}
	
	
	void videoErrorEvent( string errorCode )
	{
		Debug.Log( "videoErrorEvent. errorCode: " + errorCode );
	}
	
	
	void videoCompleteEvent()
	{
		Debug.Log( "videoCompleteEvent" );
	}	
#endif
}


