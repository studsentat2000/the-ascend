using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    [System.Serializable]
    public struct RandomSpawner {
        public string name;
        public SpawnerData spawnerData;
    }


    public GridController grid;

    public RandomSpawner[] spawner;
    public bool bossRoom = false;
    public Room room;
    public GameController gameController;

    private void OnEnable()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    private void Start()
    {
        
        room = GetComponentInParent<Room>();
        //grid = GetComponentInChildren<GridController>();
    }

    public void InitialiseObjectSpawning() 
    {
        foreach(RandomSpawner rs in spawner) 
        {
            SpawnObjects(rs);
        }
    }

    void SpawnObjects(RandomSpawner data)
    {
        if (data.spawnerData.itemToSpawn.GetComponent<BossEnemy>() != null) {
            Instantiate(data.spawnerData.itemToSpawn, grid.availablePoints[Mathf.CeilToInt(grid.availablePoints.Count / 2)], Quaternion.identity, transform);
        }
        else 
        {
            int randomIteration = Random.Range(gameController.enemySpawnMin, gameController.enemySpawnMax);

            for (int i = 0; i < randomIteration; i++)
            {
                int randomPos = Random.Range(0, grid.availablePoints.Count - 1);
                GameObject obj = Instantiate(data.spawnerData.itemToSpawn, grid.availablePoints[randomPos], Quaternion.identity, transform) as GameObject;
                grid.availablePoints.RemoveAt(randomPos);
            }
        }

        /*else if (data.spawnerData.maxSpawn == 1){
            //Debug.Log("Spawning boss");
            Debug.Log(grid.availablePoints[grid.availablePoints.Count / 2]);
            Instantiate(data.spawnerData.itemToSpawn, grid.availablePoints[Mathf.CeilToInt(grid.availablePoints.Count/2)], Quaternion.identity, transform);
        }*/
    }
}

