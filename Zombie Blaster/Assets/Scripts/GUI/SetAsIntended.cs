using UnityEngine;
using System.Collections;

public class SetAsIntended : MonoBehaviour {
	
	public float scale = 0.14f;
	// Use this for initialization
	void Update () {
		// x->width
		// y->height
		var sc = scale*Mathf.Min(Screen.width,Screen.height);
		var c = guiTexture.pixelInset;
		c.x = c.y = -0.5f*sc;
		c.width = c.height = sc;
		guiTexture.pixelInset = c;
		
	}
}
