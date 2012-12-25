using UnityEngine;
using System.Collections;

public class Loading : MonoBehaviour {
	
	public Texture2D[] screen;
	public string[] tip;
	public GUITexture guiFullscreen;
	public GUIText guiTip;
	
	void Awake()
	{
		((Store)GameObject.FindObjectOfType(typeof(Store))).showStore = false;
		guiFullscreen.texture = screen[Random.Range(0,screen.Length)];
		guiTip.text = tip[Random.Range(0,tip.Length)];
	}
	
	// Use this for initialization
	void Start () {
		Application.LoadLevel(GameEnvironment.loadingLevel);
	}
}
