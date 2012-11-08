using UnityEngine;
using System.Collections;

public class Store : MonoBehaviour {
	
	public Texture2D textureBackground;
	public Texture2D textureScreenShot;
	
	private readonly float updownbuttonheight = 0.1f*Screen.height;
	private readonly float armorydown = 0.25f*Screen.height;
	
	private static Vector2 scrollposition = Vector2.zero;
	private static int wooi = -1;
	
	private static int cost = 150;
	
	public void DrawStore()
	{	
		// Background
		GUI.DrawTexture(new Rect(0f,0f,Screen.width,Screen.height),textureBackground);
		
		// Current Currency Amount
		GUI.Box(new Rect(0,0,0.5f*Screen.width,updownbuttonheight),"Money : " + GameEnvironment.Money);
		
		// Return to Game
		if( GUI.Button(new Rect(0,Screen.height-updownbuttonheight,Screen.width*0.5f,updownbuttonheight),"Return to Game") )
		{
			wooi = -1;
			Time.timeScale = 1f;
			GameEnvironment.IgnoreButtons();
		}
		
		if( GUI.Button(new Rect(Screen.width*0.5f,Screen.height-updownbuttonheight,Screen.width*0.5f,updownbuttonheight),"Main Menu") )
		{
			Time.timeScale = 1f;
			Application.LoadLevel("mainmenu");
		}	
		if( wooi == -1 )
		{		
			// Screen Shot
			GUI.DrawTexture(new Rect(Screen.width*0.5f,updownbuttonheight,Screen.width*0.5f,Screen.height-2*updownbuttonheight),textureScreenShot);
		
			// Armory
			GUI.color = Color.white;
			GUI.Box(new Rect(0,updownbuttonheight,Screen.width*0.5f,armorydown-updownbuttonheight),"Armory");
		
			// Weapon List
			scrollposition = GUI.BeginScrollView(new Rect(0,armorydown,Screen.width*0.5f,Screen.height-armorydown-updownbuttonheight),
				scrollposition,new Rect(0,0,Screen.width*0.48f,GameEnvironment.storeGun.Length*updownbuttonheight),false,true);
		
			for(int i=0;i<GameEnvironment.storeGun.Length;i++)
			{
				
				if( GUI.Button(new Rect(0f,i*updownbuttonheight,Screen.width*0.24f,updownbuttonheight),GameEnvironment.storeGun[i].name ) )
					wooi = i;
				GUI.Box(new Rect(Screen.width*0.24f,i*updownbuttonheight,Screen.width*0.24f,updownbuttonheight),GameEnvironment.storeGun[i].AmmoInformation );
			}
		
			GUI.EndScrollView();
		}
		else
		{
			// Weapon Name
			GUI.Box(new Rect(0,updownbuttonheight,Screen.width*0.5f,updownbuttonheight),GameEnvironment.storeGun[wooi].name);
			
			// Cost of Ammo
			GUI.Box(new Rect(Screen.width*0.5f,updownbuttonheight,Screen.width*0.5f,updownbuttonheight),"Cost : " + cost);
			
			// Description
			GUI.Box(new Rect(0,2*updownbuttonheight,Screen.width*0.5f,Screen.height-3*updownbuttonheight),"Current Ammo" + GameEnvironment.storeGun[wooi].AmmoInformation);
		
			// Purchases
			if( GUI.Button(new Rect(Screen.width*0.5f,Screen.height-2*updownbuttonheight,Screen.width*0.25f,updownbuttonheight),"Purchases") ) 
			{
				if( GameEnvironment.Money >= cost )
				{
					audio.Play();
					GameEnvironment.Money -= cost;
					GameEnvironment.storeGun[wooi].store += GameEnvironment.storeGun[wooi].pocketsize;
					GameEnvironment.storeGun[wooi].enabled = true;
				}
			}
			if( GUI.Button(new Rect(Screen.width*0.75f,Screen.height-2*updownbuttonheight,Screen.width*0.25f,updownbuttonheight),"Back") ) 
			{
				wooi = -1;
			}
		}
	}
	
	void OnGUI()
	{
		if( Time.timeScale == 0.0f ) DrawStore();
	}
}
