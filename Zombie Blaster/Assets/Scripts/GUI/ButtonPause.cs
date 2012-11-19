using UnityEngine;
using System.Collections;

public class ButtonPause : MonoBehaviour {
	
	void OnMouseUp()
	{
		if( LevelInfo.Environments.control.state == GameState.Play )
		{
			GameEnvironment.IgnoreButtons();
			LevelInfo.Environments.control.state = GameState.Paused;
		}
	}
	
	void Update()
	{
		if( LevelInfo.Environments.control.state == GameState.Play )
			foreach(Touch touch in Input.touches)
				if( touch.phase == TouchPhase.Ended && guiTexture.HitTest(touch.position) )
				{
					GameEnvironment.IgnoreButtons();
					LevelInfo.Environments.control.state = GameState.Paused;
				}
	}
}
