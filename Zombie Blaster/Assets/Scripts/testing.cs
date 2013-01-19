using UnityEngine;
using System;
using System.Collections;

public class testing : MonoBehaviour {
	
	int b=0;
	GameObject g;
	// Use this for initialization
	
	static int i=0;
	
	public string AssetName;
	public string BundleURL;

    IEnumerator StartThread()
	{
	   // Download the file from the URL. It will not be saved in the Cache
	   using (WWW www = new WWW("" + Application.dataPath + BundleURL)) {
		   yield return www;
		   if (www.error != null)
			   throw new Exception("WWW download had an error:" + www.error);
		   AssetBundle bundle = www.assetBundle;
		   if (AssetName == "")
			   Instantiate(bundle.mainAsset);
		   else
			   Instantiate(bundle.Load(AssetName));
			
                   // Unload the AssetBundles compressed contents to conserve memory
                  // bundle.Unload(true);
	   }
   }
	
	void Update () 
	{
		
		if( Input.touchCount>0&&Input.touches[0].phase == TouchPhase.Began || Input.GetKeyUp(KeyCode.A))
		{
			if(b==0)
			{
				StartCoroutine(StartThread());
				//g = (GameObject)Instantiate((GameObject)Resources.Load("FARM"+i));
				//i++; if(i==5) i=0;
				b++;
			}
			else if(b==1)
			{
				Destroy(g);
				//DestroyImmediate(g);
				b++;
			}
			else
			{
				Application.LoadLevel(Application.loadedLevel);
			}
				
		}
			
	}
}
