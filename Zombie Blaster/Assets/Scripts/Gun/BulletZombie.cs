using UnityEngine;
using System.Collections;

public class BulletZombie : MonoBehaviour {
	
	public enum FireType
	{
		Straight,
		Throw
	}
	
	public bool haveSpark=false;
	public GameObject sparks;
	public float DestroyTime = 3f;
	public FireType fireType;
	public float Speed = 5f;
	
	// Use this for initialization
	void Start () {
		
		if( fireType == FireType.Straight )
		{
			Vector3 to = LevelInfo.Environments.control.transform.position; to.y = 2f;
			transform.LookAt(to,Vector3.up);
		}
		else
		{
			Vector3 dir = LevelInfo.Environments.control.transform.position - transform.position; dir.y=1;
			rigidbody.angularVelocity = new Vector3(Random.Range(0,360f),Random.Range(0,360f),Random.Range(0,360f));
			rigidbody.AddForce(dir*Speed);
		}
		
		if(haveSpark)
			Instantiate(sparks,transform.position,transform.rotation);
		Destroy(this.gameObject,DestroyTime);
	}
	
	// Update is called once per frame
	void Update () {
		
		if( fireType == FireType.Straight )
		{
			transform.Translate(Time.deltaTime*Speed*Vector3.forward);
		}

		Vector3 v = transform.position-LevelInfo.Environments.control.transform.position;
		v.y = 0;
		if( v.magnitude <= 1f )
		{
			float damage = Mathf.Min(0.01f*LevelInfo.Environments.control.currentWave,0.1f);
			LevelInfo.Environments.control.GetHealth(damage); // Player lost health by zombie shoot
			Destroy(this.gameObject);
		}
	}
}
