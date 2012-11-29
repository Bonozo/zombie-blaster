using UnityEngine;
using System.Collections;

public class ButtonPause : MonoBehaviour {
	
	
	
	void Update()
	{
		if( LevelInfo.Environments.control.state == GameState.Play )
		{
			foreach(Touch touch in Input.touches)
				if( touch.phase == TouchPhase.Ended && guiTexture.HitTest(touch.position) )
				{
					GameEnvironment.IgnoreButtons();
					LevelInfo.Environments.control.state = GameState.Paused;
				}
		}
		
		if( Input.GetMouseButtonUp(0) && this.guiTexture.HitTest(Input.mousePosition) )
		{
			GameEnvironment.IgnoreButtons();
			LevelInfo.Environments.control.state = GameState.Paused;		
		}
		

	}
}
