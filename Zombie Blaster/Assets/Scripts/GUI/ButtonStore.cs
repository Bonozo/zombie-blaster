using UnityEngine;
using System.Collections;

public class ButtonStore : MonoBehaviour {
	
	void OnPress(bool isDown)
	{
		if(isDown)
			GameEnvironment.IgnoreButtons();
	}
	
	public static GameState lastState = GameState.Play;
	
	void OnClick()
	{
		if(Fade.InProcess) return;
		if( LevelInfo.Environments.control.state == GameState.Lose )
			return;
		
		if( LevelInfo.Environments.control.state == GameState.Play || LevelInfo.Environments.control.state == GameState.Paused)
		{
			lastState = LevelInfo.Environments.control.state;
			LevelInfo.Environments.control.state = GameState.Store;
		}
	}

	
}
 