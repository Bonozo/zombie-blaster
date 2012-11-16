using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {
	
	#region Parameters
	
	public AudioSource audioSourceBackground;
	public AudioSource audioSourcePlayer;
	public AudioSource audioSourceZombies;
	

		
	public void StopEffects()
	{
		audioSourcePlayer.Stop();
		audioSourceZombies.Stop();
	}
	
	public void StopAll()
	{
		audioSourceBackground.Stop();
		StopEffects();
	}
	
	#endregion
	
	#region Background
	
	public AudioClip[] AudioGameplayBackground;
	
	public void PlayLevel(int number)
	{
		audioSourceBackground.clip = AudioGameplayBackground[number%AudioGameplayBackground.Length];
		audioSourceBackground.Play();
	}
	
	public AudioClip AudioGameOver;
	public void PlayGameOver()
	{
		audioSourceBackground.PlayOneShot(AudioGameOver);
	}
	
	#endregion
	
	#region Zombies
	
	// Attack Walk
	public AudioClip AudioZombieAttackWalk;
	public int zombieAudioAttackWalkRate = 1000;
	
	// Spawn
	public AudioClip zombieSpawnStandard;
	public AudioClip zombieSpawnRock;
	public void PlayZombieSpawn()
	{
		if( LevelInfo.State.currentLevel == 4 ) // City Level
			audioSourceZombies.PlayOneShot(zombieSpawnRock);
		else
			audioSourceZombies.PlayOneShot(zombieSpawnStandard);
	}
	
	// Fall
	public AudioClip ZombieFall;
	public void PlayZombieFalls()
	{
		audioSourceZombies.PlayOneShot(ZombieFall);
	}
	
	// Gets Attacket
	public AudioClip ZombieGetsAttacked;
	public void PlayZombieGetsAttcked()
	{
		audioSourceZombies.PlayOneShot(ZombieGetsAttacked);
	}
	#endregion
	
	#region Control
	
	public AudioClip AudioGetHit;
	public void PlayPlayerGetHit()
	{
		audioSourcePlayer.PlayOneShot(AudioGetHit);
	}
	
	#endregion


}
