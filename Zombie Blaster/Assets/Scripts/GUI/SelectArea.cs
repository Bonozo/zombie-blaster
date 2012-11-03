using UnityEngine;
using System.Collections;

public class SelectArea : MonoBehaviour {
	
	public Texture2D[] area;
	public Texture2D lockedTexture;
	public GUIText loadingtext;
	
	// Use this for initialization
	void Start () {
		GameEnvironment.StartWave = 0;
	}

	void Update()
	{
		if( Input.GetKey(KeyCode.Escape) )
			Application.LoadLevel("mainmenu");
	}
	
	// Update is called once per frame
	void OnGUI ()
	{ 
		DrawLevel(0.1f,0.1f,0.2f,0.2f,0,false);
		DrawLevel(0.4f,0.1f,0.2f,0.2f,1,!Option.UnlockAreas);
		DrawLevel(0.7f,0.1f,0.2f,0.2f,2,!Option.UnlockAreas);
		DrawLevel(0.25f,0.4f,0.2f,0.2f,3,!Option.UnlockAreas);
		DrawLevel(0.55f,0.4f,0.2f,0.2f,4,!Option.UnlockAreas);
	}
	
	void DrawLevel(float x,float y,float width,float height,int index,bool locked)
	{
		Rect rect = new Rect(Screen.width*x,Screen.height*y,Screen.width*width,Screen.height*height);
		if( !locked )
		{
			if( GUI.Button( rect,area[index]) )
			{
				GameEnvironment.StartWave = index*4;
				loadingtext.enabled = true;
				Application.LoadLevel("playgame");		
			}
		}
		else
		{
			GUI.Button(rect,area[index]);
			GUI.DrawTexture(new Rect(rect.center.x-25,rect.center.y-25,50,50),lockedTexture);
		}
	}
}
