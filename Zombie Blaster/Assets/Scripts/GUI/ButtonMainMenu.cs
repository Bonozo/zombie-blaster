using UnityEngine;
using System.Collections;

public class ButtonMainMenu : MonoBehaviour {
	
	private GameState lastState;
	
	void OnPress(bool isDown)
	{
		if(Fade.InProcess) return;
		if(!isDown)
		{
			GameObject.Find("Store").GetComponent<Store>().GoMainMenuFromGamePlay();
		}	
	}

	
}
