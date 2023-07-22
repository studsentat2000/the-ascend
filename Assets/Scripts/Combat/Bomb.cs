using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Bomb : MonoBehaviour, IUpgradeable
{
    public enum Direction
    {
        TOP, RIGHT, LEFT, BOTTOM
    };

    [SerializeField]
    public AudioClip audioClip;
    int MaxRadius = 5;
    public Direction direction;
    public CircleCollider2D circleCollider;
    public int Damage;
    bool canDamage = false;

    private Animator animator;

    void Start() {
        circleCollider = GetComponent<CircleCollider2D>();
        animator = GetComponent<Animator>();
        StartCoroutine(Detonate());
        Debug.Log("Start Bomb dmg: " +Damage);
    }

    void Update() { 
        
    }

    IEnumerator Detonate() 
    {
        Debug.Log("Start Detonate");

        yield return new WaitForSeconds(2);

        AudioSource.PlayClipAtPoint(audioClip, transform.position);
        canDamage = true;
        animator.SetTrigger("exploding");
        for (double i = circleCollider.radius; i < MaxRadius; i += 1) {
            
            //circleCollider.radius = (float)i;
            transform.localScale += Vector3.one;
            circleCollider.radius += 0.01f;


            yield return new WaitForSeconds((float)0.5);

            Debug.Log(circleCollider.radius);
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (canDamage) {
            Debug.Log("?");
            if (collision.gameObject.tag == "Enemy")
            {
                Enemy enemy = collision.gameObject.GetComponent<Enemy>();

                enemy.OnHit(Damage, Vector3.zero);
            }
            else if (collision.gameObject.tag == "Player")
            { 
                
                MovePlayerWithKeyboard player = collision.gameObject.GetComponent<MovePlayerWithKeyboard>();

                player.OnDamaged(1, Vector3.zero);
            }
        }
    }

    public void increaseDamage()
    {
        Debug.Log("Bomb dmg: " + Damage);
        Damage += 1;
    }
}

