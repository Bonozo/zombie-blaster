using UnityEngine;
using System.Collections;

public class WaveInfo : MonoBehaviour {
	
	public GameObject root;
	public UILabel numberWave;
	public UILabel countZombies;
	
	public GameObject rootWaveComplete;
	//public UISprite spriteWaveCompleteReward;
	public GameObject modelHeads,modelXtraLife,modelAmmoCrates;
	public UISprite[] spriteStar;
	public GameObject rewardSprite;
	public UILabel waveBonus;
	public UILabel waveComplete;
	
	public float Wait = 5.0f;
	private float waitfor = 0f;
	private bool showWaveComplete = false;
	private float[] startime = new float[3];
	private int stars = 0;
	private HealthPackType reward;
	private float waitforreward=0f;
	
	// Use this for initialization
	void Awake () {
		root.SetActive(false);
		rootWaveComplete.SetActive(false);
	}
	
	void OnEnable()
	{
		Update();
	}
	
	// Update is called once per frame
	void Update () {
		
		if( Input.GetKeyUp(KeyCode.M) )
		ShowWaveComplete(HealthPackType.BonusHeads,3,0.3f,0.2f,0.2f);
		
		if( waitfor > 0f ) waitfor -= Time.deltaTime;
		root.SetActive(waitfor>0&&Time.deltaTime>0);
		
		rootWaveComplete.SetActive(showWaveComplete&&Time.deltaTime>0);
		
		if( showWaveComplete )
		{
			if(waitforreward>0)
			{
				waitforreward-=Time.deltaTime;
				if(waitforreward<=0f)
				{
					if(Store.FirstTimePlay)
					{
						waveBonus.text = "HEAD BONUS: 500";	
					}
					else
					{
						int score = (LevelInfo.Environments.control.currentLevel+1)*
						LevelInfo.Environments.control.currentWave*stars*10;
						LevelInfo.Environments.control.GetScore(score,false);
					
						int heads = (LevelInfo.Environments.control.currentLevel+1)*
						LevelInfo.Environments.control.currentWave;
						Store.zombieHeads += heads;
						
						waveBonus.text = "SCORE BONUS: " + score + "    HEAD BONUS: " + heads;
					}
				}
			}
			
			for(int i=0;i<3;i++)
			{
				if( startime[i] > 0f )
				{
					if(i==0)
						LevelInfo.Audio.audioSourcePlayer.PlayOneShot(LevelInfo.Audio.clipUIStar);
					startime[i] -= Time.deltaTime;
					if( startime[i] < 0 )
						spriteStar[i].spriteName = stars>=i+1?"star full":"star empty";
					break;
				}
			}
		}
		
		if(showWaveComplete&&Time.deltaTime>0&&waitforreward<=0f)
		{
			modelHeads.SetActive(reward == HealthPackType.BonusHeads);
			modelXtraLife.SetActive(reward == HealthPackType.XtraLife);
			modelAmmoCrates.SetActive(reward == HealthPackType.SuperAmmo);
			rewardSprite.SetActive(true);
			waveBonus.gameObject.SetActive(true);
		}
		else
		{
			modelHeads.SetActive(false);
			modelXtraLife.SetActive(false);
			modelAmmoCrates.SetActive(false);
			rewardSprite.SetActive(false);
			waveBonus.gameObject.SetActive(false);
		}
	}
	
	public void ShowWave(int i,int zombieCount)
	{
		
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
		waitfor = Wait;
	}
	
	public void ShowWaveComplete(HealthPackType reward,int stars,float time1,float time2,float time3)
	{
		this.reward = reward;
		showWaveComplete = true;
		
		for(int i=0;i<3;i++) spriteStar[i].spriteName = "star empty";
		startime[0]=stars>=1?time1:0;
		startime[1]=stars>=2?time2:0;
		startime[2]=stars>=3?time3:0;
		this.stars = stars;
		
		waitforreward = startime[0]+startime[1]+startime[2]+0.5f;
		
		if(Store.FirstTimePlay)
			waveComplete.text = "PROLOGUE COMPLETE";
		else
			waveComplete.text = "WAVE " + LevelInfo.Environments.control.currentWave + " COMPLETE";
	}
	
	public void HideWaveComplete()
	{
		showWaveComplete = false;
	}
	
	public void ShowPrologueComplete()
	{
		ShowWaveComplete(HealthPackType.BonusHeads,3,0.3f,0.2f,0.2f);
	}
}
