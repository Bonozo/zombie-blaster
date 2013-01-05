using UnityEngine;
using System.Collections;

public class FlashIcon : MonoBehaviour {

	private UISprite sprite = null;
	public Color flashColor;
	public float flashTime;
	private Color beginColor;
	private Vector3 beginpos,beginsc;
	private int threads = 0;
	public float extrapixel = 60f;
	
	void Awake()
	{
		sprite = this.GetComponent<UISprite>();
		beginColor = sprite.color;
		beginpos = transform.localPosition;
		beginsc = transform.localScale;
	}
	
	void OnEnable()
	{
		sprite.color = beginColor;
		transform.localPosition = beginpos;
		transform.localScale = beginsc;
		threads=0;	
	}
	
	IEnumerator StartFlush()
	{
		if( threads == 0 ){
		threads=1;
		
		float time2 = Time.time + 0.5f*flashTime;
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
			threads=2;
		}
	}
	
	IEnumerator EndFlush()
	{
		if( threads==2 ){
		threads=1;
		Vector3 v;
		Color colmin = sprite.color, colmax = flashColor;
		float time2 = Time.time + 0.5f*flashTime;
			
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
		threads=0;
		}
	}
	
	
	public void StartFlash()
	{
		if( threads==0 )
			StartCoroutine(StartFlush());
	}
	public void EndFlash()
	{
		if( threads==2 )
			StartCoroutine(EndFlush());
	}
	
	void Update()
	{
		if( Input.GetKeyUp(KeyCode.B) )
			StartFlash();
		if( Input.GetKeyUp(KeyCode.N) )
			EndFlash();
	}
}
