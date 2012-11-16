using UnityEngine;
using System.Collections;

public class Control : MonoBehaviour {
	
	#region Parameters
	
	public float Speed = 3f;
	
	public Vector3[] VantagePoints;
	
	#endregion
	
	#region Variables
	
	private float health = 1f;
	
	#endregion
	
	#region Start, Update
	
	// Use this for initialization
	void Start ()
	{
		health = Option.Health;
	
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
		
		if( LevelInfo.State.state == GameState.Lose ) return;
		
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
		
		UpdateHealthBar();
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
	
	void UpdateHealthBar()
	{
		Vector3 v;
		// Full scale
		v = LevelInfo.Environments.ProgressBarPlayerFull.transform.localScale;
		v.x = 0.1f*Mathf.Clamp01(health);
		LevelInfo.Environments.ProgressBarPlayerFull.transform.localScale = v;
		// Full position
		v = LevelInfo.Environments.ProgressBarPlayerFull.transform.position;
		v.x = LevelInfo.Environments.ProgressBarPlayerEmpty.transform.position.x-0.05f+0.05f*Mathf.Clamp01(health);
		LevelInfo.Environments.ProgressBarPlayerFull.transform.position = v;
		
		// Armor position
		v = LevelInfo.Environments.ProgressBarPlayerArmor.transform.position;
		v.x = LevelInfo.Environments.ProgressBarPlayerEmpty.transform.position.x + 0.5f*LevelInfo.Environments.ProgressBarPlayerEmpty.transform.localScale.x + 0.5f*Mathf.Max (0f,0.1f*health-0.1f);
		LevelInfo.Environments.ProgressBarPlayerArmor.transform.position = v;
	
		// Armor scale
		v = LevelInfo.Environments.ProgressBarPlayerArmor.transform.localScale;
		v.x = Mathf.Max (0f,0.1f*health-0.1f);
		LevelInfo.Environments.ProgressBarPlayerArmor.transform.localScale = v;
		
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
		{
			LevelInfo.Environments.waveInfo.WaveComplete();
			StartCoroutine(MoveTo(VantagePoints[LevelInfo.State.currentWave%VantagePoints.Length]));
		}
	}
	
	public void GetHealth(float h)
	{
		if( Option.UnlimitedHealth ) return;
		
		if( h<0f && Option.Vibration && Application.platform == RuntimePlatform.Android)
				Handheld.Vibrate();
		
		health += h;
		if( health > 3f ) health = 3f;//?? 2x armor maximum//
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
		
		LevelInfo.Environments.waveInfo.ShowWave(LevelInfo.State.currentWave,LevelInfo.State.zombiesLeftForThisWave);
		LevelInfo.Audio.PlayLevel(LevelInfo.State.currentWave);
	}
	
	
	public void RestartWave()
	{
		LevelInfo.State.zombiesLeftForThisWave = LevelInfo.State.zombiesCountFactor*LevelInfo.State.currentWave;
		LevelInfo.Environments.generator.StartNewWave(LevelInfo.State.zombiesLeftForThisWave);
		
		LevelInfo.Environments.waveInfo.ShowWave(LevelInfo.State.currentWave,LevelInfo.State.zombiesLeftForThisWave);
		LevelInfo.Audio.PlayLevel(LevelInfo.State.currentWave);	
	}
	
	public void ToLose()
	{
		LevelInfo.Audio.StopAll();
		LevelInfo.Audio.PlayGameOver();
		audio.Stop();
		LevelInfo.State.state = GameState.Lose;
		Time.timeScale = 0.0f;
		LevelInfo.Environments.store.enabled = false;		
	}
	
	#endregion
}