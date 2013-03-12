using UnityEngine;
using System.Collections;

public class FlamethrowerFlame : MonoBehaviour
{
	//private readonly string tagZombie="Zombie",tagHead="ZombieHead";
	
  	void OnParticleCollision(GameObject other) 
	{
		if(other.tag == "Zombie" || other.tag == "ZombieHead" || other.tag == "Ufo")
		{
			other.SendMessage("GetFlame",0.03f);
		}
	}
}
