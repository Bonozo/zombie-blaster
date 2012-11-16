using UnityEngine;
using System.Collections;

#region enum GameState
	
public enum GameState
{
	Play,
	Paused,
	Lose,
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
	
	#endregion
	
	#region State
	
	public GameState _state = GameState.Play;
	public GameState state
	{
		get 
		{
			return _state;
		}
		set
		{
			_state = value;
			switch(_state)
			{
			case GameState.Lose:
				lives--;
				break;
			}
		}
	}
	
	public int currentLevel = 0;
	
	public int currentWave = 0;
	
	private int _zombiesLeftForThisWave = 0;
	public int zombiesLeftForThisWave { get { return _zombiesLeftForThisWave; } set {_zombiesLeftForThisWave = value; LevelInfo.Environments.guiZombiesLeft.text = "" + _zombiesLeftForThisWave; } }
		
	private static int startScore = 0;
	private int _score = 0;
	public int score { get { return _score; } set {_score = value; LevelInfo.Environments.guiScore.text = "" + _score; } }
	
	private static int startLives = 3;
	private int _lives = 0;
	public int lives { get { return _lives; } set {_lives = value; LevelInfo.Environments.guiLives.text = "" + _lives; } }
	
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
		GameEnvironment._playerprefs_zombieHeads = PlayerPrefs.GetInt("zombieHeads",0);
	}
	
	
	// Use this for initialization
	void Start () {
		lives = startLives;
		score = startScore;
		
		LevelInfo.Environments.lightSpot.SetActiveRecursively(Option.SpotLight);
		LevelInfo.Environments.lightDirectional.SetActiveRecursively(!Option.SpotLight);
		
		RenderSettings.fog = Option.Fog;
	}
	
	// Update is called once per frame
	void Update () {
		LevelInfo.Environments.guiZombieHeads.text = "" + GameEnvironment.zombieHeads;
	}
	
	void OnGUI()
	{
		switch(state)
		{
		case GameState.Lose:
			if( lives > 0 )
			{
				GUI.Box(new Rect(0.25f*Screen.width,0.25f*Screen.height,0.5f*Screen.width,0.5f*Screen.height),"Reply?");
				if( GUI.Button(new Rect(0.35f*Screen.width,0.4f*Screen.height,0.3f*Screen.width,0.1f*Screen.height), "Reply" ) )	
				{
					GameEnvironment.StartWave = LevelInfo.State.currentWave-1;
					startLives = lives;
					startScore = score;
					
					Time.timeScale = 1.0f;
					Application.LoadLevel(Application.loadedLevel);
				}
				if( GUI.Button(new Rect(0.35f*Screen.width,0.6f*Screen.height,0.3f*Screen.width,0.1f*Screen.height), "Main Menu" ) )
				{
					Time.timeScale = 1.0f;
					Application.LoadLevel("mainmenu");
				}			
			}
			else
			{
				GUI.Box(new Rect(0.25f*Screen.width,0.25f*Screen.height,0.5f*Screen.width,0.5f*Screen.height),"You died.");
				if( GUI.Button(new Rect(0.35f*Screen.width,0.5f*Screen.height,0.3f*Screen.width,0.1f*Screen.height), "Main Menu" ) )
				{
					Time.timeScale = 1.0f;
					Application.LoadLevel("mainmenu");
				}
			}
			break;
		}
	}
}
