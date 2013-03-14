using UnityEngine;
using System.Collections;
using System.Xml;
using System;

public class Leaderboard : MonoBehaviour {
	
	public ButtonBase buttonMainMenu;
	public GUIStyle postGUIStyle;
	public GUIStyle buttonGUIStyle;
	
	#region LeaderBoardDemo
	
	string getScoreResponse;
	string postScoreResponse;
	
	string []lines;
	
	string []splitByID;
	string []splitByNAME;
	string []splitBySCORE;
	
	string ID;
	string NAME;
	string SCORE;
	
	
	string []idArray = new string[500];
	string []nameArray = new string[500];
	string []scoreArray = new string[500];	
	
	int j = 0;
	int k = 0;
	int l = 0;
	
	int noOfTimesScoreDisplay;
	
	bool isGet;
	bool isPost;
	//*commented for warning*//bool isScoreDisplayed;
	bool isName;
	bool isConnectionError1;
	bool isLeaderBoard;
	
	const float yPosition = 0.055f;
	
	string scoreLB;
	string nameLB;
	
   IEnumerator WaitForRequest(WWW www)
    //void ResponseData()
	{
        yield return www;
		
        if (www.error == null && isGet == true)
        {
			isConnectionError1 = false;
			
			getScoreResponse = www.text;
			//getScoreResponse = "<gamescore><status>Top game store list</status><S_ID>2</S_ID><S_NAME>Jerry</S_NAME><S_SCORE>100</S_SCORE><S_ID>1</S_ID><S_NAME>Jam</S_NAME><S_SCORE>5</S_SCORE><RequestParam><method>Score details</method></RequestParam></gamescore>";
			//Debug.Log(getScoreResponse);
			
			splitByID = new string [] {"<S_ID>"};
			splitByNAME = new string [] {"<S_NAME>"};
			splitBySCORE = new string [] {"<S_SCORE>"};
			
			string []linesID = getScoreResponse.Split(splitByID, StringSplitOptions.None);
			string []linesNAME = getScoreResponse.Split(splitByNAME, StringSplitOptions.None);
			string []linesSCORE = getScoreResponse.Split(splitBySCORE, StringSplitOptions.None);
			
			
			#region ID
			for(int i = 1; i < linesID.Length; i++)
			{
					
				//Debug.Log(linesID[i]);
				
				foreach(char c in linesID[i])
				{
					if(c == '<')
					{
						break;
					}
					else
					{
						ID += c;
					}
				}
				
				if(ID != null)
					idArray[j] = ID;
				
				ID = null;
				j++;
			}
			
			for(int i = 0; idArray[i] == null; i++)
			{
				Debug.Log(idArray[i]);
			}
			#endregion
			
			
			#region NAME
			for(int i1 = 1; i1 < linesNAME.Length; i1++)
			{
					
				//Debug.Log(linesNAME[i1]);
				
				foreach(char c1 in linesNAME[i1])
				{
					if(c1 == '<')
					{
						break;
					}
					else
					{
						NAME += c1;
					}
				}
				
				if(NAME != null)
					nameArray[k] = NAME;
				
				NAME = null;
				k++;
			}
			
			for(int i = 0; nameArray[i] == null; i++)
			{
				Debug.Log(nameArray[i]);
			}
			#endregion
			
			
			#region SCORE			
			for(int i3 = 1; i3 < linesSCORE.Length; i3++)
			{
					
				//Debug.Log(linesSCORE[i3]);
				
				foreach(char c3 in linesSCORE[i3])
				{
					if(c3 == '<')
					{
						break;
					}
					else
					{
						SCORE += c3;
					}
				}
				
				if(SCORE != null)
					scoreArray[l] = SCORE;
				
				SCORE = null;
				l++;
			}
			
			for(int i = 0; scoreArray[i] == null; i++)
			{
				Debug.Log(scoreArray[i]);
			}
			#endregion				
			
		} 
		else if(www.error != null && isGet == true) 
		{
			isConnectionError1 = true;
            //Debug.Log("WWW Error: "+ www.error);
			getScoreResponse = "Error in Network Connection!";
        } 
		
		if(www.error == null && isPost == true)
		{
			postScoreResponse = "Score is successfully posted!";
			//Debug.Log("WWW Ok!: " + www.data);
		}	
		else if(www.error != null && isPost == true)
		{
			postScoreResponse = "Error in Network Connection!!";
	         //Debug.Log("WWW Error: "+ www.error);
		}	
    }	
	
	#endregion
	
	void OnEnable()
	{	
		string url = "http://therealmattharmon.com/Zombie/gettopscore.php?top=10&format=xml";
    	WWW www = new WWW(url);
		StartCoroutine(WaitForRequest(www));

		isPost = false;
		isGet = true;
	
		//*commented for warning*//isScoreDisplayed = true;
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if( buttonMainMenu.PressedUp )
		{
			MainMenu mainmenu = (MainMenu)GameObject.FindObjectOfType(typeof(MainMenu));
			mainmenu.GoState(MainMenu.MenuState.MainMenu);
		}
	}
	
	void OnGUI()
	{
		#region Post, Get Best Score
		/*if( (GUI.Button(new Rect(0.27f*Screen.width,0.67f*Screen.height,0.15f*Screen.width,0.05f*Screen.height), "Get Top Score")) && (isScoreDisplayed == false) )
		{	
        	string url = "http://therealmattharmon.com/Zombie/gettopscore.php?top=10&format=xml";
        	WWW www = new WWW(url);
        	StartCoroutine(WaitForRequest(www));
		
			isPost = false;
			isGet = true;
			
			isScoreDisplayed = true;	
		}
		if( GUI.Button(new Rect(0.425f*Screen.width,0.67f*Screen.height,0.15f*Screen.width,0.05f*Screen.height), "Post Score") )
		{			
			isGet = false;
			isPost = true;
								
			isScoreDisplayed = false;					
		}*/
		
		if(isGet)
		{
			if(isConnectionError1)
				getScoreResponse = "Error in Network Connection!";
			else
				getScoreResponse = "Waiting for Response..";
			
			if(nameArray[0] != null && isConnectionError1 == false )
			{	
				//GUI.Label(new Rect(0.38f*Screen.width,0.32f*Screen.height,0.15f*Screen.width,0.03f*Screen.height), "" + "NAME",postGUIStyle);				
				
				//GUI.Label(new Rect(0.50f*Screen.width,0.32f*Screen.height,0.15f*Screen.width,0.03f*Screen.height), "" + "SCORE",postGUIStyle);					
				
				//GUI.BeginGroup(new Rect(0.04f*Screen.width,0.14f*Screen.height,0.92f*Screen.width,0.56f*Screen.height));
				
				for(int i = 0; i < 10 && nameArray[i] != null/*by Aharon*/; i++)
				{	
					string nmb = (i+1).ToString() + (i<9?".  ":". ");
					GUI.Label(new Rect(0.05f*Screen.width, (0.15f + yPosition*i)*Screen.height, 0.3f*Screen.width,0.03f*Screen.height), nmb + nameArray[i].ToString().Replace('_',' '),postGUIStyle);
				}	
					//GUI.Label(new Rect(400,40, 200, 70), "" + scoreArray[i].ToString());
					
					//GUI.Label(new Rect(200,40 + yPosition*(i+1), 200, 70), "" + nameArray[i].ToString());
				for(int i = 0; i < 10 && nameArray[i] != null/*by Aharon*/; i++)
				{			
					GUI.Label(new Rect(0.45f*Screen.width, (0.15f + yPosition*i)*Screen.height, 0.15f*Screen.width,0.03f*Screen.height), "" + scoreArray[i].ToString(),postGUIStyle);
				}
				
				// GUI.EndGroup();
			}
			else
			{
				GUI.Label(new Rect(0.05f*Screen.width,0.15f*Screen.height,0.30f*Screen.width,0.032f*Screen.height), getScoreResponse.ToString(),postGUIStyle);
			}
			
			//isGet = false;				
		}
		
		if(isPost)
		{
			//GUI.Label(new Rect(100, 60, 200, 70), getScoreResponse.ToString());
			GUI.Label(new Rect(0.35f*Screen.width,0.355f*Screen.height, 0.1f*Screen.width,0.05f*Screen.height), "NAME: ",postGUIStyle);
			GUI.Label(new Rect(0.35f*Screen.width,0.42f*Screen.height, 0.1f*Screen.width,0.05f*Screen.height), "SCORE: ",postGUIStyle);
			
			nameLB = GUI.TextField(new Rect(0.48f*Screen.width,0.35f*Screen.height, 0.2f*Screen.width,0.05f*Screen.height), nameLB,12,postGUIStyle);
			GUI.Label(new Rect(0.48f*Screen.width,0.42f*Screen.height, 0.2f*Screen.width,0.05f*Screen.height), (LevelInfo.Environments.hubScores.GetNumber()).ToString(),postGUIStyle);
			
			//scoreLB = GUI.TextField(new Rect(0.45f*Screen.width,0.40f*Screen.height, 0.15f*Screen.width,0.035f*Screen.height), scoreLB);

			if( GUI.Button(new Rect(0.45f*Screen.width,0.5f*Screen.height, 0.10f*Screen.width,0.035f*Screen.height), "Post") )
			{
				if(nameLB.Trim().Equals(""))	
				{
					isName = false;
					postScoreResponse = "Please Enter Name!";
				}
				else
				{	
					isName = true;
					string url = "http://therealmattharmon.com/Zombie/addscoreresp.php?snm=" + nameLB + "&score=" + (LevelInfo.Environments.hubScores.GetNumber()).ToString() + "&format=xml";
	        		WWW www = new WWW(url);
	        		StartCoroutine(WaitForRequest(www));
				}						
			}
			
			if(!isName || (isPost == true))
				GUI.Label(new Rect(0.38f*Screen.width,0.57f*Screen.height,0.30f*Screen.width,0.032f*Screen.height), postScoreResponse.ToString(),postGUIStyle);					
		}

		#endregion
		
		if( GUI.Button(new Rect(0.76f*Screen.width,0.59f*Screen.height, 0.2f*Screen.width,0.1f*Screen.height), "RESET GAME",buttonGUIStyle) )
			wanttoresetgame = true;
		
		if(wanttoresetgame)
		{
			GUI.DrawTexture(new Rect(0.2f*Screen.width,0.15f*Screen.height,0.6f*Screen.width,0.6f*Screen.height),texturePopup);
			GUI.Label(new Rect(0.35f*Screen.width,0.305f*Screen.height,0.3f*Screen.width,0.2f*Screen.height),"Are you sure you want to reset game progress?",postGUIStyle);
			if(GUI.Button(new Rect(0.33f*Screen.width,0.5f*Screen.height,0.16f*Screen.width,0.1f*Screen.height), "YES", buttonGUIStyle ) )	
			{
				Store.ClearGameStats();
				Application.LoadLevel(Application.loadedLevel);
				return;
			}
			if( GUI.Button(new Rect(0.52f*Screen.width,0.5f*Screen.height,0.16f*Screen.width,0.1f*Screen.height), "NO", buttonGUIStyle ) )
			{
				wanttoresetgame = false;
			}	
		}
	}
	
	public Texture2D texturePopup;
	bool wanttoresetgame = false;
}
