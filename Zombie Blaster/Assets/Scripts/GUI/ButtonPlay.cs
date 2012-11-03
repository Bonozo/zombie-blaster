using UnityEngine;
using System.Collections;

public class ButtonPlay : ButtonBase {

	void OnMouseUp()
	{
		Application.LoadLevel("playgame");
	}
	
	protected override void Update()
	{
		foreach(Touch touch in Input.touches)
			if( guiTexture.HitTest(touch.position) && touch.phase == TouchPhase.Ended)
				Application.LoadLevel("playgame");
		base.Update();
	}
}
