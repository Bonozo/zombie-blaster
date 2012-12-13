using UnityEngine;
using System.Collections;

public class Goo : MonoBehaviour {
	
	public UISprite[] gooes;
	
	private float alphalimit = 256;
	private float alpha = 0f;
	private float reactspeed = 360f;
	private float lostspeed = 200f;
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
		default:
			foreach(var g in gooes)
				g.gameObject.SetActive(false);
			return;
		}
		
		foreach(var g in gooes)
		{
			g.gameObject.SetActive(true);
			g.color = new Color(0.5f,0.5f,0.5f,alpha/256f);
		}
	}
	
	public void Show()
	{
		state = 1;
	}
}
