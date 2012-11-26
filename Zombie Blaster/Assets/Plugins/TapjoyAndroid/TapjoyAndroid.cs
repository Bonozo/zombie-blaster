using UnityEngine;
using System.Collections;
using System.Collections.Generic;


#if UNITY_ANDROID

public enum TapjoyAd
{
	Size320x50,
	Size640x100,
	Size768x90
}


public enum TapjoyAdPlacement
{
	TopLeft,
	TopCenter,
	TopRight,
	Centered,
	BottomLeft,
	BottomCenter,
	BottomRight
}

public class TapjoyAndroid
{
	private static AndroidJavaObject _plugin;
	
		
	static TapjoyAndroid()
	{
		if( Application.platform != RuntimePlatform.Android )
			return;

		// find the plugin instance
		using( var pluginClass = new AndroidJavaClass( "com.prime31.TapjoyPlugin" ) )
			_plugin = pluginClass.CallStatic<AndroidJavaObject>( "instance" );
	}
	

	// Initializes the Tapjoy Plugin and optionally inits video ads
	public static void init( string appId, string secretKey, bool shouldInitVideo )
	{
		if( Application.platform != RuntimePlatform.Android )
			return;
		
		_plugin.Call( "init", appId, secretKey, shouldInitVideo ? 1 : 0 );
	}

	
	// Kicks off a request for a full screen ad. This is done automatically in init for you as well.
	public static void getFullScreenAd()
	{
		if( Application.platform != RuntimePlatform.Android )
			return;
		
		_plugin.Call( "getFullScreenAd" );
	}
	
	
	// If a full screen ad is ready to be shown, this will present it full screen
	public static void showFullScreenAd()
	{
		if( Application.platform != RuntimePlatform.Android )
			return;
		
		_plugin.Call( "showFullScreenAd" );
	}
	
	
	// Kicks off a request for the daily reward ad
	public static void getDailyRewardAd()
	{
		if( Application.platform != RuntimePlatform.Android )
			return;
		
		_plugin.Call( "getDailyRewardAd" );
	}
	
	
	// If the daily reward ad is ready to be shown, this will present it full screen
	public static void showDailyRewardAd()
	{
		if( Application.platform != RuntimePlatform.Android )
			return;
		
		_plugin.Call( "showDailyRewardAd" );
	}


	// Enables the paid app action with the given actionId
	public static void enablePaidAppWithActionID( string actionId )
	{
		if( Application.platform != RuntimePlatform.Android )
			return;
		
		_plugin.Call( "enablePaidAppWithActionID", actionId );
	}
	

	// Starts loading a banner ad. showDisplayAd must be called after it loads to display it.
	public static void getDisplayAd( TapjoyAd adContentSize, bool enableAutoRefresh )
	{
		if( Application.platform != RuntimePlatform.Android )
			return;
		
		_plugin.Call( "getDisplayAd", adContentSize.ToString().Substring( 4 ), enableAutoRefresh );
	}


	// Shows a banner with the placement specified
	public static void showDisplayAd( TapjoyAdPlacement placement )
	{
		if( Application.platform != RuntimePlatform.Android )
			return;
		
		_plugin.Call( "showDisplayAd", (int)placement );
	}


	// Destroys the banner and removes it from view
	public static void destroyBanner()
	{
		if( Application.platform != RuntimePlatform.Android )
			return;
		
		_plugin.Call( "destroyBanner" );
	}


	// Sets the userId for all Tapjoy methods that utilize it
	public static void setUserId( string userId )
	{
		if( Application.platform != RuntimePlatform.Android )
			return;
		
		_plugin.Call( "setUserId", userId );
	}


	// Shows the offers screen
	public static void showOffers()
	{
		if( Application.platform != RuntimePlatform.Android )
			return;
		
		_plugin.Call( "showOffers" );
	}


	// Gets the available tap points for the current user
	public static void getTapPoints()
	{
		if( Application.platform != RuntimePlatform.Android )
			return;
		
		_plugin.Call( "getTapPoints" );
	}


	// Updates the virtual currency for the user with the given spent amount of currency.
	public static void spendTapPoints( int points )
	{
		if( Application.platform != RuntimePlatform.Android )
			return;
		
		_plugin.Call( "spendTapPoints", points );
	}


	// Awards tap points to the current user
	public static void awardTapPoints( int points )
	{
		if( Application.platform != RuntimePlatform.Android )
			return;
		
		_plugin.Call( "awardTapPoints", points );
	}


	// Call this when a user completes an in-game action
	public static void actionComplete( string action )
	{
		if( Application.platform != RuntimePlatform.Android )
			return;
		
		_plugin.Call( "actionComplete", action );
	}


	// If there is a concern about bandwidth usage or storage space on the device, you can set the number of maximum cached videos with this method
	public static void setVideoCacheCount( int count )
	{
		if( Application.platform != RuntimePlatform.Android )
			return;
		
		_plugin.Call( "setVideoCacheCount", count );
	}
}
#endif
