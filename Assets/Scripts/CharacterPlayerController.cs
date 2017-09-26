using UnityEngine;
using System.Collections;

public class CharacterPlayerController : MonoBehaviour
{
    private const float PointerRayDist = 1000.0f;

    public float TurnToGoalThresholdDegs = 1.0f;
    public float ArriveAtGoalThresholdDist = 0.5f;

	void Start () 
    {
	    m_Character = GetComponent<Character>();
	}
	
	void Update () 
    {
	    if (Input.GetMouseButtonDown(0))
        {
            HandleMouseClick();
        }

        switch (m_BehaviourState)
        {
            case BehaviourState.Idle:
                UpdateIdle();
                break;

            case BehaviourState.MovingToGoal:
                UpdateMovingToGoal();
                break;

            case BehaviourState.TurningToGoal:
                UpdateTurningToGoal();
                break;

            default:
                Debug.LogError("Invalid command state: " + m_BehaviourState);
                break;
        }
	}



    private void HandleMouseClick()
    {
        Ray pointerRay = Camera.main.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

        RaycastHit hitInfo;
        if (!Physics.Raycast(pointerRay, out hitInfo, PointerRayDist))
        {
            return;
        }

        //Only move to places if the player clicks on the ground
        GameObject clickObject = hitInfo.collider.gameObject;
        if (clickObject.tag != "Ground")
        {
            return;
        }

        Debug.DrawRay(hitInfo.point, new Vector3(0.0f, 1.0f, 0.0f), Color.red, 5.0f, false);

        Vector3 goalPos = hitInfo.point;
        goalPos.y = transform.position.y;

        SetGoalPosition(goalPos);
    }

    private void SetGoalPosition(Vector3 goalPos)
    {
        m_GoalPos = goalPos;

        SetState(BehaviourState.TurningToGoal);
    }

    private void SetState(BehaviourState state)
    {
        switch (state)
        {
            case BehaviourState.Idle:
                m_Character.RotateAmount = 0.0f;
                m_Character.MoveAmount = new Vector3(0.0f, 0.0f, 0.0f);
                break;

            case BehaviourState.MovingToGoal:
                m_Character.RotateAmount = 0.0f;
                break;

            case BehaviourState.TurningToGoal:
                m_Character.MoveAmount = new Vector3(0.0f, 0.0f, 0.0f);
                break;

            default:
                Debug.LogError("Invalid command state: " + state);
                break;
        }

        m_BehaviourState = state;
    }

    private void UpdateIdle()
    {
    }

    private void UpdateTurningToGoal()
    {
        //Calc angle to goal
        Vector3 facingDir = transform.forward;
        
        Vector3 goalDir = m_GoalPos - transform.position;

        facingDir.y = 0.0f;
        goalDir.y = 0.0f;

        float goalDist = goalDir.magnitude;
        if (goalDist <= 0.0f)
        {
            SetState(BehaviourState.Idle);

            return;
        }

        float angleToGoal = Vector3.Angle(facingDir, goalDir);

        if (angleToGoal < TurnToGoalThresholdDegs)
        {
            SetState(BehaviourState.MovingToGoal);

            return;
        }

        float maxRotateThisFrame = m_Character.MaxTurnSpeed * Time.deltaTime;

        float rotatePercent = Mathf.Clamp01(angleToGoal / maxRotateThisFrame);

        Vector3 rotateAxis = Vector3.Cross(facingDir, goalDir);

        m_Character.RotateAmount = rotateAxis.y > 0.0f ? rotatePercent : -rotatePercent;
    }

    private void UpdateMovingToGoal()
    {
        Vector3 goalDir = m_GoalPos - transform.position;

        goalDir.y = 0.0f;

        float goalDist = goalDir.magnitude;
        if (goalDist <= ArriveAtGoalThresholdDist)
        {
            SetState(BehaviourState.Idle);

            return;
        }

        goalDir /= goalDist;

        float maxMoveDistThisFrame = m_Character.MaxMoveSpeed * Time.deltaTime;

        float movePercent = Mathf.Clamp01(goalDist / maxMoveDistThisFrame);

        m_Character.MoveAmount = goalDir * movePercent;
    }

    private enum BehaviourState
    {
        Idle,
        TurningToGoal,
        MovingToGoal
    }

    private Vector3 m_GoalPos;

    private Character m_Character;

    private BehaviourState m_BehaviourState;
}
