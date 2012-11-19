using UnityEngine;
using System.Collections;

public class SelectArea : MonoBehaviour {
	
	public ButtonBase[] levelButton;
	
	public Texture2D lockedTexture;
	public GUITexture loadingTexture;
	
	private bool[] unlocked = new bool[5];
	private int[] unlock_heads = new int [5] {0,1000,2500,5000,10000};
	private int unlock_index = 0;
	
	// Use this for initialization
	void Start () {
		
		GameEnvironment.StartLevel = 0;
		loadingTexture.enabled = false;
		unlocked[0] = true;
		unlocked[1] = 1==GameEnvironment.unlockFrat;
		unlocked[2] = 1==GameEnvironment.unlockStadiums;
		unlocked[3] = 1==GameEnvironment.unlockCity;
		unlocked[4] = 1==GameEnvironment.unlockCemetery;
		
		for(int i=0;i<levelButton.Length;i++)
			levelButton[i].canPressed = unlocked[i];
	}
	
	void Update()
	{
		if( unlock_index != 0 ) return;
		
		for(int i=0;i<levelButton.Length;i++)
			if( !unlocked[i] && levelButton[i].PressedDown() )
			{
				unlock_index = i;
				return;
			}
		
		for(int i=0;i<levelButton.Length;i++)
			if( unlocked[i] && levelButton[i].Pressed() )
			{
				GameEnvironment.StartLevel = i;
				loadingTexture.enabled = true;
				Application.LoadLevel("playgame");	
			}
	}

	// Update is called once per frame
	void OnGUI ()
	{ 
		for(int i=0;i<levelButton.Length;i++)
			if( !unlocked[i] )
			{
				Rect rect = new Rect(Screen.width*levelButton[i].gameObject.transform.position.x,Screen.height*(1-levelButton[i].gameObject.transform.position.y),50,50);
				GUI.DrawTexture(rect,lockedTexture);
			}
		
		if( unlock_index != 0 )
		{
			bool can_bay = GameEnvironment.zombieHeads >= unlock_heads[unlock_index];
			if( can_bay)
				GUI.Box(new Rect(0.25f*Screen.width,0.25f*Screen.height,0.5f*Screen.width,0.5f*Screen.height),"Would you like to unlock this level with " + unlock_heads[unlock_index] + " ZB heads");
			else
				GUI.Box(new Rect(0.25f*Screen.width,0.25f*Screen.height,0.5f*Screen.width,0.5f*Screen.height),"You need " + unlock_heads[unlock_index] + " ZB heads to unlock this level");
			
			if(can_bay && GUI.Button(new Rect(0.35f*Screen.width,0.4f*Screen.height,0.3f*Screen.width,0.1f*Screen.height), "UNLOCK" ) )	
			{
				GameEnvironment.UnlockLevel(unlock_index);
				unlocked[unlock_index] = true;
				GameEnvironment.zombieHeads = GameEnvironment.zombieHeads - unlock_heads[unlock_index];
				levelButton[unlock_index].canPressed = unlocked[unlock_index];
				unlock_index = 0;
			}
			if( GUI.Button(new Rect(0.35f*Screen.width,0.6f*Screen.height,0.3f*Screen.width,0.1f*Screen.height), "CANCEL" ) )
				unlock_index = 0;
		}

	}
}
