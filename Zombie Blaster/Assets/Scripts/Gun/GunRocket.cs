using UnityEngine;
using System.Collections;

public class GunRocket : GunBase {

	public GameObject BulletPrefab;
	
	private Camera mainCamera;

	void Start()
	{
		mainCamera = (Camera)GameObject.FindObjectOfType(typeof(Camera));
	}
	
	public override float ManualUpdate (Weapon weapon) 
	{
		if( weapon != Weapon.Rocket || !GameEnvironment.FireButton ) return Ammo;
		if( reloading ) return Ammo;
		if( Ammo == 0 )
		{
			if( Ammo==0 && AmmoStore != 0 && weapon == Weapon.Rocket )
				Reload();
			return Ammo;
		}
		AmmoLost();
		
		// Calculate Fire Direction
		Ray ray = mainCamera.ScreenPointToRay (GameEnvironment.lastInput);
		Quaternion q = Quaternion.LookRotation(ray.direction);
			
		// Institate Bullet
		Instantiate(BulletPrefab,transform.position,q);	
		audio.PlayOneShot(AudioFire);
		
		if( Ammo == 0.0f ) Reload();
		
		return Ammo;
	}	
}
