using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	
	#region Main Objects
	
	public Transform environmentsTransform;
	public Camera mainCamera;
	public UICamera uiCamera;
	public Control control;
	public Store store;
	public Generator generator;
	public Fade fade;
	public HealthPack healthPack;
	public Guns guns;
	public ScreenBlood screenBlood;
	public Shield shield;
	public SelectArea map;
	public GameObject guiOptions;
	public GameObject guiPaused;
	public SwipeUpCaution swipeUpCaution;
	
	public Transform nGUITopLeftTransform;
	
	public UISprite notificationStore;
	public UISprite notificationMap;
	
	public GameObject messageText;
	public GameObject messageText2;
	
	public WaveInfo waveInfo;
	public UILabel powerupTimeDamageMultiplier;
	public UILabel powerupTimeUnlimitedAmmo;
	public UILabel powerupTimeShilded;
	public Goo goo;
	public GameObject fpsGUI;
	
	public UISprite healthbarHealth;
	public UISprite healthbarArmor;
	public FlashIcon healthbarTombstone;
	public FlashIcon healthbarBoxes;
	
	public Texture ProgressBarZombieEmpty;
	public Texture ProgressBarZombieFull;
	
	public GameObject lightSpot;
	public GameObject lightDirectional;
	
	#endregion
	
	#region Weapons
	
	public Texture texturePickUpHealth;
	public Texture texturePickUpAmmo;
	public Texture texturePickUpSuperAmmo;
	public Texture texturePickUpBonusHeads;
	public Texture texturePickUpXtraLife;
	public Texture texturePickUpDamageMultiplier;
	public Texture texturePickUpArmor;
	public Texture texturePickUpShields;
	
	public GameObject particleSpark;
	public GameObject particleBlood;
	
	#endregion
	
	#region Prefabs
	
	public GameObject ufoSpaceship;
	public GameObject dirtyClodPrefab;
	public GameObject dirtyClodCityPrefab;
	public GameObject zombieHealthBar;
	
	#endregion
	
	#region Materials
	
	public Material materialLighnings;
	
	#endregion
	
	#region HUB
	
	public HubUI hubZombiesLeft;
	public HubUI hubZombieHeads;
	public HubUI hubScores;
	public HubUI hubLives;
	
	public UIButton buttonMap;
	public UIButton buttonStore;
	public UIButton buttonPause;
	public UIButton buttonPauseMap;
	public UIButton buttonPauseStore;
	public UIButton buttonPauseMainMenu;
	public UIButton buttonPauseResume;
	
	#endregion
	
	#region Initialzie
	
	void Awake()
	{
		store = (Store)GameObject.FindObjectOfType(typeof(Store));
	}
	
	#endregion
}
