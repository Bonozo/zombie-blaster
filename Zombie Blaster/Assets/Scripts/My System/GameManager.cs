using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	

	
	#region Main Objects
	
	public Camera mainCamera;
	public Control control;
	public Store store;
	public Generator generator;
	public HealthPack healthPack;
	public GameObject messageText;
	public civilian civilianPrefab;
	
	public WaveInfo waveInfo;
	
	public GUIText guiZombieHeads;
	public GUIText guiScore;
	public GUIText guiZombiesLeft;
	public GUIText guiLives;
	
	public GUITexture ProgressBarPlayerEmpty;
	public GUITexture ProgressBarPlayerFull;
	public GUITexture ProgressBarPlayerArmor;
	
	public Texture ProgressBarZombieEmpty;
	public Texture ProgressBarZombieFull;
	
	public GameObject lightSpot;
	public GameObject lightDirectional;
	
	#endregion
	
	#region Zombies
	
	public GameObject dirtyClodPrefab;
	
	#endregion
}
