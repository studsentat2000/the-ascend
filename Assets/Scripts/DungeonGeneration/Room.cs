using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using static Door;

public class Room : MonoBehaviour
{
    public float Height = 13.5f;
    public float Width = 18.5f;
    //public float Width {get {return width;}}
    //public float Height {get {return height;}}
    public int X;
    public int Y;

    public Room(int x, int y) { X = x; Y = y; }
    public GameObject player;

    public Door topDoor;
    public Door bottomDoor;
    public Door rightDoor;
    public Door leftDoor;

    public List<Door> doorList = new();
    public List<Enemy> enemyList;
    //private bool updatedDoors;
    public DoorWall topWall;
    public DoorWall bottomWall;
    public DoorWall rightWall;
    public DoorWall leftWall;
    public DoorType enterDoor;
    public Mover topMover;
    public Mover leftMover;
    public Mover bottomMover;
    public Mover rightMover;

    public bool enemyRoom = false;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        if (RoomController.instance == null)
        {
            Debug.Log("You pressed play in the wrong scene");
            return;
        }
        Enemy[] enemies = GetComponentsInChildren<Enemy>();
        BossEnemy boss = GetComponentInChildren<BossEnemy>();
        if (boss == null)
        {
            foreach (Enemy enemy in enemies)
            {
                enemyList.Add(enemy);
            }
        }
        if (boss != null) {
            Debug.Log(boss);
            enemyList.Add(boss); 
        }
        Door[] doors = GetComponentsInChildren<Door>();
        foreach (Door door in doors)
        {
            doorList.Add(door);
            switch (door.doorType)
            {
                case Door.DoorType.TOP:
                    topDoor = door;
                    break;
                case Door.DoorType.LEFT:
                    leftDoor = door;
                    break;
                case Door.DoorType.RIGHT:
                    rightDoor = door;
                    break;
                case Door.DoorType.BOTTOM:
                    bottomDoor = door;
                    break;
            }
        }

        DoorWall[] walls = GetComponentsInChildren<DoorWall>(includeInactive: true);
        foreach (DoorWall wall in walls)
        {
            switch (wall.wallType)
            {
                case DoorWall.WallType.TOP:
                    topWall = wall;
                    break;
                case DoorWall.WallType.LEFT:
                    leftWall = wall;
                    break;
                case DoorWall.WallType.RIGHT:
                    rightWall = wall;
                    break;
                case DoorWall.WallType.BOTTOM:
                    bottomWall = wall;
                    break;
            }
        }
        RegisterMovers();
        RoomController.instance.RegisterRoom(this);
    }

    private void Update()
    {
        if (enemyList.Count == 0 && enemyRoom)
        {
            OpenDoors();
            player.GetComponent<MovePlayerWithKeyboard>().ClearedRoom();
            enemyRoom = false;
        }
    }

    public void RemoveUnconnectedDoors() 
    {
        foreach (Door door in doorList)
        { 
            switch(door.doorType)
            {
                case Door.DoorType.TOP:
                    if (GetTop() == null)
                    {
                        door.gameObject.SetActive(false);
                        topWall.gameObject.SetActive(true);
                        topWall.gameObject.GetComponent<SpriteRenderer>().enabled = false;
                    }
                    break;
                case Door.DoorType.LEFT:
                    if (GetLeft() == null)
                    {
                        
                        door.gameObject.SetActive(false);
                        leftWall.gameObject.SetActive(true);
                        leftWall.gameObject.GetComponent<SpriteRenderer>().enabled = false;
                    }
                    break;
                case Door.DoorType.RIGHT:
                    if (GetRight() == null)
                    {
                        
                        door.gameObject.SetActive(false);
                        rightWall.gameObject.SetActive(true);
                        rightWall.gameObject.GetComponent<SpriteRenderer>().enabled = false;
                    }
                    break;
                case Door.DoorType.BOTTOM:
                    if (GetBottom() == null)
                    {
                        
                        door.gameObject.SetActive(false);
                        bottomWall.gameObject.SetActive(true);
                        bottomWall.gameObject.GetComponent<SpriteRenderer>().enabled = false;
                    }
                    break;
            }
        }
        
    }

    public void CloseConnectedDoors() 
    {
        foreach (Door door in doorList)
        { 
            switch(door.doorType)
            {
                case Door.DoorType.TOP:
                    if (GetTop() == null)
                    {
                        door.gameObject.SetActive(false);
                        topWall.gameObject.SetActive(true);
                    }
                    break;
                case Door.DoorType.LEFT:
                    if (GetLeft() == null)
                    {
                        leftWall.gameObject.SetActive(true);
                        door.gameObject.SetActive(false);
                    }
                    break;
                case Door.DoorType.RIGHT:
                    if (GetRight() == null)
                    {
                        rightWall.gameObject.SetActive(true);
                        door.gameObject.SetActive(false);
                    }
                    break;
                case Door.DoorType.BOTTOM:
                    if (GetBottom() == null)
                    {
                        bottomWall.gameObject.SetActive(true);
                        door.gameObject.SetActive(false);
                    }
                    break;
            }
        }
        
    }

    public void CloseDoors() 
    {
        foreach (Door door in doorList)
        { 
            if(door.gameObject.activeSelf) {
                switch(door.doorType)
                {
                    case Door.DoorType.TOP:
                        
                            door.gameObject.SetActive(false);
                            topWall.gameObject.SetActive(true);
                        
                        break;
                    case Door.DoorType.LEFT:
                    
                        leftWall.gameObject.SetActive(true);
                        door.gameObject.SetActive(false);
                        
                        break;
                    case Door.DoorType.RIGHT:
                        
                        rightWall.gameObject.SetActive(true);
                        door.gameObject.SetActive(false);
                        
                        break;
                    case Door.DoorType.BOTTOM:
                        
                        bottomWall.gameObject.SetActive(true);
                        door.gameObject.SetActive(false);
                        
                        break;
                }
            }
        }
    }

    public void OpenDoors() 
    {
        foreach (Door door in doorList)
        { 
            switch(door.doorType)
            {
                case Door.DoorType.TOP:
                    if(GetTop() != null) {
                        door.gameObject.SetActive(true);
                        topWall.gameObject.SetActive(false);
                    }
                    break;
                case Door.DoorType.LEFT:
                    if (GetLeft() != null) {
                        leftWall.gameObject.SetActive(false);
                        door.gameObject.SetActive(true);
                    }
                    break;
                case Door.DoorType.RIGHT:
                    if (GetRight() != null) {
                        rightWall.gameObject.SetActive(false);
                        door.gameObject.SetActive(true);
                    }
                    break;
                case Door.DoorType.BOTTOM:
                    if (GetBottom() != null) {
                        bottomWall.gameObject.SetActive(false);
                        door.gameObject.SetActive(true);
                    }
                    break;
            }
        }
    }

    public Room GetRight()
    {
        if (RoomController.instance.DoesRoomExist(X + 1, Y)) 
        {
            return RoomController.instance.FindRoom(X + 1, Y);
        }
        return null;
    }
    public Room GetLeft()
    {
        if (RoomController.instance.DoesRoomExist(X - 1, Y))
        {
            return RoomController.instance.FindRoom(X - 1, Y);
        }
        return null;

    }
    public Room GetTop()
    {
        if (RoomController.instance.DoesRoomExist(X, Y + 1))
        {
            return RoomController.instance.FindRoom(X, Y + 1);
        }
        return null;

    }
    public Room GetBottom()
    {
        if (RoomController.instance.DoesRoomExist(X, Y -1))
        {
            return RoomController.instance.FindRoom(X, Y-1);
        }
        return null;

    }



    private void OnDrawGizmos()
    {
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(Width, Height, 0));
    }

    public Vector3 GetRoomCentre()
    {

        Debug.Log("X: "+ X);
        Debug.Log("Y: "+ Y);
        Debug.Log("Width: "+ Width);
        Debug.Log("Height: " + Height);

        double offsetHeight;
        if (Y == 0)
        {
            offsetHeight = 0.1f;
        }
        else {
            offsetHeight = 0.1f;
        }
        double offsetWidth;
        if (X == 0)
        {
            offsetWidth = 0.24f;
        }
        else { 
            offsetWidth = 0.24f;
        }
        return new Vector3(X * (Width) + (float)offsetWidth, Y * (Height) - (float)offsetHeight);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            RoomController.instance.OnPlayerEnterRoom(this);
            if (enemyList.Count != 0) {
                enemyRoom = true;
                //TODO future me: Change this name pls
                MovePlayerWithKeyboard player = collision.gameObject.GetComponent<MovePlayerWithKeyboard>();
                switch(enterDoor) {
                    case DoorType.TOP:
                        player.enteredRoom(topMover);
                        break;
                    case DoorType.LEFT:
                        player.enteredRoom(leftMover);
                        break;
                    case DoorType.RIGHT:
                        player.enteredRoom(rightMover);
                        break;
                    case DoorType.BOTTOM:
                        player.enteredRoom(bottomMover);
                        break;
                }
                CloseDoors();
            }
            foreach (Enemy e in enemyList)
            {
                
                e.playerInRoom = true;
                StartCoroutine(e.activateDamage());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.tag == "Player")
        {
            foreach (Enemy e in enemyList)
            {
                e.playerInRoom = false;
                e.moveSpeed = 0;
            }
        }
    }

    private void RegisterMovers() {
        Mover[] movers = GetComponentsInChildren<Mover>();
        foreach (Mover mover in movers)
        {
            switch(mover.tag) {
                case "topMover":
                    topMover = mover;
                    break;
                case "bottomMover":
                    bottomMover = mover;
                    break;
                case "leftMover":
                    leftMover = mover;
                    break;
                case "rightMover":
                    rightMover = mover;
                    break;
            }
        }

    }
}
