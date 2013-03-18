using UnityEngine;
using System.Collections;

public class GunBase : MonoBehaviour {
	
	/// <summary>
	/// the index of the storeguns
	/// </summary>
	public int index;
	
	public float AmmoIncreaseDelta;
	public float AmmoReduceDelta;
	
	public int Ammo
	{
		get
		{
			return GameEnvironment.storeGun[index].current;
		}
		set
		{
			GameEnvironment.storeGun[index].current = value;
		}
	}
	public int AmmoStore
	{
		get
		{
			return GameEnvironment.storeGun[index].store;
		}
		set
		{
			GameEnvironment.storeGun[index].store = value;
			if(GameEnvironment.storeGun[index].store > GameEnvironment.storeGun[index].maxammo)
				GameEnvironment.storeGun[index].store = GameEnvironment.storeGun[index].maxammo;
		}	
	}
	
	public int PacketSize
	{
		get
		{
			return GameEnvironment.storeGun[index].pocketsize;
		}
		set
		{
			GameEnvironment.storeGun[index].pocketsize = value;
		}			
	}
	
	public bool EnabledGun
	{
		get
		{
			return GameEnvironment.storeGun[index].enabled;
		}
		set
		{
			GameEnvironment.storeGun[index].enabled = value;
		}			
	}
	
	public AudioClip AudioFire;
	public AudioClip AudioReload;
	
	public Texture2D texture;
	
	public bool reloading { get; private set; }
	
	private float ammoTimeReduce = 0.0f;
	private float ammoTimeIncrease = 0.0f;
	
	public void AmmoLost()
	{
		if(LevelInfo.Environments.control.UnlimitedAmmo) return;
		ammoTimeReduce -= Time.deltaTime;
		if( ammoTimeReduce <= 0 )
		{
			if( !GameEnvironment.storeGun[index].unlimited ) Ammo--;
			ammoTimeReduce = AmmoReduceDelta;
		}
	}
	
	public void GetAmmoStorePacket(int coef)
	{
		AmmoStore += PacketSize*coef;
	}
	
	public void GetAmmoStorePacketWithMaxAmmo()
	{
		AmmoStore = GameEnvironment.storeGun[index].maxammo;
		Ammo = PacketSize;
		reloading = false;
	}
	
	public void GetAllAmmo()
	{
		if( Ammo == 0 && AmmoStore == 0 )
		{
			AmmoStore = GameEnvironment.storeGun[index].maxammo;
			Ammo = PacketSize;
			reloading = false;
		}
	}
	
	public void Reload()
	{
		if( AmmoStore > 0 && Ammo < PacketSize)
		{
			LevelInfo.Audio.audioSourcePlayer.PlayOneShot(AudioReload);
			reloading = true;
			ammoTimeIncrease = 0.0f;
		}
	}
	
	protected void Update()
	{
		if( AmmoStore == 0 )
			reloading = false;
		
		if( reloading )
		{
			ammoTimeIncrease -= Time.deltaTime;
			if( ammoTimeIncrease <= 0.0f )
			{
				if(!GameEnvironment.storeGun[index].unlimitedclips) AmmoStore--;
				Ammo++;
				if( Ammo == PacketSize )
					reloading = false;
				ammoTimeIncrease = AmmoIncreaseDelta;
			}
		}
	}
	
	public virtual float ManualUpdate(Weapon weapon) { return Ammo; }
	
	public string AmmoInformation { get {
		if( GameEnvironment.storeGun[index].unlimited ) return "Unlimited";	
		if(GameEnvironment.storeGun[index].unlimitedclips) return "" + Ammo + "/#";
		return "" + Ammo + "/" + AmmoStore;	
	}}
	
	public float AmmoCurrentPercent { get { /*if(GameEnvironment.storeGun[index].unlimited ) return 1f;*/ return (float)Ammo/(float)PacketSize; }}
	
	protected Vector3 RaycastsTargetPosition(Camera mainCamera,Ray ray,RaycastHit hit)
	{
		if(hit.collider == null )
		{
			return mainCamera.transform.position + 100f*ray.direction;
		}
		else
			return hit.point;
		//float distfromcamera = hit.collider == null ? 100f : Vector3.Distance(mainCamera.transform.position,hit.collider.gameObject.transform.position);
		//return mainCamera.transform.position + distfromcamera*ray.direction;
	}
}
