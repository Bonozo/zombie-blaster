using UnityEngine;
using System.Collections;

public class GunSniper : GunBase {
	
	public GameObject BulletPrefab;
	public GUITexture scopeTarget;

	void Start()
	{
	}
	
	public override float ManualUpdate (Weapon weapon) 
	{
		Vector3 lastinputnext = GameEnvironment.lastInput; lastinputnext.y -= 0.2f*Screen.height;
		Ray ray = LevelInfo.Environments.mainCamera.ScreenPointToRay (lastinputnext);	
		
		//lastinputnext = GameEnvironment.lastInput01; lastinputnext.y -= 0.2f;
		lastinputnext.x /= Screen.width;
		lastinputnext.y /= Screen.height;
		
		RaycastHit hit;
		Physics.Raycast(ray.origin,ray.direction,out hit);
		
		scopeTarget.enabled = false;
		
		if( weapon != Weapon.Sniper) return Ammo;
		
		if( GameEnvironment.TouchedScreen )
		{
			scopeTarget.transform.position = lastinputnext;
			scopeTarget.enabled = true;
		}
		
		if( !GameEnvironment.FireButton ) return Ammo;
		if( reloading ) return Ammo;
		if( Ammo == 0 )
		{
			if( Ammo==0 && AmmoStore != 0 && weapon == Weapon.Sniper )
				Reload();
			return Ammo;
		}
		AmmoLost();
		
		GameObject g = (GameObject)Instantiate(BulletPrefab,transform.position,Quaternion.identity);
		g.transform.LookAt(RaycastsTargetPosition(LevelInfo.Environments.mainCamera,ray,hit),Vector3.up);
		g.SendMessage("SetReticleTarget",lastinputnext);
		audio.PlayOneShot(AudioFire);
		
		if( Ammo == 0.0f ) Reload();
		
		return Ammo;
	}
}
