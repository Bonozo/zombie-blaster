using UnityEngine;
using System.Collections;

public class FlashText : MonoBehaviour {
	
	private UILabel label = null;
	private Color textBeginColor;
	
	void Awake()
	{
		label = this.GetComponent<UILabel>();
		textBeginColor = label.color;
	}
	
	IEnumerator StartFlush(Color flashColor,float flashTime,string newvalue)
	{
		float time2 = Time.time + 0.5f*flashTime;
	
		Color colmin = label.color, colmax = flashColor;
		while( Time.time < time2 )
		{
			float percent = 1-2f*(time2-Time.time)/flashTime;
			label.color = new Color(colmin.r + percent*(colmax.r-colmin.r),colmin.g + percent*(colmax.g-colmin.g),colmin.b + percent*(colmax.b-colmin.b),colmin.a + percent*(colmax.a-colmin.a) );
			yield return new WaitForEndOfFrame();
		}
	
		label.text = newvalue;
		
		time2 = Time.time + 0.5f*flashTime;
		while( Time.time < time2 )
		{
			float percent = 2f*(time2-Time.time)/flashTime;
			label.color = new Color(colmin.r + percent*(colmax.r-colmin.r),colmin.g + percent*(colmax.g-colmin.g),colmin.b + percent*(colmax.b-colmin.b),colmin.a + percent*(colmax.a-colmin.a) );
	
			yield return new WaitForEndOfFrame();
		}
		
		label.color = textBeginColor;
	}
	
	public void Flash(Color flashColor,float flashTime,string newvalue)
	{
		StartCoroutine(StartFlush(flashColor,flashTime,newvalue));
	}
	
}
