using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Door : MonoBehaviour
{
    public enum DoorType
    {
        LEFT, RIGHT, TOP, BOTTOM
    }
    
    public DoorType doorType;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Entered");
        Room room = GetComponentInParent<Room>();
        room.enterDoor = this.doorType;
    }
}
