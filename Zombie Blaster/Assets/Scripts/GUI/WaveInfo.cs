using UnityEngine;
using System.Collections;

public class WaveInfo : MonoBehaviour {
	
	public GameObject root;
	public GUIText numberWave;
	public GUIText countZombies;
	public float Wait = 5.0f;
	private float wait;
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if( wait > 0f )
		{
			wait -= Time.deltaTime;
			if (wait <= 0 )
				root.SetActiveRecursively(false);
		}
	}
	
	public void ShowWave(int i,int zombieCount)
	{
		root.SetActiveRecursively(true);
		numberWave.text = "" + i;
		countZombies.text = "" + zombieCount;
		wait = Wait;
	}
}
