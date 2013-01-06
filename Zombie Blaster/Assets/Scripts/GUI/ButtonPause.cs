using UnityEngine;
using System.Collections;

public class ButtonPause : MonoBehaviour {
	
	private GameState lastState;
	
	void OnPress(bool isDown)
	{
		if(!isDown)
		{	
			if( LevelInfo.Environments.control.state == GameState.Lose )
				return;
			
			if( LevelInfo.Environments.control.state == GameState.Play || LevelInfo.Environments.control.state == GameState.WaveCompleted)
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
		else
			GameEnvironment.IgnoreButtons();
	}
}
