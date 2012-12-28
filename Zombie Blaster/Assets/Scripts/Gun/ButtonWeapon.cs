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
		float p = LevelInfo.Environments.guns.gun[weaponIndex].AmmoCurrentPercent;
		percentSprite.fillAmount = p;
		
		// Icon and Colors
		percentSprite.spriteName = weaponSprite.spriteName = "weapon_" + GameEnvironment.storeGun[weaponIndex].name;
		
		if( LevelInfo.Environments.guns.gun[weaponIndex].reloading)
			percentSprite.color = new Color(p,1f,p,1f);
		else
			percentSprite.color = new Color(1f,p,p,1f);
	}
}
