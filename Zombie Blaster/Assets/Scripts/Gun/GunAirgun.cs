using UnityEngine;
using System.Collections;

public class GunAirgun : GunBase {
	
	public GameObject BulletPrefab;
	public GUITexture scopeTarget;
	
	private Camera mainCamera;
	private float dt = 0.5f;

	void Start()
	{
		mainCamera = (Camera)GameObject.FindObjectOfType(typeof(Camera));
	}
	
	public override float ManualUpdate (Weapon weapon) 
	{
		Vector3 lastinputnext = GameEnvironment.lastInput;
		Ray ray = mainCamera.ScreenPointToRay (lastinputnext);	
		
		//lastinputnext = GameEnvironment.lastInput01; lastinputnext.y -= 0.2f;
		lastinputnext.x /= Screen.width;
		lastinputnext.y /= Screen.height;
		
		scopeTarget.enabled = false;
		
		if( weapon != Weapon.BB) return Ammo;
		
		if( GameEnvironment.TouchedScreen )
		{
			scopeTarget.transform.position = lastinputnext;
			scopeTarget.enabled = true;
			dt -= Time.deltaTime;
		}
		else
			dt = 0.5f;
		
		if( !GameEnvironment.FireButton && dt > 0f) return Ammo;
		if( reloading ) return Ammo;
		if( Ammo == 0 )
		{
			if( Ammo==0 && AmmoStore != 0 && weapon == Weapon.BB )
				Reload();
			return Ammo;
		}
		AmmoLost();
		
		RaycastHit hit;
		Physics.Raycast(ray.origin,ray.direction,out hit);
		
		GameObject g = (GameObject)Instantiate(BulletPrefab,transform.position,Quaternion.identity);
		g.transform.LookAt(RaycastsTargetPosition(mainCamera,ray,hit),Vector3.up);
		audio.PlayOneShot(AudioFire);
		
		if( Ammo == 0.0f ) Reload();
		dt = 0.5f;
		
		return Ammo;
	}
}
