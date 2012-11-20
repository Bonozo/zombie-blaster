using UnityEngine;
using System.Collections;

public class barrel : MonoBehaviour {
	
	public GameObject FlameParticle;
	
	// Use this for initialization
	void Start () {
		gameObject.tag = "Barrel";
		FlameParticle.particleEmitter.maxSize = FlameParticle.particleEmitter.minSize = 5f;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void Explode()
	{
		Instantiate(FlameParticle,transform.position,Quaternion.identity);
		Destroy(this.gameObject);
	}
	
	void OnCollisionEnter(Collision col)
	{	
		//  PulseShotGun Attack
		if( HitWithName(col.gameObject.name,"BulletPulseShotGun") || col.gameObject.tag == "Bullet" )
		{
			Explode();
			return;
		}
		
		// Airgun Attack
		if( HitWithName(col.gameObject.name,"BulletAirgun") )
		{
			Explode();
			return;
		} 
		
		// Crossbow Attack
		if( HitWithName(col.gameObject.name,"BulletCrossbow") )
		{
			Explode();
			return;
		} 
	}
	
	private bool HitWithName(string name,string comparewith)
	{
		return name.Length >= comparewith.Length && name.Substring(0,comparewith.Length) == comparewith;
	}
}
