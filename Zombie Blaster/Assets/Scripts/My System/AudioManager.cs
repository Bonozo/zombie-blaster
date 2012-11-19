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
	
	public AudioClip AudioWaveComplete;
	
	public AudioClip AudioPause;
	public AudioClip AudioUnPause;
	
	public void PlayAudioPause(bool onoff)
	{
		if( onoff ) audioSourcePlayer.PlayOneShot(AudioPause);
		else audioSourcePlayer.PlayOneShot(AudioUnPause);
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
		if( LevelInfo.Environments.control.currentLevel %2 == 1 ) // City or Frat
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
	
	public AudioClip AudioZombieHeadshot;
	public void PlayZombieHeadShot()
	{
		audioSourcePlayer.PlayOneShot(AudioZombieHeadshot);
	}
	
	#endregion
	
	#region PickUps
	
	public AudioClip audioPickUpHealth;
	public AudioClip audioPickUpAmmo;
	public AudioClip audioPickUpSuperAmmo;
	public AudioClip audioPickUpBonusHeads;
	public AudioClip audioPickUpXtraLife;
	public AudioClip audioPickUpDamageMultiplier;
	public AudioClip audioPickUpArmor;
	public AudioClip audioPickUpShield;
	public AudioClip audioPickUpWeapon;
	
	public void PlayPickUp(HealthPackType healthPackType)
	{
		switch(healthPackType)
		{
		case HealthPackType.Ammo:
			audioSourcePlayer.PlayOneShot(audioPickUpAmmo);
			break;
		case HealthPackType.Armor:
			audioSourcePlayer.PlayOneShot(audioPickUpArmor);
			break;
		case HealthPackType.BonusHeads:
			audioSourcePlayer.PlayOneShot(audioPickUpBonusHeads);
			break;
		case HealthPackType.DamageMultiplier:
			audioSourcePlayer.PlayOneShot(audioPickUpDamageMultiplier);
			break;
		case HealthPackType.Health:
			audioSourcePlayer.PlayOneShot(audioPickUpHealth);
			break;
		case HealthPackType.Shield:
			audioSourcePlayer.PlayOneShot(audioPickUpShield);
			break;
		case HealthPackType.SuperAmmo:
			audioSourcePlayer.PlayOneShot(audioPickUpSuperAmmo);
			break;
		case HealthPackType.Weapon:
			audioSourcePlayer.PlayOneShot(audioPickUpWeapon);
			break;
		case HealthPackType.XtraLife:
			audioSourcePlayer.PlayOneShot(audioPickUpXtraLife);
			break;	
		}		
	}
	
	#endregion


}
