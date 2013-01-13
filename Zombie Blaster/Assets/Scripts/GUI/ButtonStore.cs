using UnityEngine;
using System.Collections;

public class ButtonStore : MonoBehaviour {
	
	private GameState lastState;
	
	void OnPress(bool isDown)
	{
		if(Fade.InProcess) return;
		if(!isDown)
		{
			if( LevelInfo.Environments.control.state == GameState.Lose )
				return;
			
			if( LevelInfo.Environments.control.state == GameState.Play || LevelInfo.Environments.control.state == GameState.Paused)
			{
				//GameEnvironment.IgnoreButtons();
				lastState = LevelInfo.Environments.control.state;
				LevelInfo.Environments.control.state = GameState.Store;
			}

		}	
		else
			GameEnvironment.IgnoreButtons();
	}

	
}
