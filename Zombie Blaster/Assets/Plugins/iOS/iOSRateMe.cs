using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

/*
 * This file is a part of the RateMe Unity Plugin
 * 
 * Copyright ioPixel 2012
 */

public class iOSRateMe {
#if UNITY_IPHONE
	[DllImport ("__Internal")]
	private static extern void n_askToRate(string appID, string title, string message, string rateButton, string remindMeButton,
		string cancelButton);
		
	public static void askToRate(string appID, string title, string message, string rateButton, string remindMeButton,
		string cancelButton) {
		if (Application.platform != RuntimePlatform.IPhonePlayer) return;
		n_askToRate(appID, title, message, rateButton, remindMeButton, cancelButton);
	}
#endif
}

