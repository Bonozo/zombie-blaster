using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * This file is a part of the RateMe Unity Plugin
 * 
 * Copyright ioPixel 2012
 */

public class RateMe {
	private static int remindMeDelay = 24 * 5 * 60 * 60;
	
	public static int getUnixTime() {
		return (int) (System.DateTime.UtcNow - new System.DateTime(1970, 1, 1)).TotalSeconds;
	}
	
	// Open a native (iOS or Android) popup dialog with a title, a message, 
	// and 3 buttons labels (rate / remindme and cancel button).
	// The popup doesn't open in the following cases:
	//   - The user already clicked on the "Rate" button
	//   - The user already clicked on the "Cancel" button
	//   - The user clicked on the "RemindMe" button and the "remindMeDelay" is not yet elapsed
	public static void askToRate(string appID, string title, string message, string rateButton, string remindMeButton,
		string cancelButton) {
		int futureAskDate = PlayerPrefs.GetInt("rateme-plugin-futureaskdate", 0);
		if (futureAskDate != -1 && getUnixTime() > futureAskDate) {
#if   UNITY_ANDROID
			AndroidRateMe.askToRate(appID, title, message, rateButton, remindMeButton, cancelButton);
#elif UNITY_IPHONE
			iOSRateMe.askToRate(appID, title, message, rateButton, remindMeButton, cancelButton);
#endif			
		}
	}

	// Set the RemindMe Delay (default it 5 days), for instance:
	//   - Set RemindMe Delay for 2 days: setRemindMeDelay(new System.TimeSpan(2, 0, 0, 0));
	//   - Set RemindMe Delay for 3 hours: setRemindMeDelay(new System.TimeSpan(0, 3, 0, 0));
	//   - Set RemindMe Delay for 5 hours 35 minutes 12 seconds: setRemindMeDelay(new System.TimeSpan(0, 5, 35, 12));
	public static void setRemindMeDelay(System.TimeSpan newDelay) {
		remindMeDelay = (int) newDelay.TotalSeconds;
	}

	// Get the RemindMe Delay
	public static int getRemindMeDelay() {
		return remindMeDelay;
	}
	
	// Reset state. The next "askToRate" call will open the dialog (call 
	// this method if you want to override user response)
	public static void resetState() {
		PlayerPrefs.SetInt("rateme-plugin-futureaskdate", 0);
		PlayerPrefs.Save();
	}
}
