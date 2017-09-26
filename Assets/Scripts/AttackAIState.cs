using System;
using System.Collections.Generic;
using UnityEngine;

class AttackAIState : AIState
{
    public AttackAIState(Character owningCharacter, CharacterAIController aiController)
        : base(owningCharacter, aiController)
    {
    }

    public override void Activate()
    {
    }

    public override void Deactivate()
    {
        //Clear the movement
        Character.MoveAmount = Vector3.zero;
    }

    public override void Update()
    {
        //If you can't see the player, switch to the find target state to try to find the player again.
        if (!CanSeePlayer())
        {
            AIController.SetState(new FindTargetAIState(Character, AIController));
        }

        //Update moving and looking
        MoveToTarget();

        UpdateLookDirection();

        //TODO: attack the player if you are close enough
    }

    public override void OnControllerColliderHit(ControllerColliderHit hitInfo) 
    {

    }

    public override string GetName()
    {
        return "Attack State";
    }

    private void MoveToTarget()
    {
        //Get the target
        GameObject targetObj = GetTarget();
        if (targetObj == null)
        {
            //If you don't have a target switch back to the wander state
            AIController.SetState(new WanderAIState(Character, AIController));

            return;
        }

        //Move toward the target at full speed
        Vector3 moveDir = targetObj.transform.position - Character.transform.position;
        moveDir.y = 0.0f;

        moveDir.Normalize();

        Character.MoveAmount = moveDir;
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
}
