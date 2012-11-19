using UnityEngine;
using System.Collections;

public class Guns : MonoBehaviour {
	
	public GunBase[] gun;
	
	public Texture2D ProgressBarFull;
	public Texture2D ProgressBarReloading;
	public Texture2D ButtonReload;
	public Texture2D ButtonPlus,ButtonMinus;
	
	private int current = 0;
	//private bool showotherbuttons = false;
	
	
	// Use this for initialization
	void Start () {	
		// initialize guns from option
		for(int i=1;i<GameEnvironment.storeGun.Length;i++)
			GameEnvironment.storeGun[i].SetEnabled(Option.WeaponsUnlocked);
		if( Option.UnlimitedAmmo )
			for(int i=0;i<GameEnvironment.storeGun.Length;i++)
				GameEnvironment.storeGun[i].SetAsUnlimited();
	}
	
	// Update is called once per frame
	void Update () {
		if( Time.timeScale == 0.0f ) return;
		foreach( GunBase g in gun ) g.ManualUpdate((Weapon)current );
	}
	
	#region OnGUI
	
	private float gbw=80f, gbh=80;
	private float pbw=50f,pbh=50f;
	// ?? // End
	
	void OnGUI()
	{
		if( LevelInfo.Environments.control.state == GameState.Play )
			DrawWeapons();
		
	}
	
	private void DrawWeapons()
	{
		// Draw Progress Bar
	    Texture2D fullTexture = gun[current].reloading ? ProgressBarReloading : ProgressBarFull;
		GUI.DrawTexture(new Rect(0,Screen.height-gbh,gbw*Mathf.Clamp01(gun[current].AmmoCurrentPercent),gbh), fullTexture);
		
		
		// Select Gun with tapping current gun icon
		if( GUI.Button(new Rect(0,Screen.height-gbh,gbw,gbh),gun[current].texture ))
		{
			GameEnvironment.IgnoreButtons();
			/*while(true)
			{
				if( ++current == gun.Length ) current = 0;
				if( gun[current].EnabledGun ) break;
			}*/
			gun[current].Reload();//New
		}
		
		// Select Plus or Minus Button
		/*var pmtex = showotherbuttons? ButtonMinus : ButtonPlus;
		if( GUI.Button(new Rect(gbw,Screen.height-pbh,pbw,pbh),pmtex) )
		{
			showotherbuttons = !showotherbuttons;
			GameEnvironment.IgnoreButtons();
		}*/
		
		/*if( showotherbuttons )
		{*/
			for(int i=0,j=0;i<gun.Length;i++)
				if(gun[i].EnabledGun)
				{
					if( GUI.Button(new Rect(gbw+pbw/*pbw*/+j*/*gbw*/pbw,Screen.height-pbh,pbw,pbh/*gbh,gbw,gbh*/),gun[i].texture) )
					{
						current = i;
						GameEnvironment.IgnoreButtons();
					}
					j++;
				}
		/*}*/
		
		// Select Reload Button
		/*if( GUI.Button(new Rect(Screen.width-gbw,Screen.height-gbh,gbw,gbh),ButtonReload ))
		{
			GameEnvironment.IgnoreButtons();
			gun[current].Reload();
		}*/
		
		// Draw Ammo Information
		//GUI.color = Color.black;
		GUI.Label(new Rect(20f,Screen.height-gbh-20f,gbw,20f),gun[current].AmmoInformation);		
	}
	
	#endregion
	
	#region Messages
	
/*	public void GetAmmo(int coef)
	{
		gun[current].GetAmmoStorePacket(coef);
	}
	
	public void GetWeapon(int gunnumber,int coef)
	{
		if( !gun[gunnumber].EnabledGun )
		{
			// Get Pocket and fast Reload
			gun[gunnumber].EnabledGun = true;
			gun[gunnumber].Ammo += gun[gunnumber].PacketSize;
			gun[gunnumber].GetAmmoStorePacket(coef-1);
		}
		else
		{
			// Get Pocket
			gun[gunnumber].GetAmmoStorePacket(coef);
		}
	}
*/	
	public void GetWeaponWithMAX(Weapon weapon)
	{
		int gunnumber = (int)weapon;
		if( !gun[gunnumber].EnabledGun )
		{
			// Get Pocket and fast Reload
			gun[gunnumber].EnabledGun = true;
			gun[gunnumber].Ammo += gun[gunnumber].PacketSize;
		}
		
		gun[gunnumber].GetAmmoStorePacketWithMaxAmmo();
	}
	
	public void GetAmmoWithMax(Weapon weapon)
	{
		gun[(int)weapon].GetAmmoStorePacketWithMaxAmmo();
	}
	
	
	#endregion
}
