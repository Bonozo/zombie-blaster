using UnityEngine;
using System.Collections;

public class GameEnvironment : MonoBehaviour {
	
	#region Static Local
	
	public static int StartLevel = 0;
	public static int StartWave = 5;
	
	
	public static string loadingLevel = "error";
	public static void LoadLevel(string levelName)
	{
		loadingLevel = levelName;
		Application.LoadLevel("loading");
	}
	
	public static bool ToMap = false;
	
	#endregion
	
	#region Swipe Input 
	
	private static bool ignore = false;
	
	public static void IgnoreButtons()
	{
		ignore = true;
	}
	
	#if UNITY_ANDROID || UNITY_IPHONE
	private static Vector2 beginPos;
	private static Vector2 startPos;
	private static bool couldBeSwipe;
	//public static bool moved = false;
	public static float Swipe { get {
		float res = 0.0f;
		if (Input.touchCount > 0) 
		{
        	var touch = Input.touches[0];
			switch (touch.phase)
				{
				case TouchPhase.Began:
					//moved = false;
	                startPos = touch.position;
					beginPos = touch.position;
	                break;
	            case TouchPhase.Moved:
					//moved = true;
					if(Mathf.Abs(touch.position.x-beginPos.x) > Screen.width*Option.Sensitivity ) IgnoreButtons();
					res = ignore?startPos.x -touch.position.x:0;
					startPos = touch.position;
	                break;
	            case TouchPhase.Stationary:
	                break;
	            case TouchPhase.Ended:
					ignore=false;
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
			
			if( Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Ended && !ignore)
			{
				lastInput = Input.touches[0].position;
				ans = true;
			}
			if( Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Ended  )
				ignore = false;
			
			return ans;
		}}
	
	private static int flameframes;
	public static bool FlameButton { get {
			bool ans = false;
			
			if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began )
				flameframes = Option.FlameWaitingFrames;
			
			if (Input.touchCount > 0 && (Input.touches[0].phase == TouchPhase.Stationary || Input.touches[0].phase == TouchPhase.Moved) && /*!ignore &&*/ flameframes==0)
			{
				lastInput = Input.touches[0].position;
				ans = true;	
			}
			
			if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Ended )
				ignore = false;
			
			if(flameframes>0) flameframes--;
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
	private static float llastx;
	private static float lastx;
	private static bool lastpressed = false;
	public static float Swipe { get {
			float mouseposx = Input.mousePosition.x/Screen.width;
			float res = 0.0f;
			
			if( lastpressed )
			{
				if( Mathf.Abs(llastx - mouseposx) > Option.Sensitivity ) IgnoreButtons();
				res = ignore?lastx-mouseposx:0;
			}
			lastpressed = Input.GetMouseButton(0);
			if( lastpressed )
				lastx = mouseposx;
			if( Input.GetMouseButtonDown(0) )
				llastx = mouseposx;
			if( Input.GetMouseButtonUp(0) )
				ignore = false;
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
	
	private static int flameframes;
	public static bool FlameButton { get {
			bool ans = false;
			
			if( Input.GetMouseButtonDown(0) )
				flameframes = Option.FlameWaitingFrames;
			if( Input.GetMouseButton(0) /*&& !ignore */&& flameframes==0)
			{
				lastInput = Input.mousePosition;
				ans = true;
			}
			if( Input.GetMouseButtonUp(0) )
				ignore = false;
			if(flameframes>0) flameframes--;
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
		public int maxammo;
		public int price;
		public float speed;
		public float reloadTime;
		public bool unlimited;
		public bool unlimitedclips;
		public int damage;
		public float accuracy;
		public StoreGun(string name,bool enabled,int pocketsize,int maxammo,int price,float speed,float reloadTime,int damage,float accuracy)
		{
			this.name = name;
			this.enabled = enabled;
			this.pocketsize = pocketsize;
			this.maxammo = maxammo;
			this.price = price;
			this.speed = speed;
			this.reloadTime = reloadTime;
			this.unlimited = false;
			this.unlimitedclips = enabled;
			this.damage = damage;
			this.accuracy = accuracy;
			
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
			unlimited = false;
			current = pocketsize;
			store = maxammo;
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
			unlimited=true;
			current = pocketsize;
			store = pocketsize;
		}

		public string AmmoInformation
		{
			get
			{	
				if(unlimited) return "Unlimited";
				if(unlimitedclips) return "" + current + "/#";
				return "" + current + "/" + store;
			}
		}
		
		public string AmmoInformationFormal
		{
			get
			{
				if(unlimited) return "Unlimited";
				if(unlimitedclips) return "" + current + "/#";
				return "" + pocketsize + "/" + maxammo;
			}			
		}
	}
	
	public static StoreGun[] storeGun = new StoreGun[10]
	{	
		//				name			enabled    clip  maxammo   price   speed   reload  power  accuracy
		new StoreGun("Airsoft"			,true,		20,		100,	0,		12,		2,		20,		80),	//0
		new StoreGun("Revolver"			,false,		6,		30,		200,	25,		5,		100,	100),	//1
		new StoreGun("Shotgun"			,false,		5,		25,		400,	15,		3,		100,	70),	//2
		new StoreGun("Flamethrower"		,false,		250,	750,	300,	40,		8,		30,		90),	//3
		new StoreGun("Football"			,false,		5,		25,		400,	50,		5,		100,	100),	//4
		new StoreGun("Machine Gun"		,false,		250,	750,	750,	9,		8,		60,		60),	//5
		new StoreGun("Grenades"			,false,		5,		25,		400,	50,		5,		100,	10),	//6
		new StoreGun("Crossbow"			,false,		10,		60,		1000,	20,		5,		40,		90),	//7
		new StoreGun("Rocket Launcher"	,false,		3,		24,		500,	10,		5,		100,	100), 	//8
		new StoreGun("Alien Blaster"	,false,		10,		50,		2000,	25,		10,		100,	100)	//9
	};
	
	
	public static int[] levelPrice = new int [5] {0,500,1000,1500,2000};
	
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
