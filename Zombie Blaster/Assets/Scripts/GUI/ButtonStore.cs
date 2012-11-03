using UnityEngine;
using System.Collections;

public class ButtonStore : ButtonBase {
	
	Store store;
	
	void Start()
	{
		store = (Store)GameObject.FindObjectOfType(typeof(Store));
	}
	
	void OnMouseUp()
	{
		Time.timeScale = 0.0f;
	}
	
	protected override void Update()
	{
		foreach(Touch touch in Input.touches)
			if( guiTexture.HitTest(touch.position) && touch.phase == TouchPhase.Ended)
				
				Time.timeScale = 0.0f;
		base.Update();
	}
}
