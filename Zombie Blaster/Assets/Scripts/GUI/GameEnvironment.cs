using UnityEngine;
using System.Collections;

public class GameEnvironment : MonoBehaviour {
	
	#region Static Local
	
	public static int StartLevel = 4;
	public static int StartWave = 0;
	
	
	public static string loadingLevel = "error";
	public static void LoadLevel(string levelName)
	{
		loadingLevel = levelName;
		Application.LoadLevel("loading");
	}
	
	#endregion
	
	#region Swipe Input 
	
	private static bool ignore = false;
	
	public static void IgnoreButtons()
	{
		ignore = true;
	}
	
	#if UNITY_ANDROID || UNITY_IPHONE
	private static float startTime;
	private static Vector2 startPos;
	private static bool couldBeSwipe;
	public static bool moved = false;
	public static float Swipe { get {
		float res = 0.0f;
		if (Input.touchCount > 0) 
		{
        	var touch = Input.touches[0];
			switch (touch.phase)
				{
			case TouchPhase.Began:
				moved = false;
                startPos = touch.position;
                break;
            case TouchPhase.Moved:
				moved = true;
				IgnoreButtons();
				res = startPos.x -touch.position.x;
				startPos = touch.position;
                break;
            case TouchPhase.Stationary:
                break;
            case TouchPhase.Ended:
                break;
				}
		}
		return res*0.05f;
	}}
	
	public static Vector2 lastInput = Vector2.zero;
	
	public static Vector2 lastInput01 { get { 
		Vector2 v = lastInput;
		v.x /= Screen.width;
		v.y /= Screen.height;
		return v;
		}}
	
	public static bool FireButton { get {
			bool ans = false;
			
			if( (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Ended && !moved) && !ignore)
			{
				lastInput = Input.touches[0].position;
				ans = true;
			}
			if( Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Ended  )
				ignore = false;
			
			return ans;
		}}
	public static bool FlameButton { get {
			bool ans = false;
			
			if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Stationary && !ignore)
			{
				lastInput = Input.touches[0].position;
				ans = true;	
			}
			
			if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Ended )
				ignore = false;
			
			return ans;
		}}
	
	public static bool TouchedScreen { get {
			bool ans = false;
			if(Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Stationary)
			{
				lastInput = Input.mousePosition;
				ans = true;			
			}
			//if( Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Ended )
			//	ignore = false;
		return ans;
		}}
	
	private static Vector2 startPos2;
	private static Vector2 startPos3;
	public static Vector2 AbsoluteSwipe { get {
		Vector2 res = Vector2.zero;
		if (Input.touchCount > 0) 
		{
        	var touch = Input.touches[0];
			Vector2 pos = touch.position;
				
			pos.x /= Screen.width;
			pos.y /= Screen.height;
			
			switch (touch.phase)
			{
			case TouchPhase.Began:
                startPos2 = pos;
                break;
            case TouchPhase.Ended:
		        res = pos - startPos2;       
				startPos3 = pos;
                break;
			}
		}
		return res;
	}}
	
	public static Vector2 AbsoluteSwipeBegin { get {
		return new Vector2(Screen.width*startPos2.x,Screen.height*startPos2.y);
		}}
	public static Vector2 AbsoluteSwipeEnd { get {
		return new Vector2(Screen.width*startPos3.x,Screen.height*startPos3.y);
		}}	

	public static Vector3 InputAxis { get {
		Vector3 dir = Vector3.zero;
		dir.x = -Input.acceleration.y;
		dir.z = Input.acceleration.x;
		
		if(dir.sqrMagnitude > 1)
			dir.Normalize();
		dir.y = dir.z; dir.z = 0;
			
		return dir;
	}}
	#else
	private static float lastx;
	private static bool lastpressed = false;
	public static float Swipe { get {
		float mouseposx = Input.mousePosition.x/Screen.width;
		float res = 0.0f;
		
		if( lastpressed )
		{
			if( lastx != mouseposx ) IgnoreButtons();
			res = lastx-mouseposx;
		}
		lastpressed = Input.GetMouseButton(0);
		if( lastpressed )
			lastx = mouseposx;
				
		return 30.0f*res;
	}}
	
	public static Vector2 lastInput = Vector2.zero;
	public static Vector2 lastInput01 { get { 
		Vector2 v = lastInput;
		v.x /= Screen.width;
		v.y /= Screen.height;
		return v;
		}}
	
	public static bool FireButton { get {
			bool ans = false;
			if( Input.GetMouseButtonUp(0) && !ignore)
			{
				lastInput = Input.mousePosition;
				ans = true;
			}
			if( Input.GetMouseButtonUp(0) )
				ignore = false;
			return ans;
		}}
	public static bool FlameButton { get {
			bool ans = false;
			if( Input.GetMouseButton(0) && !ignore)
			{
				lastInput = Input.mousePosition;
				ans = true;
			}
			if( Input.GetMouseButtonUp(0) )
				ignore = false;
			return ans;
		}}
	
	public static bool TouchedScreen { get {
			bool ans = false;
			if( Input.GetMouseButton(0) )
			{
				if( lastInput.x == Input.mousePosition.x && lastInput.y == Input.mousePosition.y )
					ans = true;
				lastInput = Input.mousePosition;			
			}
			//if( Input.GetMouseButtonUp(0) )
			//	ignore = false;
		return ans;
		}}
	private static Vector2 last;
	private static Vector2 last2;
	public static Vector2 AbsoluteSwipe { get {
		Vector2 mousepos = Input.mousePosition;
		mousepos.x /= Screen.width;
		mousepos.y /= Screen.height;

		Vector2 res = Vector2.zero;
		if( Input.GetMouseButtonDown(0) )
		{
			last = mousepos;
		}
		if( Input.GetMouseButtonUp(0) )
		{
			res = mousepos-last;
			last2 = mousepos;
		}
		
		return res;
	}}


	public static Vector2 AbsoluteSwipeBegin { get {
		return new Vector2(Screen.width*last.x,Screen.height*last.y);
		}}

	public static Vector2 AbsoluteSwipeEnd { get {
		return new Vector2(Screen.width*last2.x,Screen.height*last2.y);
		}}
	
	public static Vector3 InputAxis { get {
		return new Vector3(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical"),0f);
	}}
	#endif
	
	#endregion
	
	#region Store
	
	public struct StoreGun
	{
		public string name;
		public int current;
		public int store;
		public bool enabled;
		public int pocketsize;
		public int price;
		public float speed;
		public float reloadTime;
		public StoreGun(string name,bool enabled,int pocketsize,int price,float speed,float reloadTime)
		{
			this.name = name;
			this.enabled = enabled;
			this.pocketsize = pocketsize;
			this.price = price;
			this.speed = speed;
			this.reloadTime = reloadTime;
			
			this.current = this.store = 0;
			if( enabled )
				ResetAmmoStandart();
			else
				ResetAmmoZero();
			

		}
		
		public void ResetAmmoZero()
		{
			current = store = 0;
		}
		
		public void ResetAmmoStandart()
		{
			current = pocketsize;
			store = 5*pocketsize;
		}
		
		public void SetEnabled(bool enb)
		{
			this.enabled = enb;
			if( enb )
				ResetAmmoStandart();
			else
				ResetAmmoZero();		
		}
		
		public void SetAsUnlimited()
		{
			pocketsize = int.MaxValue;
			current = pocketsize;
			store = pocketsize;
		}

		public string AmmoInformation
		{
			get
			{
				if( Option.UnlimitedAmmo ) return enabled?"Unlimited":"0/0";	
				return "" + current + "/" + store;
			}
		}
	}
	
	public static StoreGun[] storeGun = new StoreGun[9]
	{
		
		new StoreGun("Airsoft",true,20,0,9,1),				//0
		new StoreGun("Crossbow",false,12,30,25,3),			//1
		new StoreGun("Shotgun",false,5,75,15,1),			//2
		new StoreGun("Flamethrower",false,100,100,30,12),	//3
		new StoreGun("Football",false,5,100,100,3),			//4
		new StoreGun("Machine Gun",false,100,150,9,12),		//5
		new StoreGun("Grenades",false,5,150,50,3),			//6
		new StoreGun("Revolver",false,6,300,25,1),			//7
		new StoreGun("Rocket Launcher",false,5,300,10,3) 	//8
		
		
		
		//new StoreGun("Sniper",false,12)		//9
	};
	
	#endregion
	
	#region Helpful
	
	public static float DistXZ(Vector3 a,Vector3 b)
	{
		a.y = b.y = 0;
		return Vector3.Distance(a,b);
	}
	
	public static Vector3 ProjectionXZ(Vector3 c)
	{
		c.y = 0;
		return c;
	}
	
	#endregion
	
}
