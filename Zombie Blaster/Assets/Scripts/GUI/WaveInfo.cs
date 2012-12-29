using UnityEngine;
using System.Collections;

public class WaveInfo : MonoBehaviour {
	
	public GameObject root;
	public UILabel numberWave;
	public UILabel countZombies;
	
	public GameObject rootWaveComplete;
	public UISprite spriteWaveCompleteReward;
	
	public float Wait = 5.0f;
	private float waitfor = 0f;
	private bool showWaveComplete = false;
	
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
		root.SetActive(waitfor>0&&Time.timeScale>0);
		rootWaveComplete.SetActive(showWaveComplete&&Time.timeScale>0);
	}
	
	public void ShowWave(int i,int zombieCount)
	{
		numberWave.text = "Wave " + i;
		countZombies.text = "" + zombieCount + " zombies";
		waitfor = Wait;
	}
	
	public void ShowWaveComplete(int reward)
	{
		// 0 - LevelInfo.Environments.texturePickUpXtraLife
		// 1 - LevelInfo.Environments.texturePickUpBonusHeads
		if(reward == 0 )
			spriteWaveCompleteReward.spriteName = "Lives_box";
		else
			spriteWaveCompleteReward.spriteName = "Heads_box";
		showWaveComplete = true;
	}
	
	public void HideWaveComplete()
	{
		showWaveComplete = false;
	}
}
