using UnityEngine;
using System.Collections;

public enum HealthPackType
{
	Ammo,
	Armor,
	BonusHeads,
	DamageMultiplier,
	Health,
	Shield,
	SuperAmmo,
	ScoreMultiplier,
	XtraLife
}

[AddComponentMenu("GamePlay/Pickup")]
public class HealthPack : MonoBehaviour {
	
	public HealthPackType packType;
	
	public float Health = 0.1f;
	public float DeadTime = 20f;
	
	private float rotateSpeed = 150f;
	private float StartHeight = 2.5f;
	
	private RaycastHit hit;
	private Ray ray;
	
	private bool picked = false;
	
	private Weapon gunindexifweapon = 0;
	private float autopickuptime = 0.0f;
	private bool rgb = true;
	
	public bool scooby = false;
	
	// Use this for initialization
	void Start () {

		transform.Translate(0,StartHeight-transform.position.y,0);
		transform.LookAt(LevelInfo.Environments.control.transform.position,Vector3.up);
		
		var level = LevelInfo.State.level[LevelInfo.Environments.control.currentLevel];
		
		switch(packType)
		{
		case HealthPackType.Ammo:
			if( !Store.WeaponUnlocked((int)level.allowedGun[level.allowedGun.Length-1]) )
				gunindexifweapon = level.allowedGun[Random.Range(1,level.allowedGun.Length-1)];
			else
				gunindexifweapon = level.allowedGun[Random.Range(1,level.allowedGun.Length)];
			break;
		/*case HealthPackType.Armor:
			gameObject.renderer.material.mainTexture = LevelInfo.Environments.texturePickUpArmor;
			break;*/
		/*case HealthPackType.BonusHeads:
			gameObject.renderer.material.mainTexture = LevelInfo.Environments.texturePickUpBonusHeads;
			break;*/
		/*case HealthPackType.DamageMultiplier:
			gameObject.renderer.material.mainTexture = LevelInfo.Environments.texturePickUpDamageMultiplier;
			break;*/
		/*case HealthPackType.Health:
			gameObject.renderer.material.mainTexture = LevelInfo.Environments.texturePickUpHealth;
			break;*/
		/*case HealthPackType.Shield:
			gameObject.renderer.material.mainTexture = LevelInfo.Environments.texturePickUpShields;
			break;*/
		case HealthPackType.SuperAmmo:
			gameObject.renderer.material.mainTexture = LevelInfo.Environments.texturePickUpSuperAmmo;
			break;
		/*case HealthPackType.ScoreMultiplier:
			break;*/
		/*case HealthPackType.XtraLife:
			gameObject.renderer.material.mainTexture = LevelInfo.Environments.texturePickUpXtraLife;
			break;*/
		}		
	}
	
	void Update()
	{
		if(Fade.InProcess) return;
		if( picked ) return;
		
		autopickuptime -= Time.deltaTime;
		if( autopickuptime <= 0f && GameEnvironment.DistXZ(transform.position,LevelInfo.Environments.control.transform.position) < 2.8f )
		{
			rgb = false;
			StartCoroutine(PickedUp());
			return;
		}
		
		var v = transform.rotation.eulerAngles;
		v.x=v.z=0;
		transform.rotation = Quaternion.Euler(v);
		if(rigbd) transform.Rotate(0,rotateSpeed*Time.deltaTime,0);
		
		
		
		if( LevelInfo.Environments.control.state == GameState.Paused ) return;
		if( (DeadTime -= Time.deltaTime) <= 0 ) Destroy(this.gameObject);

		
		foreach(Touch touch in Input.touches)
		{
	        if(touch.phase == TouchPhase.Began)
			{
				ray = LevelInfo.Environments.mainCamera.ScreenPointToRay(touch.position);
				if( Physics.Raycast(ray.origin,ray.direction,out hit) && hit.collider.gameObject == gameObject )
				{
					StartCoroutine(PickedUp());
					return;
				}
			}
        }
		ray = LevelInfo.Environments.mainCamera.ScreenPointToRay(Input.mousePosition);
		if(Physics.Raycast(ray.origin,ray.direction,out hit))
			if(Input.GetMouseButtonDown(0) && hit.collider.gameObject == gameObject )
				StartCoroutine(PickedUp());
	}
	
	bool rigbd=false;
	void OnCollisionEnter()
	{
		if(!rigbd)
		{
			Destroy(this.rigidbody);
			rigbd=true;
		}
	}
	
	private IEnumerator PickedUp()
	{
		if(gameObject.rigidbody == null )
			gameObject.AddComponent("Rigidbody");
		if(rgb) rigidbody.AddForce(0,200,0);
		GameEnvironment.IgnoreButtons();
		picked = true;
		
		LevelInfo.Audio.PlayPickUp(packType);
		
		float time = Time.time+0.5f;
		while( Time.time <= time )
			yield return new WaitForEndOfFrame();
		
		string pickupname = "";
		//control.GetScore(Score);
		switch(packType)
		{
		case HealthPackType.Ammo:
			if(LevelInfo.Environments.guns.gun[(int)gunindexifweapon].EnabledGun)
				LevelInfo.Environments.guns.GetAmmoWithMax(gunindexifweapon);
			pickupname = GameEnvironment.storeGun[(int)gunindexifweapon].name + " Ammo";
			break;
			
		case HealthPackType.Armor:
			LevelInfo.Environments.control.GetArmor(0.25f);
			pickupname = "Leathers";
			break;
			
		case HealthPackType.BonusHeads:
			Store.zombieHeads = Store.zombieHeads+10;
			pickupname = "+10 Heads";
			break;
			
		case HealthPackType.DamageMultiplier:
			LevelInfo.Environments.control.DamageMultiply();
			pickupname = "x4 Damage";
			break;
			
		case HealthPackType.Health:
			LevelInfo.Environments.control.GetHealth(Health);
			pickupname = "First Aid";
			break;
			
		case HealthPackType.Shield:
			LevelInfo.Environments.control.Shield();
			pickupname = "Invincibility";
			break;
			
		case HealthPackType.SuperAmmo:
			for(int i=0;i<LevelInfo.Environments.guns.gun.Length;i++)
				if( LevelInfo.Environments.guns.gun[i].EnabledGun )
					LevelInfo.Environments.guns.GetWeaponWithMAX((Weapon)i);
			pickupname = "All Ammo";
			break;
			
		case HealthPackType.ScoreMultiplier:
			
			pickupname = "x2 Score";
			break;
			
		case HealthPackType.XtraLife:
			LevelInfo.Environments.hubLives.SetNumberWithFlash(LevelInfo.Environments.hubLives.GetNumber()+1);
			pickupname = "Xtra Life";
			break;	
			
		}
		
		LevelInfo.Environments.generator.GenerateMessageText(transform.position + new Vector3(0,0.75f,0),pickupname);
		
		LevelInfo.Environments.control.GetScore(LevelInfo.State.scoreForPickUp,true);
		
		Destroy(this.gameObject);
	}
}