using UnityEngine;
using System.Collections;

public class ButtonMainMenu : MonoBehaviour {
	
	private GameState lastState;
	
	void OnClick()
	{
		if(Fade.InProcess) return;
		LevelInfo.Environments.control.WantToExit = true;
		//Store.Instance.GoMainMenuFromGamePlay();	
	}

	
}
