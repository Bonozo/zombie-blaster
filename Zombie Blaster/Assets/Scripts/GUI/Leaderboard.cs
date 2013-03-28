using UnityEngine;
using System.Collections;
using System.Xml;
using System;

public class Leaderboard : MonoBehaviour {
	
	public ButtonBase buttonMainMenu;
	public GUIStyle postGUIStyle;
	public GUIStyle buttonGUIStyle;
	public AudioSource audioOpen,audioNo;
	
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
		GUI.matrix = GameEnvironment.GetGameGUIMatrix();
		
		#region Post, Get Best Score
		/*if( (GUI.Button(new Rect(0.27f*GameEnvironment.GUIWidth,0.67f*GameEnvironment.GUIHeight,0.15f*GameEnvironment.GUIWidth,0.05f*GameEnvironment.GUIHeight), "Get Top Score")) && (isScoreDisplayed == false) )
		{	
        	string url = "http://therealmattharmon.com/Zombie/gettopscore.php?top=10&format=xml";
        	WWW www = new WWW(url);
        	StartCoroutine(WaitForRequest(www));
		
			isPost = false;
			isGet = true;
			
			isScoreDisplayed = true;	
		}
		if( GUI.Button(new Rect(0.425f*GameEnvironment.GUIWidth,0.67f*GameEnvironment.GUIHeight,0.15f*GameEnvironment.GUIWidth,0.05f*GameEnvironment.GUIHeight), "Post Score") )
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
				//GUI.Label(new Rect(0.38f*GameEnvironment.GUIWidth,0.32f*GameEnvironment.GUIHeight,0.15f*GameEnvironment.GUIWidth,0.03f*GameEnvironment.GUIHeight), "" + "NAME",postGUIStyle);				
				
				//GUI.Label(new Rect(0.50f*GameEnvironment.GUIWidth,0.32f*GameEnvironment.GUIHeight,0.15f*GameEnvironment.GUIWidth,0.03f*GameEnvironment.GUIHeight), "" + "SCORE",postGUIStyle);					
				
				//GUI.BeginGroup(new Rect(0.04f*GameEnvironment.GUIWidth,0.14f*GameEnvironment.GUIHeight,0.92f*GameEnvironment.GUIWidth,0.56f*GameEnvironment.GUIHeight));
				
				for(int i = 0; i < 10 && nameArray[i] != null/*by Aharon*/; i++)
				{	
					string nmb = (i+1).ToString() + (i<9?".  ":". ");
					GUI.Label(new Rect(0.05f*GameEnvironment.GUIWidth, (0.15f + yPosition*i)*GameEnvironment.GUIHeight, 0.3f*GameEnvironment.GUIWidth,0.03f*GameEnvironment.GUIHeight), nmb + nameArray[i].ToString().Replace('_',' '),postGUIStyle);
				}	
					//GUI.Label(new Rect(400,40, 200, 70), "" + scoreArray[i].ToString());
					
					//GUI.Label(new Rect(200,40 + yPosition*(i+1), 200, 70), "" + nameArray[i].ToString());
				for(int i = 0; i < 10 && nameArray[i] != null/*by Aharon*/; i++)
				{			
					GUI.Label(new Rect(0.45f*GameEnvironment.GUIWidth, (0.15f + yPosition*i)*GameEnvironment.GUIHeight, 0.15f*GameEnvironment.GUIWidth,0.03f*GameEnvironment.GUIHeight), "" + scoreArray[i].ToString(),postGUIStyle);
				}
				
				// GUI.EndGroup();
			}
			else
			{
				GUI.Label(new Rect(0.05f*GameEnvironment.GUIWidth,0.15f*GameEnvironment.GUIHeight,0.30f*GameEnvironment.GUIWidth,0.032f*GameEnvironment.GUIHeight), getScoreResponse.ToString(),postGUIStyle);
			}
			
			//isGet = false;				
		}
		
		if(isPost)
		{
			//GUI.Label(new Rect(100, 60, 200, 70), getScoreResponse.ToString());
			GUI.Label(new Rect(0.35f*GameEnvironment.GUIWidth,0.355f*GameEnvironment.GUIHeight, 0.1f*GameEnvironment.GUIWidth,0.05f*GameEnvironment.GUIHeight), "NAME: ",postGUIStyle);
			GUI.Label(new Rect(0.35f*GameEnvironment.GUIWidth,0.42f*GameEnvironment.GUIHeight, 0.1f*GameEnvironment.GUIWidth,0.05f*GameEnvironment.GUIHeight), "SCORE: ",postGUIStyle);
			
			nameLB = GUI.TextField(new Rect(0.48f*GameEnvironment.GUIWidth,0.35f*GameEnvironment.GUIHeight, 0.2f*GameEnvironment.GUIWidth,0.05f*GameEnvironment.GUIHeight), nameLB,12,postGUIStyle);
			GUI.Label(new Rect(0.48f*GameEnvironment.GUIWidth,0.42f*GameEnvironment.GUIHeight, 0.2f*GameEnvironment.GUIWidth,0.05f*GameEnvironment.GUIHeight), (LevelInfo.Environments.hubScores.GetNumber()).ToString(),postGUIStyle);
			
			//scoreLB = GUI.TextField(new Rect(0.45f*GameEnvironment.GUIWidth,0.40f*GameEnvironment.GUIHeight, 0.15f*GameEnvironment.GUIWidth,0.035f*GameEnvironment.GUIHeight), scoreLB);

			if( GUI.Button(new Rect(0.45f*GameEnvironment.GUIWidth,0.5f*GameEnvironment.GUIHeight, 0.10f*GameEnvironment.GUIWidth,0.035f*GameEnvironment.GUIHeight), "Post") )
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
				GUI.Label(new Rect(0.38f*GameEnvironment.GUIWidth,0.57f*GameEnvironment.GUIHeight,0.30f*GameEnvironment.GUIWidth,0.032f*GameEnvironment.GUIHeight), postScoreResponse.ToString(),postGUIStyle);					
		}

		#endregion
		
		if( GUI.Button(new Rect(0.76f*GameEnvironment.GUIWidth,0.59f*GameEnvironment.GUIHeight, 0.2f*GameEnvironment.GUIWidth,0.1f*GameEnvironment.GUIHeight), "RESET GAME",buttonGUIStyle) && !wanttoresetgame)
		{
			audioOpen.Play();
			wanttoresetgame = true;
		}
		if(wanttoresetgame)
		{
			GUI.DrawTexture(new Rect(0.2f*GameEnvironment.GUIWidth,0.15f*GameEnvironment.GUIHeight,0.6f*GameEnvironment.GUIWidth,0.6f*GameEnvironment.GUIHeight),texturePopup);
			GUI.Label(new Rect(0.35f*GameEnvironment.GUIWidth,0.305f*GameEnvironment.GUIHeight,0.3f*GameEnvironment.GUIWidth,0.2f*GameEnvironment.GUIHeight),"Are you sure you want to reset game progress?",postGUIStyle);
			if(GUI.Button(new Rect(0.33f*GameEnvironment.GUIWidth,0.5f*GameEnvironment.GUIHeight,0.16f*GameEnvironment.GUIWidth,0.1f*GameEnvironment.GUIHeight), "YES", buttonGUIStyle ) )	
			{
				Store.ClearGameStats();
				Application.LoadLevel(Application.loadedLevel);
				return;
			}
			if( GUI.Button(new Rect(0.52f*GameEnvironment.GUIWidth,0.5f*GameEnvironment.GUIHeight,0.16f*GameEnvironment.GUIWidth,0.1f*GameEnvironment.GUIHeight), "NO", buttonGUIStyle ) )
			{
				wanttoresetgame = false;
				audioNo.Play();
			}	
		}
	}
	
	public Texture2D texturePopup;
	bool wanttoresetgame = false;
}
