using UnityEngine;
using System.Collections;

public class ShowMessage : MonoBehaviour {
	
	public float ShowTime = 1f;
	public float UpSpeed = 0.001f;
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate(0f,UpSpeed*Time.deltaTime,0f);
		ShowTime -= Time.deltaTime;
		if( ShowTime <= 0 )
			Destroy(this.gameObject);
	}

	public void ChangeMessage(string message)
	{
		guiText.text = message;
	}
}
