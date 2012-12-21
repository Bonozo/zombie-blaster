using UnityEngine;
using System.Collections;

public class ButtonBase : MonoBehaviour {
	
	public AudioSource audioPressed;
	public Texture standartTexture;
	public Texture pressedTexture;
	public bool canPressed = true;
	
	/*private bool st = false;
	
	public bool Pressed()
	{
		foreach(Touch touch in Input.touches)
			if( this.guiTexture.HitTest(touch.position ) && touch.phase == TouchPhase.Ended)
			{
				if( audioPressed != null ) audioPressed.Play();
				if( st ) { st = false; return true; }
			}
		
		if( Input.GetMouseButtonUp(0) && this.guiTexture.HitTest(Input.mousePosition) )
		{
			if( audioPressed != null ) audioPressed.Play();
			if( st ) { st = false; return true; }
		}
		
		return false;
	}
	
	public bool PressedDow()
	{
		foreach(Touch touch in Input.touches)
			if( this.guiTexture.HitTest(touch.position ) && touch.phase == TouchPhase.Began)
			{
				st = true;
				return true;
			}
		
		if( Input.GetMouseButtonDown(0) && this.guiTexture.HitTest(Input.mousePosition) )
		{
			st = true;
			return true;
		}
		
		return false;
	}*/
	
	private bool down = false;
	private bool up = false;
	
	public bool PressedDown { get { if(audioPressed!=null) audioPressed.Play(); return down; } }
	public bool PressedUp { get {if(up) {up=false; if(audioPressed!=null) audioPressed.Play(); return true;} else return false;} }
	
	public void Ignore() { down=up=false; }
	
	virtual protected void Update()
	{
		foreach(Touch touch in Input.touches)
		{
			if( this.guiTexture.HitTest(touch.position ) )
			{
				if(!aspressed) guiTexture.texture = (touch.phase!=TouchPhase.Ended && canPressed)?pressedTexture:standartTexture;
				if( touch.phase == TouchPhase.Began ) down = true;
				if( touch.phase == TouchPhase.Ended && down) up = true;
				return;
			}
		}
		
		if( this.guiTexture.HitTest(Input.mousePosition) )
		{
			if( Input.GetMouseButtonDown(0) ) down=true;
			if( Input.GetMouseButtonUp(0) && down) up=true;
			if(!aspressed) guiTexture.texture = (Input.GetMouseButton(0) && canPressed)?pressedTexture:standartTexture;
			return;
		}
		
		down = up = false;
		if(!aspressed) guiTexture.texture = standartTexture;
	}
	
	private bool aspressed = false;
	public void SetAsPressed()
	{
		aspressed = true;
		guiTexture.texture = pressedTexture;
	}
}
