using UnityEngine;
using System.Collections;

public class CivilianGenerator : MonoBehaviour {
	
	public GameObject civilianPrefab;
	float deltatime = 3f;
	
	private Control control;
	
	// Use this for initialization
	void Start () {
		control = (Control)GameObject.FindObjectOfType(typeof(Control));
	}
	
	// Update is called once per frame
	void Update () {
		deltatime -= Time.deltaTime;
		if( deltatime <= 0f )
		{
			if( GameObject.FindObjectOfType(typeof(civilian)) == null )
				Instantiate(civilianPrefab,RandomPosition(),Quaternion.identity);
			deltatime = Random.Range(5f,10f);
		}
	}
					
	Vector3 RandomPosition()
	{
		//return new Vector3(0,0,8);
		float r = Random.Range(8f,10f);
		float alpa = Random.Range(0f,360f);
		Vector3 v = Vector3.zero;
		v.x = r*Mathf.Cos(alpa);
		v.z = r*Mathf.Sin(alpa);
		return v+control.transform.position;
	}
}
