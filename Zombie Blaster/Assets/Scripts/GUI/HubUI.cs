using UnityEngine;
using System.Collections;

public class HubUI : MonoBehaviour {
	
	public UISprite sprite;
	public UILabel label;
	public float flashTime;
	public Color textFlashColor;
	public Color iconFlashColor;
	public float iconScale = 60;
	
	private int number = 0;
	private Color textBeginColor;
	
	// icon
	private Color beginColor;
	private Vector3 beginpos,beginsc;
	private int threads = 0, threads1 = 0;
	
	public int GetNumber() { return number; }
	public void SetNumber(int newnumber) { number = newnumber; label.text = "" + number; }
	
	void Start()
	{
		textBeginColor = label.color;
		
		// icon
		beginColor = sprite.color;
		beginpos = transform.localPosition;
		beginsc = transform.localScale;
	}
	
	public void SetNumberWithFlash(int newnumber)
	{
		if( number != newnumber)
		{
			number = newnumber;
			StartCoroutine(FlashTextThread(number));
			StartCoroutine(FlashIconThread());
		}
	}
	
	private IEnumerator FlashTextThread(int newvalue)
	{	
		if( threads == 0 ) { threads++;
		float time2 = Time.time + 0.5f*flashTime;
	
		Color colmin = label.color, colmax = textFlashColor;
		while( Time.time < time2 )
		{
			float percent = 1-2f*(time2-Time.time)/flashTime;
			label.color = new Color(colmin.r + percent*(colmax.r-colmin.r),colmin.g + percent*(colmax.g-colmin.g),colmin.b + percent*(colmax.b-colmin.b),colmin.a + percent*(colmax.a-colmin.a) );
			yield return new WaitForEndOfFrame();
		}
	
		label.text = "" + number;
		
		time2 = Time.time + 0.5f*flashTime;
		while( Time.time < time2 )
		{
			float percent = 2f*(time2-Time.time)/flashTime;
			label.color = new Color(colmin.r + percent*(colmax.r-colmin.r),colmin.g + percent*(colmax.g-colmin.g),colmin.b + percent*(colmax.b-colmin.b),colmin.a + percent*(colmax.a-colmin.a) );
	
			yield return new WaitForEndOfFrame();
		}
		
		label.color = textBeginColor;
		threads--;
		}
	}
	
	IEnumerator FlashIconThread()
	{
		if(threads1==0){ threads1++;
		float time2 = Time.time + 0.5f*flashTime;
		float extrapixel = iconScale;
		Vector3 v;
	
		Color colmin = sprite.color, colmax = iconFlashColor;
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
			threads1--;
		}
	}
	
	void Update()
	{
		if( Input.GetKeyUp(KeyCode.E) )
		{
			Debug.Log("Update");
			SetNumberWithFlash(GetNumber()+1);
			
		}
	}
}
