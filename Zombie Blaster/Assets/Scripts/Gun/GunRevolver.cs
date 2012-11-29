using UnityEngine;
using System.Collections;

public class GunRevolver : GunBase {
	
	public GameObject BulletPrefab;
	

	void Start()
	{
	}
	
	public override float ManualUpdate (Weapon weapon) 
	{
		if( weapon != Weapon.Revolver) return Ammo;
		
		Vector3 lastinputnext = GameEnvironment.lastInput;
		Ray ray = LevelInfo.Environments.mainCamera.ScreenPointToRay (lastinputnext);	
		
		//lastinputnext = GameEnvironment.lastInput01; lastinputnext.y -= 0.2f;
		lastinputnext.x /= Screen.width;
		lastinputnext.y /= Screen.height;
		
		if( GameEnvironment.TouchedScreen ) {}
		
		if( !GameEnvironment.FireButton ) return Ammo;
		if( reloading ) return Ammo;
		if( Ammo == 0 )
		{
			if( Ammo==0 && AmmoStore != 0 && weapon == Weapon.Revolver )
				Reload();
			return Ammo;
		}
		AmmoLost();
		
		
		RaycastHit hit;
		Physics.Raycast(ray.origin,ray.direction,out hit);
		GameObject g = (GameObject)Instantiate(BulletPrefab,transform.position,Quaternion.identity);
		g.transform.LookAt(RaycastsTargetPosition(LevelInfo.Environments.mainCamera,ray,hit),Vector3.up);
		audio.PlayOneShot(AudioFire);
		
		if( Ammo == 0.0f ) Reload();
		
		return Ammo;
	}
}
