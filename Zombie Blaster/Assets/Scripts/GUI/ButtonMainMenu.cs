using UnityEngine;
using System.Collections;

public class ButtonMainMenu : MonoBehaviour {
	
	private GameState lastState;
	
	void OnClick()
	{
		if(Fade.InProcess) return;
		GameObject.Find("Store").GetComponent<Store>().GoMainMenuFromGamePlay();	
	}

	
}
