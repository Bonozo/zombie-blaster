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
	
	public Texture2D ProgressBarEmpty;
	public Texture2D ProgressBarFull;
	public Texture2D ProgressBarArmor;
	
	#endregion
	
	#region Zombies
	
	public GameObject dirtyClodPrefab;
	
	#endregion
}
