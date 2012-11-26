using UnityEngine;
using System.Collections.Generic;


public class TapjoyUIManager : MonoBehaviour
{
#if UNITY_ANDROID
	void OnGUI()
	{
		float yPos = 5.0f;
		float xPos = 5.0f;
		float width = ( Screen.width >= 800 || Screen.height >= 800 ) ? 320 : 160;
		float height = ( Screen.width >= 800 || Screen.height >= 800 ) ? 70 : 35;
		float heightPlus = height + 10.0f;
	
	
		if( GUI.Button( new Rect( xPos, yPos, width, height ), "Init" ) )
		{
			TapjoyAndroid.init( "YOUR_INFO", "GOES_HERE", true );
		}
	
	
		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Load Ad Banner" ) )
		{
			TapjoyAndroid.getDisplayAd( TapjoyAd.Size320x50, true );
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

	}
#endif
}
