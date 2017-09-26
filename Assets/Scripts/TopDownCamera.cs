using UnityEngine;
using System.Collections;

public class TopDownCamera : MonoBehaviour 
{
    public float CameraPosEaseSpeed = 3.0f;

	void Start () 
    {
        m_FollowPosObj = GameObject.Find("Player/CameraFollowPos");
	}
	
	void Update () 
    {
        //Update Position
        Vector3 goalCameraPosition = m_FollowPosObj.transform.position;

        transform.position = MathUtils.ExponentialEase(
            CameraPosEaseSpeed,
            transform.position,
            goalCameraPosition,
            Time.deltaTime
            );

	}

    GameObject m_FollowPosObj;
}
