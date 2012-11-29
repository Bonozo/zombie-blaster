using UnityEngine;
using System.Collections;

public class HeadHit : MonoBehaviour {

	public Zombi HeadContainer;
	private BoxCollider boxCollider;
	private Vector3 beginsize;
	
	void Start()
	{
		boxCollider = (BoxCollider)transform.collider;
		beginsize = boxCollider.size;
		//Destroy(this.collider);
	}
	
	void Update()
	{
		boxCollider.size = beginsize*(0.05f*HeadContainer.XZDistFromPlayer()+0.925f);
	}
	
	public void DieDamaged()
	{
		DiePrepare();
		HeadContainer.SendMessage("DieNormal");
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
	
	private void DiePrepare()
	{
		InstantiateMessage();
		if( HeadContainer.NearPlayer() )
			GameObject.Find("Goo").SendMessage("Show");
		Store.zombieHeads++;
		LevelInfo.Environments.control.score += LevelInfo.State.scoreForHeadShot - LevelInfo.State.scoreForZombie;
		LevelInfo.Audio.PlayZombieHeadShot();
	}
	
	private void InstantiateMessage()
	{
		LevelInfo.Environments.generator.GenerateMessageText(transform.position+ new Vector3(0f,0.75f,0),"Headshot");
	}
}
