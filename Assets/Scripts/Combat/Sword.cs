using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour, IUpgradeable
{
    public float knockbackForce = 1;

    private int swordDamage = 1;

    [SerializeField]
    public AudioClip audioClip;

    public void increaseDamage()
    {
        swordDamage += 1;
        Debug.Log("Increased to " +swordDamage);
    }

    public void OnEnemy(Collider2D other) {
        
        Debug.Log("Entered Trigger");
        if (other.tag == "Enemy")
        {
            Debug.Log("hit");
            Debug.Log(other.GetComponent<Enemy>().health);
            Vector3 parentPosition = gameObject.GetComponentInParent<Transform>().position;

            Vector2 direction = (Vector2)(other.gameObject.transform.position - parentPosition).normalized;
            Debug.Log("Knockback: " + knockbackForce);
            Vector2 knockback = direction * knockbackForce;

            //other.SendMessage("OnHit", swordDamage, knockback);
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            enemy.OnHit(swordDamage, knockback);
        }
        else {
            AudioSource.PlayClipAtPoint(audioClip, transform.position);
        }
    }

}
