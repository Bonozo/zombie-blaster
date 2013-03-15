using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {
	
	#region Parameters
	
	public AudioSource audioSourceBackground;
	public AudioSource audioSourcePlayer;
	public AudioSource audioSourceZombies;
	public AudioSource audioSourceWindLoop;
	public AudioSource audioSourceFlamethrower;
	
	public void StopEffects()
	{
		audioSourcePlayer.Stop();
		audioSourceZombies.Stop();
		audioSourceFlamethrower.Stop();
	}
	
	public void StopAll()
	{
		audioSourceBackground.Stop();
		StopEffects();
	}
	
	public void PauseAll()
	{
		audioSourceBackground.Pause();
		audioSourceWindLoop.Pause();
		StopEffects();
	}
	
	public void UnPauseAll()
	{
		audioSourceBackground.Play();
		audioSourceWindLoop.Play();
		audioSourcePlayer.Play();
		audioSourceZombies.Play();
		
	}
	
	public void Awake()
	{
		InitVolume();
	}
	
	public void InitVolume()
	{
		//// init volumes
		audioSourceBackground.volume = Option.hSlideVolume;

		audioSourcePlayer.volume = Option.sfxVolume;
		audioSourceZombies.volume = Option.sfxVolume;
		audioSourceWindLoop.volume = Option.sfxVolume;
		audioSourceFlamethrower.volume = Option.sfxVolume;
		
		if(Option.hSlideVolume==0f && Option.sfxVolume == 0f)
			AudioListener.volume = 0f;
		else
			AudioListener.volume = 1f;
		//AudioListener.volume = Mathf.Max(Option.hSlideVolume,Option.sfxVolume);		
	}
	
	public AudioDistortionFilter filter;
	
	#endregion
	
	#region Background
	
	public AudioClip[] AudioGameplayBackground;
	
	public void PlayLevel(int number)
	{
		number%=LevelInfo.Environments.control.VantagePoints.Length;
		audioSourceBackground.clip = AudioGameplayBackground[2*number+Random.Range(0,2)];
		audioSourceBackground.Play();
	}
	
	public AudioClip AudioGameOver;
	public void PlayGameOver()
	{
		audioSourceBackground.PlayOneShot(AudioGameOver);
	}
	
	public AudioClip AudioWaveBegin;
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
	
	public AudioClip[] AudioGetHit;
	public void PlayPlayerGetHit()
	{
		//gethits.Enqueue(Time.time + 0.175f);
		audioSourcePlayer.PlayOneShot(AudioGetHit[Random.Range(0,AudioGetHit.Length)]);
		audioSourcePlayer.PlayOneShot(AudioGetHit2[Random.Range(0,AudioGetHit2.Length)]);
	}
	
	public AudioClip[] AudioGetHit2;

	
	public AudioClip AudioZombieHeadshot;
	public void PlayZombieHeadShot()
	{
		audioSourcePlayer.PlayOneShot(AudioZombieHeadshot);
	}
	
	public AudioClip clipShove;
	public void PlayPlayerShove()
	{
		audioSourcePlayer.PlayOneShot(clipShove);
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
			// Disabled for double playing
			//audioSourcePlayer.PlayOneShot(audioPickUpShield);
			break;
		case HealthPackType.SuperAmmo:
			audioSourcePlayer.PlayOneShot(audioPickUpSuperAmmo);
			break;
		/*case HealthPackType.ScoreMultiplier:
			audioSourcePlayer.PlayOneShot(audioPickUpWeapon);
			break;*/
		case HealthPackType.XtraLife:
			audioSourcePlayer.PlayOneShot(audioPickUpXtraLife);
			break;	
		}		
	}
	
	#endregion
	
	#region Powerups
	
	public AudioClip clipShieldUp;
	public AudioClip clipShieldDown;
	
	public void PlayShield(bool up)
	{
		if(up)
			audioSourcePlayer.PlayOneShot(clipShieldUp);
		else
			audioSourcePlayer.PlayOneShot(clipShieldDown);
	}
	
	#endregion
	
	#region Update
	
	/*private System.Collections.Generic.Queue<float> gethits = new System.Collections.Generic.Queue<float>();
	
	void Update()
	{
		// PlayPlayerGetHit
		if( gethits.Count > 0 &&  Time.time > gethits.Peek() )
		{
			audioSourcePlayer.PlayOneShot(AudioGetHit[Random.Range(0,AudioGetHit.Length)]);
			audioSourcePlayer.PlayOneShot(AudioGetHit2[Random.Range(0,AudioGetHit2.Length)]);
			gethits.Dequeue();
		}
		
	}*/
	
	#endregion
	
	#region Weapons
	
	public AudioClip clipGunEmpty;
	
	#endregion
	
	#region Wave Complete
	
	public AudioClip clipUIStar;
	
	#endregion
}
