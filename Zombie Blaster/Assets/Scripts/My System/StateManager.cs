using UnityEngine;
using System.Collections;

#region enum GameState
	
public enum GameState
{
	Play,
	Paused,
	Lose,
	Store,
	WaveCompleted
}
	
#endregion

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
	
	public float playerMaxHealth = 3f;
	
	#endregion
}
