using UnityEngine;
using System.Collections;

public class ButtonMap : MonoBehaviour {
	
	private GameState lastState;
	
	void OnPress(bool isDown)
	{
		if(!isDown)
		{
			if( LevelInfo.Environments.control.state == GameState.Lose )
				return;
			
			if( LevelInfo.Environments.control.state == GameState.Play || LevelInfo.Environments.control.state == GameState.WaveCompleted || LevelInfo.Environments.control.state == GameState.Paused)
			{
				//GameEnvironment.IgnoreButtons();
				lastState = LevelInfo.Environments.control.state;
				LevelInfo.Environments.control.state = GameState.Map;
			}

		}	
		else
			GameEnvironment.IgnoreButtons();
	}

	
}
