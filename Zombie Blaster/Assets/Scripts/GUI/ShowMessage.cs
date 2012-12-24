using UnityEngine;
using System.Collections;

public class ShowMessage : MonoBehaviour {
	
	public float ShowTime = 1f;
	public float UpSpeed = 0.001f;
	private bool up = true;
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		guiText.enabled = Time.timeScale>0;
		if(up)
			transform.Translate(0f,UpSpeed*Time.deltaTime,0f);
		ShowTime -= Time.deltaTime;
		if( ShowTime <= 0 )
			Destroy(this.gameObject);
	}

	public void ChangeMessage(string message)
	{
		guiText.text = message;
	}
	
	public void ShowMessageText2D(string message,bool up,float time)
	{
		ShowTime = time;
		this.up = up;
		guiText.text = message;
	}
}
