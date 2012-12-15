using UnityEngine;
using System.Collections;

public class ExplosibleObject : MonoBehaviour {
	
	public GameObject FlameParticle;
	public Transform explodeTransform;
	public int countToExplode = 1;
	private bool exploded = false;
	public bool destroyWhenExplode = true;
	public float explodeSize = 5;
	
	// Use this for initialization
	void Start () {
		gameObject.tag = "Explosible";
		FlameParticle.particleEmitter.maxSize = FlameParticle.particleEmitter.minSize = explodeSize;
		if( explodeTransform == null ) explodeTransform = gameObject.transform;
	}
	
	void Explode()
	{
		if( exploded ) return;
		exploded = true;
		Instantiate(FlameParticle,explodeTransform.position,Quaternion.identity);
		if(destroyWhenExplode ) Destroy(this.gameObject);
	}
	
	void OnCollisionEnter(Collision col)
	{		
		if( exploded ) return;
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
