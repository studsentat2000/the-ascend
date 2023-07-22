using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildSword : MonoBehaviour
{

    public Sword sword;

    private void Start()
    {
        sword = transform.parent.gameObject.GetComponent<Sword>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        sword.OnEnemy(collision);
    }
}
