using System;
using System.Collections.Generic;
using UnityEngine;

class FindTargetAIState : AIState
{
    //These values are used to calculate a random time between changing directions
    public float MinTimeToChangeDirection = 1.0f;
    public float MaxTimeToChangeDirection = 5.0f;

    public float MaxTimeToGiveUpOnTarget = 5.0f;

    public FindTargetAIState(Character owningCharacter, CharacterAIController aiController)
        : base(owningCharacter, aiController)
    {
    }

    public override void Activate()
    {
        ChooseNewDirection();

        m_TimeToReturnToWander = MaxTimeToGiveUpOnTarget;
    }

    public override void Deactivate()
    {
        //Clear the movement info
        Character.MoveAmount = Vector3.zero;
    }

    public override void Update()
    {
        //If you can see the player switch to your attack state
        if (CanSeePlayer())
        {
            AIController.SetState(new AttackAIState(Character, AIController));

            return;
        }

        //If you don't see your target after a certain amount of time return to your wander state
        if (m_TimeToReturnToWander > 0.0f)
        {
            m_TimeToReturnToWander -= Time.deltaTime;
        }
        else
        {
            AIController.SetState(new WanderAIState(Character, AIController));

            return;
        }

        //Continue in the same direction for a bit of time then choose a new direction
        if (m_TimeLeftTillChangeDirection > 0.0f)
        {
            m_TimeLeftTillChangeDirection -= Time.deltaTime;
        }
        else
        {
            ChooseNewDirection();
        }

        //Update look direction
        UpdateLookDirection();
    }

    public override void OnControllerColliderHit(ControllerColliderHit hitInfo) 
    {
        //Check if the collision normal is almost horizontal
        float hitAngleFromUp = Vector3.Angle(hitInfo.normal, Vector3.up);

        if (!MathUtils.AlmostEquals(hitAngleFromUp, 90.0f, 1.0f))
        {
           return;
        }

        //Choose a random direction
        ChooseNewDirection();

        //Make sure the new direction isn't going into the wall
        Character.MoveAmount = MathUtils.ReflectIfAgainstNormal(Character.MoveAmount, hitInfo.normal);
    }

    public override string GetName()
    {
        return "Find Target State";
    }

    private void ChooseNewDirection()
    {
        //If there isn't a target anymore, return to the wander state
        GameObject targetObj = GetTarget();
        if (targetObj == null)
        {
            AIController.SetState(new WanderAIState(Character, AIController));

            return;
        }

        //Calculate the direction towards the target
        Vector3 targetDir = targetObj.transform.position - Character.transform.position;
        targetDir.y = 0.0f;
        targetDir.Normalize();

        //Calculate a random direction
        Vector2 randomDir2D = MathUtils.RandomUnitVector2();
        
        //Combine the directions
        targetDir.x += randomDir2D.x;
        targetDir.z += randomDir2D.y;
        targetDir.Normalize();

        //Set the movement
        Character.MoveAmount = targetDir;

        //Update timer
        m_TimeLeftTillChangeDirection = UnityEngine.Random.Range(MinTimeToChangeDirection, MaxTimeToChangeDirection);
    }

    private void UpdateLookDirection()
    {
        //if you aren't moving don't bother changing your direction
        if (Character.MoveAmount.sqrMagnitude <= 0.0f)
        {
            return;
        }

        //Look in your movement direction
        Character.SetLookDirection(Character.MoveAmount);
    }

    private float m_TimeLeftTillChangeDirection;

    private float m_TimeToReturnToWander;
}
