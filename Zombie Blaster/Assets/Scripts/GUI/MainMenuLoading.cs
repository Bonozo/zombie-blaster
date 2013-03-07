using UnityEngine;
using System.Collections;

public class MainMenuLoading : MonoBehaviour {
	
	public float wait = 2f;
	
	IEnumerator Start()
	{
		yield return new WaitForSeconds(wait);
		Application.LoadLevel("mainmenu");
	}
}
