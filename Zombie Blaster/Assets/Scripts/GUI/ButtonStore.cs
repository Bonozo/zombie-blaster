using UnityEngine;
using System.Collections;

public class ButtonStore : ButtonBase {
	
	Store store;
	
	void Start()
	{
		store = (Store)GameObject.FindObjectOfType(typeof(Store));
	}
	
	/*void OnMouseUp()
	{
		Time.timeScale = 0.0f;
	}*/
	
	protected override void Update()
	{
		base.Update();
		
		if( base.Pressed() )
				Time.timeScale = 0.0f;
		
	}
}
