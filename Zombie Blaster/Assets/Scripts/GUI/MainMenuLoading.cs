using UnityEngine;
using System.Collections;

public class MainMenuLoading : MonoBehaviour {
	
	public float wait = 2f;
	
	// Update is called once per frame
	void Update () {
		wait -= Time.deltaTime;
		if (wait<=0f)
			Application.LoadLevel("mainmenu");
	}
}
