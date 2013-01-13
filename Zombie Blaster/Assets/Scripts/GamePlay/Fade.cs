using UnityEngine;
using System.Collections;

public class Fade : MonoBehaviour {
	
	public static bool InProcess = false;
	
	public GameObject gui;
	public GUITexture fullblack;
	
	/// <summary>
	/// Fade in time
	/// </summary>
	private float upSpeed=3f;
	
	/// <summary>
	/// Fade out time
	/// </summary>
	private float downSpeed=3f;
	
	public float deltaTime = 0.016f;
	
	private bool up;
	private bool down;
	
	void Start()
	{
		upSpeed = 0.5f/upSpeed;
		downSpeed = 0.5f/downSpeed;
	}
	
	void Update()
	{
		if(up && setalphadelta(upSpeed*deltaTime)==1 ) { up=false; }
		if(down && setalphadelta(-downSpeed*deltaTime)==-1) {down=false; gui.SetActive(false); }
	}
	
	public void Show()
	{
		gui.SetActive(true);
		if(!down) setalphato(.0f);
		up=true;
		down=false;
	}
	
	public void Hide()
	{
		gui.SetActive(true);
		if(!up) setalphato(.5f);
		down=true;
		up=false;
	}
	
	/// <summary>
	/// Call after Finished
	/// </summary>
	public void Disable()
	{
		gui.SetActive(false);
	}
	
	public bool Finished { get { return !up && !down; }}
	
	private void setalphato(float a)
	{
		Color c=fullblack.color;
		c.a=a;
		fullblack.color=c;
	}
	
	private int setalphadelta(float delta)
	{
		int res = 0;
		Color c=fullblack.color;
		c.a = Mathf.Clamp(c.a+delta,0f,0.5f);
		if(c.a==0) res=-1;
		if(c.a==0.5f) res=1;
		fullblack.color=c;
		return res;
	}
	
	public void SetActive(bool value)
	{
		gui.SetActive(value);
	}
}
