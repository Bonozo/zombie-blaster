using UnityEngine;
using System.Collections;

#region enum GameState
	
public enum GameState
{
	Play,
	Paused,
	Lose,
	Store,
	Map
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
	
	public int scoreForPickUp = 0;
	public int scoreForZombie = 0;
	public int scoreForHeadShot = 0;
	public int scoreForUFO = 0;
	
	#endregion
}
