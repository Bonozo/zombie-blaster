using UnityEngine;
using System.Collections;

public class Generator : MonoBehaviour {
	
	#region Zombies
	
	public bool generateZombies = false;
	
	public float GenerationRateMin = 2f, GenerationRateMax=4f;
	
	private float zombieRate = 0.0f;
	private int zombiesLeft = 0;
	private int scoobyZombieCount = 10;
	
	private GameObject WhatZombieToSpawn()
	{
		GameObject z;
		var level = LevelInfo.State.level[LevelInfo.Environments.control.currentLevel];
		z = level.standardZombie[Random.Range(0,level.standardZombie.Length)];
		if(LevelInfo.Environments.control.currentLevel != 4 && LevelInfo.Environments.control.currentWave > 1)
		{
			if(scoobyZombieCount <= 10 && Random.Range(0,scoobyZombieCount+1) == 0 )
			{
				z = level.scoobyZombies[Random.Range(0,level.scoobyZombies.Length)];
				scoobyZombieCount += 10;
			}
			scoobyZombieCount--;
		}
		return (GameObject)Instantiate(z,RandomPosition(),Quaternion.Euler(0,180,0) );
	}
	
	private	Vector3 RandomPosition()
	{
		//return new Vector3(0,0,8);
		float r = Random.Range(10f,13f);
		float alpa = Random.Range(0f,360f);
		Vector3 v = Vector3.zero;
		v.x = r*Mathf.Cos(alpa);
		v.z = r*Mathf.Sin(alpa);
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
	
	// Use this for initialization
	void Start () {
	}
	
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
				if( NearAtZombie(newzombie) ) 
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
			if( civilianRate <= 0f )
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
		GameObject g = (GameObject)Instantiate(LevelInfo.Environments.messageText,pos,Quaternion.identity);
		g.SendMessage("ChangeMessage",text);
	}
	
	public void GenerateMessageText(Vector3 pos,string text,bool up,float time)
	{
		GameObject g = (GameObject)Instantiate(LevelInfo.Environments.messageText2,pos,Quaternion.identity);
		g.GetComponent<ShowMessage>().ShowMessageText2D(text,up,time);
	}	
	#endregion
}
