using UnityEngine;
using System.Collections;

public class InstantiateObject : MonoBehaviour {
	
	public GameObject PrefabMessageText;
	
	private Camera mainCamera;
	
	// Use this for initialization
	void Start () {
		mainCamera = (Camera)GameObject.FindObjectOfType(typeof(Camera));
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	public void InstantiateMessageText(Vector3 worldPosition,string text)
	{
		Vector3 pos = mainCamera.WorldToScreenPoint(worldPosition);
		pos.x /= Screen.width;
		pos.y /= Screen.height;
		GameObject g = (GameObject)Instantiate(PrefabMessageText,pos,Quaternion.identity);
		g.SendMessage("ChangeMessage",text);
	}
}
