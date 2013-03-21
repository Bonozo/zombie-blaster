using UnityEngine;
using System.Collections;

public class WaveInfo : MonoBehaviour {
	
	public GameObject mainroot;
	
	#region Show Wave Start

	public GameObject root;
	public UILabel numberWave;
	public UILabel countZombies;
	
	public void ShowWave(int i,int zombieCount)
	{
		StartCoroutine(ShowWaveThread(i,zombieCount));
	}
	
	IEnumerator ShowWaveThread(int i,int zombieCount)
	{
		root.SetActive(true);
		if( Store.FirstTimePlay )
		{
			numberWave.text = "PROLOGUE";
			countZombies.text = "";
		}
		else
		{
			numberWave.text = "Wave " + i;
			countZombies.text = "" + zombieCount + " zombies";
		}		
		yield return new WaitForSeconds(5f);
		root.SetActive(false);
	}
	
	#endregion
	
	#region Show Wave Complete
	
	public GameObject rootWaveComplete;
	public GameObject modelHeads,modelXtraLife,modelAmmoCrates;
	public UISprite[] spriteStar;
	public GameObject rewardSprite;
	public UILabel waveBonus;
	public UILabel headBonus;
	public UILabel waveComplete;
	
	public void ShowWaveComplete(HealthPackType reward,int stars)
	{
		StartCoroutine(ShowWaveCompleteThread(reward,stars));
	}
	
	IEnumerator ShowWaveCompleteThread(HealthPackType reward,int stars)
	{
		WaveCompleteInProgress = true;
		HideWaveComplete();
		rootWaveComplete.SetActive(true);	
		
		if(Store.FirstTimePlay)
			waveComplete.text = "PROLOGUE COMPLETE";
		else
			waveComplete.text = "WAVE " + LevelInfo.Environments.control.currentWave + " COMPLETE";
		
		yield return new WaitForSeconds(0.4f);
		
		LevelInfo.Audio.audioSourcePlayer.PlayOneShot(LevelInfo.Audio.clipUIStar);
		spriteStar[0].spriteName = stars>0?"star full":"star empty";
		yield return new WaitForSeconds(0.2f);
		
		spriteStar[1].spriteName = stars>1?"star full":"star empty";
		yield return new WaitForSeconds(0.2f);
		
		spriteStar[2].spriteName = stars>2?"star full":"star empty";
		
		yield return new WaitForSeconds(0.5f);
		
		if(Store.FirstTimePlay)
		{
			headBonus.text = "HEAD BONUS: 500";	
			headBonus.gameObject.SetActive(true);
			Store.zombieHeads += 500;
			yield return new WaitForSeconds(2f);
		}
		else
		{
			int score = (LevelInfo.Environments.control.currentLevel+1)*
			LevelInfo.Environments.control.currentWave*stars*10;
			
			LevelInfo.Environments.control.GetScore(score,false);
			
			waveBonus.text = "SCORE BONUS: " + score;
			waveBonus.gameObject.SetActive(true);
			yield return new WaitForSeconds(1f);
			
			int heads = (LevelInfo.Environments.control.currentLevel+1)*
			LevelInfo.Environments.control.currentWave;
			
			Store.zombieHeads += heads;
				
			headBonus.text = "HEAD BONUS: " + heads;
			headBonus.gameObject.SetActive(true);
			
			yield return new WaitForSeconds(1f);
			rewardSprite.gameObject.SetActive(true);
			
			//Give reward to the player
			switch(reward)
			{
			case HealthPackType.XtraLife:
				modelXtraLife.SetActive(true);
				LevelInfo.Environments.hubLives.SetNumberWithFlash(LevelInfo.Environments.hubLives.GetNumber()+1);
				break;
			case HealthPackType.BonusHeads:
				modelHeads.SetActive(true);
				Store.zombieHeads = Store.zombieHeads + 50;
				break;
			case HealthPackType.SuperAmmo:
				modelAmmoCrates.SetActive(true);
				for(int i=0;i<LevelInfo.Environments.guns.gun.Length;i++)
				if( LevelInfo.Environments.guns.gun[i].EnabledGun )
					LevelInfo.Environments.guns.GetAmmoWithMax((Weapon)i);
				break;
			default:
				Debug.LogError("ZB Error: Rewards must be heads or xtralife, but now is " + reward);
				break;
			}
		}
		
		yield return new WaitForSeconds(3f);
		WaveCompleteInProgress = false;
	}

	public void HideWaveComplete()
	{
		rootWaveComplete.SetActive(false);	
		for(int i=0;i<3;i++) spriteStar[i].spriteName = "star empty";
		
		modelHeads.SetActive(false);
		modelXtraLife.SetActive(false);
		modelAmmoCrates.SetActive(false);
		
		rewardSprite.SetActive(false);
		
		headBonus.gameObject.SetActive(false);
		waveBonus.gameObject.SetActive(false);
	}
	
	public void ShowPrologueComplete()
	{
		ShowWaveComplete(HealthPackType.BonusHeads,3);
	}
	
	public bool WaveCompleteInProgress{get;private set;}
	
	#endregion
	
	#region Mono Events
	
	void Awake () 
	{
		root.SetActive(false);
		rootWaveComplete.SetActive(false);
	}
	
	void OnEnable()
	{
		Update();
	}
	
	void Update () 
	{
		mainroot.SetActive(Time.deltaTime>0);
	}
	
	#endregion
}
