using UnityEngine;
using System.Collections;

public enum HealthPackType
{
	Health=0,
	Ammo=1,
	Weapon=2
}

public class HealthPack : MonoBehaviour {
	
	public Texture TextureHealth;
	public Texture TextureAmmo;
	
	public AudioClip AudioHealth;
	public AudioClip AudioAmmo;
	public AudioClip AudioWeapon;

	public float Health = 0.1f;
	public float DeadTime = 20f;
	
	private float StartHeight = 2.5f;
	
	private Control control;
	private Guns guns;
	
	private Camera mainCamera;
	private RaycastHit hit;
	private Ray ray;
	
	private bool picked = false;
	private float riseTime = 0.5f;
	
	private HealthPackType packType;
	private InstantiateObject instantiater;
	private int gunindexifweapon = 0;
	
	// Use this for initialization
	void Start () {
		mainCamera = (Camera)GameObject.FindObjectOfType(typeof(Camera));
		control = (Control)GameObject.FindObjectOfType(typeof(Control));
		guns = (Guns)GameObject.FindObjectOfType(typeof(Guns));	
		instantiater = (InstantiateObject)GameObject.FindObjectOfType(typeof(InstantiateObject));
		
		
		transform.Translate(0,StartHeight-transform.position.y,0);
		
		packType = (HealthPackType)Random.Range(0,3);
		switch(packType)
		{
		case HealthPackType.Health:
			gameObject.renderer.material.mainTexture = TextureHealth;
			break;
		case HealthPackType.Ammo:
			gameObject.renderer.material.mainTexture = TextureAmmo;
			break;
		case HealthPackType.Weapon:
			gunindexifweapon = Random.Range(0,GameEnvironment.storeGun.Length);
			gameObject.renderer.material.mainTexture = guns.gun[gunindexifweapon].texture;
			break;
		}		
	}
	
	void Update()
	{
		
		if( picked )
		{
			//transform.Translate(0,riseSpeed*Time.deltaTime,0);
			riseTime -= Time.deltaTime;
			if( riseTime <= 0 )
			{
				string pickupname = "";
				//control.GetScore(Score);
				switch(packType)
				{
				case HealthPackType.Health:
					control.GetHealth(Health);
					pickupname = "Health";
					break;
				case HealthPackType.Ammo:
					guns.GetAmmo(1);
					pickupname = "Ammo";
					break;
				case HealthPackType.Weapon:
					guns.GetGun(gunindexifweapon,1);
					pickupname = GameEnvironment.storeGun[gunindexifweapon].name;
					break;
				}
				
				instantiater.InstantiateMessageText(transform.position+ new Vector3(0,0.75f,0),pickupname);
				
				Destroy(this.gameObject);
			}
			return;
		}
		
		DeadTime -= Time.deltaTime;
		if( DeadTime <= 0 )
		{
			Destroy(this.gameObject);
			return;
		}
		
		foreach(Touch touch in Input.touches)
		{
        	ray = mainCamera.ScreenPointToRay(touch.position);
	        if(touch.phase == TouchPhase.Began && Physics.Raycast(ray.origin,ray.direction,out hit)){
				if(hit.collider.gameObject == gameObject )
					PickedUp();
			}
        }
		ray = mainCamera.ScreenPointToRay(Input.mousePosition);
		if(Physics.Raycast(ray.origin,ray.direction,out hit))
			if(Input.GetMouseButtonDown(0) && hit.collider.gameObject == gameObject )
				PickedUp();
	}
	
	private void PickedUp()
	{
		rigidbody.AddForce(0,1000,0);
		GameEnvironment.IgnoreButtons();
		picked = true;
		
		switch(packType)
		{
		case HealthPackType.Health:
			audio.PlayOneShot(AudioHealth);
			break;
		case HealthPackType.Ammo:
			audio.PlayOneShot(AudioAmmo);
			break;
		case HealthPackType.Weapon:
			audio.PlayOneShot(AudioWeapon);
			break;
		}				
	}
}