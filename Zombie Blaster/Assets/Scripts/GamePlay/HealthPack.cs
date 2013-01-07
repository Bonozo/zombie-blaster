using UnityEngine;
using System.Collections;

public enum HealthPackType
{
	Ammo,//yellow
	Armor,//blue increase health to max
	BonusHeads,//green +10 Heads
	DamageMultiplier,//purple
	Health,//white
	Shield,//gray increase health and armor to max
	SuperAmmo,//orange
	Weapon,//red
	XtraLife,//pink
}

[AddComponentMenu("GamePlay/Pickup")]
public class HealthPack : MonoBehaviour {
	
	public HealthPackType packType;
	
	public float Health = 0.1f;
	public float DeadTime = 20f;
	
	private float StartHeight = 2.5f;
	
	private RaycastHit hit;
	private Ray ray;
	
	private bool picked = false;
	
	private Weapon gunindexifweapon = 0;
	
	public bool scooby = false;
	
	// Use this for initialization
	void Start () {

		transform.Translate(0,StartHeight-transform.position.y,0);
		transform.LookAt(LevelInfo.Environments.control.transform.position,Vector3.up);
		
		var level = LevelInfo.State.level[LevelInfo.Environments.control.currentLevel];
		
		switch(packType)
		{
		/*case HealthPackType.Ammo:
			int t = Random.Range(0,LevelInfo.Environments.guns.gun.Length);
			while( !LevelInfo.Environments.guns.gun[t].EnabledGun ) t--;
			gunindexifweapon = (Weapon)t;
				
			gameObject.renderer.material.mainTexture = LevelInfo.Environments.guns.gun[(int)gunindexifweapon].texture;
			gameObject.renderer.material.color = Color.yellow;
			break;*/
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
		case HealthPackType.Weapon:
			gunindexifweapon = Weapon.None;
			int ind = Random.Range(0,level.allowedGun.Length);
			for(int i=0;i<level.allowedGun.Length;i++) 
				if(!LevelInfo.Environments.guns.gun[(int)level.allowedGun[(ind+i)%level.allowedGun.Length]].EnabledGun)
				{
					gunindexifweapon = level.allowedGun[(ind+i)%level.allowedGun.Length];
					break;
				}
			if( gunindexifweapon == Weapon.None )
			{
				if( Random.Range(0,2)==0 )
				{
					packType = HealthPackType.DamageMultiplier;
					gameObject.renderer.material.mainTexture = LevelInfo.Environments.texturePickUpDamageMultiplier;
				}
				else
				{
					packType = HealthPackType.XtraLife;
					gameObject.renderer.material.mainTexture = LevelInfo.Environments.texturePickUpXtraLife;
				}
			}
			else
				gameObject.renderer.material.mainTexture = LevelInfo.Environments.guns.gun[(int)gunindexifweapon].texture;
			//gameObject.renderer.material.color = Color.red;
			break;
		/*case HealthPackType.XtraLife:
			gameObject.renderer.material.mainTexture = LevelInfo.Environments.texturePickUpXtraLife;
			break;*/
		}		
	}
	
	void Update()
	{
		if( picked ) return;
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
	
	private IEnumerator PickedUp()
	{
		rigidbody.AddForce(0,200,0);
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
			LevelInfo.Environments.guns.GetAmmoWithMax((Weapon)LevelInfo.Environments.guns.CurrentWeaponIndex);
			pickupname = "Ammo";
			break;
			
		case HealthPackType.Armor:
			LevelInfo.Environments.control.Health = Mathf.Max(LevelInfo.Environments.control.Health,1f);
			pickupname = "Protection";
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
			
		case HealthPackType.Weapon:
			LevelInfo.Environments.guns.GetWeaponWithMAX(gunindexifweapon);
			pickupname = GameEnvironment.storeGun[(int)gunindexifweapon].name;
			break;
			
		case HealthPackType.XtraLife:
			LevelInfo.Environments.hubLives.SetNumberWithFlash(LevelInfo.Environments.hubLives.GetNumber()+1);
			pickupname = "Xtra Life";
			break;	
			
		}
		
		LevelInfo.Environments.generator.GenerateMessageText(transform.position+ new Vector3(0,0.75f,0),pickupname);
		
		LevelInfo.Environments.hubScore.SetNumberWithFlash(LevelInfo.Environments.hubScore.GetNumber() + LevelInfo.State.scoreForPickUp);
		
		Destroy(this.gameObject);
	}
}