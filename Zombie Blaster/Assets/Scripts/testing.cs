using UnityEngine;
using System;
using System.Collections;

public class testing : MonoBehaviour {
	
	int b=0;
	
	void Update () 
	{
		
		if( Input.touchCount>0&&Input.touches[0].phase == TouchPhase.Began || Input.GetKeyUp(KeyCode.A))
		{
			if(b==0)
			{
				Application.LoadLevel("load");
				b++;
			}
			else if(b==1)
			{
				b++;
			}
			else if(b==2)
			{
				b++;
			}
			else
			{
				b=0;
			}
				
		}
			
	}
}
