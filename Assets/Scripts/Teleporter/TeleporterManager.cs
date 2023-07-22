using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterManager : MonoBehaviour
{
    public Teleporter[] teleporters;

    public int currentActiveIndex;

    MovePlayerWithKeyboard player;

    private void Awake()
    {
        
    }


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<MovePlayerWithKeyboard>();
        currentActiveIndex = player.bossIndex;
        //currentActiveIndex = 0;
        teleporters = GetComponentsInChildren<Teleporter>();
        teleporters[currentActiveIndex].GetComponent<Teleporter>().ActivateTeleporter();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeWorld(int index)
    {

        Debug.Log("before" + currentActiveIndex);
        teleporters[currentActiveIndex].GetComponent<Teleporter>().DeactivateTeleporter();
        this.currentActiveIndex = index;
        teleporters[currentActiveIndex].GetComponent<Teleporter>().ActivateTeleporter();
        Debug.Log("after" + currentActiveIndex);
    }

    public void ActivateTeleporters()
    {
        foreach (Teleporter teleporter in teleporters)
        {
            teleporter.gameObject.SetActive(true);
        }
    }

    public void DeactivateTeleporters()
    {
        foreach(Teleporter teleporter in teleporters) { 
            teleporter.gameObject.SetActive(false);
        }
    }
}
