using UnityEngine;
using System.Collections;

public class GunFootball : GunBase {

	public GameObject BulletPrefab;
	
	
	void Start()
	{
	}
	
	// Update is called once per frame
	public override float ManualUpdate (Weapon weapon)
	{
		if( weapon != Weapon.Football || !GameEnvironment.FireButton ) return Ammo;
		if( reloading ) return Ammo;
		if( Ammo == 0 )
		{
			if( Ammo==0 && AmmoStore != 0 && weapon == Weapon.Football)
				Reload();
			return Ammo;
		}
		AmmoLost();
		
		// Institate Bullet
		Ray ray = LevelInfo.Environments.mainCamera.ScreenPointToRay (GameEnvironment.lastInput);	
		RaycastHit hit;
		Physics.Raycast(ray.origin,ray.direction,out hit);
		GameObject g = (GameObject)Instantiate(BulletPrefab,transform.position,Quaternion.identity);
		g.transform.LookAt(RaycastsTargetPosition(LevelInfo.Environments.mainCamera,ray,hit),Vector3.up);	
		audio.PlayOneShot(AudioFire);
		
		if( Ammo == 0.0f ) Reload();
		
		return Ammo;
	}
}
