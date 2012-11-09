using UnityEngine;
using System.Collections;

public class Control : MonoBehaviour {
	
	#region Parameters
	
	public float Speed = 3f;
	
	public float FlameEffectDistance = 7f;
	
	public Texture2D ProgressBarEmpty;
	public Texture2D ProgressBarFull;
	public Texture2D ProgressBarArmor;
	
	public int NumberZombiesForEachWave = 25;
	
	public Vector3[] VantagePoints;
	public GameObject[] Level;
	
	private float waitfornewwave = 0f;
	
	public AudioClip AudioGetHit;
	public AudioClip AudioGameOver;
	
	#endregion
	
	#region Variables
	
	private Store store;
	
	private float health = 1f;
	private int wavezombiecount = 0;
	private int currentwave = 0;
	private int score = 0;
	
	private Vector2 ProgressBarHealthPos = new Vector2(50,10);
	private Vector2 ProgressBarHealthSize = new Vector2(100,30);
	private Rect ProgressBarHealthLabelRect = new Rect(10,15,100,20);
	
	public static bool lose = false;
	
	#endregion
	
	#region Start, Update
	
	// Use this for initialization
	void Start ()
	{
		store = (Store)GameObject.FindObjectOfType(typeof(Store));
		
		health = Option.Health;
		
		lose = false;
		
		score = 0;
		
		// Deactivate Levels
		for(int i=0;i<Level.Length;i++)
			AddLevelLayer(i);
		for(int i=0;i<Level.Length;i++)
			LevelActivate(i,false);
		
		currentwave = GameEnvironment.StartWave;
		Vector3 pos =  VantagePoints[currentwave%VantagePoints.Length];
		pos.y = transform.position.y;
		transform.position = pos;
		CreateNewZombieWave();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if( Input.GetKey(KeyCode.H) )
			health -= 0.05f;
		if( Input.GetKey(KeyCode.J) )
			health += 0.05f;
		
		if( lose ) return;
		
		if( Option.UnlimitedHealth ) health = 1.0f;
		
		if( health <= 0 ) { ToLose(); return; }
		
		if (Moving )
		{
			if( angling )
			{
				Vector3 c = transform.rotation.eulerAngles;
				if( c.y < angleY ) c.y += 360f;
				// c.y > angleY
				if( c.y - angleY <= 180.0f ) c.y -= MovingRotateSpeed*Time.deltaTime;
				else c.y += MovingRotateSpeed*Time.deltaTime;
				if( Mathf.Abs(c.y - angleY) <= 1f )	{ c.y = angleY; angling = false; }
				transform.rotation = Quaternion.Euler(c);
				return;
			}
		
			Vector3 dir = destination-GameEnvironment.ProjectionXZ(transform.position); dir.Normalize();
			transform.Translate(dir*MovingSpeed*Time.deltaTime,Space.World);
			if( GameEnvironment.DistXZ(transform.position,destination) <= 0.5f )
			{
				Moving = false;
				CreateNewZombieWave();
			}
			return;
		}
		
		if( GameEnvironment.AbsoluteSwipe.y >= 0.3f )
		{
			GameObject[] zombies = GameObject.FindGameObjectsWithTag("Zombie");
			foreach(var z in zombies )
			{
				if( GameEnvironment.DistXZ(z.transform.position,transform.position) <= 1.5f &&
					camera.WorldToScreenPoint(z.transform.position).z > 0 )
				{
					z.SendMessage("ThrowOut");
				}
			}
		}
		
		
		Vector3 rot = transform.rotation.eulerAngles;
		if(rot.y >= 180.0f ) rot.y -= 360f;
		
		rot.y += Speed*Time.deltaTime*GameEnvironment.Swipe;
		rot.y += Speed*Time.deltaTime*Input.GetAxis("Horizontal"); // for PC version test.
		
		transform.rotation = Quaternion.Euler(rot);
		
		// wave setup
		if( waitfornewwave > 0 )
		{
			waitfornewwave -= Time.deltaTime;
			if( waitfornewwave <= 0f )
				MoveTo(VantagePoints[currentwave%VantagePoints.Length]);
		}
	}
	
	#endregion
	
	#region Moving
	
	public float MovingSpeed = 1f;
	public float MovingRotateSpeed = 10f;
	private bool Moving = false;

	private Vector3 destination;
	private float angleY;
	private bool angling = false;
	
		
	public void MoveTo(Vector3 dest)
	{
		//GameObject[] g = GameObject.FindGameObjectsWithTag("HealthPack");
		//foreach(GameObject gg in g) Destroy(gg);
		
		Moving = true;
		angling = true;
		destination = dest;
		angleY = Quaternion.LookRotation(destination-transform.position,Vector3.up).eulerAngles.y;
	}
	
	#endregion
	
	#region On GUI
	
	void OnGUI()
	{	
		// Draw Health
	    GUI.DrawTexture(new Rect(ProgressBarHealthPos.x, ProgressBarHealthPos.y, ProgressBarHealthSize.x, ProgressBarHealthSize.y), ProgressBarEmpty);
	    GUI.DrawTexture(new Rect(ProgressBarHealthPos.x, ProgressBarHealthPos.y, ProgressBarHealthSize.x * Mathf.Clamp01(health), ProgressBarHealthSize.y), ProgressBarFull);
	    GUI.DrawTexture(new Rect(ProgressBarHealthPos.x+ProgressBarHealthSize.x, ProgressBarHealthPos.y, ProgressBarHealthSize.x * Mathf.Max (0f,health-1), ProgressBarHealthSize.y), ProgressBarArmor);
		GUI.Label(ProgressBarHealthLabelRect,"Health");	
		
		// Draw Lose Window
		
		if( lose)
		{	
			GUI.Box(new Rect(0.25f*Screen.width,0.25f*Screen.height,0.5f*Screen.width,0.5f*Screen.height),"Reply?");
			if( GUI.Button(new Rect(0.35f*Screen.width,0.4f*Screen.height,0.3f*Screen.width,0.1f*Screen.height), "Reply" ) )	
			{
				GameEnvironment.StartWave = currentwave-1;
				Time.timeScale = 1.0f;
				Application.LoadLevel(Application.loadedLevel);
			}
			if( GUI.Button(new Rect(0.35f*Screen.width,0.6f*Screen.height,0.3f*Screen.width,0.1f*Screen.height), "Main Menu" ) )
			{
				Time.timeScale = 1.0f;
				Application.LoadLevel("mainmenu");
			}	
		}
		
		
		GUI.Label(new Rect(0,120,200,25), "Score : " + score);
		GUI.Label(new Rect(0,140,200,25), "Money : " + GameEnvironment.Money);
		//GUI.Label(new Rect(0,160,200,25), "Health : " + health);
	}
	
	#endregion
	
	#region Methods
	
	#endregion
	
	#region Properties
	
	public bool Died { get { return health <= 0; }}
	
	public float Health { get { return health; }}
	
	public int CurrentLevel { get { return (currentwave/4)%Level.Length; } }
	
	public int CurrentWave { get { return currentwave; }}
	
	#endregion

	#region Messages
	
	public void GetBite(float lostHealth)
	{
		audio.PlayOneShot(AudioGetHit);
		GetHealth(-lostHealth);
	}
	
	public void GetZombie(int sc)
	{
		GetScore(sc);
		wavezombiecount--;
		if( wavezombiecount <= 0 && GameObject.FindGameObjectsWithTag("ZombieHead").Length == 1)
		{
			waitfornewwave = 5f;
			//CreateNewZombieWave();
		}
	}
	
	public void GetScore(int sc)
	{
		score += sc;
		GameEnvironment.Money += sc;
	}
	
	public void GetHealth(float h)
	{
		if( Option.UnlimitedHealth ) return;
		
		if( h<0f && Option.Vibration && Application.platform == RuntimePlatform.Android)
				Handheld.Vibrate();
		
		health += h;
		if( health <= 0.0f)
		{
			health = 0.0f;
			ToLose();
		}
	}
	
	public void AddLevelLayer(int numb)
	{
		/*Transform[] det = (Transform[])Level[numb].GetComponentsInChildren<Transform>();
		foreach(var d in det)
		{
			d.gameObject.layer = 8;
			if( d.gameObject.light )
				d.gameObject.light.enabled = false;
		}
		Level[numb].layer = 8;*/
		Level[numb].SetActiveRecursively(false);
	}
	
	public void LevelActivate(int numb,bool act)
	{
		/*Level[numb].layer = act?0:8;
		Transform[] det = (Transform[])Level[numb].GetComponentsInChildren<Transform>();
		foreach(var d in det)
		{
			d.gameObject.layer = act?0:8;
			if( d.gameObject.light )
				d.gameObject.light.enabled = act;
		}*/
		Level[numb].SetActiveRecursively(act);
	}
	
	public void CreateNewZombieWave()
	{	
		for(int i=0;i<Level.Length;i++)
			LevelActivate(i,false);
		LevelActivate((currentwave/4)%Level.Length,true);
		
		//RenderSettings.fog = CurrentLevel==2;

		currentwave++;
		wavezombiecount = NumberZombiesForEachWave;
		LevelInfo.Environments.zombieGenerator.StartNewWave(wavezombiecount);
		
		WaveInfo waveInfo = (WaveInfo)GameObject.FindObjectOfType(typeof(WaveInfo));
		waveInfo.ShowWave(currentwave);
		LevelInfo.Audio.PlayLevel(currentwave);
	}
	public void ToLose()
	{
		LevelInfo.Audio.StopAll();
		audio.PlayOneShot(AudioGameOver);
		audio.Stop();
		lose = true;
		Time.timeScale = 0.0f;
		store.enabled = false;		
	}
	
	#endregion
}