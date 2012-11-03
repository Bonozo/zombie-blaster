using UnityEngine;
using System.Collections;

public class ButtonGoScene : ButtonBase {
	
	public string scenename;
	
	void OnMouseUp()
	{
		Application.LoadLevel(scenename);
	}
	
	protected override void Update()
	{
		foreach(Touch touch in Input.touches)
			if( guiTexture.HitTest(touch.position) && touch.phase == TouchPhase.Ended)
				Application.LoadLevel(scenename);
		base.Update();
	}
}
