using UnityEngine;
using System.Collections;

public class BulletGrenade : MonoBehaviour {
	
	public GameObject ParticleExplosion;
	
	public float DestroyTime = 4f;
	public float Y = 2.5f;
	public float ExplosionRadius = 3f;
	
	// Use this for initialization
	void Start () {
		transform.position = new Vector3(transform.position.x,Y,transform.position.z);
		rigidbody.AddForce(2000*transform.forward);
		rigidbody.angularVelocity = Random.rotation.eulerAngles;
	}
	
	// Update is called once per frame
	void Update () {
		DestroyTime -= Time.deltaTime;
		if( DestroyTime <= 0 )
			Destroy(this.gameObject);
	}
	
	void OnCollisionEnter()
	{
		GameObject[] zomb = GameObject.FindGameObjectsWithTag("Zombie");
		foreach( GameObject zombi in zomb )
		{
			if( GameEnvironment.DistXZ(zombi.transform.position,transform.position ) <= ExplosionRadius )
				zombi.SendMessage("DieWithFireAndSmoke");
		}
		
		GameObject[] barr = GameObject.FindGameObjectsWithTag("Barrel");
		foreach( GameObject zombi in barr )
		{
			if( GameEnvironment.DistXZ(zombi.transform.position,transform.position ) <= ExplosionRadius )
				zombi.SendMessage("Explode");
		}
		
		Control control = (Control)GameObject.FindObjectOfType(typeof(Control));
		if( GameEnvironment.DistXZ(control.transform.position,transform.position ) <= 2*ExplosionRadius )
			control.GetHealth(-0.2f);
		
		iTween.ShakeRotation( LevelInfo.Environments.mainCamera.gameObject,new Vector3(1f,1f,0f),0.5f);
		Instantiate(ParticleExplosion,transform.position,Quaternion.identity);
		Destroy(this.gameObject);
	}
	
}
