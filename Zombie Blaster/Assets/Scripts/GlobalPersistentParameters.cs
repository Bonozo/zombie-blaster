using UnityEngine;
using System.Collections;

public class GlobalPersistentParameters : MonoBehaviour {

	
	// true - Amazon Store Build
	// false - Google Play Store Build
	public static bool AmazonBuild = true;
	
	// true - Can show debug options button in options
	// false - Can't
	public static bool ShowDebugOptions = false;
}
