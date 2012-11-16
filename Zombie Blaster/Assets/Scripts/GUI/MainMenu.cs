using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {
	
	#region State Enum
	
	private enum MenuState
	{
		MainMenu,
		Option,
		Credits,
		AreaMap,
		Store
	}
	
	#endregion
	
	#region Parameters
	
	public AudioSource audioPressed;
	public GUITexture guiOptionButton,guiPlayButton,guiStore;
	public GameObject objMenu,objOption,objCreadits,objAreaMap,objStore;
	public AudioSource SoundBackground,SoundWind;
	
	private MenuState _state;
	private MenuState State{
		get
		{
			return _state;
		}
		set
		{
			if( _state == MenuState.AreaMap )
				SoundBackground.Play();
			
			_state = value;
			
			objMenu.SetActiveRecursively(_state == MenuState.MainMenu);
			objOption.SetActiveRecursively(_state == MenuState.Option);
			objCreadits.SetActiveRecursively(_state == MenuState.Credits);
			objAreaMap.SetActiveRecursively(_state == MenuState.AreaMap);
			objStore.SetActiveRecursively(_state == MenuState.Store);
			
			if( _state == MenuState.AreaMap )
				SoundBackground.Stop();
		}
	}
	
	public void GoMainState()
	{
		State = MenuState.MainMenu;
	}
	
	#endregion
	
	#region Methods
	
	private bool ButtonPressed(GUITexture button)
	{
		foreach(Touch touch in Input.touches)
			if( button.HitTest(touch.position) && touch.phase == TouchPhase.Ended)
			{
				if( audioPressed != null ) audioPressed.Play();
				return true;
			}
		if( Input.GetMouseButtonUp(0) && button.HitTest(Input.mousePosition) )
		{
			if( audioPressed != null ) audioPressed.Play();
			return true;
		}
		
		return false;
	}
	
	// Use this for initialization
	void Start ()
	{
		State = MenuState.MainMenu;
	}
	
	// Update is called once per frame
	void Update()
	{
		switch(State)
		{
		case MenuState.MainMenu:
			if( ButtonPressed(guiOptionButton) )
				State = MenuState.Option;
			if( ButtonPressed(guiPlayButton) )
				State = MenuState.AreaMap;
			if( ButtonPressed(guiStore) )
			{
				Time.timeScale = 0.0f;
				State = MenuState.Store;
			}
			if( Input.GetKeyUp(KeyCode.Escape) )
				Application.Quit();
			break;
		case MenuState.Option:
			if( Input.GetKeyUp(KeyCode.Escape) ) State = MenuState.MainMenu;
			break;
		case MenuState.Credits:
			if( Input.GetKeyUp(KeyCode.Escape) ) State = MenuState.Option;
			break;
		case MenuState.AreaMap:
			if( Input.GetKeyUp(KeyCode.Escape) ) State = MenuState.MainMenu;
			break;
		}
	}
	
	void OnGUI()
	{
		switch(State)
		{
		case MenuState.MainMenu:
			break;
		case MenuState.Option:
			if( GUI.Button( new Rect(20,Screen.height-60,80,40),"Credits"))
				State = MenuState.Credits;
			if( GUI.Button( new Rect(Screen.width-100,Screen.height-60,80,40),"Menu"))
				State = MenuState.MainMenu;
			break;
		case MenuState.Credits:
			break;
		case MenuState.AreaMap:
			break;
		}
		
	}
	
	#endregion
}
