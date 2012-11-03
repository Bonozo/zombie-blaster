using UnityEngine;
using System.Collections;

public class SoundPlayer : MonoBehaviour {
	
	// It's created for title audio
	

	
	private Control control;

	
	// Use this for initialization
	void Awake() {
		control = (Control)GameObject.FindObjectOfType(typeof(Control));
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void StopAll()
	{
		audio.Stop();
	}
	
	#region Play Level
	
	public AudioClip[] Gameplay;
	
	public void PlayLevel(int number)
	{
		audio.clip = Gameplay[number%Gameplay.Length];
		audio.Play();
	}
	
	#endregion
	
	#region Play ZSombie Fall
	
	public AudioClip ZombieFall;
	
	public void PlayZombieFalls()
	{
		audio.PlayOneShot(ZombieFall,1f);
	}
	
	#endregion
	
	#region Play Zombie Spawn
	
	public AudioSource ZombieSpawn;
	public AudioSource ZombieSpawnRock;
	
	public void PlayZombieSpawn()
	{
		if( control.CurrentLevel == 4 ) // City Level
			ZombieSpawnRock.Play();
		else
			ZombieSpawn.Play();
	}
	
	#endregion
}
