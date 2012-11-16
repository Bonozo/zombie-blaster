using UnityEngine;
using System.Collections;

public class Control : MonoBehaviour {
	
	#region Parameters
	
	public float Speed = 3f;
	
	public Vector3[] VantagePoints;
	
	#endregion
	
	#region Variables
	
	private float health = 1f;
	
	private Vector2 ProgressBarHealthPos = new Vector2(50,10);
	private Vector2 ProgressBarHealthSize = new Vector2(100,30);
	private Rect ProgressBarHealthLabelRect = new Rect(10,15,100,20);
	
	#endregion
	
	#region Start, Update
	
	// Use this for initialization
	void Start ()
	{
		
		health = Option.Health;
		
		LevelInfo.State.lose = false;		
		LevelInfo.State.score = 0;
		
		transform.position.Set(VantagePoints[0].x,transform.position.y,VantagePoints[0].z);
		
		LevelInfo.State.ForceLevel(GameEnvironment.StartLevel,0);
		
		CreateNewZombieWave();
	}
	
	// Update is called once per frame
	void Update () 
	{
		//??//
		if( Input.GetKey(KeyCode.H) )
			health -= 0.05f;
		if( Input.GetKey(KeyCode.J) )
			health += 0.05f;
		
		if( LevelInfo.State.lose ) return;
		
		if( Option.UnlimitedHealth ) health = 1.0f;
		
		if( health <= 0 ) { ToLose(); return; }
		
		if (Moving )
		{
			MovingUpdate();
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
	}
	
	private void MovingUpdate()
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
	}
	
	#endregion
	
	#region Moving
	
	public float MovingSpeed = 1f;
	public float MovingRotateSpeed = 10f;
	private bool Moving = false;

	private Vector3 destination;
	private float angleY;
	private bool angling = false;
	
		
	private IEnumerator MoveTo(Vector3 dest)
	{
		// wait 5 seconds
		float time = Time.time + 5f;
		while ( Time.time < time )
			yield return new WaitForEndOfFrame();
		
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
	    GUI.DrawTexture(new Rect(ProgressBarHealthPos.x, ProgressBarHealthPos.y, ProgressBarHealthSize.x, ProgressBarHealthSize.y),LevelInfo.Environments.ProgressBarEmpty);
	    GUI.DrawTexture(new Rect(ProgressBarHealthPos.x, ProgressBarHealthPos.y, ProgressBarHealthSize.x * Mathf.Clamp01(health), ProgressBarHealthSize.y), LevelInfo.Environments.ProgressBarFull);
	    GUI.DrawTexture(new Rect(ProgressBarHealthPos.x+ProgressBarHealthSize.x, ProgressBarHealthPos.y, ProgressBarHealthSize.x * Mathf.Max (0f,health-1), ProgressBarHealthSize.y), LevelInfo.Environments.ProgressBarArmor);
		GUI.Label(ProgressBarHealthLabelRect,"Health");	
		
		// Draw Lose Window
		
		if( LevelInfo.State.lose)
		{	
			GUI.Box(new Rect(0.25f*Screen.width,0.25f*Screen.height,0.5f*Screen.width,0.5f*Screen.height),"Reply?");
			if( GUI.Button(new Rect(0.35f*Screen.width,0.4f*Screen.height,0.3f*Screen.width,0.1f*Screen.height), "Reply" ) )	
			{
				GameEnvironment.StartWave = LevelInfo.State.currentWave-1;
				Time.timeScale = 1.0f;
				Application.LoadLevel(Application.loadedLevel);
			}
			if( GUI.Button(new Rect(0.35f*Screen.width,0.6f*Screen.height,0.3f*Screen.width,0.1f*Screen.height), "Main Menu" ) )
			{
				Time.timeScale = 1.0f;
				Application.LoadLevel("mainmenu");
			}	
		}
		
		//GUI.Label(new Rect(0,160,200,25), "Health : " + health);
	}
	
	#endregion
	
	#region Properties
	
	public bool Died { get { return health <= 0; }}
	
	public float Health { get { return health; }}
	
	#endregion

	#region Other Methods
	
	public void GetBite(float lostHealth)
	{
		LevelInfo.Audio.PlayPlayerGetHit();
		GetHealth(-lostHealth);
	}
	
	public void GetZombie()
	{
		LevelInfo.State.score += LevelInfo.State.scoreForZombie;
		LevelInfo.State.zombiesLeftForThisWave--;
		if( LevelInfo.State.zombiesLeftForThisWave <= 0 && GameObject.FindGameObjectsWithTag("ZombieHead").Length == 1)
			StartCoroutine(MoveTo(VantagePoints[LevelInfo.State.currentWave%VantagePoints.Length]));
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
	
	public void CreateNewZombieWave()
	{	
		LevelInfo.State.currentWave++;

		LevelInfo.State.zombiesLeftForThisWave = LevelInfo.State.zombiesCountFactor*LevelInfo.State.currentWave;
		LevelInfo.Environments.generator.StartNewWave(LevelInfo.State.zombiesLeftForThisWave);
		
		LevelInfo.Environments.waveInfo.ShowWave(LevelInfo.State.currentWave);
		LevelInfo.Audio.PlayLevel(LevelInfo.State.currentWave);
	}

	public void ToLose()
	{
		LevelInfo.Audio.StopAll();
		LevelInfo.Audio.PlayGameOver();
		audio.Stop();
		LevelInfo.State.lose = true;
		Time.timeScale = 0.0f;
		LevelInfo.Environments.store.enabled = false;		
	}
	
	#endregion
}