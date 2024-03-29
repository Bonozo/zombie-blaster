using UnityEngine;
using System.Collections;

public class Generator : MonoBehaviour {
	
	#region Resources Controller
	[System.Serializable]
	public class LevelZombie
	{
		public string name;
		public int count;
		public int fromwave;
		public GameObject[] obj=null;
	}
	
	[System.Serializable]
	public class LevelZombies
	{
		public string name;
		public LevelZombie[] zombie;
		public LevelZombie[] scoobyZombies;
	}
	
	public LevelZombies[] levelZombies;
	
	[System.NonSerialized]
	public GameObject objLevel;
	
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
		
		objLevel = (GameObject)Instantiate(Resources.Load("Environments/"+GameEnvironment.levelName[GameEnvironment.StartLevel]));
		objLevel.SetActive(true);
		objLevel.transform.parent = LevelInfo.Environments.environmentsTransform;
		
		for(int i=0;i<levelZombies[lev].zombie.Length;i++)
		{
			levelZombies[lev].zombie[i].obj = new GameObject[levelZombies[lev].zombie[i].count];
			for(int j=1;j<=levelZombies[lev].zombie[i].count;j++)
				levelZombies[lev].zombie[i].obj[j-1] = (GameObject)Resources.Load(GetZombieResourcePath(levelZombies[lev].zombie[i],j));
		}
		
		
		for(int i=0;i<levelZombies[lev].scoobyZombies.Length;i++)
		{
			levelZombies[lev].scoobyZombies[i].obj = new GameObject[1];
			levelZombies[lev].scoobyZombies[i].obj[0] = (GameObject)Resources.Load(GetZombieResourcePath(levelZombies[lev].scoobyZombies[i],1));
		}
	}
	
	#endregion
		
	#region Zombies
	
	public bool generateZombies = false;
	
	[System.NonSerializedAttribute]
	public float GenerationRateMin = 2f, GenerationRateMax=4f;
	
	[System.NonSerializedAttribute]
	public float GenerationDistanceMin = 10f, GenerationDistanceMax = 13f;
	
	private float zombieRate = 0;
	
	[System.NonSerializedAttribute]
	public int zombiesLeft = 0;
	private int scoobyZombieCount = 20;
	
	private GameObject WhatZombieToSpawn()
	{	
		int currentLevel = LevelInfo.Environments.control.currentLevel;
		int currentWave = LevelInfo.Environments.control.currentWave;
		
		// Find random zombie used current level to instantiate
		int index;
		do
		{
			index = Random.Range(0,levelZombies[currentLevel].zombie.Length);
		}
		while(currentWave < levelZombies[currentLevel].zombie[index].fromwave);
		GameObject z = levelZombies[currentLevel].zombie[index].obj[Random.Range(0,levelZombies[currentLevel].zombie[index].count)];
		
		// Check scooby zombie to spawn
		if(LevelInfo.Environments.control.currentWave > 1)
		{
			if(scoobyZombieCount <= 20 && Random.Range(0,scoobyZombieCount+1) == 0 )
			{
				z = levelZombies[currentLevel].scoobyZombies[Random.Range(0,levelZombies[currentLevel].scoobyZombies.Length)].obj[0];
				scoobyZombieCount += 20;
			}
			scoobyZombieCount--;
		}
		return (GameObject)Instantiate(z,RandomPosition(),Quaternion.Euler(0,180,0) );
	}
	
	private	Vector3 RandomPosition()
	{
		float r = Random.Range(GenerationDistanceMin,GenerationDistanceMax);
		float alpa = Random.Range(0f,360f);
		
		// special for Level1 Wave1
		if( (LevelInfo.Environments.control.currentLevel == 0 && LevelInfo.Environments.control.currentWave == 1) || Store.FirstTimePlay )
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
		// set up minimum and maximum rate and distance for wave
		GenerationDistanceMin = Mathf.Max(10.5f-0.5f*LevelInfo.Environments.control.currentWave,5f);
		GenerationDistanceMax = 12f;
		
		GenerationRateMin = Mathf.Max(5.5f-0.5f*LevelInfo.Environments.control.currentWave,2f);
		GenerationRateMax = Mathf.Max(8f-0.5f*LevelInfo.Environments.control.currentWave,6f);
		
		
		zombiesLeft = numberzombies;
		generateZombies = true;
		zombieRate = Random.Range(GenerationRateMin,GenerationRateMax);
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
	
	#region Update (Zombies, Civilians)
	
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
					Debug.Log("ZB error: New generated zombie is null");
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
		if(!Store.FirstTimePlay && LevelInfo.Environments.control.currentLevel!=0 || LevelInfo.Environments.control.currentWave >= 3 )
		{
			civilianRate -= Time.deltaTime;
			if( civilianRate <= 0f && LevelInfo.Environments.control.IsInWave)
			{
				if( GameObject.FindObjectOfType(typeof(civilian)) == null )
					Instantiate(civilianPrefabs[Random.Range(0,civilianPrefabs.Length)],RandomPosition(),Quaternion.identity);
				civilianRate = Random.Range(10f,15f);
			}
		}
	}
	
	#endregion
	
	#region Generate Message Text
	
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
	
	#region Generate HealthPack
	
	public HealthPack[] powerups;
	
	
	private HealthPackType WhatPowerupToSpawn(bool scooby)
	{
		HealthPackType packType = HealthPackType.Ammo;
		
		if( scooby )
		{
			int r = Random.Range(0,3);
			
			switch(r)
			{
			case 0: packType = HealthPackType.DamageMultiplier; break;
			case 1: packType = HealthPackType.XtraLife; break;
			case 2: packType = HealthPackType.Shield; break;
			}
			
			// If there are many of alive zombies more probablity to spawn Shield
			if(LevelInfo.Environments.control.AliveZombieCount > 5)
			{
				int prob = Mathf.Max(10-LevelInfo.Environments.control.AliveZombieCount,0)+2;
				// 2<=prob<=6
				if(Random.Range(0,prob)==1) packType = HealthPackType.Shield;
			}
		}
		else
		{
			int r = Random.Range(0,6);
			
			// some complication code
			if( (r==0||r==5) && !ExistAvailableWeaponForAmmo())
				r = Random.Range(1,5);
			
			switch(r)
			{
			case 0: packType = HealthPackType.Ammo; break;
			case 1: packType = HealthPackType.Armor; break;
			case 2: packType = HealthPackType.BonusHeads; break;
			case 3: packType = HealthPackType.Health; break;
			case 4: packType = HealthPackType.Rampage; break;
			case 5: packType = HealthPackType.SuperAmmo; break;
			}	
			
			// Replace Ammo instead of AllAmmo with probablity 40%
			if(packType == HealthPackType.SuperAmmo && Random.Range(0,5)<2 )
				packType = HealthPackType.Ammo;
			
			// If player has few health more probablity to spawn FirstAid;
			if(LevelInfo.Environments.control.Health < 0.5f )
			{
				int prob = (int)(LevelInfo.Environments.control.Health*10)+2;
				// 2<=prob<=6
				if(Random.Range(0,prob)==1) packType = HealthPackType.Health;
			}
				
			
		}		
		return packType;
	}
	
	private bool ExistAvailableWeaponForAmmo()
	{
		for(int i=1;i<Store.countWeapons-1;i++)
			if(Store.WeaponUnlocked(i))
				return true;
		return false;
	}
	
	public void InstantiatePowerup(Vector3 position,Quaternion rotation,bool scooby)
	{
		int ind = (int)WhatPowerupToSpawn(scooby);
		HealthPack er = (HealthPack)Instantiate(powerups[ind],position,rotation);
		er.scooby = scooby;
	}
	
	#endregion
}
