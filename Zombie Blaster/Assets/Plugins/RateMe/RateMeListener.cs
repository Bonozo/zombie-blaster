using System;
using System.Collections;
using UnityEngine;

/*
 * This file is a part of the RateMe Unity Plugin
 * 
 * Copyright ioPixel 2012
 */

public class RateMeListener : MonoBehaviour {
	public static event Action OnRemindMeClickedEvent;
	public static event Action OnRateClickedEvent;
	public static event Action OnCancelClickedEvent;

	void OnRemindMeClicked(string dummy) {
		int futureAskDate = RateMe.getUnixTime() + RateMe.getRemindMeDelay();
		PlayerPrefs.SetInt("rateme-plugin-futureaskdate", futureAskDate);
		PlayerPrefs.Save();
		if (OnRemindMeClickedEvent != null) {
			OnRemindMeClickedEvent();
		}
	}
	
	void OnRateClicked(string dummy) {
		// Don't ask again
		PlayerPrefs.SetInt("rateme-plugin-futureaskdate", -1);
		if (OnRateClickedEvent != null) {
			OnRateClickedEvent();
		}
	}

	void OnCancelClicked(string dummy) {
		// Don't ask again
		PlayerPrefs.SetInt("rateme-plugin-futureaskdate", -1);
		if (OnCancelClickedEvent != null) {
			OnCancelClickedEvent();
		}
	}
}
