using UnityEngine;
using System.Collections;

public class WaveInfo : MonoBehaviour {
	
	public GameObject root;
	public UILabel numberWave;
	public UILabel countZombies;
	
	public GameObject rootWaveComplete;
	public UISprite spriteWaveCompleteReward;
	public UISprite[] spriteStar;
	
	public float Wait = 5.0f;
	private float waitfor = 0f;
	private bool showWaveComplete = false;
	private float[] startime = new float[3];
	private int stars = 0;
	
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
		root.SetActive(waitfor>0&&LevelInfo.Environments.control.state != GameState.Store);
		rootWaveComplete.SetActive(showWaveComplete&&LevelInfo.Environments.control.state != GameState.Store);
		
		if( showWaveComplete )
		{
			for(int i=0;i<3;i++)
			{
				if( startime[i] > 0f )
				{
					startime[i] -= Time.deltaTime;
					if( startime[i] < 0 )
						spriteStar[i].spriteName = stars>=i+1?"star full":"star empty";
					break;
				}
			}
		}
	}
	
	public void ShowWave(int i,int zombieCount)
	{
		numberWave.text = "Wave " + i;
		countZombies.text = "" + zombieCount + " zombies";
		waitfor = Wait;
	}
	
	public void ShowWaveComplete(int reward,int stars,float time1,float time2,float time3)
	{
		// 0 - LevelInfo.Environments.texturePickUpXtraLife
		// 1 - LevelInfo.Environments.texturePickUpBonusHeads
		if(reward == 0 )
			spriteWaveCompleteReward.spriteName = "Lives_box";
		else
			spriteWaveCompleteReward.spriteName = "Heads_box";
		showWaveComplete = true;
		
		for(int i=0;i<3;i++) spriteStar[i].spriteName = "star empty";
		startime[0]=time1;
		startime[1]=time2;
		startime[2]=time3;
		this.stars = stars;
	}
	
	public void HideWaveComplete()
	{
		showWaveComplete = false;
	}
}
