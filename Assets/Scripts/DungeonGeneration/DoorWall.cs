using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorWall : MonoBehaviour
{
    public enum WallType
    {
        LEFT, RIGHT, TOP, BOTTOM
    }

    public WallType wallType;
}
