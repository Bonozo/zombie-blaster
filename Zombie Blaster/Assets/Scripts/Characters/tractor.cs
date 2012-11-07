using UnityEngine;
using System.Collections;

public class tractor : MonoBehaviour {
	
	public GameObject FlameParticle;
	public int countToExplode = 10;
	private bool exploded = false;
	
	// Use this for initialization
	void Start () {
		gameObject.tag = "Barrel";
		FlameParticle.particleEmitter.maxSize = FlameParticle.particleEmitter.minSize = 15f;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void Explode()
	{
		if( exploded ) return;
		exploded = true;
		Instantiate(FlameParticle,transform.position,Quaternion.identity);
	}
	
	void OnCollisionEnter(Collision col)
	{		
		//  PulseShotGun Attack
		if( HitWithName(col.gameObject.name,"BulletPulseShotGun") ||
			HitWithName(col.gameObject.name,"BulletAirgun")  ||
			HitWithName(col.gameObject.name,"BulletCrossbow") )
			if(--countToExplode == 0 )
				Explode();
	}
	
	private bool HitWithName(string name,string comparewith)
	{
		return name.Length >= comparewith.Length && name.Substring(0,comparewith.Length) == comparewith;
	}
}
