using UnityEngine;
using System.Collections;

public class ButtonStore : ButtonBase {
	
	void Start()
	{
	}

	protected override void Update()
	{
		base.Update();
		
		if( base.PressedUp )
				Time.timeScale = 0.0f;
		
	}
}
