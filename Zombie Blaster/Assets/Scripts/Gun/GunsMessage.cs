using UnityEngine;
using System.Collections;

public class GunsMessage : MonoBehaviour {
	
	public UILabel label;
	public ColorPlayNGUI colorPlay;
	
	private int lastgun = 0;
	private bool reloading=false;
	private float timetoshow=0f;
	
	void OnEnable()
	{
		label.gameObject.SetActive(reloading);
	}
	
	public void ShowReloading(bool enb)
	{
		if(enb)
		{
			if( !reloading )
			{
				reloading = true;
				label.text = "Reloading";
				colorPlay.Reset(new Color(1f,1f,1f,1f), new Color(0.5f,1f,0.5f,1f),10f,0f,0f);
			}
		}
		else
			reloading = false;
	}
	
	public void ShowCurrentWeaponName()
	{
		lastgun = LevelInfo.Environments.guns.CurrentWeaponIndex;
		label.text = GameEnvironment.storeGun[lastgun].name;
		colorPlay.Reset(new Color(0f,0f,0f,0f), new Color(1f,0f,0f,1f),1.7f,0f,0f);
		timetoshow=1f;
	}
	
	void Update()
	{
		if(LevelInfo.Environments.guns.CurrentWeaponIndex != lastgun)
			ShowCurrentWeaponName();
		if(timetoshow>0f)
		{
			timetoshow -= Time.deltaTime;
			if( timetoshow <= 0f && reloading) 
				ShowReloading(true);
		}
		else
			ShowReloading(LevelInfo.Environments.guns.gun[LevelInfo.Environments.guns.CurrentWeaponIndex].reloading );
		
		label.gameObject.SetActive((timetoshow>0f||reloading)&&Time.timeScale!=0.0f);
		
	}
}
