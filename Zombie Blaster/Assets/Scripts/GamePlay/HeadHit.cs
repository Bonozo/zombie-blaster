using UnityEngine;
using System.Collections;

[AddComponentMenu("GamePlay/HeadHit")]
public class HeadHit : MonoBehaviour {
	
	public bool HeadShotted = false;
	public Zombi HeadContainer;
	private Vector3 beginsize;
	
	void Start()
	{
		//boxCollider = (BoxCollider)transform.collider;
		BoxCollider c = (BoxCollider)transform.collider;
		Destroy(this.collider);
		SphereCollider sphere = gameObject.AddComponent<SphereCollider>();
		
		var cc = c.center;// cc.z += 0.02f;
		sphere.center = cc;
		sphere.radius = c.size.x*0.5f;
		//sph.radius = rad;
		//beginsize = boxCollider.size;
		//Destroy(this.collider);
	}
	
	void Update()
	{
		//boxCollider.size = beginsize*(0.05f*HeadContainer.XZDistFromPlayer()+0.925f);
	}
	
	public void DieDamaged()
	{
		DiePrepare();
		HeadContainer.SendMessage("DieNormal");
	}
	
	public void GetFlame(float delta)
	{
		HeadContainer.GetFlame(2*delta);
	}
	
	//private int headshotscountairsoft=0;
	public void DieWithAirsoft()
	{
		if( HeadContainer.GetHitDamagedTest(5) )
			DiePrepare();
		HeadContainer.GetHitDamaged(5);	
		
	}
	
	public void DieNormal()
	{
		if( HeadContainer.haveHelmet )
			HeadContainer.SendMessage("GetHitDamaged",2);
		else
		{
			DiePrepare();
			HeadContainer.SendMessage("DieNormal");
		}
	}
	
	public void DieWithJump()
	{	
		DiePrepare();
		HeadContainer.SendMessage("DieWithJump");
	}
	
	public void DieWithFireAndSmoke()
	{
		DiePrepare();
		HeadContainer.SendMessage("DieWithFireAndSmoke");
	}
	
	public void DieWithFootball()
	{
		DiePrepare();
		HeadContainer.SendMessage("DieWithFootball");
	}
	
	public void DieWithElectricity()
	{
		DiePrepare();
		HeadContainer.SendMessage("DieWithElectricity");	
	}
	
	private void DiePrepare()
	{
		HeadShotted = true;
		LevelInfo.Environments.control.currentHeadshotsInWave++;
		InstantiateMessage();
		if( HeadContainer.NearPlayer() )
			LevelInfo.Environments.goo.Show();
		Store.zombieHeads++;
		LevelInfo.Environments.control.GetScore(LevelInfo.State.scoreForHeadShot - LevelInfo.State.scoreForZombie,true);
		LevelInfo.Audio.PlayZombieHeadShot();
	}
	
	private void InstantiateMessage()
	{
		LevelInfo.Environments.generator.GenerateMessageText(transform.position+ new Vector3(0f,0.75f,0),"Headshot");
	}
}
