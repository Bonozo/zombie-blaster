using UnityEngine;
using System.Collections;

public class GunPulseShotGun : GunBase {

	public GameObject BulletPrefab;
	
	private Camera mainCamera;
	
	void Start()
	{
		mainCamera = (Camera)GameObject.FindObjectOfType(typeof(Camera));
	}
	
	// Update is called once per frame
	public override float ManualUpdate (Weapon weapon)
	{
		if( weapon != Weapon.PulseShotGun || !GameEnvironment.FireButton ) return Ammo;
		if( reloading ) return Ammo;
		if( Ammo == 0 )
		{
			if( Ammo==0 && AmmoStore != 0 && weapon == Weapon.PulseShotGun)
				Reload();
			return Ammo;
		}
		AmmoLost();
		
		// Institate Bullet
		Ray ray = mainCamera.ScreenPointToRay (GameEnvironment.lastInput);	
		RaycastHit hit;
		Physics.Raycast(ray.origin,ray.direction,out hit);
		GameObject g = (GameObject)Instantiate(BulletPrefab,transform.position,Quaternion.identity);
		g.transform.LookAt(RaycastsTargetPosition(mainCamera,ray,hit),Vector3.up);	
		audio.PlayOneShot(AudioFire);
		
		if( Ammo == 0.0f ) Reload();
		
		return Ammo;
	}
}
