using UnityEngine;
using System.Collections;

public class ZBiPhoneTapjoy : MonoBehaviour {
	#if UNITY_IPHONE
	private int tapjoyAddCallCnt = 0 ;

	// Use this for initialization
	void Start () {
		if(Application.loadedLevel == 1)
			showMainMenuAdd();
		else if(Application.loadedLevel == 2)
			showInGameAdd();
	}
	
	void showMainMenuAdd() {
		TapjoyBinding.init( "b6b895c2-5c52-4eac-b102-4ae2a6bcaf84", "IaaQuo03VctIM6m3Qr5Z", true );	
		TapjoyBinding.showFullScreenAd();
	}
	
	void showInGameAdd() {
		TapjoyBinding.init( "b6b895c2-5c52-4eac-b102-4ae2a6bcaf84", "IaaQuo03VctIM6m3Qr5Z", true );
		TapjoyBinding.setAdContentSize( TapjoyAdContentSize.Size320x50 );
		TapjoyBinding.createBanner( TapjoyAdPosition.BottomCenter );
		InvokeRepeating("RefreshAdd",20,20);	
		
	}
		
	void RefreshAdd() {
		if(tapjoyAddCallCnt == 5)
			CancelInvoke("RefreshAdd");
		tapjoyAddCallCnt++;
		TapjoyBinding.refreshBanner();
	}
	#endif
}
