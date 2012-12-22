using UnityEngine;
using System.Collections;

public class testing : MonoBehaviour {

	// Use this for initialization
	void Start () {
		#if UNITY_ANDROID || UNITY_IPHONE
		iPhoneUtils.PlayMovie("Movies/zombieblasterpendingapproval9.mp4",Color.black, iPhoneMovieControlMode.Hidden);
		#endif
	}
}
