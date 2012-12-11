using UnityEngine;
using System.Collections;

public class ButtonCurrentWeapon : MonoBehaviour {
	
	public UILabel ammoInformation;
	public UISprite weaponSprite;
	public UISprite percentSprite;
	
	void OnPress(bool isDown)
	{
		if(isDown)
		{
			GameEnvironment.IgnoreButtons();
			LevelInfo.Environments.guns.ReloadCurrentWeapon();
		}
	}
	
	void Update()
	{
		// Percent
		Vector3 v = percentSprite.transform.localScale;
		v.x = 80f*LevelInfo.Environments.guns.CurrentWeapon.AmmoCurrentPercent;
		percentSprite.transform.localScale = v;
		
		// Percent Color
		percentSprite.spriteName = LevelInfo.Environments.guns.CurrentWeapon.reloading?"backrose":"backred";
		
		// Weapon Icon
		weaponSprite.spriteName = "weapon_" + GameEnvironment.storeGun[LevelInfo.Environments.guns.CurrentWeaponIndex].name;
		
		// Ammo Information
		ammoInformation.text = LevelInfo.Environments.guns.CurrentWeapon.AmmoInformation;
		
	}
}
