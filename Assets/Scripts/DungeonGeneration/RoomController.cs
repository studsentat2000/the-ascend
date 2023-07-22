using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEngine.UI;

public class RoomInfo 
{
    public string Name;
    public int X;
    public int Y;
}

public class RoomController : MonoBehaviour
{

    public static RoomController instance;

    public string currentWorldName;

    RoomInfo currentLoadRoomData;

    Queue<RoomInfo> roomQueue = new Queue<RoomInfo>();

    public List<Room> loadedRooms = new List<Room>();

    bool isLoadingRoom = false;

    bool spawnedBossRoom = false;

    bool updatedRoom = false;

    public Canvas canvas;

    public LoadingScreen loadingScreen;

    public MovePlayerWithKeyboard player;

    Room currentRoom;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<MovePlayerWithKeyboard>();
        canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Canvas>();
        canvas.gameObject.SetActive(false);
        player.SceneLoading();
    }

    private void Update()
    {
        UpdateRoomQueue();
    }

    void UpdateRoomQueue ()
    {
        if (isLoadingRoom)
        {
            return;
        }
        if (roomQueue.Count == 0) 
        {
            if (!spawnedBossRoom)
            {
                StartCoroutine(SpawnBossRoom());
            }
            else if (spawnedBossRoom && !updatedRoom)
            {
                foreach (Room room in loadedRooms)
                {
                    room.RemoveUnconnectedDoors();
                }
                updatedRoom = true;

                canvas.gameObject.SetActive(true);
                player.SceneLoaded();
                Destroy(loadingScreen.gameObject);
            }
                return;
            
        }
        currentLoadRoomData = roomQueue.Dequeue();
        isLoadingRoom = true;

        StartCoroutine(LoadRoomRoutine(currentLoadRoomData));
    }

    IEnumerator SpawnBossRoom()
    {
        yield return new WaitForSeconds(0.1f);
        if (roomQueue.Count == 0)
        {
            Room bossRoom = loadedRooms[loadedRooms.Count - 1];
            Vector2Int tempRoom = new Vector2Int(bossRoom.X, bossRoom.Y);
            Destroy(bossRoom.gameObject);

            var roomToRemove = loadedRooms.Single(r => r.X == tempRoom.x && r.Y == tempRoom.y);

            loadedRooms.Remove(roomToRemove);

            LoadRoom("End", tempRoom.x, tempRoom.y);
            foreach (RoomInfo room in roomQueue) {
                Debug.Log(room.Name);
            }
            
        }
        yield return null;
    }

    public void LoadRoom(string name, int x, int y)
    {
        if (DoesRoomExist(x,y)) 
        {
            return;
        }
        RoomInfo newRoomData = new RoomInfo();
        newRoomData.Name = name;
        newRoomData.X = x;
        newRoomData.Y = y;

        roomQueue.Enqueue(newRoomData);
    }

    IEnumerator LoadRoomRoutine(RoomInfo info)
    { 
        string roomName = currentWorldName + info.Name;

        //AsyncOperation loadRoom = SceneManager.LoadSceneAsync(roomName, LoadSceneMode.Additive);
        
        Instantiate(Resources.Load("Rooms/" +roomName));
        
        if (info.Name == "End") {
            spawnedBossRoom = true;
        }

        /*while (loadRoom.isDone == false) 
        { 
            yield return null;
        }*/
        yield return null;
    }

    public void RegisterRoom(Room room)
    {

        if (!DoesRoomExist(currentLoadRoomData.X, currentLoadRoomData.Y))
        {
            room.transform.position = new Vector3(
                    currentLoadRoomData.X * room.Width,
                    currentLoadRoomData.Y * room.Height,
                    0
                );

            room.X = currentLoadRoomData.X;
            room.Y = currentLoadRoomData.Y;
            room.name = currentWorldName + "-" + currentLoadRoomData.Name + " " + room.X + ", " + room.Y;
            //room.transform.parent = transform;
            room.transform.SetParent(transform, false);

            isLoadingRoom = false;

            if (loadedRooms.Count == 0)
            {
                CameraController.instance.currentRoom = room;
            }

            if(currentLoadRoomData.Name == "End") {
                room.RemoveUnconnectedDoors();
            }

            loadedRooms.Add(room);
        }
        else
        {
            Destroy(room.gameObject);
            isLoadingRoom = false;
        }

    }

    public bool DoesRoomExist(int x, int y)
    { 
        return loadedRooms.Find(item => item.X == x && item.Y == y) != null;
    }

    public Room FindRoom(int x, int y)
    {
        return loadedRooms.Find(item => item.X == x && item.Y == y);
    }

    public void OnPlayerEnterRoom(Room room)
    {
        CameraController.instance.currentRoom = room;
        currentRoom = room;
    }
}
