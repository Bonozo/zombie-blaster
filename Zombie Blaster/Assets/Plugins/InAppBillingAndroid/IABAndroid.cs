using UnityEngine;
using System.Collections;
using System.Collections.Generic;


#if UNITY_ANDROID
public class IABAndroid
{
	private static AndroidJavaObject _plugin;
	
		
	static IABAndroid()
	{
		if( Application.platform != RuntimePlatform.Android )
			return;

		// find the plugin instance
		using( var pluginClass = new AndroidJavaClass( "com.prime31.IABPlugin" ) )
			_plugin = pluginClass.CallStatic<AndroidJavaObject>( "instance" );
	}


	// Starts up the billing service.  This will also check to see if in app billing is supported and fire the appropriate event
	public static void init( string publicKey )
	{
		if( Application.platform != RuntimePlatform.Android )
			return;
			
		_plugin.Call( "init", publicKey );
	}
	
	
	// Sends a request to see if billing is supported on the current device
	public static void startCheckBillingAvailableRequest()
	{
		if( Application.platform != RuntimePlatform.Android )
			return;

		_plugin.Call( "startCheckBillingAvailableRequest" );
	}

	
	// Restores any purchases that might have occurred on a different device or if the user deleted then reinstalled your game
	public static void restoreTransactions()
	{
		if( Application.platform != RuntimePlatform.Android )
			return;
		
		_plugin.Call( "restoreTransactions" );
	}


	// Purchases the product with the given productId
	public static void purchaseProduct( string productId )
	{
		if( Application.platform != RuntimePlatform.Android )
			return;
		
		_plugin.Call( "purchaseProduct", productId );
	}


	// Performs a test purchase
	public static void testPurchaseProduct()
	{
		purchaseProduct( "android.test.purchased" );
	}


	// Performs a test refund
	public static void testRefundedProduct()
	{
		purchaseProduct( "android.test.refunded" );
	}


	// Stops the billing service.  Call this when you are done using in app billing for this session
	public static void stopBillingService()
	{
		if( Application.platform != RuntimePlatform.Android )
			return;
		
		_plugin.Call( "stopService" );
	}

}
#endif
