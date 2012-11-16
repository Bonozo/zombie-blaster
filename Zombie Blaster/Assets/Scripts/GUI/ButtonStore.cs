using UnityEngine;
using System.Collections;

public class ButtonStore : ButtonBase {
	
	void Start()
	{
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
