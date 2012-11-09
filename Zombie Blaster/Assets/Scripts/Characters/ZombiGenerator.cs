using UnityEngine;
using System.Collections;

public class ZombiGenerator : MonoBehaviour {
	
	public GameObject[] Zombie;
	public GameObject[] ZombieFootballPlayers;
	
	public float GenerationRateMin = 2f, GenerationRateMax=5f;
	
	private float rate = 0.0f;
	private int numberZombies = 0;
	
	private Control control;
	
	// Use this for initialization
	void Start () {
		control = (Control)GameObject.FindObjectOfType(typeof(Control));
	}
	
	GameObject WhatZombieToSpawn()
	{
		GameObject g;
		if( control.CurrentLevel == 1 ) // FottballPlayerLevel
			g = (GameObject)Instantiate(ZombieFootballPlayers[Random.Range(0,ZombieFootballPlayers.Length)],RandomPosition(),Quaternion.Euler(0,180,0) );
		else
			g = (GameObject)Instantiate(Zombie[Random.Range(0,Zombie.Length)],RandomPosition(),Quaternion.Euler(0,180,0) );
		return g;
	}
	
	// Update is called once per frame
	void Update () {
		rate -= Time.deltaTime;
		if( rate <= 0 )
		{
			rate = Random.Range(GenerationRateMin,GenerationRateMax);
			
			GameObject newzombie = WhatZombieToSpawn();
			if( NearAtZombie(newzombie) ) 
			{
				Destroy(newzombie);
				return;
			}
			
			numberZombies--;
		}
		
		if( numberZombies == 0 )
			GetComponent<ZombiGenerator>().enabled = false;
	}
	
	Vector3 RandomPosition()
	{
		//return new Vector3(0,0,8);
		float r = Random.Range(Option.SpawnDistanceMin,Option.SpawnDistanceMax);
		float alpa = Random.Range(0f,360f);
		Vector3 v = Vector3.zero;
		v.x = r*Mathf.Cos(alpa);
		v.z = r*Mathf.Sin(alpa);
		return v+control.transform.position;
	}
	
	public void StartNewWave(int numberzombies)
	{
		this.numberZombies = numberzombies;
		GetComponent<ZombiGenerator>().enabled = true;
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
}
