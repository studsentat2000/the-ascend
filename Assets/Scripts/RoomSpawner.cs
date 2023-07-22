 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    public int openingDireciton;
    // 1 Bottom
    // 2 Top
    // 3 Left
    // 4 Right

    private RoomTemplates templates;
    private int rand;
    public bool spawned = false;

    public float waitTime = 4f;

    void Start() {
        Destroy(gameObject, waitTime);

        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        Invoke("SpawnRooms", 0.1f);

    }

    void SpawnRooms() {
        if(spawned == false) {
            Vector3 position = transform.position;
            position.z = 0;
            if(openingDireciton == 1) {
                rand = Random.Range(0, templates.bottomRooms.Length);
                Instantiate(templates.bottomRooms[rand], position, templates.bottomRooms[rand].transform.rotation);
            } else if (openingDireciton == 2) {
                rand = Random.Range(0, templates.topRooms.Length);
                Instantiate(templates.topRooms[rand], position, templates.topRooms[rand].transform.rotation);
            } else if (openingDireciton == 3) {
                rand = Random.Range(0, templates.leftRooms.Length);
                Instantiate(templates.leftRooms[rand], position, templates.leftRooms[rand].transform.rotation);
            } else if (openingDireciton == 4) {
                rand = Random.Range(0, templates.rightRooms.Length);
                Instantiate(templates.rightRooms[rand], position, templates.rightRooms[rand].transform.rotation);
            }
            spawned = true;
        }
    }

    void OnTriggerEnter2D(Collider2D other){
		if(other.CompareTag("Spawn Point")){
			if(other.GetComponent<RoomSpawner>().spawned == false && spawned == false){
                Vector3 position = transform.position;
                position.z = 0;
				Instantiate(templates.closedRoom, position, Quaternion.identity);
				Destroy(gameObject);
			} 
			spawned = true;
		}
	}
    
}
