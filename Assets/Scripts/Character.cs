using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour 
{
    public float MaxMoveSpeed = 10.0f;
    public float MaxTurnSpeed = 1000.0f;

	void Start () 
    {
        MoveAmount = new Vector3();
	}

    void Update()
    {
        CharacterController characterController = GetComponent<CharacterController>();

        //Update position
        Vector3 moveVelocity = MoveAmount * MaxMoveSpeed;

        characterController.SimpleMove(moveVelocity);

        //Update rotation
        float rotationThisFrame = RotateAmount * MaxTurnSpeed * Time.deltaTime;

        Quaternion rotation = Quaternion.AngleAxis(rotationThisFrame, new Vector3(0.0f, 1.0f, 0.0f));

        transform.rotation *= rotation;
    }

    void OnControllerColliderHit(ControllerColliderHit hitInfo) 
    {
        //If the character hits anything, notify all of the components that implement the 
        //ColliderHitListener interface
        Component[] hitListeners = GetComponents(typeof(ColliderHitListener));

        foreach (ColliderHitListener hitListener in hitListeners)
        {
            hitListener.OnControllerColliderHit(hitInfo);
        }
    }

    //These properties and functions can be used to control the movement of the character
    public Vector3 MoveAmount{get; set;}

    public float RotateAmount { get; set; }

    public void SetLookDirection(Vector3 direction)
    {
        transform.rotation = Quaternion.LookRotation(direction);
    }
}
