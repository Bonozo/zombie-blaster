using UnityEngine;
using System.Collections;

public class GunZapper : GunBase {

	public GunCylinderBase zapper;
	
	void Start()
	{
		audio.clip = AudioFire;
	}
	
	// Update is called once per frame
	public override float ManualUpdate (Weapon weapon) 
	{
		bool canattack = true;
		if( weapon != Weapon.Zapper || !GameEnvironment.FlameButton ) canattack = false;
		if( reloading ) canattack = false;
		if( Ammo == 0 )
		{
			if( Ammo==0 && AmmoStore != 0 && weapon == Weapon.Zapper )
				Reload();
			zapper.Disappear();
			return Ammo;
		}
		
		if( !canattack )
		{
			if( !reloading ) audio.Stop();
			zapper.Disappear();
			return Ammo;
		}
		
		AmmoLost();
		
		Ray ray = LevelInfo.Environments.mainCamera.ScreenPointToRay (GameEnvironment.lastInput);
		zapper.ChangeRotation(Quaternion.LookRotation(ray.direction));
		zapper.Stream(Time.deltaTime);

		if( !audio.isPlaying) audio.Play();
		if( Ammo == 0.0f ) Reload();
		
		return Ammo;
	}
}
