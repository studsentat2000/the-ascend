using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    Vector2 rightAttackOffset;
    Collider2D bottomCollider;
    Collider2D topCollider;
    Collider2D leftCollider;
    Collider2D rightCollider;

    [SerializeField]
    public AudioClip audioClip;

    // Start is called before the first frame update
    void Start()
    {
        Collider2D[] colliders = GetComponentsInChildren<Collider2D>();
        Debug.Log(colliders.Length);
        foreach (Collider2D collider in colliders)
        {
            switch(collider.gameObject.tag) {
                case "bottomCollider":
                    
                    bottomCollider = collider;
                    break;
                case "topCollider":
                    
                    topCollider = collider;
                    break;
                case "leftCollider":
                    
                    leftCollider = collider;
                    break;
                case "rightCollider":
                    rightCollider = collider;
                    break;
            }
        }
    }

    public void AttackLeft() {
        Debug.Log("Left");
        //AudioSource.PlayClipAtPoint(audioClip, transform.position);
        leftCollider.enabled = true;
        //transform.position = rightAttackOffset;
    }

    public void AttackRight() {
        //AudioSource.PlayClipAtPoint(audioClip, transform.position);
        Debug.Log("Right");
        rightCollider.enabled = true;
    }

    public void AttackDown() {
        //AudioSource.PlayClipAtPoint(audioClip, transform.position);
        Debug.Log("Down");
        bottomCollider.enabled = true;
        //transform.position = rightAttackOffset;
    }

    public void AttackUp() {
        //AudioSource.PlayClipAtPoint(audioClip, transform.position);
        Debug.Log("Up");
        topCollider.enabled = true;
        //transform.position = rightAttackOffset;
    }

    public void stopAttack() {
        topCollider.enabled = false;
        bottomCollider.enabled = false;
        leftCollider.enabled = false;
        rightCollider.enabled = false;
    }

}
