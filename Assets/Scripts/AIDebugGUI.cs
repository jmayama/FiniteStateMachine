using UnityEngine;
using System.Collections;

public class AIDebugGUI : MonoBehaviour 
{

	void Start () 
    {
	
	}
	
	void Update () 
    {
	
	}

    void OnGUI()
    {
        //This is a quick bit of code to display information from the AI system.  Note that a more elaborate
        //system will propably be needed if the info is going to come from multiple AI entities.
        GUI.Label(new Rect(30, 30, 3000, 20), AIDebugDisplayMsg);
    }

    public string AIDebugDisplayMsg { set; get; }
}
