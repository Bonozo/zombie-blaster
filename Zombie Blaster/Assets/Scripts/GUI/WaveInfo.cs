using UnityEngine;
using System.Collections;

public class WaveInfo : MonoBehaviour {
	
	public GUIText countZombies;
	public float Wait = 5.0f;
	private float wait;
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if( !guiText.enabled ) return;
		wait -= Time.deltaTime;
		if (wait <= 0 )
		{
			guiText.enabled = false;
			countZombies.gameObject.SetActiveRecursively(false);
		}
	}
	
	public void ShowWave(int i,int zombieCount)
	{
		guiText.enabled = true;
		countZombies.gameObject.SetActiveRecursively(true);
		guiText.text = "Wave " + i;
		countZombies.text = zombieCount + " zombies";
		wait = Wait;
	}
	
	public void WaveComplete()
	{
		guiText.enabled = true;
		countZombies.gameObject.SetActiveRecursively(true);
		guiText.text = "Wave";
		countZombies.text = "complete";
		wait = Wait*0.8f;	
	}
}
