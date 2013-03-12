using UnityEngine;
using System.Collections;

public class ButtonOptions : MonoBehaviour {

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
			
		if( LevelInfo.Environments.control.state == GameState.Play  || LevelInfo.Environments.control.state == GameState.Paused)
		{
			LevelInfo.Environments.control.state = GameState.Options;
		}
	}

}
