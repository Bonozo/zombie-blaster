using UnityEngine;
using System.Collections;

public class ButtonCurrentWeapon : MonoBehaviour {
	
	public UILabel ammoInformation;
	public UISprite weaponSprite;
	public UIFilledSprite percentSprite;
	
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
		percentSprite.fillAmount = LevelInfo.Environments.guns.CurrentWeapon.AmmoCurrentPercent;
		
		// Icon and Colors
		percentSprite.spriteName = weaponSprite.spriteName = "weapon_" + GameEnvironment.storeGun[LevelInfo.Environments.guns.CurrentWeaponIndex].name;
		percentSprite.color = LevelInfo.Environments.guns.CurrentWeapon.reloading?Color.green:Color.red;
		
		// Ammo Information
		ammoInformation.text = LevelInfo.Environments.guns.CurrentWeapon.AmmoInformation;
		
	}
}

/*
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
*/