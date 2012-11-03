using UnityEngine;
using System.Collections;

public class WaveInfo : MonoBehaviour {
	
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
			guiText.enabled = false;
	}
	
	public void ShowWave(int i)
	{
		guiText.enabled = true;
		guiText.text = "Wave " + i;
		wait = Wait;
	}
}
