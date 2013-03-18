using UnityEngine;
using System.Collections;

public class SetAsIntended : MonoBehaviour {
	
	public bool leftanchor=false;
	public bool rightanchor=false;
	public bool upanchor=false;
	public bool downanchor=false;
	
	public float scale = 0.14f;
	// Use this for initialization
	void Awake () 
	{
		// x->width
		// y->height
		var sc = scale*Mathf.Min(Screen.width,Screen.height);
		var c = guiTexture.pixelInset;
		c.x = c.y = -0.5f*sc;
		
		if(leftanchor) 	c.x -= 0.5f*sc;
		if(rightanchor) c.x += 0.5f*sc;
		if(upanchor) 	c.y += 0.5f*sc;
		if(downanchor) 	c.y -= 0.5f*sc;
		
		c.width = c.height = sc;
		guiTexture.pixelInset = c;
		
	}
}
