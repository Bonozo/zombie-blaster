using UnityEngine;
using System.Collections;

public class GunMicrowaveGun : GunBase {

/*	public GunCylinderBase microwave;
	
	private Camera mainCamera;
	
	void Start()
	{
		mainCamera = (Camera)GameObject.FindObjectOfType(typeof(Camera));
	}
	
	// Update is called once per frame
	public override float ManualUpdate (Weapon weapon) {
		Ray ray = mainCamera.ScreenPointToRay (GameEnvironment.lastInput);
		microwave.ChangeRotation(Quaternion.LookRotation(ray.direction));
		if( weapon == Weapon.Microwave && GameEnvironment.FlameButton
			&& (Ammo = Mathf.Max(0,Ammo-Time.deltaTime*AmmoReduce)) > 0f)
		{
			Ammo -= Time.deltaTime*AmmoReduce;
			if( Ammo < 0 ) Ammo = 0.0f;
			microwave.Stream(Time.deltaTime);
		}
		else
		{
			Ammo += Time.deltaTime*AmmoIncrease;
			if( Ammo > 1f ) Ammo = 1f;
			microwave.Disappear();
		}
		
		return Ammo;
	} */
}
