using UnityEngine;
using System.Collections;

public class SelectArea : MonoBehaviour {
	
	public ButtonBase[] levelButton;
	
	public Texture2D[] area;
	public Texture2D lockedTexture;
	public GUITexture loadingTexture;
	
	// Use this for initialization
	void Start () {
		GameEnvironment.StartLevel = 0;
		loadingTexture.enabled = false;
	}
	
	void Update()
	{
		for(int i=0;i<levelButton.Length;i++)
		{
			bool cango = Option.UnlockAreas?true:i==0;
			if( cango && levelButton[i].Pressed() )
			{
				GameEnvironment.StartLevel = i;
				loadingTexture.enabled = true;
				Application.LoadLevel("playgame");	
			}
		}
	}

	// Update is called once per frame
	void OnGUI ()
	{ 
		if( !Option.UnlockAreas )
			for(int i=1;i<levelButton.Length;i++)
			{
				Rect rect = new Rect(Screen.width*levelButton[i].gameObject.transform.position.x,Screen.height*(1-levelButton[i].gameObject.transform.position.y),50,50);
				GUI.DrawTexture(rect,lockedTexture);
			}
		/*DrawLevel(0.1f,0.1f,0.2f,0.2f,0,false);
		DrawLevel(0.4f,0.1f,0.2f,0.2f,1,!Option.UnlockAreas);
		DrawLevel(0.7f,0.1f,0.2f,0.2f,2,!Option.UnlockAreas);
		DrawLevel(0.25f,0.4f,0.2f,0.2f,3,!Option.UnlockAreas);
		DrawLevel(0.55f,0.4f,0.2f,0.2f,4,!Option.UnlockAreas);*/
	}
	
	void DrawLevel(float x,float y,float width,float height,int index,bool locked)
	{
		Rect rect = new Rect(Screen.width*x,Screen.height*y,Screen.width*width,Screen.height*height);
		if( !locked )
		{
			if( GUI.Button( rect,area[index]) )
			{
				GameEnvironment.StartLevel = index;
				loadingTexture.enabled = true;
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
