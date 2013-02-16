using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System;

public class MainScene : MonoBehaviour 
{
	string tapPointsLabel = "";
	bool autoRefresh = false;
	
	void Start ()
	{
#if UNITY_ANDROID
		// Attach our thread to the java vm; obviously the main thread is already attached but this is good practice..
		if (Application.platform == RuntimePlatform.Android)
			UnityEngine.AndroidJNI.AttachCurrentThread();
#endif
		// Enable logging.
		TapjoyPlugin.EnableLogging(true);
		
		// Set the Handler class. This needs to be a unity GameObject
		TapjoyPlugin.SetCallbackHandler("MainScene");											// Set this to the name of your linked GameObject 
		
		// Connect to the Tapjoy servers.
		if (Application.platform == RuntimePlatform.Android)
		{
			TapjoyPlugin.RequestTapjoyConnect(	"bba49f11-b87f-4c0f-9632-21aa810dd6f1", 				// YOUR APP ID GOES HERE
		    	          						"yiQIURFEeKm0zbOggubu");								// YOUR SECRET KEY GOES HERE
		}
		else if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			TapjoyPlugin.RequestTapjoyConnect(	"93e78102-cbd7-4ebf-85cc-315ba83ef2d5", 				// YOUR APP ID GOES HERE
		    	          						"JWxgS26URM0XotaghqGn");								// YOUR SECRET KEY GOES HERE
		}
		
		// Get a banner ad
		TapjoyPlugin.GetDisplayAd();
	}

	
	#region Tapjoy Callback Methods (These must be implemented in your own c# script file.)
	
	// CONNECT
	public void TapjoyConnectSuccess(string message)
	{
		print(message);
	}
	
	public void TapjoyConnectFail(string message)
	{
		print(message);
	}
	
	// VIRTUAL CURRENCY
	public void TapPointsLoaded(String message)
	{
		print("TapPointsLoaded: " + message);
		tapPointsLabel = "Total TapPoints: " + TapjoyPlugin.QueryTapPoints();
	}
	
	public void TapPointsLoadedError(String message)
	{
		print("TapPointsLoadedError: " + message);
		tapPointsLabel = "TapPointsLoadedError: " + message;
	}
	
	public void TapPointsSpent(string message)
	{
		print("TapPointsSpent: " + message);
		tapPointsLabel = "Total TapPoints: " + TapjoyPlugin.QueryTapPoints();
	}

	public void TapPointsSpendError(string message)
	{
		print("TapPointsSpendError: " + message);
		tapPointsLabel = "TapPointsSpendError: " + message;
	}

	public void TapPointsAwarded(string message)
	{
		print("TapPointsAwarded: " + message);
		tapPointsLabel = "Total TapPoints: " + TapjoyPlugin.QueryTapPoints();
	}

	public void TapPointsAwardError(string message)
	{
		print("TapPointsAwardError: " + message);
		tapPointsLabel = "TapPointsAwardError: " + message;
	}
	
	public void CurrencyEarned(string message)
	{
		print("CurrencyEarned: " + message);
		tapPointsLabel = "Currency Earned: " + message;
		
		TapjoyPlugin.ShowDefaultEarnedCurrencyAlert();
	}
	
	// FULL SCREEN ADS
	public void FullScreenAdLoaded(string message)
	{
		print("FullScreenAdLoaded: " + message);
		tapPointsLabel = "FullScreenAdLoaded: " + message;
		
		TapjoyPlugin.ShowFullScreenAd();
	}
	
	public void FullScreenAdError(string message)
	{
		print("FullScreenAdError: " + message);
		tapPointsLabel = "FullScreenAdError: " + message;
	}
	
	// DAILY REWARD ADS
	public void DailyRewardAdLoaded(string message)
	{
		print("DailyRewardAd: " + message);
		tapPointsLabel = "DailyRewardAd: " + message;
		
		TapjoyPlugin.ShowDailyRewardAd();
	}
	
	public void DailyRewardAdError(string message)
	{
		print("DailyRewardAd: " + message);
		tapPointsLabel = "DailyRewardAd: " + message;
	}
	
	// DISPLAY ADS
	public void DisplayAdLoaded(string message)
	{
		print("DisplayAdLoaded: " + message);
		tapPointsLabel = "DisplayAdLoaded: " + message;
		
		TapjoyPlugin.ShowDisplayAd();
	}
	
	public void DisplayAdError(string message)
	{
		print("DisplayAdError: " + message);
		tapPointsLabel = "DisplayAdError: " + message;
	}
	
	// VIDEO
	public void VideoAdStart(string message)
	{
		print("VideoAdStart: " + message);
		tapPointsLabel = "VideoAdStart: " + message;
	}
	
	public void VideoAdError(string message)
	{
		print("VideoAdError: " + message);
		tapPointsLabel = "VideoAdError: " + message;
	}
	
	public void VideoAdComplete(string message)
	{
		print("VideoAdComplete: " + message);
		tapPointsLabel = "VideoAdComplete: " + message;
	}
	
	#endregion
	
	#region GUI for sample app
	
	public void ResetTapPointsLabel()
	{
		tapPointsLabel = "Updating Tap Points...";
	}
	
	void OnGUI()
	{
		GUIStyle labelStyle = new GUIStyle();
		labelStyle.alignment = TextAnchor.MiddleCenter;
		labelStyle.normal.textColor = Color.white;
		
		float centerx = Screen.width / 2;
		//float centery = Screen.height / 2;
		float spaceSize = 60;
		float buttonWidth = 300;
		float buttonHeight = 50;
		float fontSize = 20;
		float spacer = 100;
		
		// Quit app on BACK key.
		if (Input.GetKeyDown(KeyCode.Escape)) { Application.Quit(); }
		
		GUI.Label(new Rect(centerx - 200, spacer, 400, 25), "Tapjoy Connect Sample App", labelStyle);
		
		spacer += fontSize + 10;
		
		if (GUI.Button(new Rect(centerx - (buttonWidth / 2), spacer, buttonWidth, buttonHeight), "Show Offers"))
		{
			TapjoyPlugin.ShowOffers();
		}
		
		spacer += spaceSize;
		
		if (GUI.Button(new Rect(centerx - (buttonWidth / 2), spacer, buttonWidth, buttonHeight), "Show Daily Reward Ad"))
		{
			TapjoyPlugin.GetDailyRewardAd();
		}
		
		spacer += spaceSize;
		
		if (GUI.Button(new Rect(centerx - (buttonWidth / 2), spacer, buttonWidth, buttonHeight), "Get Display Ad"))
		{
			TapjoyPlugin.GetDisplayAd();
		}
		
		spacer += spaceSize;
		
		if (GUI.Button(new Rect(centerx - (buttonWidth / 2), spacer, buttonWidth, buttonHeight), "Hide Display Ad"))
		{
			TapjoyPlugin.HideDisplayAd();
		}
		
		spacer += spaceSize;
		
		if (GUI.Button(new Rect(centerx - (buttonWidth / 2), spacer, buttonWidth, buttonHeight), "Toggle Display Ad Auto-Refresh"))
		{
			autoRefresh = !autoRefresh;
			TapjoyPlugin.EnableDisplayAdAutoRefresh(autoRefresh);
		}
		
		spacer += spaceSize;
		
		if (GUI.Button(new Rect(centerx - (buttonWidth / 2), spacer, buttonWidth, buttonHeight), "Show FullScreen Ad"))
		{
			TapjoyPlugin.GetFullScreenAd();
		}
		
		spacer += spaceSize;
		
		if (GUI.Button(new Rect(centerx - (buttonWidth / 2), spacer, buttonWidth, buttonHeight), "Get Tap Points"))
		{
			TapjoyPlugin.GetTapPoints();
			ResetTapPointsLabel();
		}
		
		spacer += spaceSize;
		
		if (GUI.Button(new Rect(centerx - (buttonWidth / 2), spacer, buttonWidth, buttonHeight), "Spend Tap Points"))
		{
			TapjoyPlugin.SpendTapPoints(10);
			ResetTapPointsLabel();
		}
		
		spacer += spaceSize;
		
		if (GUI.Button(new Rect(centerx - (buttonWidth / 2), spacer, buttonWidth, buttonHeight), "Award Tap Points"))
		{
			TapjoyPlugin.AwardTapPoints(10);
			ResetTapPointsLabel();
		}
		
		spacer += fontSize;
		
		// Display status
		GUI.Label(new Rect(centerx - 200, spacer, 400, 150), tapPointsLabel, labelStyle);
	}
	
	#endregion
}