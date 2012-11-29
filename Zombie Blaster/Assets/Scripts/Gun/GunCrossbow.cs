using UnityEngine;
using System.Collections;

public class GunCrossbow : GunBase{
	
	public GameObject BulletPrefab;
	public GUITexture scopeTarget;
	
	private float waitforscopetarget = 0.5f;

	void Start()
	{
	}
	
	public override float ManualUpdate (Weapon weapon) 
	{
		scopeTarget.enabled = false;
		
		if( weapon != Weapon.Crossbow ) return Ammo;
		
		if( GameEnvironment.TouchedScreen )
		{
			waitforscopetarget -= Time.deltaTime;
			if( waitforscopetarget <= 0f )
			{
				scopeTarget.transform.position = GameEnvironment.lastInput01;
				scopeTarget.enabled = true;
			}
		}
		else
			waitforscopetarget = 0.5f;
		
		if( !GameEnvironment.FireButton ) return Ammo;
		if( reloading ) return Ammo;
		if( Ammo == 0 )
		{
			if( Ammo==0 && AmmoStore != 0 && weapon == Weapon.Crossbow)
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
		
		waitforscopetarget = 0.5f;
		
		return Ammo;
	}
}
