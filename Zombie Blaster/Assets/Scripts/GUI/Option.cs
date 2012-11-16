using UnityEngine;
using System.Collections;

public class Option : MonoBehaviour {
	
	public static float Health = 1f;
	public static float hSlideVolume = 1f;
	public static bool WeaponsUnlocked = false;
	public static float SpawnDistanceMin=10f,SpawnDistanceMax=13f;
	public static bool UnlimitedHealth = false;
	public static bool UnlimitedAmmo = false;
	public static bool UnlockAreas = false;
	public static bool Vibration = true;
	//public static int Quality = QualitySettings.GetQualityLevel();
	
	//--------------- store purchase code ------------------//
	
	public string storePublicKey;
	private string storemessage = "";//"store message";
	private string storemessage2 = "";//"store message2";
	private static int purchased = 0;
	
	void Awake() {
		
		purchased = PlayerPrefs.GetInt("purchased",0);
		
		IABAndroidManager.purchaseSucceededEvent += HandleIABAndroidManagerpurchaseSucceededEvent;
		IABAndroidManager.purchaseFailedEvent += HandleIABAndroidManagerpurchaseFailedEvent;
		IABAndroidManager.purchaseCancelledEvent += HandleIABAndroidManagerpurchaseCancelledEvent;
		IABAndroidManager.billingSupportedEvent += HandleIABAndroidManagerbillingSupportedEvent;
		
		IABAndroidManager.transactionRestoreFailedEvent += HandleIABAndroidManagertransactionRestoreFailedEvent;
		IABAndroidManager.transactionsRestoredEvent += HandleIABAndroidManagertransactionRestoredEvent;
		IABAndroidManager.purchaseSignatureVerifiedEvent +=  HandleIABAndroidManagerpurchaseSignatureVerifiedEvent;
		IABAndroidManager.purchaseRefundedEvent += HandleIABAndroidManagerpurchaseRefundedEvent;
	}

	void HandleIABAndroidManagerpurchaseRefundedEvent (string obj)
	{
		//storemessage2 = "purchaseRefunded : " + obj;
	}

	void HandleIABAndroidManagerpurchaseSignatureVerifiedEvent (string arg1, string arg2)
	{
		//storemessage2 = "purchaseSignatureVerified : " + arg1 + "," + arg2;
	}
	
	void HandleIABAndroidManagertransactionRestoredEvent()
	{
		storemessage2 = "Transaction Restored";
	}
	
	void HandleIABAndroidManagertransactionRestoreFailedEvent (string obj)
	{
		storemessage2 = "Transaction Restore Failed : " + obj;
	}

	void HandleIABAndroidManagerbillingSupportedEvent (bool obj)
	{
		//storemessage = "Billing Supported : " + obj;
	}

	void HandleIABAndroidManagerpurchaseCancelledEvent (string obj)
	{
		storemessage = "Purchase Cancelled : " + obj;
	}

	void HandleIABAndroidManagerpurchaseFailedEvent (string obj)
	{
		storemessage = "Purchase Failed : " + obj;
	}

	void HandleIABAndroidManagerpurchaseSucceededEvent (string obj)
	{
		purchased = 1;
		PlayerPrefs.SetInt("purchased",purchased);
		storemessage = "Purchase Succeeded : " + obj;
	}
	
	void Start()
	{
		//IABAndroid.init( storePublicKey );
		//IABAndroid.startCheckBillingAvailableRequest();
		//IABAndroid.restoreTransactions();
		
	}
	
	public void OnApplicationQuit()
	{
		//IABAndroid.stopBillingService();
	}
	
	//--------------- store purchase code end ------------------//
	
	// Update is called once per frame
	void Update () 
	{
		AudioListener.volume = hSlideVolume;
	}
	
	private Vector2 textSize = new Vector2(Screen.width*0.2f,30);
	private Vector2 buttonSize = new Vector2(Screen.width*0.2f,30);

	private Rect textRect(float index)
	{
		float w = 0f;
		if( index > 8 ) { w = Screen.width*0.5f; index-=8; }
		return new Rect(w + Screen.width*0.05f,index*40f,textSize.x,textSize.y);
	}
	private Rect buttonRect(float index)
	{
		float w = 0f;
		if( index > 8 ) { w = Screen.width*0.5f; index-=8; }
		return new Rect(w+Screen.width*0.1f+textSize.x,index*40f,buttonSize.x,buttonSize.y);
	}
	
	void OnGUI()
	{
		GUI.Label(textRect(1),"Volume");
		hSlideVolume = GUI.HorizontalSlider(buttonRect(1),hSlideVolume,0f,1f);
		
		GUI.Label(textRect(2),"Health");
		Health = GUI.HorizontalSlider(buttonRect(2),Health,0.05f,1f);
		
		GUI.Label(textRect(3),"Weapons Unlocked");
		if( GUI.Button(buttonRect(3),WeaponsUnlocked?"ON":"OFF" ))
		{
			WeaponsUnlocked = !WeaponsUnlocked;
			if( WeaponsUnlocked )
				for(int i=1;i<GameEnvironment.storeGun.Length;i++)
					GameEnvironment.storeGun[i].SetEnabled(Option.WeaponsUnlocked);
		}
		
		GUI.Label(textRect(4),"Spawn Distance Min");
		SpawnDistanceMin = GUI.HorizontalSlider(buttonRect(4),SpawnDistanceMin,5f,13f);
		if(SpawnDistanceMin>SpawnDistanceMax) SpawnDistanceMin=SpawnDistanceMax;
		
				
		GUI.Label(textRect(5),"Spawn Distance Max");
		SpawnDistanceMax = GUI.HorizontalSlider(buttonRect(5),SpawnDistanceMax,5f,13f);
		if( SpawnDistanceMax<SpawnDistanceMin) SpawnDistanceMax=SpawnDistanceMin;
		
		GUI.Label(textRect(6),"Unlimited Health");
		if( GUI.Button(buttonRect(6),UnlimitedHealth?"ON":"OFF" ) )
			UnlimitedHealth = !UnlimitedHealth;
		
		GUI.Label(textRect(7),"Unlimited Ammo");
		if( GUI.Button(buttonRect(7),UnlimitedAmmo?"ON":"OFF" ) )
			UnlimitedAmmo = !UnlimitedAmmo;
		
		GUI.Label(textRect(8),"Unlock Areas");
		if( GUI.Button(buttonRect(8),UnlockAreas?"ON":"OFF" ) )
		{
			//if( purchased == 1 )
				UnlockAreas = !UnlockAreas;
		}
		
		GUI.Label(textRect(9),"Vibration");
		if( GUI.Button(buttonRect(9),Vibration?"ON":"OFF" ) )
			Vibration = !Vibration;
		
		GUI.Label(textRect(10),"Quality");
		
		if( GUI.Button(buttonRect(10),QualitySettings.names[QualitySettings.GetQualityLevel()] ) )
		{
			if( QualitySettings.GetQualityLevel() == QualitySettings.names.Length-1 )
				QualitySettings.SetQualityLevel(0);
			else
				QualitySettings.IncreaseLevel();
		}
		
		/*GUI.Label(textRect(16),"Purchase UnlockAreas");
		if( GUI.Button(buttonRect(16),purchased==1?"purshased":"get this" ) )
		{
			IABAndroid.purchaseProduct("zombie_blaster_testing_unlockareas");
		}	*/
		
		GUI.Label(new Rect(Screen.width*0.5f,Screen.height*0.75f,200,200),storemessage);
		GUI.Label(new Rect(Screen.width*0.5f,Screen.height*0.85f,200,200),storemessage2);
	}
}
