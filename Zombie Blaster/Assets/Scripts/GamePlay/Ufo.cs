using UnityEngine;
using System.Collections;

public class Ufo : MonoBehaviour {
	
	public float Speed = 10f;
	public float ExplosionRadius = 3f;
	public GameObject particleExplode;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 pos = new Vector3(0,transform.position.y,0);
		transform.RotateAround(pos,Vector3.up,Time.deltaTime*Speed);
	}
	
	void OnCollisionEnter(Collision col)
	{
		if( col.gameObject.tag == "Bullet" )
		{
			if(!rigidbody)
			{
				gameObject.AddComponent("Rigidbody");
				LevelInfo.Environments.control.GetScore(LevelInfo.State.scoreForUFO,true);
			}
		}
		else
		{
			
			Vector3 exppos = col.contacts[0].point;
			Instantiate(particleExplode,exppos,Quaternion.identity);
					GameObject[] zomb = GameObject.FindGameObjectsWithTag("Zombie");
		
			foreach( GameObject zombi in zomb )
				if( GameEnvironment.DistXZ(zombi.transform.position,transform.position ) <= ExplosionRadius )
					zombi.SendMessage("DieWithFireAndSmoke");
		
			GameObject[] barr = GameObject.FindGameObjectsWithTag("Explosible");
			foreach( GameObject zombi in barr )
				if( GameEnvironment.DistXZ(zombi.transform.position,transform.position ) <= ExplosionRadius )
					zombi.SendMessage("Explode");
				
			LevelInfo.Environments.control.Shake();
			Destroy(this.gameObject);
		}
	}
}
