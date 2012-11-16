using UnityEngine;
using System.Collections;

public class GameEnvironment : MonoBehaviour {
	
	#region Static
	
	public static int _playerprefs_zombieHeads=0;
	public static int zombieHeads
	{
		get
		{
			return _playerprefs_zombieHeads;
		}
		set
		{
			_playerprefs_zombieHeads = value;
			PlayerPrefs.SetInt("zombieHeads",_playerprefs_zombieHeads);
		}
	}
	public static int StartLevel = 0;
	public static int StartWave = 0;
	
	#endregion
	
	#region Swipe Input 
	
	private static bool ignore = false;
	
	public static void IgnoreButtons()
	{
		ignore = true;
	}
	
	#if !UNITY_ANDROID
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
				lastInput = Input.mousePosition;
				ans = true;			
			}
			//if( Input.GetMouseButtonUp(0) )
			//	ignore = false;
		return ans;
		}}
	private static Vector2 last;
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
		}
		
		return res;
	}}
	#else
	
	private static float startTime;
	private static Vector2 startPos;
	private static bool couldBeSwipe;
	private static bool moved = false;
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
                break;
			}
		}
		return res;
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
		public StoreGun(string name,bool enabled,int pocketsize)
		{
			this.name = name;
			this.enabled = enabled;
			this.pocketsize = pocketsize;
			
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
		new StoreGun("BB",true,100),			//0
		new StoreGun("Flamethrower",false,100),	//1
		new StoreGun("Rocket",false,5),			//2
		new StoreGun("PulseShotGun",false,5),	//3
		new StoreGun("Grenade",false,5),		//4
		new StoreGun("MachineGun",false,100),	//5
		new StoreGun("Crossbow",false,12),		//6
		new StoreGun("Football",false,5),		//7
		new StoreGun("Sniper",false,12)			//8
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
