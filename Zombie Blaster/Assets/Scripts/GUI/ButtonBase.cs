using UnityEngine;
using System.Collections;

public class ButtonBase : MonoBehaviour {
	
	public AudioSource audioPressed;
	public Texture standartTexture;
	public Texture pressedTexture;
	
	public bool Pressed()
	{
		foreach(Touch touch in Input.touches)
			if( this.guiTexture.HitTest(touch.position ) )
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
	

	
	virtual protected void Update()
	{
		foreach(Touch touch in Input.touches)
		{
			if( this.guiTexture.HitTest(touch.position ) )
			{
				if( touch.phase != TouchPhase.Ended )
					guiTexture.texture = pressedTexture;
				return;
			}
		}
		guiTexture.texture = standartTexture;
	}
}
