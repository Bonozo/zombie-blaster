using UnityEngine;
using System.Collections;

public class ButtonPause : MonoBehaviour {
	
	void OnMouseUp()
	{
		if( LevelInfo.State.state == GameState.Lose ) return;
		Time.timeScale = 1-Time.timeScale;
	}
	
	void Update()
	{
		if( LevelInfo.State.state == GameState.Lose ) return;
		foreach(Touch touch in Input.touches)
			if( touch.phase == TouchPhase.Ended && guiTexture.HitTest(touch.position) )
			{
				Time.timeScale = 1-Time.timeScale;
			}
	}
}
