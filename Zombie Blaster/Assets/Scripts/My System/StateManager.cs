using UnityEngine;
using System.Collections;

public class StateManager : MonoBehaviour {
	
	#region classes
	
	[System.Serializable]
	public class Level
	{
		
		public string name;
		public GameObject hierarchyPlace;
		public GameObject[] standardZombie;
		public GameObject[] scoobyZombies;
		public Weapon[] allowedGun;
	}
	
	#endregion
	
	#region Parametes
	
	public Level[] level;
	
	public int zombiesCountFactor = 10;
	
	public int scoreForPickUp = 100;
	public int scoreForZombie = 250;
	public int scoreForHeadShot = 500;
	
	#endregion
	
	#region State
	
	public bool lose = false;
	public int currentLevel = 0;
	public int currentWave = 0;
	public int zombiesLeftForThisWave = 0;
	public int score = 0;
	
	public void ForceLevel(int levelnumber,int currentwave)
	{
		currentLevel = levelnumber;
		currentWave = currentwave;
		foreach(var c in level)
			c.hierarchyPlace.SetActiveRecursively(false);
		level[currentLevel].hierarchyPlace.SetActiveRecursively(true);
	}
	
	#endregion
	
	void Awake()
	{
		GameEnvironment.zombieHeads = PlayerPrefs.GetInt("zombieHeads",0);
	}
	
	void OnDestroy()
	{
		PlayerPrefs.SetInt("zombieHeads",GameEnvironment.zombieHeads);
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		LevelInfo.Environments.guiZombieHeads.text = "" + GameEnvironment.zombieHeads;
		LevelInfo.Environments.guiScore.text = "" + score;
		LevelInfo.Environments.guiZombiesLeft.text = "" + zombiesLeftForThisWave;
	}
}
