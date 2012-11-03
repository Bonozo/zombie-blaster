using UnityEngine;
using System.Collections;

public class ScreenBlood : MonoBehaviour {
	
	private float dangerhealth = 0.5f;
	private float alpamax = 128f;
	private float chagefactor = 40f;
	private float deltamin = 0f, deltamax = 30f;
	private bool deltaincrease = true;
	private Control control;
	//private float lasthealth;
	private float delta = 0;
	
	// Use this for initialization
	void Start () {
		control = (Control)GameObject.FindObjectOfType(typeof(Control));
		//lasthealth = control.Health;
	}
	
	// Update is called once per frame
	void Update () {
		
		if( control.Health >= dangerhealth )
		{
			//lasthealth = control.Health;
			guiTexture.color = new Color(0.5f,0.5f,0.5f,0f);
			audio.Stop();
			return;
		}
		
		if( deltaincrease )
		{
			delta += Time.deltaTime*chagefactor;
			if( delta >= deltamax )
				deltaincrease = false;
		}
		else
		{
			delta -= Time.deltaTime*chagefactor;
			if( delta <= deltamin )
				deltaincrease = true;	
		}
		
		float alpha = formula() + delta;
		
		guiTexture.color = new Color(0.5f,0.5f,0.5f,alpha/256f);
		if( Time.deltaTime == 0.0f ) audio.Stop();
		else if( !audio.isPlaying ) audio.Play();
		audio.volume = 1-control.Health/dangerhealth;
		
		//lasthealth = control.Health;
	}
	
	private float formula()
	{
		return alpamax*(1-control.Health/dangerhealth);
	}
	
	void OnGUI()
	{
		//GUI.Label(new Rect(0,400,200,200),"alpa : " + alpha + "\ntarget : " + alphatarget);
	}
}
