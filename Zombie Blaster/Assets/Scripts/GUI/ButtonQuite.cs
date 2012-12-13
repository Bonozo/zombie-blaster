using UnityEngine;
using System.Collections;

public class ButtonQuite : ButtonBase {


	// Update is called once per frame
	protected override void Update () {
		base.Update();
		if( base.PressedUp )
				Application.Quit();
	}
}
