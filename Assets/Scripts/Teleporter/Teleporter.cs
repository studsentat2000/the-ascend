using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleporter : MonoBehaviour
{
    public string sceneName;
    Animator animator;
    GameController controller;

    bool teleporterActive = false;
    MovePlayerWithKeyboard movePlayerWithKeyboard;
    public TeleporterManager teleporterManager;

    private void Start()
    {
        animator = GetComponent<Animator>();
        movePlayerWithKeyboard = GameObject.FindGameObjectWithTag("Player").GetComponent<MovePlayerWithKeyboard>();
        controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

        if (sceneName == "Hub") { 
            teleporterActive = true;
        }
    }

    private void Update()
    {
        if (teleporterActive)
        {
            animator.enabled = true;
        }
        else {
            animator.enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("EnteredTrigger");

        if (other.tag == "Player")
        {
            //StartCoroutine(movePlayerWithKeyboard.teleported());

            if (sceneName == "Hub")
            {
                SceneManager.LoadSceneAsync(sceneName);
                teleporterManager.ActivateTeleporters();
            }
            else if (other.CompareTag("Player") && teleporterActive)
            {
                SceneManager.LoadSceneAsync(sceneName + "Main");
                teleporterManager.DeactivateTeleporters();
            }

            
        }
    }

    public void ActivateTeleporter() { 
        this.teleporterActive = true;
    }

    public void DeactivateTeleporter()
    {
        this.teleporterActive = false;
    }

}
