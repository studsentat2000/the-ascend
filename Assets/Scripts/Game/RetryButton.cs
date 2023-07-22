using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RetryButton : MonoBehaviour
{
    private GameObject finishUI;
    private Button button;
    private GameController gameController;

    // Start is called before the first frame update
    void Start()
    {
        finishUI = GameObject.FindGameObjectWithTag("FinishUI");
        button = GameObject.FindGameObjectWithTag("retryButton").GetComponent<Button>();
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

        finishUI.SetActive(false);

        button.onClick.AddListener(() => gameController.restart());
    }

    public void activateUI() {
        gameController.finished();
        this.finishUI.SetActive(true);
    }
}
