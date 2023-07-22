using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum DIFFICULTY { 
    EASY, HARD
}

public class GameController : MonoBehaviour
{
    public int enemySpawnMax = 5;

    public int enemySpawnMin = 3;

    public float enemyMoveSpeed = 3f;

    public int enemyHealh = 5;

    public int enemyHealthPart = 0;

    public int bossHealth = 10;

    public int stage = 0;

    public int fireDistance = 7;

    public float stunnedDuration;

    public DIFFICULTY difficulty;
    public RetryButton retryButton;


    MovePlayerWithKeyboard player;

    public GameObject shopShield;

    private void Awake()
    {
        retryButton = GameObject.FindGameObjectWithTag("retryButton").GetComponent<RetryButton>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<MovePlayerWithKeyboard>();
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (difficulty == DIFFICULTY.HARD)
        {
            stunnedDuration = 0.2f;
        }
        else {
            stunnedDuration = 0.3f;
        }
    }

    public void ClearedStage()
    {
        this.enemyHealh += 1;
        this.stage += 1;
        this.enemyMoveSpeed += 0.3f;
        this.bossHealth += 5;
        if (difficulty == DIFFICULTY.HARD) {
            this.enemySpawnMin += 1;
            this.enemySpawnMax += 1;
        }

        if (stage >= 3) 
        {
            retryButton.activateUI();
        }
    }

    public void finished() {
        
        Destroy(player.gameObject);
    }

    public void restart() {
        Destroy(GameObject.FindGameObjectWithTag("Canvas"));
        Destroy(GameObject.FindGameObjectWithTag("Canvas2"));
        Destroy(GameObject.FindGameObjectWithTag("GameController"));
        SceneManager.LoadSceneAsync("Main");
    }

    public void upgraded(string update) 
    {
        if (difficulty == DIFFICULTY.HARD)
        {
            switch (update)
            {
                case "sword":
                    if (fireDistance >= 4)
                    {
                        this.fireDistance -= 1;
                    }
                    break;
                case "fire":
                    enemyHealthPart += 2;
                    break;
                case "bomb":
                    this.enemySpawnMax += 1;
                    enemyHealthPart++;
                    break;
                case "fireTimeout":
                    this.enemySpawnMax += 1;
                    this.enemySpawnMin += 1;
                    break;
                case "fireSpeed":
                    this.enemyMoveSpeed += 0.5f;
                    break;
            }
            enemyHealthPart++;
            if (enemyHealthPart == 3)
            {
                this.enemyHealh += 1;
                this.enemyHealthPart = 0;
            }
        }
    }
    
}
