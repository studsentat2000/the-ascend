using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChangeDifficulty : MonoBehaviour
{
    public GameObject difficultyTile;

    private GameObject health;
    private GameObject bombs;
    private GameObject gold;

    private Transform container;

    private Button easyButton;
    private Button hardButton;

    private GameController gameController;

    private GameObject diffUI;

    private GameObject player;

    // Start is called before the first frame update
    void Awake()
    {
        health = GameObject.FindGameObjectWithTag("HealthUI");   
        bombs = GameObject.FindGameObjectWithTag("BombsUI");   
        gold = GameObject.FindGameObjectWithTag("GoldUI");
        diffUI = GameObject.FindGameObjectWithTag("DiffUI");

        easyButton = GameObject.FindGameObjectWithTag("Easy").GetComponent<Button>();
        hardButton = GameObject.FindGameObjectWithTag("Hard").GetComponent<Button>();
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

        player = GameObject.FindGameObjectWithTag("Player");

    }

    private void Start()
    {
        player.GetComponent<Transform>().position = Vector3.zero;
        easyButton.onClick.AddListener(() => ChooseDifficulty("Easy"));
        hardButton.onClick.AddListener(() => ChooseDifficulty("Hard"));
        health.SetActive(false);
        bombs.SetActive(false); 
        gold.SetActive(false);
        player.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ChooseDifficulty(string diff) 
    {
        switch (diff) {
            case "Easy":
                gameController.difficulty = DIFFICULTY.EASY;
                break;
            case "Hard":
                gameController.difficulty = DIFFICULTY.HARD;
                break;
        }
        SceneManager.LoadSceneAsync("Hub");
        health.SetActive(true);
        bombs.SetActive(true);
        gold.SetActive(true);
        player.SetActive(true);
        diffUI.SetActive(false);
    }

    void Show() { 
        

    }

    void Hide() { 
        
    
    }
}
