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
	public GUIStyle myGuiStyle;
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	private float index = 0;
	private Rect textRect(bool big)
	{
		index++;
		myGuiStyle.fontSize = big?50:30;
		float w = 0f;
		if( index > 8 ) { w = Screen.width*0.5f; index-=8; }
		return new Rect(w,index*25f,Screen.width*0.5f,Screen.height*0.1f);
	}
	
	void OnGUI()
	{
		foreach(var mb in members )
		{
			GUI.Label(textRect(false),mb.title,myGuiStyle);
			foreach(var nm in mb.name)
				GUI.Label(textRect(true),nm,myGuiStyle);
			index++;
		}
		index = 0;
	}
}
