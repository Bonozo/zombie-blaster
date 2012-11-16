using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

/*
 * This file is a part of the RateMe Unity Plugin
 * 
 * Copyright ioPixel 2012
 */

public class AndroidRateMe {
#if UNITY_ANDROID
	static AndroidJavaClass ioPixelRateMeClass;
        
	static AndroidRateMe() {
		if (Application.platform != RuntimePlatform.Android) return;
		AndroidJavaClass playerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		ioPixelRateMeClass = new AndroidJavaClass("com.iopixel.rateme.RateMe");
		if (playerClass == null || ioPixelRateMeClass == null) {
			Debug.LogError("ioPixelRateMeClass not correctly initialized on Android");
		}
	}
	
	public static void askToRate(string appID, string title, string message, string rateButton, string remindMeButton,
		string cancelButton) {
		if (Application.platform != RuntimePlatform.Android) return;
		ioPixelRateMeClass.CallStatic("askToRate", appID, title, message, rateButton, remindMeButton, cancelButton);
	}
#endif
}

