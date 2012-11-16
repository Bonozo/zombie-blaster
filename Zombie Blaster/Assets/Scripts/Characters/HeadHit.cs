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
	}
	
	void Update()
	{
		boxCollider.size = beginsize*(0.2f*HeadContainer.XZDistFromPlayer()+0.7f);
	}
	
	public void DieNormal()
	{
		InstantiateMessage();
		if( HeadContainer.NearPlayer() )
			GameObject.Find("Goo").SendMessage("Show");
		if( HeadContainer.haveHelmet )
			HeadContainer.SendMessage("GetHitDamaged",2);
		else
		{
			HeadContainer.SendMessage("DieNormal");
			GameEnvironment.zombieHeads++;
			LevelInfo.State.score += LevelInfo.State.scoreForHeadShot - LevelInfo.State.scoreForZombie;
		}
	}
	
	public void DieWithJump()
	{	
		InstantiateMessage();
		if( HeadContainer.NearPlayer() )
			GameObject.Find("Goo").SendMessage("Show");
		GameEnvironment.zombieHeads++;
		LevelInfo.State.score += LevelInfo.State.scoreForHeadShot - LevelInfo.State.scoreForZombie;
		HeadContainer.SendMessage("DieWithJump");
	}
	
	public void DieWithFireAndSmoke()
	{
		InstantiateMessage();
		if( HeadContainer.NearPlayer() )
			GameObject.Find("Goo").SendMessage("Show");
		GameEnvironment.zombieHeads++;
		LevelInfo.State.score += LevelInfo.State.scoreForHeadShot - LevelInfo.State.scoreForZombie;
		HeadContainer.SendMessage("DieWithFireAndSmoke");
	}
	
	public void DieWithFootball()
	{
		InstantiateMessage();
		if( HeadContainer.NearPlayer() )
			GameObject.Find("Goo").SendMessage("Show");
		GameEnvironment.zombieHeads++;
		LevelInfo.State.score += LevelInfo.State.scoreForHeadShot - LevelInfo.State.scoreForZombie;
		HeadContainer.SendMessage("DieWithFootball");
	}
	
	private void InstantiateMessage()
	{
		LevelInfo.Environments.generator.GenerateMessageText(transform.position+ new Vector3(0f,0.75f,0),"Headshot");
	}
}
