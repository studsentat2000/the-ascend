using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{

    private void Awake()
    {
        Vector2 aspectRatio = new Vector2(755, 550);

        if (aspectRatio != Vector2.zero)
        {
            float x = Screen.height * (aspectRatio.x / aspectRatio.y);
            float y = Screen.height;
            Screen.SetResolution((int)x, (int)y, true);
            }
    }
    // Start is called before the first frame update
    void Start()
    {
        
        Debug.Log("Does this run?");
        //SceneManager.LoadSceneAsync("Hub");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
