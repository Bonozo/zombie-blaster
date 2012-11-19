using UnityEngine;
using System.Collections;

public class ButtonBase : MonoBehaviour {
	
	public AudioSource audioPressed;
	public Texture standartTexture;
	public Texture pressedTexture;
	public bool canPressed = true;
	
	public bool Pressed()
	{
		foreach(Touch touch in Input.touches)
			if( this.guiTexture.HitTest(touch.position ) && touch.phase == TouchPhase.Ended)
			{
				if( audioPressed != null ) audioPressed.Play();
				return true;
			}
		
		if( Input.GetMouseButtonUp(0) && this.guiTexture.HitTest(Input.mousePosition) )
		{
			if( audioPressed != null ) audioPressed.Play();
			return true;
		}
		
		return false;
	}
	
	public bool PressedDown()
	{
		foreach(Touch touch in Input.touches)
			if( this.guiTexture.HitTest(touch.position ) && touch.phase == TouchPhase.Began)
				return true;
		
		if( Input.GetMouseButtonDown(0) && this.guiTexture.HitTest(Input.mousePosition) )
			return true;
		
		return false;
	}
	

	
	virtual protected void Update()
	{
		foreach(Touch touch in Input.touches)
		{
			if( this.guiTexture.HitTest(touch.position ) )
			{
				if( touch.phase != TouchPhase.Ended && canPressed )
					guiTexture.texture = pressedTexture;
				return;
			}
		}
		guiTexture.texture = standartTexture;
	}
}
