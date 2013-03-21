using UnityEngine;
using System.Collections;

public class Credits : MonoBehaviour {
	
	[System.Serializable]
	public class CreditStruct
	{
		public string title;
		public string[] name;
	}
	
	public CreditStruct[] members;
	
	public float Speed = 0.05f;
	
	public GUIStyle guiStyleTitle;
	public GUIStyle guiStyleName;
	public GUIStyle buttonGUIStyle;
	private float currentHeight = -1f;
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if( Input.touchCount == 0 )
			currentHeight += Time.deltaTime*Speed;
	}
	
	void OnEnable()
	{
		currentHeight = -1;
	}
	
	private Rect RectScreen(float a,float b,float w,float h)
	{
		return new Rect(a*GameEnvironment.GUIWidth,b*GameEnvironment.GUIHeight,w*GameEnvironment.GUIWidth,h*GameEnvironment.GUIHeight);
	}
	
	void OnGUI()
	{
		GUI.matrix = GameEnvironment.GetGameGUIMatrix();
		foreach(Touch touch in Input.touches)
				currentHeight += 0.25f*touch.deltaPosition.y/GameEnvironment.GUIHeight;
		if( currentHeight < -1 ) currentHeight = -1;
		
		float index = -currentHeight;
		
		foreach(var mb in members )
		{
			GUI.Label(RectScreen(0.25f,index,0.5f,0.1f),mb.title,guiStyleTitle);
			index += 0.075f;
			foreach(var nm in mb.name)
			{
				GUI.Label(RectScreen(0.2f,index,0.6f,0.1f),nm,guiStyleName);
				index += 0.1f;
			}
			index += 0.1f;
		}
		if(index<0f)
		{
			currentHeight = -1;
			MainMenu mainmenu = (MainMenu)GameObject.FindObjectOfType(typeof(MainMenu));
			mainmenu.GoState(MainMenu.MenuState.Option);
		}
		
		if(GUI.Button(new Rect(5f,5f,0.2f*GameEnvironment.GUIWidth,0.075f*GameEnvironment.GUIHeight),"Game Trailer",buttonGUIStyle))
			Application.OpenURL(@"http://youtu.be/gtRwZtht-1Q");
		if(GUI.Button(new Rect(0.8f*GameEnvironment.GUIWidth-5f,5f,0.2f*GameEnvironment.GUIWidth,0.075f*GameEnvironment.GUIHeight),"Behind the Scenes",buttonGUIStyle))
			Application.OpenURL(@"http://youtu.be/NB9TwvKv4RM");
			
	}
}
