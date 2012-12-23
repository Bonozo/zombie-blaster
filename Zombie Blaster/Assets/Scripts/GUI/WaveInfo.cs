using UnityEngine;
using System.Collections;

public class WaveInfo : MonoBehaviour {
	
	public GameObject root;
	public UILabel numberWave;
	public UILabel countZombies;
	public float Wait = 5.0f;
	private float waitfor = 0f;
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if( waitfor > 0f ) waitfor -= Time.deltaTime;
		root.SetActive(waitfor>0&&Time.timeScale>0);
	}
	
	public void ShowWave(int i,int zombieCount)
	{
		numberWave.text = "Wave " + i;
		countZombies.text = "" + zombieCount + " zombies";
		waitfor = Wait;
	}
}
