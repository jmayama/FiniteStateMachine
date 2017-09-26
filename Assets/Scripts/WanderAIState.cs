using System;
using System.Collections.Generic;
using UnityEngine;

class WanderAIState : AIState
{
    //These values are used to calculate a random time between changing directions
    public float MinTimeToChangeDirection = 1.0f;
    public float MaxTimeToChangeDirection = 5.0f;

    public WanderAIState(Character owningCharacter, CharacterAIController aiController)
        : base(owningCharacter, aiController)
    {
    }

    public override void Activate()
    {
        ChooseNewDirection();
    }

    public override void Deactivate()
    {
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

        //Get random direction
        ChooseNewDirection();

        //Make sure the new direction isn't going into the wall
        Character.MoveAmount = MathUtils.ReflectIfAgainstNormal(Character.MoveAmount, hitInfo.normal);
    }

    public override string GetName()
    {
        return "Wander State";
    }

    private void ChooseNewDirection()
    {
        //Choose a random direction
        Vector2 direction2D = MathUtils.RandomUnitVector2();
        
        Character.MoveAmount = new Vector3(direction2D.x, 0.0f, direction2D.y);

        //update timer
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
}
