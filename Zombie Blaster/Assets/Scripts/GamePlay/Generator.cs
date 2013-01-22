using UnityEngine;
using System.Collections;

public class Generator : MonoBehaviour {
	
	#region Zombies As Resourses Test
	[System.Serializable]
	public class LevelZombie
	{
		public string name;
		public int count;
		public int fromwave;
	}
	
	[System.Serializable]
	public class LevelZombies
	{
		public string name;
		public LevelZombie[] zombie;
		public LevelZombie[] scoobyZombies;
	}
	
	public LevelZombies[] levelZombies;
	public GameObject[] zombiestospawn;
	public GameObject objLevel;

	#endregion
	
	private string GetZombieResourcePath(LevelZombie z,int cur)
	{
		string path = "Zombies/" + z.name + "(" + 
			z.count + ")/" + z.name + cur + 
				"/" + z.name + cur;	;
		return path;
	}
	
	IEnumerator Start()
	{
		AsyncOperation unload = Resources.UnloadUnusedAssets();
		while(unload.isDone) yield return null;
		
		int lev = GameEnvironment.StartLevel;
		
		objLevel = (GameObject)Instantiate(Resources.Load("Environments/"+LevelInfo.State.level[GameEnvironment.StartLevel].name));
		objLevel.SetActive(true);
		objLevel.transform.parent = GameObject.Find("Environments").transform;
		
		int n=0;
		for(int i=0;i<levelZombies[lev].zombie.Length;i++)
			for(int j=1;j<=levelZombies[lev].zombie[i].count;j++)
				n++;
		
		zombiestospawn = new GameObject[n];
		
		n=0;
		for(int i=0;i<levelZombies[lev].zombie.Length;i++)
			for(int j=1;j<=levelZombies[lev].zombie[i].count;j++)
			{
				zombiestospawn[n++] = (GameObject)Resources.Load(GetZombieResourcePath(levelZombies[lev].zombie[i],j));
			}
	}
	
	/*public void OnGUI()
	{
		//GUI.Label(new Rect(100,100,100,100),"len = " + zombiestospawn.Length);
		//if( zombiestospawn.Length>0 && zombiestospawn[0] == null )
		//	GUI.Label(new Rect(100,150,100,100),"first emelemt is null");
	}*/
	
	#region Zombies
	
	public bool generateZombies = false;
	
	public float GenerationRateMin = 2f, GenerationRateMax=4f;
	
	public GameObject EmoKidZombie;
	
	private float zombieRate = 0;
	private int zombiesLeft = 0;
	private int scoobyZombieCount = 10;
	
	private GameObject WhatZombieToSpawn()
	{	
		GameObject zombie = (GameObject)Instantiate(zombiestospawn[Random.Range(0,zombiestospawn.Length)]
			,RandomPosition(),Quaternion.Euler(0,180,0));
		return zombie;
		
		/*int index = Random.Range(0,levelZombies.Length);
		int cur = Random.Range(1,levelZombies[index].count+1);
		string path = "Zombies/" + levelZombies[index].name + "(" + 
			levelZombies[index].count + ")/" + levelZombies[index].name + cur + 
				"/" + levelZombies[index].name + cur;
		//Debug.Log(path);
		GameObject zombie = (GameObject)Instantiate(Resources.Load(path,typeof(GameObject)),RandomPosition(),Quaternion.Euler(0,180,0));
		return zombie;*/
		
		
		/*GameObject z;
		var level = LevelInfo.State.level[LevelInfo.Environments.control.currentLevel];
		
		// special for Farm Level
		if( LevelInfo.Environments.control.currentLevel == 0 )
		{
			// ...
			// 5 cowboys
			// 1 sheriff
			if( LevelInfo.Environments.control.currentWave >=4 && Random.Range(0,6) == 1 )//16% Sheriff
				z = level.standardZombie[level.standardZombie.Length-1];
			else if( LevelInfo.Environments.control.currentWave >=2 && Random.Range(0,6) == 1)//16% Cowboy
				z = level.standardZombie[Random.Range(level.standardZombie.Length-6,level.standardZombie.Length-1)];
			else
				z = level.standardZombie[Random.Range(0,level.standardZombie.Length-6)];
		}
		else
		{
			z = level.standardZombie[Random.Range(0,level.standardZombie.Length)];
		}
		
		if( LevelInfo.Environments.control.currentWave >= 5 && Random.Range(0,12)==1 )
			z = EmoKidZombie;
		
		if(LevelInfo.Environments.control.currentWave > 1)
		{
			if(scoobyZombieCount <= 10 && Random.Range(0,scoobyZombieCount+1) == 0 )
			{
				z = level.scoobyZombies[Random.Range(0,level.scoobyZombies.Length)];
				scoobyZombieCount += 10;
			}
			scoobyZombieCount--;
		}
		return (GameObject)Instantiate(z,RandomPosition(),Quaternion.Euler(0,180,0) );*/
	}
	
	// Where to spawn
	private	Vector3 RandomPosition()
	{
		float r = Random.Range(10f,13f);
		float alpa = Random.Range(0f,360f);
		
		// special for Level1 Wave1
		if( LevelInfo.Environments.control.currentLevel == 0 && LevelInfo.Environments.control.currentWave == 1 )
		{
			alpa = 90f+Random.Range(-35f,35f);
		}

		Vector3 v = Vector3.zero;
		v.x = r*Mathf.Cos(alpa*Mathf.PI/180f);
		v.z = r*Mathf.Sin(alpa*Mathf.PI/180f);
		return v+LevelInfo.Environments.control.transform.position;
	}
	
	private bool CanMoveForward(Transform transform)
	{
		float forwarddist = 0.5f;
		float leftrightdist = 0.3f;
		float diagonaldist = 0.7f;
		RaycastHit hit;    
		if(Physics.Raycast(transform.position, transform.forward, out hit, forwarddist))
			return false;
		if(Physics.Raycast(transform.position, transform.right, out hit, leftrightdist))
			if( transform.position.magnitude > hit.collider.gameObject.transform.position.magnitude )
				return false;
		if(Physics.Raycast(transform.position, -transform.right, out hit, leftrightdist))
			if( transform.position.magnitude > hit.collider.gameObject.transform.position.magnitude )
				return false;
		if(Physics.Raycast(transform.position, 0.5f*(transform.right+transform.forward), out hit, diagonaldist))
			if( transform.position.magnitude > hit.collider.gameObject.transform.position.magnitude )
				return false;
		if(Physics.Raycast(transform.position, 0.5f*(-transform.right+transform.forward), out hit, diagonaldist))
			if( transform.position.magnitude > hit.collider.gameObject.transform.position.magnitude )
				return false;
		return true;
	}
	
	private bool NearAtZombie(GameObject gObject)
	{
		float distance = 1.2f;
		GameObject[] zbs = GameObject.FindGameObjectsWithTag("Zombie");
		foreach( GameObject g in zbs )
		{
			Vector3 p1 = g.transform.position; p1.y = 0;
			Vector3 p2 = gObject.transform.position; p2.y = 0;
			if(gObject != g && Vector3.Distance(p1,p2) <= distance )
				return true;
		}
		return false;
	}
	
	public void StartNewWave(int numberzombies)
	{
		zombiesLeft = numberzombies;
		generateZombies = true;
		zombieRate = zombieRate = Random.Range(GenerationRateMin,GenerationRateMax);
	}
	
	#endregion
	
	#region Civilian
	
	public GameObject[] civilianPrefabs;
	
	private float civilianRate = 3f;
	
	Vector3 CivilianRandomPosition()
	{
		//return new Vector3(0,0,8);
		float r = Random.Range(8f,10f);
		float alpa = Random.Range(0f,360f);
		Vector3 v = Vector3.zero;
		v.x = r*Mathf.Cos(alpa);
		v.z = r*Mathf.Sin(alpa);
		return v+LevelInfo.Environments.control.transform.position;
	}
	
	#endregion
	
	// Update is called once per frame
	void Update () {
		
		// zombies generate update
		if( generateZombies )
		{
			zombieRate -= Time.deltaTime;
			if( zombieRate <= 0 )
			{
				zombieRate = Random.Range(GenerationRateMin,GenerationRateMax);
				
				GameObject newzombie = WhatZombieToSpawn();
				if(newzombie == null ) 
					Debug.Log("New generated zombie is null");
				else if( NearAtZombie(newzombie) ) 
				{
					Destroy(newzombie);
					return;
				}
				
				zombiesLeft--;
			}
			if( zombiesLeft == 0 ) generateZombies = false;
		}
		
		// Civilian
		if( LevelInfo.Environments.control.currentLevel!=0 || LevelInfo.Environments.control.currentWave >= 3 )
		{
			civilianRate -= Time.deltaTime;
			if( civilianRate <= 0f && LevelInfo.Environments.control.IsInWave)
			{
				if( GameObject.FindObjectOfType(typeof(civilian)) == null )
					Instantiate(civilianPrefabs[Random.Range(0,civilianPrefabs.Length)],RandomPosition(),Quaternion.identity);
				civilianRate = Random.Range(5f,10f);
			}
		}
	}

	#region Message text
	
	public void GenerateMessageText(Vector3 worldPosition,string text)
	{
		Vector3 pos = LevelInfo.Environments.mainCamera.WorldToScreenPoint(worldPosition);
		pos.x /= Screen.width;
		pos.y /= Screen.height;
		pos.z=-3;
		GameObject g = (GameObject)Instantiate(LevelInfo.Environments.messageText,pos,Quaternion.identity);
		g.SendMessage("ChangeMessage",text);
	}
	
	public void GenerateMessageText(Vector3 pos,string text,bool up,float time)
	{
		GameObject g = (GameObject)Instantiate(LevelInfo.Environments.messageText2,pos,Quaternion.identity);
		g.GetComponent<ShowMessage>().ShowMessageText2D(text,up,time);
	}	
	#endregion
	
	#region HealthPack
	
	public HealthPack[] powerups;
	
	private bool firstvelmaforL1W3 = false;
	
	private HealthPackType WhatPowerupToSpawn(bool scooby)
	{
		HealthPackType packType = HealthPackType.Ammo;
		
		if( scooby )
		{
			int r = Random.Range(0,3);
			
			// some complicated code
			if( LevelInfo.Environments.control.currentLevel==0&&LevelInfo.Environments.control.currentWave==3
				&& !firstvelmaforL1W3 )
			{
				r = 0;
				firstvelmaforL1W3 = true;
			}
			
			switch(r)
			{
			case 0: packType = HealthPackType.Weapon; break;
			case 1: packType = HealthPackType.XtraLife; break;
			case 2: packType = HealthPackType.DamageMultiplier; break;
			}
		}
		else
		{
			int r = Random.Range(0,6);
			
			// some complicated code
			if( LevelInfo.Environments.control.currentLevel==0&&LevelInfo.Environments.control.currentWave<4
				&& r==5)
			{
				r = Random.Range(0,5);
				
			}
			
			switch(r)
			{
			case 0: packType = HealthPackType.Ammo; break;
			case 1: packType = HealthPackType.Armor; break;
			case 2: packType = HealthPackType.BonusHeads; break;
			case 3: packType = HealthPackType.Health; break;
			case 4: packType = HealthPackType.Shield; break;
			case 5: packType = HealthPackType.SuperAmmo; break;
			}		
		}		
		
		if(packType == HealthPackType.Weapon && AllWeaponsOwned() )
			packType = Random.Range(0,2)==0?HealthPackType.XtraLife:HealthPackType.DamageMultiplier;
		
		return packType;
	}
	
	public void InstantiatePowerup(Vector3 position,Quaternion rotation,bool scooby)
	{
		int ind = (int)WhatPowerupToSpawn(scooby);
		HealthPack er = (HealthPack)Instantiate(powerups[ind],position,rotation);
		er.scooby = scooby;
	}
	
	private bool AllWeaponsOwned()
	{
		var level = LevelInfo.State.level[LevelInfo.Environments.control.currentLevel];
		for(int i=0;i<level.allowedGun.Length;i++) 
		{
			int index = (int)level.allowedGun[i];
			if(!LevelInfo.Environments.guns.gun[index].EnabledGun && LevelInfo.Environments.store.WeaponAvailable(index))
				return false;
		}
		return true;
	}
	
	#endregion
}
