using UnityEngine;
using System.Collections;

public class ButtonWeapon : MonoBehaviour {
	
	public UISprite weaponSprite;
	public UISprite percentSprite;
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
		Vector3 v = percentSprite.transform.localScale;
		v.x = 80f*LevelInfo.Environments.guns.gun[weaponIndex].AmmoCurrentPercent;
		percentSprite.transform.localScale = v;
		
		// Weapon Icon
		weaponSprite.spriteName = "weapon_" + GameEnvironment.storeGun[weaponIndex].name;
	}
}
