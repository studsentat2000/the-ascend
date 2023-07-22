using UnityEngine;

[CreateAssetMenu(fileName="Spawner.asset", menuName = "Spawners/Spawner")]
public class SpawnerData : ScriptableObject
{
    public GameObject itemToSpawn;

    //public int minSpawn => itemToSpawn.GetComponent<BossEnemy>() == null ? gameController.enemySpawnMin : 1;

    //public int maxSpawn => itemToSpawn.GetComponent<BossEnemy>() == null ? gameController.enemySpawnMax : 1;

}
