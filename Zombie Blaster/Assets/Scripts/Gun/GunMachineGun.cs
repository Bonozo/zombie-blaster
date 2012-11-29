using UnityEngine;
using System.Collections;

public class GunMachineGun : GunBase {

	public GameObject BulletPrefab;
	public float DeltaWait = 0.1f;
	
	private float deltawait = 0.0f;

	void Start()
	{
	}
	
	public override float ManualUpdate (Weapon weapon) 
	{
		deltawait -= Time.deltaTime;
		if( deltawait > 0 ) return Ammo;
		if( weapon != Weapon.MachineGun || !GameEnvironment.FlameButton ) return Ammo;
		if( reloading ) return Ammo;
		if( Ammo == 0 )
		{
			if( Ammo==0 && AmmoStore != 0 && weapon == Weapon.MachineGun )
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
		
		deltawait = DeltaWait;
		
		return Ammo;
	}
}
