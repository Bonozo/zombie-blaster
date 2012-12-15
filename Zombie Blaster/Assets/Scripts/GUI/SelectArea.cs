using UnityEngine;
using System.Collections;

public class SelectArea : MonoBehaviour {
	
	public ButtonBase[] levelButton;
	public Texture headSack;
	
	public Texture2D lockedTexture;
	public GUITexture loadingTexture;
	public GUITexture[] bloods;
	public GUIText headsText;
	
	private bool[] unlocked = new bool[5];
	private int[] unlock_heads = new int [5] {0,500,1000,1500,2000};
	private int unlock_index = 0;
	private int countUnlocked=0;
	
	private void UpdateBlood()
	{
		foreach(var g in bloods )
		{
			Color c = g.color;
			c.a = 0.115f*(countUnlocked-1);
			g.color = c;
		}
	}
	
	private Store store;
	
	// Use this for initialization
	void Start () {
		
		store = (Store)GameObject.FindObjectOfType(typeof(Store));
		
		GameEnvironment.StartLevel = 0;
		loadingTexture.enabled = false;
		for(int i=0;i<Store.countLevel;i++)
		{
			unlocked[i] = Store.LevelUnlocked(i);
			if(unlocked[i]) countUnlocked++;
		}
		UpdateBlood();

		
		for(int i=0;i<levelButton.Length;i++)
			levelButton[i].canPressed = unlocked[i];
	}
	
	void Update()
	{
		headsText.text = "" + Store.zombieHeads;
		if( unlock_index != 0 ) return;
		
		for(int i=0;i<levelButton.Length;i++)
			if( !unlocked[i] && levelButton[i].PressedUp )
			{
				unlock_index = i;
				return;
			}
		
		for(int i=0;i<levelButton.Length;i++)
			if( unlocked[i] && levelButton[i].PressedUp )
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
			bool can_bay = Store.zombieHeads >= unlock_heads[unlock_index];

			GUI.Box(new Rect(0.25f*Screen.width,0.25f*Screen.height,0.5f*Screen.width,0.5f*Screen.height),"");
			GUI.Label(new Rect(0.45f*Screen.width,0.305f*Screen.height,0.1f*Screen.width,0.1f*Screen.height),"" + unlock_heads[unlock_index]);
			GUI.DrawTexture(new Rect(0.5f*Screen.width,0.29f*Screen.height,0.08f*Screen.width,0.08f*Screen.height),headSack);
			
			int cc = 0;
			for(int i=0; i<Store.weaponsForLevel[unlock_index].Length; i++)
				if( Store.WeaponUnlocked((int)Store.weaponsForLevel[unlock_index][i]) )
					cc++;
			for(int i=0,j=0; i<Store.weaponsForLevel[unlock_index].Length; i++)
				if( Store.WeaponUnlocked((int)Store.weaponsForLevel[unlock_index][i]) )
				{
				if( j < 5 )
					GUI.DrawTexture(new Rect( (0.5f-0.05f*Mathf.Min(cc,5)+0.1f*j)*Screen.width,0.41f*Screen.height,0.08f*Screen.width,0.08f*Screen.height),store.weaponIcon[(int)Store.weaponsForLevel[unlock_index][i]]);
				else
					GUI.DrawTexture(new Rect( (0.5f-0.05f*(cc-5)+0.1f*(j-5))*Screen.width,0.51f*Screen.height,0.08f*Screen.width,0.08f*Screen.height),store.weaponIcon[(int)Store.weaponsForLevel[unlock_index][i]]);
					j++;
				}
			
			if( can_bay)
			{
				if(GUI.Button(new Rect(0.26f*Screen.width,0.6f*Screen.height,0.19f*Screen.width,0.1f*Screen.height), "UNLOCK" ) )	
				{
					Store.UnlockLevel(unlock_index);
					unlocked[unlock_index] = true;
					countUnlocked++;
					UpdateBlood();
					Store.zombieHeads = Store.zombieHeads - unlock_heads[unlock_index];
					levelButton[unlock_index].canPressed = unlocked[unlock_index];
					unlock_index = 0;
				}
			}
			else
			{
				if( GUI.Button(new Rect(0.26f*Screen.width,0.6f*Screen.height,0.19f*Screen.width,0.1f*Screen.height),"GET MORE HEADS"))
					store.Get1000HeadsEvent();
			}
	
			if( GUI.Button(new Rect(0.54f*Screen.width,0.6f*Screen.height,0.19f*Screen.width,0.1f*Screen.height), "CANCEL" ) )
				unlock_index = 0;
		}

	}
}
