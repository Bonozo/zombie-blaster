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
		if( reloading && !LevelInfo.Environments.control.UnlimitedAmmo)
		{
			
			LevelInfo.Audio.audioSourcePlayer.PlayOneShot(LevelInfo.Audio.clipGunEmpty);
			return Ammo;
		}
		if( Ammo == 0 )
		{
			if( Ammo==0 && AmmoStore != 0 && weapon == Weapon.Grenade )
				Reload();
			return Ammo;
		}
		AmmoLost();
		
		// Calculate Fire Direction
		Vector3 lastinputnext = GameEnvironment.lastInput;
		float peturb = (100f-GameEnvironment.storeGun[(int)Weapon.Grenade].accuracy)/20f;
		float phb = Mathf.Min(Screen.width,Screen.height)*peturb*0.01f;
		lastinputnext += new Vector3( Random.Range(-phb,phb) , Random.Range(-phb,phb),0f);
		
		Ray ray = LevelInfo.Environments.mainCamera.ScreenPointToRay (lastinputnext);
		Quaternion q = Quaternion.LookRotation(ray.direction);
			
		// Institate Bullet
		Instantiate(BulletPrefab,transform.position,q);	
		LevelInfo.Audio.audioSourcePlayer.PlayOneShot(AudioFire);
		
		if( Ammo == 0.0f ) Reload();
		
		return Ammo;
	}	
}
