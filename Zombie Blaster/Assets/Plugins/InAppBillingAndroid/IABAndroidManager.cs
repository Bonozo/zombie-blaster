using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public class IABAndroidManager : MonoBehaviour
{
#if UNITY_ANDROID
	// Fired when the call to startCheckBillingAvailableRequest returns
	public static event Action<bool> billingSupportedEvent;
	
	// Fired when a product is successfully purchased
	public static event Action<string> purchaseSucceededEvent;
	
	// Fired when a product purchase is verfied on the device. The parameters are the signedData and the signature
	public static event Action<string,string> purchaseSignatureVerifiedEvent;
	
	// Fired when a purchase is cancelled
	public static event Action<string> purchaseCancelledEvent;
	
	// Fired when a purchase has been refunded
	public static event Action<string> purchaseRefundedEvent;
	
	// Fired when a purchase fails
	public static event Action<string> purchaseFailedEvent;
	
	// Fired when the restoreTransactions call completes
	public static event Action transactionsRestoredEvent;
	
	// Fired when the restoreTransactions call fails
	public static event Action<string> transactionRestoreFailedEvent;


	void Awake()
	{
		// Set the GameObject name to the class name for easy access from Obj-C
		gameObject.name = this.GetType().ToString();
		DontDestroyOnLoad( this );
	}


	public void billingSupported( string isSupported )
	{
		if( billingSupportedEvent != null )
			billingSupportedEvent( isSupported == "1" );
	}
	
	
	public void purchaseSignatureVerified( string data )
	{
		if( purchaseSignatureVerifiedEvent != null )
		{
			var parts = data.Split( new string[] { "~~~" }, StringSplitOptions.RemoveEmptyEntries );
			if( parts.Length == 2 )
				purchaseSignatureVerifiedEvent( parts[0], parts[1] );
		}
	}


	public void purchaseSucceeded( string productId )
	{
		if( purchaseSucceededEvent != null )
			purchaseSucceededEvent( productId );
	}


	public void purchaseCancelled( string productId )
	{
		if( purchaseCancelledEvent != null )
			purchaseCancelledEvent( productId );
	}


	public void purchaseRefunded( string productId )
	{
		if( purchaseRefundedEvent != null )
			purchaseRefundedEvent( productId );
	}


	public void purchaseFailed( string productId )
	{
		if( purchaseFailedEvent != null )
			purchaseFailedEvent( productId );
	}


	public void transactionsRestored( string empty )
	{
		if( transactionsRestoredEvent != null )
			transactionsRestoredEvent();
	}


	public void transactionRestoreFailed( string error )
	{
		if( transactionRestoreFailedEvent != null )
			transactionRestoreFailedEvent( error );
	}
#endif
}

