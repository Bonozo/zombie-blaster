using UnityEngine;
using System.Collections;

public class GunFlamethrower : GunBase {

	public GameObject FlameThrower;
	//public GunCylinderBase FlameThrowerTarget;
	public float MaxEffectDistance = 10;
	
	
	void Start()
	{
		audio.clip = AudioFire;
	}
	
	// Update is called once per frame
	public override float ManualUpdate (Weapon weapon) 
	{
		FlameThrower.particleEmitter.emit = false;
		bool canattack = true;
		if( weapon != Weapon.Flamethrower || !GameEnvironment.FlameButton ) canattack = false;
		if( reloading ) canattack = false;
		if( Ammo == 0 )
		{
			if( Ammo==0 && AmmoStore != 0 && weapon == Weapon.Flamethrower)
				Reload();
			//FlameThrowerTarget.Disappear();
			return Ammo;
		}
		
		if( !canattack )
		{
			if( !reloading ) audio.Stop();
			//FlameThrowerTarget.Disappear();
			return Ammo;
		}
		
		AmmoLost();
		
		Ray ray = LevelInfo.Environments.mainCamera.ScreenPointToRay (GameEnvironment.lastInput);

		//FlameThrowerTarget.ChangeRotation(Quaternion.LookRotation(ray.direction));
		FlameThrower.transform.rotation = Quaternion.LookRotation(ray.direction);
		FlameThrower.particleEmitter.emit = true;
		//FlameThrowerTarget.Stream(Time.deltaTime);
		//FlameThrowerTarget.Clamp(MaxEffectDistance);
		
		if( !audio.isPlaying) audio.Play();
		if( Ammo == 0.0f ) Reload();
		
		return Ammo;
	}
	
}