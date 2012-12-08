using UnityEngine;
using System.Collections;

public class Leaderboard : MonoBehaviour {
	
	public ButtonBase buttonMainMenu;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if( buttonMainMenu.PressedUp )
		{
			MainMenu mainmenu = (MainMenu)GameObject.FindObjectOfType(typeof(MainMenu));
			mainmenu.GoState(MainMenu.MenuState.MainMenu);
		}
	}
}
