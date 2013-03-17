using UnityEngine;
using System.Collections;

#region enum GameState
	
public enum GameState
{
	Play,
	Paused,
	Lose,
	Store,
	Map,
	Options
}
	
#endregion

public class StateManager : MonoBehaviour {
	
	#region Parametes
	
	public int zombiesCountFactor = 10;
	
	public int scoreForPickUp = 0;
	public int scoreForZombie = 0;
	
	public int scoreForStandardZombieHeadshot = 5;
	public int scoreForScoobyZombieHeadshot = 8;
	
	public int scoreForUFO = 0;
	
	#endregion
}
