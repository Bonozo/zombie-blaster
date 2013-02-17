using UnityEngine;
using System.Collections;

public class ButtonStore : MonoBehaviour {

	void OnPress(bool isDown)
	{
		if(Fade.InProcess) return;
		if(!isDown)
		{
			if( LevelInfo.Environments.control.state == GameState.Lose )
				return;
			
			if( LevelInfo.Environments.control.state == GameState.Play || LevelInfo.Environments.control.state == GameState.Paused)
			{
				LevelInfo.Environments.control.state = GameState.Store;
			}

		}	
		else
			GameEnvironment.IgnoreButtons();
	}

	
}
 