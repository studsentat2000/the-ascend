using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public static CameraController instance;

    public Room currentRoom;

    public float cameraMoveSpeed;

    private void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        updatePosition();
    }

    Vector3 GetCameraTargetPosition()
    {
        if(currentRoom == null)
        {
            return Vector3.zero;
        }

        Vector3 targetPosition = currentRoom.GetRoomCentre();

        targetPosition.z = transform.position.z;

        Debug.Log(targetPosition);

        return targetPosition;
    }

    void updatePosition()
    {
        if (currentRoom == null)
        {
            return;
        }

        Vector3 targetPosition = GetCameraTargetPosition();

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * cameraMoveSpeed);
    }

    public bool isSwitchingScene()
    {
        return transform.position.Equals(GetCameraTargetPosition()) == false;
    }
}
