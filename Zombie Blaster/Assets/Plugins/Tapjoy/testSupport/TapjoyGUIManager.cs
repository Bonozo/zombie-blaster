using UnityEngine;
using System.Collections.Generic;
using Prime31;


public class TapjoyGUIManager : MonoBehaviourGUI
{
#if UNITY_IPHONE
	void OnGUI()
	{
		beginColumn();
		
		if( GUILayout.Button( "Init" ) )
		{
			TapjoyBinding.init( "b6b895c2-5c52-4eac-b102-4ae2a6bcaf84", "IaaQuo03VctIM6m3Qr5Z", true );
			
			// optionally set the custom userId
			//TapjoyBinding.setUserId( "CUSTOMER_USER_ID" );
		}
	
	
		if( GUILayout.Button( "Show Offers" ) )
		{
			TapjoyBinding.showOffers();
		}
	
	
		if( GUILayout.Button( "Is Full Screen Ad Ready?" ) )
		{
			Debug.Log( "is full screen ad ready? " + TapjoyBinding.isFullscreenAdReady() );
		}
	
	
		if( GUILayout.Button( "Show Full Screen Ad" ) )
		{
			TapjoyBinding.showFullScreenAd();
		}
		
		
		if( GUILayout.Button( "Get Tap Points" ) )
		{
			TapjoyBinding.getTapPoints();
		}
		
		
		if( GUILayout.Button( "Spend 2 Tap Points" ) )
		{
			TapjoyBinding.spendTapPoints( 2 );
		}
		
		
		endColumn( true );
		
		
		if( GUILayout.Button( "Create Banner" ) )
		{
			TapjoyBinding.setAdContentSize( TapjoyAdContentSize.Size320x50 );
			TapjoyBinding.createBanner( TapjoyAdPosition.BottomCenter );
		}
		
		
		if( GUILayout.Button( "Refresh Banner" ) )
		{
			TapjoyBinding.refreshBanner();
		}
		
		
		if( GUILayout.Button( "Destroy Banner" ) )
		{
			TapjoyBinding.destroyBanner();
		}

		
		if( GUILayout.Button( "Action Complete" ) )
		{
			TapjoyBinding.actionComplete( "some_action" );
		}
		
		
		if( GUILayout.Button( "Award 20 Tap Points" ) )
		{
			TapjoyBinding.awardTapPoints( 20 );
		}
		
		
		if( GUILayout.Button( "Set Transition Effect" ) )
		{
			TapjoyBinding.setTransitionEffect( 1 );
		}
		
		
		if( GUILayout.Button( "Get Daily Reward Ad" ) )
		{
			TapjoyBinding.getDailyRewardAd();
		}
		
		
		if( GUILayout.Button( "Show Daily Reward Ad" ) )
		{
			TapjoyBinding.showDailyRewardAd();
		}
		
		endColumn();
	}
#endif
}
