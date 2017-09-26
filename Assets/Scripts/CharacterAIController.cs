using UnityEngine;
using UnityEditor;
using System.Collections;

public class CharacterAIController : MonoBehaviour, ColliderHitListener 
{
    void Start () 
    {
        Character character = GetComponent<Character>();

        //Start the AI in the wander state
	    SetState(new WanderAIState(character, this));
	}
	
	void Update () 
    {
        //Update the state if you have one
        if (m_CurrentAIState != null)
        {
            m_CurrentAIState.Update();
        }

        //Update debug info
        UpdateDebugDisplay();
	}

    public void OnControllerColliderHit(ControllerColliderHit hitInfo)
    {
        //Pass hit events onto your state for processing
        if (m_CurrentAIState != null)
        {
            m_CurrentAIState.OnControllerColliderHit(hitInfo);
        }
    }

    public void SetState(AIState state)
    {
        //Deactivate your old state
        if (m_CurrentAIState != null)
        {
            m_CurrentAIState.Deactivate();
        }

        //switch to the new state
        m_CurrentAIState = state;

        //Activate the new state
        if (m_CurrentAIState != null)
        {
            m_CurrentAIState.Activate();
        }
    }

    //Update the displayed info about the AI.  
    private void UpdateDebugDisplay()
    {
        AIDebugGUI debugGUI = Camera.main.GetComponent<AIDebugGUI>();
        if (debugGUI == null)
        {
            return;
        }

        //Ignore this if the object isn't selected.  Note that this won't work properly if there is more
        //than one ai entity selected.
        if (!Selection.Contains(gameObject))
        {
            return;
        }

        debugGUI.AIDebugDisplayMsg = "CurrentState = ";

        if (m_CurrentAIState != null)
        {
            debugGUI.AIDebugDisplayMsg += m_CurrentAIState.GetName();
        }
        else
        {
            debugGUI.AIDebugDisplayMsg += "null";
        }
    }

    private AIState m_CurrentAIState;
}
