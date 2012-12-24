using UnityEngine;
using System.Collections;

public class ButtonWeapon : MonoBehaviour {
	
	public UISprite weaponSprite;
	public UIFilledSprite percentSprite;
	public int weaponIndex = 0;
	
	void OnPress(bool isDown)
	{
		if(isDown)
		{
			GameEnvironment.IgnoreButtons();
			LevelInfo.Environments.guns.CurrentWeaponIndex = weaponIndex;
		}
	}
	
	void Update()
	{
		// Percent
		percentSprite.fillAmount = LevelInfo.Environments.guns.gun[weaponIndex].AmmoCurrentPercent;
		
		// Icon and Colors
		percentSprite.spriteName = weaponSprite.spriteName = "weapon_" + GameEnvironment.storeGun[weaponIndex].name;
		percentSprite.color = Color.red;
	}
}
