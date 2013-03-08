using UnityEngine;
using System.Collections;

public class ButtonPause : MonoBehaviour {
	
	private GameState lastState;
	
	
	
	void OnPress(bool isDown)
	{
		if(isDown)
			GameEnvironment.IgnoreButtons();
	}
	
	void OnClick()
	{
		if(Fade.InProcess) return;
		if( LevelInfo.Environments.control.state == GameState.Lose )
			return;
			
		if( LevelInfo.Environments.control.state == GameState.Play )
		{
			//GameEnvironment.IgnoreButtons();
			lastState = LevelInfo.Environments.control.state;
			LevelInfo.Environments.control.state = GameState.Paused;		
		}
			
		else if( LevelInfo.Environments.control.state == GameState.Paused)
		{
			LevelInfo.Environments.control.state = lastState;
		}
	}
	
	void OnApplicationPause(bool pause)
	{
		if(pause)
		{
			lastState = LevelInfo.Environments.control.state;
			LevelInfo.Environments.control.state = GameState.Paused;	
		}
	}
}
