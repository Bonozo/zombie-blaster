using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public class IABAndroidEventListener : MonoBehaviour
{
#if UNITY_ANDROID
	void OnEnable()
	{
		// Listen to all events for illustration purposes
		IABAndroidManager.billingSupportedEvent += billingSupportedEvent;
		IABAndroidManager.purchaseSignatureVerifiedEvent += purchaseSignatureVerifiedEvent;
		IABAndroidManager.purchaseSucceededEvent += purchaseSucceededEvent;
		IABAndroidManager.purchaseCancelledEvent += purchaseCancelledEvent;
		IABAndroidManager.purchaseRefundedEvent += purchaseRefundedEvent;
		IABAndroidManager.purchaseFailedEvent += purchaseFailedEvent;
		IABAndroidManager.transactionsRestoredEvent += transactionsRestoredEvent;
		IABAndroidManager.transactionRestoreFailedEvent += transactionRestoreFailedEvent;
	}


	void OnDisable()
	{
		// Remove all event handlers
		IABAndroidManager.billingSupportedEvent -= billingSupportedEvent;
		IABAndroidManager.purchaseSignatureVerifiedEvent -= purchaseSignatureVerifiedEvent;
		IABAndroidManager.purchaseSucceededEvent -= purchaseSucceededEvent;
		IABAndroidManager.purchaseCancelledEvent -= purchaseCancelledEvent;
		IABAndroidManager.purchaseRefundedEvent -= purchaseRefundedEvent;
		IABAndroidManager.purchaseFailedEvent -= purchaseFailedEvent;
		IABAndroidManager.transactionsRestoredEvent -= transactionsRestoredEvent;
		IABAndroidManager.transactionRestoreFailedEvent -= transactionRestoreFailedEvent;
	}



	void billingSupportedEvent( bool isSupported )
	{
		Debug.Log( "billingSupportedEvent: " + isSupported );
	}
	
	
	void purchaseSignatureVerifiedEvent( string signedData, string signature )
	{
		Debug.Log( "purchaseSignatureVerifiedEvent. signedData: " + signedData + ", signature: " + signature );
	}


	void purchaseSucceededEvent( string productId )
	{
		Debug.Log( "purchaseSucceededEvent: " + productId );
	}


	void purchaseCancelledEvent( string productId )
	{
		Debug.Log( "purchaseCancelledEvent: " + productId );
	}


	void purchaseRefundedEvent( string productId )
	{
		Debug.Log( "purchaseRefundedEvent: " + productId );
	}


	void purchaseFailedEvent( string productId )
	{
		Debug.Log( "purchaseFailedEvent: " + productId );
	}


	void transactionsRestoredEvent()
	{
		Debug.Log( "transactionsRestoredEvent" );
	}


	void transactionRestoreFailedEvent( string error )
	{
		Debug.Log( "transactionRestoreFailedEvent: " + error );
	}
#endif
}


