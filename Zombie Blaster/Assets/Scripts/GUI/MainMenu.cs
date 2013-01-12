using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {
	
	#region State Enum
	
	public enum MenuState
	{
		MainMenu,
		Option,
		Credits,
		AreaMap,
		Store,
		Leaderboard
	}
	
	#endregion
	
	#region Parameters
	
	public AudioSource audioCredits;
	public AudioSource audioPressed;
	public ButtonBase buttonOption,buttonPlay,buttonStore,buttonStats;
	public GameObject objMenu,objOption,objCreadits,objAreaMap,objLeaderboard;
	public AudioSource SoundBackground,SoundWind;
	public ButtonBase backButton;
	
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
			objLeaderboard.SetActiveRecursively(_state == MenuState.Leaderboard);
			
			backButton.gameObject.SetActive(_state == MenuState.AreaMap || _state == MenuState.Leaderboard || _state == MenuState.Option || _state == MenuState.Credits );
			
			if( _state == MenuState.AreaMap )
				SoundBackground.Stop();
			
			if( _state == MenuState.Credits )
				audioCredits.Play();
			
			if( _state == MenuState.Store )
				GameObject.Find("Store").GetComponent<Store>().showStore = true;
		}
	}
	
	public bool mapToStore = false;
	public void GoState(MenuState state)
	{
		State = state;
	}
	
	#endregion
	
	#region Methods
	
	private bool ButtonPressed(GUITexture button)
	{
		foreach(Touch touch in Input.touches)
			if( button.HitTest(touch.position) && touch.phase == TouchPhase.Ended)
			{
				//if( audioPressed != null ) audioPressed.Play();
				return true;
			}
		if( Input.GetMouseButtonUp(0) && button.HitTest(Input.mousePosition) )
		{
			//if( audioPressed != null ) audioPressed.Play();
			return true;
		}
		
		return false;
	}
	
	void Awake()
	{
		Time.timeScale = 1f;
		GameObject.Find("Store").GetComponent<Store>().showStore = false;
		GameObject.Find("Store").GetComponent<Store>().LoadingGUI.SetActive(false);
	}
	
	// Use this for initialization
	void Start ()
	{
		State = MenuState.MainMenu;
		
		if( GameEnvironment.firstTimePlayed )
		{
			GameEnvironment.firstTimePlayed = false;
			State = MenuState.AreaMap;
		}
	}
	
	// Update is called once per frame
	void Update()
	{
		switch(State)
		{
		case MenuState.MainMenu:
			if( buttonOption.PressedUp )
				State = MenuState.Option;
			if( buttonPlay.PressedUp )
				State = MenuState.AreaMap;
			if( buttonStats.PressedUp )
				State = MenuState.Leaderboard;
			if( buttonStore.PressedUp )
			{
				State = MenuState.Store;
			}
			break;
		
		case MenuState.Credits:
			if( backButton.PressedUp ) State = MenuState.Option;
			break;
		case MenuState.AreaMap:
		case MenuState.Leaderboard:
		case MenuState.Option:
			//if( Input.GetKeyUp(KeyCode.Escape) ) State = MenuState.MainMenu;
			if( backButton.PressedUp  ) State = MenuState.MainMenu;
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
			break;
		case MenuState.Credits:
			break;
		case MenuState.AreaMap:
			break;
		case MenuState.Leaderboard:
			break;
		}
		
	}
	
	#endregion
}
