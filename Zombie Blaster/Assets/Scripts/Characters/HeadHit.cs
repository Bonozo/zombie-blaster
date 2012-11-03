using UnityEngine;
using System.Collections;

public class HeadHit : MonoBehaviour {

	public Zombi HeadContainer;
	private BoxCollider boxCollider;
	private Vector3 beginsize;
	private InstantiateObject instantiater;
	
	void Start()
	{
		boxCollider = (BoxCollider)transform.collider;
		instantiater = (InstantiateObject)GameObject.FindObjectOfType(typeof(InstantiateObject));
		beginsize = boxCollider.size;
	}
	
	void Update()
	{
		//boxCollider.size = beginsize*(0.2f*HeadContainer.XZDistFromPlayer()+0.7f);
	}
	
	public void DieNormal()
	{
		InstantiateMessage();
		if( HeadContainer.NearPlayer() )
			GameObject.Find("Goo").SendMessage("Show");
		HeadContainer.SendMessage("DieNormal");
	}
	
	public void DieWithJump()
	{	
		InstantiateMessage();
		if( HeadContainer.NearPlayer() )
			GameObject.Find("Goo").SendMessage("Show");
		HeadContainer.SendMessage("DieWithJump");
	}
	
	public void DieWithFireAndSmoke()
	{
		InstantiateMessage();
		if( HeadContainer.NearPlayer() )
			GameObject.Find("Goo").SendMessage("Show");
		HeadContainer.SendMessage("DieWithFireAndSmoke");
	}
	
	public void DieWithFootball()
	{
		InstantiateMessage();
		if( HeadContainer.NearPlayer() )
			GameObject.Find("Goo").SendMessage("Show");
		HeadContainer.SendMessage("DieWithFootball");
	}
	
	private void InstantiateMessage()
	{
		instantiater.InstantiateMessageText(transform.position+ new Vector3(0f,0.75f,0),"Headshot");
	}
}
