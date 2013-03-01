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
	
	public Fade fade;
	public SelectArea map;
	
	public AudioSource audioCredits;
	public AudioSource audioPressed;
	public ButtonBase buttonOption,buttonPlay,buttonStore,buttonStats,buttonQuit;
	public GameObject objMenu,objOption,objCreadits,objAreaMap,objLeaderboard;
	public AudioSource SoundBackground,SoundWind;
	public ButtonBase buttonBack,buttonFacebook,buttonTwitter;
	
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
			
			objMenu.SetActive(_state == MenuState.MainMenu);
			objOption.SetActive(_state == MenuState.Option);
			objCreadits.SetActive(_state == MenuState.Credits);
			objAreaMap.SetActive(_state == MenuState.AreaMap);
			objLeaderboard.SetActive(_state == MenuState.Leaderboard);
			
			buttonBack.gameObject.SetActive(_state == MenuState.AreaMap || _state == MenuState.Leaderboard || _state == MenuState.Option || _state == MenuState.Credits );
			
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
		
		if( GameEnvironment.ToMap )
		{
			GameEnvironment.ToMap = false;
			State = MenuState.AreaMap;
		}
		
		fade.Hide(1.5f);
		System.GC.Collect();
	}
	
	// Update is called once per frame
	void Update()
	{
		if(oktogothmapforfirstgameplay) {fade.Disable(); State = MenuState.AreaMap; }
		if(Fade.InProcess) return;
		
		switch(State)
		{
		case MenuState.MainMenu:
			if( buttonOption.PressedUp )
				State = MenuState.Option;
			if( buttonPlay.PressedUp )
			{
				if( Store.FirstTimePlay )
					StartCoroutine(FirstTimePlayThread());
				else
					State = MenuState.AreaMap;
			}
			if( buttonStats.PressedUp )
				State = MenuState.Leaderboard;
			if( buttonStore.PressedUp )
			{
				State = MenuState.Store;
			}
			
			if( buttonQuit.PressedUp )
			{
				StartCoroutine(QuitGameThread());
				return;
			}
			
			break;
		
		case MenuState.Credits:
			if( buttonBack.PressedUp ) State = MenuState.Option;
			break;
		case MenuState.AreaMap:
		case MenuState.Leaderboard:
		case MenuState.Option:
			//if( Input.GetKeyUp(KeyCode.Escape) ) State = MenuState.MainMenu;
			if( buttonBack.PressedUp  ) State = MenuState.MainMenu;
			break;
		}
	}
	
	private bool oktogothmapforfirstgameplay=false; // (:
	public IEnumerator FirstTimePlayThread()
	{
		DisableMenuButtons();
		Fade.InProcess = true;
		fade.Show(1.5f);
		while( !fade.Finished )
		{
			yield return new WaitForEndOfFrame();
		}
		//fade.Disable();
		yield return new WaitForEndOfFrame();
		Fade.InProcess=false;
		oktogothmapforfirstgameplay=true;
	}
	
	public AudioSource audioQuitGame;
	public IEnumerator QuitGameThread()
	{
		DisableMenuButtons();
		audioQuitGame.Play();
		
		Fade.InProcess = true;
		fade.Show(1.5f);
		while( !fade.Finished )
		{
			yield return new WaitForEndOfFrame();
		}
		Fade.InProcess=false;
		Application.Quit();
	}
	
	#endregion
	
	#region Methods
	
	public void DisableMenuButtons()
	{
		buttonOption.DisableButtonForUse();
		buttonPlay.DisableButtonForUse();
		buttonStore.DisableButtonForUse();
		buttonStats.DisableButtonForUse();
		buttonQuit.DisableButtonForUse();
		buttonBack.DisableButtonForUse();
		buttonFacebook.DisableButtonForUse();
		buttonTwitter.DisableButtonForUse();
	
	}
	
	#endregion
}
