using UnityEngine;
using System.Collections;

public class Loading : MonoBehaviour {
	
	public Texture2D[] screen;
	public string[] tip;
	public GUITexture guiFullscreen;
	public GUIText guiTip;
	
	// Use this for initialization
	void Update () {
		if( Input.touchCount>0&&Input.touches[0].phase == TouchPhase.Began || Input.GetKeyUp(KeyCode.A))
		{
			Application.LoadLevel("mainmenu");
		}
	}
}
