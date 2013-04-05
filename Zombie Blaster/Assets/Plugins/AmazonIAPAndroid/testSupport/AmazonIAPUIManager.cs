using UnityEngine;
using System.Collections.Generic;


public class AmazonIAPUIManager : MonoBehaviour
{
#if UNITY_ANDROID
	void OnGUI()
	{
		float yPos = 5.0f;
		float xPos = 5.0f;
		float width = ( Screen.width >= 800 || Screen.height >= 800 ) ? 320 : 160;
		float height = ( Screen.width >= 800 || Screen.height >= 800 ) ? 80 : 40;
		float heightPlus = height + 10.0f;
	
	
		if( GUI.Button( new Rect( xPos, yPos, width, height ), "Initiate Item Data Request" ) )
		{
			AmazonIAP.initiateItemDataRequest( new string[] { "coinpack.tier.2", "coinpack.tier.5", "coinpack.tier.10" } );
		}
	
	
		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Initiate Purchase Request" ) )
		{
			AmazonIAP.initiatePurchaseRequest( "coinpack.tier.2" );
		}
	
	
		if( GUI.Button( new Rect( xPos, yPos += heightPlus, width, height ), "Initiate Get User ID Request" ) )
		{
			AmazonIAP.initiateGetUserIdRequest();
		}
	}
#endif
}
