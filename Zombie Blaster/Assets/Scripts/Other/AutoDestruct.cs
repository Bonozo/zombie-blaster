using UnityEngine;
using System.Collections;

public class AutoDestruct : MonoBehaviour {
	
	public float time=1f;
	
	// Use this for initialization
	void Start () {
		Destroy(this.gameObject,time);
	}
}
