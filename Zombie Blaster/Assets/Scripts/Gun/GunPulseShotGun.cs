using UnityEngine;
using System.Collections;

public class GunPulseShotGun : GunBase {

	public GameObject BulletPrefab;
	
	void Start()
	{
	}
	
	// Update is called once per frame
	public override float ManualUpdate (Weapon weapon)
	{
		if( weapon != Weapon.PulseShotGun || !GameEnvironment.FireButton ) return Ammo;
		if( reloading && !LevelInfo.Environments.control.UnlimitedAmmo)
		{
			LevelInfo.Audio.audioSourcePlayer.PlayOneShot(LevelInfo.Audio.clipGunEmpty);
			return Ammo;
		}
		if( Ammo == 0 )
		{
			if( Ammo==0 && AmmoStore != 0 && weapon == Weapon.PulseShotGun)
				Reload();
			return Ammo;
		}
		AmmoLost();
		
		Shoot();
		Shoot();
		Shoot();
		Shoot();
		Shoot();
		Shoot();
		Shoot();
		
		LevelInfo.Audio.audioSourcePlayer.PlayOneShot(AudioFire);
		
		if( Ammo == 0.0f ) Reload();
		
		return Ammo;
	}
	
	void Shoot()
	{
		// Institate Bullet
		Vector3 lastinputnext = GameEnvironment.lastInput;
		float peturb = (100f-GameEnvironment.storeGun[(int)Weapon.PulseShotGun].accuracy)/20f;
		float phb = Mathf.Min(Screen.width,Screen.height)*peturb*0.01f;
		lastinputnext += new Vector3( Random.Range(-5*phb,5*phb) , Random.Range(-phb,phb),0f);
		Ray ray = LevelInfo.Environments.mainCamera.ScreenPointToRay (lastinputnext);
		
		RaycastHit hit;
		Physics.Raycast(ray.origin,ray.direction,out hit);
		GameObject g = (GameObject)Instantiate(BulletPrefab,transform.position,Quaternion.identity);
		g.transform.LookAt(RaycastsTargetPosition(LevelInfo.Environments.mainCamera,ray,hit),Vector3.up);		
	}
}
