using UnityEngine;
using System.Collections;

public class ColorPlay : MonoBehaviour {
	
	public Color colmin = new Color(0.5f,0.5f,0.5f,0.5f),colmax = new Color(0.5f,0.5f,0.5f,0.5f);
	public float speed = 0.1f;
	
	private bool up=true;
	private float percent = 0f;
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		percent += Time.deltaTime * speed * (up?1:-1);
		if(percent>1||percent<0) up=!up;
		
		guiTexture.color = new Color(colmin.r + percent*(colmax.r-colmin.r),colmin.g + percent*(colmax.g-colmin.g),colmin.b + percent*(colmax.b-colmin.b),0.5f );
	}
}
