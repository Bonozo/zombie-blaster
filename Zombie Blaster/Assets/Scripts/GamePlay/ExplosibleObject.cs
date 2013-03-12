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
		LevelInfo.Environments.control.Shake();
		if(destroyWhenExplode ) Destroy(this.gameObject);
	}
	
	void OnCollisionEnter(Collision col)
	{		
		if( exploded ) return;
		if( col.gameObject.tag == "Bullet" )
			if(--countToExplode == 0 )
				Explode();
	}
}
