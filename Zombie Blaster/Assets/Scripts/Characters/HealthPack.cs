using UnityEngine;
using System.Collections;

public enum HealthPackType
{
	Health=0,
	Ammo=1,
	SuperAmmo=2,
	Weapon=3
}

public class HealthPack : MonoBehaviour {
	
	public Texture TextureHealth;
	public Texture TextureAmmo;
	public Texture TextureSuperAmmo;
	
	public AudioClip AudioHealth;
	public AudioClip AudioAmmo;
	public AudioClip AudioWeapon;
	public AudioClip AudioSuperAmmo;

	public float Health = 0.1f;
	public float DeadTime = 20f;
	
	private float StartHeight = 2.5f;

	private Guns guns;
	
	private RaycastHit hit;
	private Ray ray;
	
	private bool picked = false;
	
	private HealthPackType packType;
	private Weapon gunindexifweapon = 0;
	
	public bool scooby = false;
	
	// Use this for initialization
	void Start () {
		guns = (Guns)GameObject.FindObjectOfType(typeof(Guns));	
		
		transform.Translate(0,StartHeight-transform.position.y,0);
		
		packType = (HealthPackType)Random.Range(0,4);
		if( !scooby && packType == HealthPackType.Weapon ) packType = HealthPackType.Ammo;//ha ha ha... wrong logic 
		var level = LevelInfo.State.level[LevelInfo.State.currentLevel];
		
		switch(packType)
		{
		case HealthPackType.Health:
			gameObject.renderer.material.mainTexture = TextureHealth;
			gameObject.renderer.material.color = Color.white;
			break;
		case HealthPackType.Ammo:
			int t = Random.Range(0,guns.gun.Length);
			while( !guns.gun[t].EnabledGun ) t--;
			gunindexifweapon = (Weapon)t;
				
			gameObject.renderer.material.mainTexture = guns.gun[(int)gunindexifweapon].texture;
			gameObject.renderer.material.color = Color.blue;
			break;
		case HealthPackType.SuperAmmo:
			gameObject.renderer.material.mainTexture = TextureAmmo;
			gameObject.renderer.material.color = Color.yellow;
			break;
		case HealthPackType.Weapon:
			gunindexifweapon = level.allowedGun[Random.Range(0,level.allowedGun.Length)];
			gameObject.renderer.material.mainTexture = guns.gun[(int)gunindexifweapon].texture;
			gameObject.renderer.material.color = Color.red;
			break;
		}		
	}
	
	void Update()
	{
		if( picked ) return;
		if( (DeadTime -= Time.deltaTime) <= 0 ) Destroy(this.gameObject);

		
		foreach(Touch touch in Input.touches)
		{
        	ray = LevelInfo.Environments.mainCamera.ScreenPointToRay(touch.position);
	        if(touch.phase == TouchPhase.Began && Physics.Raycast(ray.origin,ray.direction,out hit)){
				if(hit.collider.gameObject == gameObject )
					StartCoroutine(PickedUp());
			}
        }
		ray = LevelInfo.Environments.mainCamera.ScreenPointToRay(Input.mousePosition);
		if(Physics.Raycast(ray.origin,ray.direction,out hit))
			if(Input.GetMouseButtonDown(0) && hit.collider.gameObject == gameObject )
				StartCoroutine(PickedUp());
	}
	
	private IEnumerator PickedUp()
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
		case HealthPackType.SuperAmmo:
			audio.PlayOneShot(AudioSuperAmmo);
			break;
		case HealthPackType.Weapon:
			audio.PlayOneShot(AudioWeapon);
			break;
		}	
		
		float time = Time.time+0.5f;
		while( Time.time <= time )
			yield return new WaitForEndOfFrame();
		
		string pickupname = "";
		//control.GetScore(Score);
		switch(packType)
		{
		case HealthPackType.Health:
			LevelInfo.Environments.control.GetHealth(Health);
			pickupname = "Health";
			break;
		case HealthPackType.Ammo:
			guns.GetAmmoWithMax(gunindexifweapon);
			pickupname = "Ammo";
			break;
		case HealthPackType.SuperAmmo:
			for(int i=0;i<guns.gun.Length;i++)
				if( guns.gun[i].EnabledGun )
					guns.GetWeaponWithMAX((Weapon)i);
			pickupname = "Super Ammo";
			break;
		case HealthPackType.Weapon:
			guns.GetWeaponWithMAX(gunindexifweapon);
			pickupname = GameEnvironment.storeGun[(int)gunindexifweapon].name;
			break;
		}
		
		LevelInfo.Environments.generator.GenerateMessageText(transform.position+ new Vector3(0,0.75f,0),pickupname);
		
		LevelInfo.State.score += LevelInfo.State.scoreForPickUp;
		
		Destroy(this.gameObject);

			
	}
}