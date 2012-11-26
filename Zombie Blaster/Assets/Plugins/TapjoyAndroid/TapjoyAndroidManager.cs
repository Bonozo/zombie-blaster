using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Prime31;



public class TapjoyAndroidManager : MonoBehaviour
{
#if UNITY_ANDROID
	// Fired when a full screen ad load completes
	public static event Action<bool> fullScreenAdDidLoadEvent;
	
	// Fired when the daily reward ad either loads or fails to load
	public static event Action<bool> dailyRewardAdLoadedEvent;
	
	// Fired when the request to getTapPoints returns
	public static event Action<int> getUpdatePointsEvent;
	
	// Fired when the request to getTapPoints fails
	public static event Action<string> getUpdatePointsFailedEvent;
	
	// Fired when successfully spending tap points
	public static event Action<string> getSpendPointsResponseEvent;
	
	// Fired when spending tap points fails
	public static event Action<string> getSpendPointsResponseFailedEvent;
	
	// Fired when awarding tap points succeeds
	public static event Action<string> getAwardPointsResponseEvent;
	
	// Fired when awarding tap points fails
	public static event Action<string> getAwardPointsResponseFailedEvent;
	
	// Fired whenever tap points are earned
	public static event Action<string> earnedTapPointsEvent;
	
	// Fired when the call to getPurchasedItems completes
	public static event Action<ArrayList> purchasedItemsDidLoadEvent;
	
	// Fired when an ad banner is ready to be displayed
	public static event Action getDisplayAdResponseEvent;
	
	// Fired when an ad banner fails to load
	public static event Action<string> getDisplayAdResponseFailedEvent;
	
	// Fired when a video ad is ready
	public static event Action videoReadyEvent;
	
	// Fired when a video ad fails to load
	public static event Action<string> videoErrorEvent;
	
	// Fired when a video ad has completed playing
	public static event Action videoCompleteEvent;
	
	

	void Awake()
	{
		// Set the GameObject name to the class name for easy access from Obj-C
		gameObject.name = this.GetType().ToString();
		DontDestroyOnLoad( this );
	}


	public void fullScreenAdDidLoad( string didLoadString )
	{
		if( fullScreenAdDidLoadEvent != null )
			fullScreenAdDidLoadEvent( didLoadString == "1" );
	}
	
	
	public void dailyRewardAdLoaded( string didLoadString )
	{
		if( dailyRewardAdLoadedEvent != null )
			dailyRewardAdLoadedEvent( didLoadString == "1" );
	}


	public void getUpdatePoints( string param )
	{
		if( getUpdatePointsEvent != null )
			getUpdatePointsEvent( int.Parse( param ) );
	}


	public void getUpdatePointsFailed( string error )
	{
		if( getUpdatePointsFailedEvent != null )
			getUpdatePointsFailedEvent( error );
	}


	public void getSpendPointsResponse( string param )
	{
		if( getSpendPointsResponseEvent != null )
			getSpendPointsResponseEvent( param );
	}


	public void getSpendPointsResponseFailed( string error )
	{
		if( getSpendPointsResponseFailedEvent != null )
			getSpendPointsResponseFailedEvent( error );
	}


	public void getAwardPointsResponse( string param )
	{
		if( getAwardPointsResponseEvent != null )
			getAwardPointsResponseEvent( param );
	}


	public void getAwardPointsResponseFailed( string error )
	{
		if( getAwardPointsResponseFailedEvent != null )
			getAwardPointsResponseFailedEvent( error );
	}


	public void earnedTapPoints( string param )
	{
		if( earnedTapPointsEvent != null )
			earnedTapPointsEvent( param );
	}
	
	
	public void purchasedItemsDidLoad( string json )
	{
		if( purchasedItemsDidLoadEvent != null )
			purchasedItemsDidLoadEvent( json.arrayListFromJson() );
	}


	public void getDisplayAdResponse( string empty )
	{
		if( getDisplayAdResponseEvent != null )
			getDisplayAdResponseEvent();
	}


	public void getDisplayAdResponseFailed( string error )
	{
		if( getDisplayAdResponseFailedEvent != null )
			getDisplayAdResponseFailedEvent( error );
	}
	
	
	public void videoReady( string empty )
	{
		if( videoReadyEvent != null )
			videoReadyEvent();
	}
	
	
	public void videoError( string errorCode )
	{
		if( videoErrorEvent != null )
			videoError( errorCode );
	}
	
	
	public void videoComplete( string empty )
	{
		if( videoCompleteEvent != null )
			videoCompleteEvent();
	}
	
#endif
}

