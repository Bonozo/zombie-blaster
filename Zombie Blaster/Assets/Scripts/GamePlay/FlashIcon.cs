using UnityEngine;
using System.Collections;

public class FlashIcon : MonoBehaviour {

	private UISprite sprite = null;
	private Color beginColor;
	private Vector3 beginpos,beginsc;
	private int threads = 0;
	
	void Awake()
	{
		sprite = this.GetComponent<UISprite>();
		beginColor = sprite.color;
		beginpos = transform.localPosition;
		beginsc = transform.localScale;
	}
	
	IEnumerator StartFlush(Color flashColor,float flashTime)
	{
		if( threads == 0 ){
		threads++;
		
		float time2 = Time.time + 0.5f*flashTime;
		float extrapixel = 60f;
		Vector3 v;
	
		Color colmin = sprite.color, colmax = flashColor;
		while( Time.time < time2 )
		{
			float percent = 1-2f*(time2-Time.time)/flashTime;
			sprite.color = new Color(colmin.r + percent*(colmax.r-colmin.r),colmin.g + percent*(colmax.g-colmin.g),colmin.b + percent*(colmax.b-colmin.b),colmin.a + percent*(colmax.a-colmin.a) );
			v = transform.localScale;
			v.y += extrapixel*Time.deltaTime;
			v.x += extrapixel*Time.deltaTime;
			transform.localScale = v;
			v = transform.localPosition;
			v.y += 0.5f*extrapixel*Time.deltaTime;
			v.x += 0.5f*extrapixel*Time.deltaTime;
			transform.localPosition = v;
			
			yield return new WaitForEndOfFrame();
		}
		
		time2 = Time.time + 0.5f*flashTime;
		while( Time.time < time2 )
		{
			float percent = 2f*(time2-Time.time)/flashTime;
			sprite.color = new Color(colmin.r + percent*(colmax.r-colmin.r),colmin.g + percent*(colmax.g-colmin.g),colmin.b + percent*(colmax.b-colmin.b),colmin.a + percent*(colmax.a-colmin.a) );
			v = transform.localScale;
			v.y -= extrapixel*Time.deltaTime;
			v.x -= extrapixel*Time.deltaTime;
			transform.localScale = v;
			v = transform.localPosition;
			v.y -= 0.5f*extrapixel*Time.deltaTime;
			v.x -= 0.5f*extrapixel*Time.deltaTime;
			transform.localPosition = v;
			yield return new WaitForEndOfFrame();
		}
		
		sprite.color = beginColor;
		transform.localPosition = beginpos;
		transform.localScale = beginsc;
		threads--;
		}
	}
	
	public void Flash(Color flashColor,float flashTime)
	{
		StartCoroutine(StartFlush(flashColor,flashTime));
	}
	
	/*void Update()
	{
		if( Input.GetKeyUp(KeyCode.L) )
			Flash(Color.green,0.25f);
	}*/
}
