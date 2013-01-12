using UnityEngine;
using System.Collections;

public class WaveInfo : MonoBehaviour {
	
	public GameObject root;
	public UILabel numberWave;
	public UILabel countZombies;
	
	public GameObject rootWaveComplete;
	//public UISprite spriteWaveCompleteReward;
	public GameObject modelHeads,modelXtraLife;
	public UISprite[] spriteStar;
	public GameObject rewardSprite;
	
	public float Wait = 5.0f;
	private float waitfor = 0f;
	private bool showWaveComplete = false;
	private float[] startime = new float[3];
	private int stars = 0;
	private int reward;
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
		if( waitfor > 0f ) waitfor -= Time.deltaTime;
		root.SetActive(waitfor>0&&Time.deltaTime>0);
		
		rootWaveComplete.SetActive(showWaveComplete&&Time.deltaTime>0);
		
		if( showWaveComplete )
		{
			waitforreward-=Time.deltaTime;
			
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
			modelHeads.SetActive(reward==1);
			modelXtraLife.SetActive(reward==0);
			rewardSprite.SetActive(true);
		}
		else
		{
			modelHeads.SetActive(false);
			modelXtraLife.SetActive(false);
			rewardSprite.SetActive(false);
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
	
	public void ShowWaveComplete(int reward,int stars,float time1,float time2,float time3)
	{
		this.reward = reward;
		// 0 - LevelInfo.Environments.texturePickUpXtraLife
		// 1 - LevelInfo.Environments.texturePickUpBonusHeads
		/*if(reward == 0 )
			spriteWaveCompleteReward.spriteName = "Lives_box";
		else
			spriteWaveCompleteReward.spriteName = "Heads_box";*/
		showWaveComplete = true;
		
		for(int i=0;i<3;i++) spriteStar[i].spriteName = "star empty";
		startime[0]=stars>=1?time1:0;
		startime[1]=stars>=2?time2:0;
		startime[2]=stars>=3?time3:0;
		this.stars = stars;
		
		waitforreward = startime[0]+startime[1]+startime[2]+0.5f;
	}
	
	public void HideWaveComplete()
	{
		showWaveComplete = false;
	}
}
