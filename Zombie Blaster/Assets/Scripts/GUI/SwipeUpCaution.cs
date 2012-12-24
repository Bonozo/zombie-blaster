using UnityEngine;
using System.Collections;

public class SwipeUpCaution : MonoBehaviour {
	
	public UISprite knockleft,knockright,swipeup;
	
	private float ymax = 100f;
	private float speed = 100f;
	
	private Vector3 beginpos = new Vector3(0,-50,0);
	private float deltay = 0f;
	private bool isactive = false;
	private bool leftactive = false, leftactiveincrease = false;
	private bool rightactive = false, rightactiveincrease = false;
	
	public void Activate(Vector3 scp)
	{	
		
		// swipe up
		if(Option.ShoveHelper && scp.z > 0 && scp.x>=0 && scp.x <= Screen.width )
		{
			if( !isactive )
			{
				beginpos = new Vector3(0,-50,0);
				deltay = 0f;
				swipeup.transform.localPosition = beginpos;
				isactive = true;	
			}
			return;
		}
		
		if(scp.z<0) scp.x = -scp.x;
		
		if(scp.x < 0 )
		{
			if( ! leftactive )
				leftactive = leftactiveincrease = true;
		}
		else 
		{
			if( !rightactive )
				rightactive = rightactiveincrease = true;
		}
	}
	
	public void Deactivate()
	{
		isactive = false;
		leftactive = rightactive = false;
	}
	
	// Use this for initialization
	void Start () {
		Color c = knockleft.color;
		c.a = 0.0f; knockleft.color = c;
		c = knockright.color;
		c.a = 0.0f; knockright.color = c;
	}
	
	// Update is called once per frame
	void Update () {
		
		swipeup.gameObject.SetActive(isactive);
		knockleft.gameObject.SetActive(leftactive);
		knockright.gameObject.SetActive(rightactive);
		
		if( isactive )
		{
			Vector3 pos = beginpos;
			deltay += speed*Time.deltaTime;
			if( deltay > ymax ) isactive=false;
			pos.y += deltay;
			swipeup.transform.localPosition = pos;		
		}
		
		if( leftactive )
		{
			Color c = knockleft.color;
			if( leftactiveincrease )
			{
				c.a += Time.deltaTime * 2f;
				if( c.a >= 0.5f ) leftactiveincrease = false;
			}
			else
			{
				c.a -= Time.deltaTime * 2f;
				if( c.a <= 0f ) leftactive = false;	
			}
		
			knockleft.color = c;
		}
				
		if( rightactive )
		{
			Color c = knockright.color;
			if( rightactiveincrease )
			{
				c.a += Time.deltaTime * 2f;
				if( c.a >= 0.5f ) rightactiveincrease = false;
			}
			else
			{
				c.a -= Time.deltaTime * 2f;
				if( c.a <= 0f ) rightactive = false;	
			}
		
			knockright.color = c;
		}
	}
}
