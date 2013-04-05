using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public class AmazonIAPEventListener : MonoBehaviour
{
#if UNITY_ANDROID
	void OnEnable()
	{
		// Listen to all events for illustration purposes
		AmazonIAPManager.itemDataRequestFailedEvent += itemDataRequestFailedEvent;
		AmazonIAPManager.itemDataRequestFinishedEvent += itemDataRequestFinishedEvent;
		AmazonIAPManager.purchaseFailedEvent += purchaseFailedEvent;
		AmazonIAPManager.purchaseSuccessfulEvent += purchaseSuccessfulEvent;
		AmazonIAPManager.purchaseUpdatesRequestFailedEvent += purchaseUpdatesRequestFailedEvent;
		AmazonIAPManager.purchaseUpdatesRequestSuccessfulEvent += purchaseUpdatesRequestSuccessfulEvent;
		AmazonIAPManager.onSdkAvailableEvent += onSdkAvailableEvent;
		AmazonIAPManager.onGetUserIdResponseEvent += onGetUserIdResponseEvent;
	}


	void OnDisable()
	{
		// Remove all event handlers
		AmazonIAPManager.itemDataRequestFailedEvent -= itemDataRequestFailedEvent;
		AmazonIAPManager.itemDataRequestFinishedEvent -= itemDataRequestFinishedEvent;
		AmazonIAPManager.purchaseFailedEvent -= purchaseFailedEvent;
		AmazonIAPManager.purchaseSuccessfulEvent -= purchaseSuccessfulEvent;
		AmazonIAPManager.purchaseUpdatesRequestFailedEvent -= purchaseUpdatesRequestFailedEvent;
		AmazonIAPManager.purchaseUpdatesRequestSuccessfulEvent -= purchaseUpdatesRequestSuccessfulEvent;
		AmazonIAPManager.onSdkAvailableEvent -= onSdkAvailableEvent;
		AmazonIAPManager.onGetUserIdResponseEvent -= onGetUserIdResponseEvent;
	}



	void itemDataRequestFailedEvent()
	{
		Debug.Log( "itemDataRequestFailedEvent" );
	}


	void itemDataRequestFinishedEvent( List<string> unavailableSkus, List<AmazonItem> availableItems )
	{
		Debug.Log( "itemDataRequestFinishedEvent. unavailable skus: " + unavailableSkus.Count + ", avaiable items: " + availableItems.Count );
	}


	void purchaseFailedEvent( string reason )
	{
		Debug.Log( "purchaseFailedEvent: " + reason );
	}


	void purchaseSuccessfulEvent( AmazonReceipt receipt )
	{
		Debug.Log( "purchaseSuccessfulEvent: " + receipt );
	}


	void purchaseUpdatesRequestFailedEvent()
	{
		Debug.Log( "purchaseUpdatesRequestFailedEvent" );
	}


	void purchaseUpdatesRequestSuccessfulEvent( List<string> revokedSkus, List<AmazonReceipt> receipts )
	{
		Debug.Log( "purchaseUpdatesRequestSuccessfulEvent. revoked skus: " + revokedSkus.Count );
		foreach( var receipt in receipts )
			Debug.Log( receipt );
	}


	void onSdkAvailableEvent( bool isTestMode )
	{
		Debug.Log( "onSdkAvailableEvent. isTestMode: " + isTestMode );
	}


	void onGetUserIdResponseEvent( string userId )
	{
		Debug.Log( "onGetUserIdResponseEvent: " + userId );
	}

#endif
}


