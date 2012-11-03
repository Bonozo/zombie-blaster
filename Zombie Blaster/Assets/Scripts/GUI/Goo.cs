using UnityEngine;
using System.Collections;

public class Goo : MonoBehaviour {
	
	public GUITexture[] gooes;
	
	private float alphalimit = 128;
	private float alpha = 0f;
	private float reactspeed = 180f;
	private float lostspeed = 100f;
	private int state = 0;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		switch(state)
		{
		case 1:
			alpha += Time.deltaTime*reactspeed;
			if( alpha >= alphalimit )
			{
				state = 2;
				alpha = alphalimit;
			}
			break;
		case 2:
			alpha -= Time.deltaTime*lostspeed;
			if( alpha <= 0 )
			{
				state = 0;
				alpha = 0;
			}
			break;
		}
		
		foreach(var g in gooes)
			g.color = new Color(0.5f,0.5f,0.5f,alpha/256f);
	}
	
	void Show()
	{
		state = 1;
	}
}
