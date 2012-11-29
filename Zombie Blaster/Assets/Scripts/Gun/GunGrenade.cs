using UnityEngine;
using System.Collections;

public class GunGrenade : GunBase {

	public GameObject BulletPrefab;

	void Start()
	{
	}
	
	public override float ManualUpdate (Weapon weapon) 
	{
		if( weapon != Weapon.Grenade || !GameEnvironment.FireButton ) return Ammo;
		if( reloading ) return Ammo;
		if( Ammo == 0 )
		{
			if( Ammo==0 && AmmoStore != 0 && weapon == Weapon.Grenade )
				Reload();
			return Ammo;
		}
		AmmoLost();
		
		// Calculate Fire Direction
		Ray ray = LevelInfo.Environments.mainCamera.ScreenPointToRay (GameEnvironment.lastInput);
		Quaternion q = Quaternion.LookRotation(ray.direction);
			
		// Institate Bullet
		Instantiate(BulletPrefab,transform.position,q);	
		audio.PlayOneShot(AudioFire);
		
		if( Ammo == 0.0f ) Reload();
		
		return Ammo;
	}	
}
