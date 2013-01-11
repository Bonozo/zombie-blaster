using UnityEngine;
using System.Collections;

public class Guns : MonoBehaviour {
	
	#region Methods
	
	public void ReloadCurrentWeapon()
	{
		gun[current].Reload();
	}
	
	public float CurrentWeaponsAmmoPercent()
	{
		return gun[current].AmmoCurrentPercent;
	}
	
	public GunBase CurrentWeapon { get { return gun[current]; } }
	public int CurrentWeaponIndex { get { return current; } set { current = value; }}
	
	#endregion
	
	public GunBase[] gun;
	
	public ButtonWeapon[] weaponsUI;
	
	/*public tk2dSprite currentGun;
	public tk2dSprite currentFull;
	public tk2dButton currentButton;*/
	
	public Texture2D ProgressBarFull;
	public Texture2D ProgressBarReloading;
	public Texture2D ButtonReload;
	public Texture2D ButtonPlus,ButtonMinus;
	
	/*public GUIText guiTextAmmoInformation;
	public GUITexture guiTextureCurrentWeaponPercent;
	public GUITexture guiTextureCurrentGun;*/
	
	private int current = 0;
	//private bool showotherbuttons = false;
	
	
	// Use this for initialization
	void Start () {	
		// initialize guns from option
		
		for(int i=0;i<GameEnvironment.storeGun.Length;i++)
			GameEnvironment.storeGun[i].SetEnabled(false);
		
		Weapon[] w = LevelInfo.State.level[GameEnvironment.StartLevel].allowedGun;
		for(int i=0;i<w.Length;i++)
		{
			GameEnvironment.storeGun[(int)w[i]].SetEnabled(Store.WeaponUnlocked((int)w[i]));
		}
		if( Option.UnlimitedAmmo )
			for(int i=0;i<GameEnvironment.storeGun.Length;i++)
				GameEnvironment.storeGun[i].SetAsUnlimited();
	}
	
	// Update is called once per frame
	public void Update () {
		
		if( Time.timeScale == 0.0f ) 
		{
			foreach( GunBase g in gun ) g.ManualUpdate(Weapon.None);
			return;
		}
		foreach( GunBase g in gun ) g.ManualUpdate((Weapon)current );
		
		int cweapon = 0;
		for(int i=0;i<gun.Length;i++)
				if(gun[i].EnabledGun && i!=current)
				{
					weaponsUI[cweapon].gameObject.SetActive(true);
					weaponsUI[cweapon].weaponIndex = i;
					cweapon++;
				}
		for(;cweapon<weaponsUI.Length;cweapon++) weaponsUI[cweapon].gameObject.SetActive(false);
		
		//if( GameEnvironment.storeGun[9].enabled )
		//	Debug.Log("OK 2");
		//for(int i=0;i<gun.Length;i++)
		//	if( gun[i].EnabledGun )
		//		Debug.Log("Enabled " + i);
	}
		
	bool pressed(GUITexture button)
	{
		if( LevelInfo.Environments.control.state == GameState.Play )
		{
			foreach(Touch touch in Input.touches)
				if( touch.phase == TouchPhase.Ended && button.HitTest(touch.position) )
				{
					GameEnvironment.IgnoreButtons();
					return true;
				}
		}
		
		if( Input.GetMouseButtonUp(0) && button.HitTest(Input.mousePosition) )
		{
			GameEnvironment.IgnoreButtons();
			return true;	
		}
		return false;
	}
	#region OnGUI
	
	//private float gbw=80f, gbh=80;
	//private float pbw=50f,pbh=50f;
	
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
			LevelInfo.Environments.control.ShowStoreNotifiaction();
		}
		
		gun[gunnumber].GetAmmoStorePacketWithMaxAmmo();
	}
	
	public void GetAmmoWithMax(Weapon weapon)
	{
		gun[(int)weapon].GetAmmoStorePacketWithMaxAmmo();
	}
	
	
	#endregion
}
