using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	

	
	#region Main Objects
	
	public Camera mainCamera;
	public Control control;

	public Generator generator;
	public HealthPack healthPack;
	public Guns guns;
	
	public GameObject messageText;
	public GameObject messageText2;
	public civilian civilianPrefab;
	
	public WaveInfo waveInfo;
	public GUIText guiDamageMultiplier;
	
	public GUIText guiZombieHeads;
	public GUIText guiScore;
	public GUIText guiZombiesLeft;
	public GUIText guiLives;
	
	public GUITexture healthbarHealth;
	public GUITexture healthbarArmor;
	
	public Texture ProgressBarZombieEmpty;
	public Texture ProgressBarZombieFull;
	
	public GameObject lightSpot;
	public GameObject lightDirectional;
	
	public Texture texturePickUpHealth;
	public Texture texturePickUpAmmo;
	public Texture texturePickUpSuperAmmo;
	public Texture texturePickUpBonusHeads;
	public Texture texturePickUpXtraLife;
	public Texture texturePickUpDamageMultiplier;
	public Texture texturePickUpArmor;
	public Texture texturePickUpShields;
	
	#endregion
	
	#region Zombies
	
	public GameObject dirtyClodPrefab;
	public GameObject dirtyClodCityPrefab;
	
	#endregion
}
