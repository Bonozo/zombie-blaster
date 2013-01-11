using UnityEngine;
using System.Collections;

public class Shield : MonoBehaviour {
	
	public GameObject gui;
	public UISprite[] parts;
	public float upSpeed=1f;
	public float downSpeed=1f;
	
	private bool up;
	private bool down;
	
	void Awake()
	{
		LevelInfo.Audio.filter.enabled = false;
	}
	
	void OnEnable()
	{
		gui.SetActive(false);
	}
	
	void Update()
	{
		if(up && setalphadelta(upSpeed*Time.deltaTime)==1 ) up=false;
		if(down && setalphadelta(-downSpeed*Time.deltaTime)==-1) {down=false; gui.SetActive(false); }
	}
	
	public void Show()
	{
		up=true;
		down=false;
		setalphato(.0f);
		LevelInfo.Audio.filter.enabled = true;
	}
	
	public void Hide()
	{
		down=true;
		up=false;
		LevelInfo.Audio.filter.enabled = false;
	}
	
	public void HideImmediately()
	{
		up=down=false;
		gui.SetActive(false);
		LevelInfo.Audio.filter.enabled = false;
	}
	
	public void ShowImmediately()
	{
		up=down=false;
		gui.SetActive(true);
		setalphato(1f);
		LevelInfo.Audio.filter.enabled = true;
	}
	
	private void setalphato(float a)
	{
		foreach(var p in parts)
		{
			Color c=p.color;
			c.a=a;
			p.color=c;
		}
	}
	
	private int setalphadelta(float delta)
	{
		int res = 0;
		foreach(var p in parts)
		{
			Color c=p.color;
			c.a = Mathf.Clamp01(c.a+delta);
			if(c.a==0) res=-1;
			if(c.a==1) res=1;
			p.color=c;
		}
		return res;
	}
	
	public void SetActive(bool value)
	{
		gui.SetActive(value);
	}
}
