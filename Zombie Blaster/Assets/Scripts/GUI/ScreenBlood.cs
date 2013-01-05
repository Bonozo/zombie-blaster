using UnityEngine;
using System.Collections;

public class ScreenBlood : MonoBehaviour {
	
	private float dangerhealth = 0.5f;
	private float alpamax = 128f;
	private float chagefactor = 80f;
	private float deltamin = 0f, deltamax = 50f;
	private bool deltaincrease = true;
	//private float lasthealth;
	private float delta = 0;
	private float pulse = 0;
	private UISprite sprite;
	
	// Use this for initialization
	void Awake () {
		sprite = this.GetComponent<UISprite>();
		//lasthealth = control.Health;
	}
	
	void OnEnable()
	{
		Update();
	}
	
	// Update is called once per frame
	void Update () {
		
		if( LevelInfo.Environments.control.Health >= dangerhealth )
		{
			//lasthealth = control.Health;
			if(pulse>0) pulse = Mathf.Clamp(pulse,0,pulse-Time.deltaTime*chagefactor*0.5f);
			sprite.color = new Color(1f,1f,1f,pulse/256f);
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
		
		sprite.color = new Color(1f,1f,1f,alpha/256f);
		if( Time.deltaTime == 0.0f ) audio.Stop();
		else if( !audio.isPlaying ) audio.Play();
		audio.volume = 1-LevelInfo.Environments.control.Health/dangerhealth;
		
		//lasthealth = control.Health;
	}
	
	public void Pulse()
	{
		if(LevelInfo.Environments.control.Health >= dangerhealth)
			pulse = 50;
		else
			delta = deltamax+50;
		
	}
	
	private float formula()
	{
		return alpamax*(1-LevelInfo.Environments.control.Health/dangerhealth);
	}
	
	void OnGUI()
	{
		//GUI.Label(new Rect(0,400,200,200),"alpa : " + alpha + "\ntarget : " + alphatarget);
	}
}
