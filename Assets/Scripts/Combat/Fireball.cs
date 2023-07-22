using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class Fireball : MonoBehaviour, IUpgradeable
{
    public enum Direction{
        top,bottom,left, right
    };

    public Direction direction;
    public float moveSpeed;
    public int fireDmg;
    Rigidbody2D rb;
    Vector2 position;
    int knockback = 1;
    public bool enemyAttack = false;
    public Vector3 playerPosition;

    private SpriteRenderer spriteRenderer;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        position = transform.position;
        Debug.Log("FireDmg: " +fireDmg);
        if (enemyAttack)
        {
            Vector3 direction = (playerPosition - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            rb.rotation = angle - 90f;
        }
        else {
            flipAnimation(direction);
        }
    }

    bool destroyStarted = false;
    void FixedUpdate()
    {
        if (enemyAttack)
        {
            
            rb.MovePosition(Vector3.MoveTowards(transform.position, playerPosition, 0.1f));
            Debug.Log("Enemy fireball");
            if (playerPosition == transform.position)
            {
                Destroy(gameObject);
            }
            if (!destroyStarted)
            {
                StartCoroutine(enemyBall());
                destroyStarted = true;
            }
            
        }
        else
        {
            rb.MovePosition(
                    MoveBall(direction)
            );
            if (Vector2.Distance(position, transform.position) >= 10)
            {
                Destroy(gameObject);
            }
            Debug.Log("Player fireball");
        }
        
        
    }

    IEnumerator enemyBall ()
    {
        yield return new WaitForSeconds(3);
        if (gameObject) {
            Destroy(gameObject);
        }
        
    }

    void flipAnimation(Direction direction) {
        switch (direction)
        {
            case Direction.top:
                break;
            case Direction.bottom:
                spriteRenderer.flipY = true;
                break;
            case Direction.left:
                transform.rotation = Quaternion.Euler(0, 0, 90);
                break;
            case Direction.right:
                transform.rotation = Quaternion.Euler(0, 0, 270);
                break;
        }
    }

    Vector2 MoveBall(Direction direction) 
    {
        float x = position.x;
        float y = position.y;
        switch(direction) {
            case Direction.top:
                y = transform.position.y + moveSpeed * Time.fixedDeltaTime;
                break;
            case Direction.bottom:
                y = transform.position.y + moveSpeed * Time.fixedDeltaTime * -1;
                break;
            case Direction.left:
                x = transform.position.x + moveSpeed * Time.fixedDeltaTime *-1;
                break;
            case Direction.right:
                x = transform.position.x + moveSpeed * Time.fixedDeltaTime;
                break;
        }
        return new Vector2(x,y);
    }

    int knockbackForce = 15;

    private void OnTriggerEnter2D(Collider2D other) {
        Vector3 parentPosition = gameObject.GetComponentInParent<Transform>().position;
        Vector2 direction = (Vector2)(other.gameObject.transform.position - parentPosition).normalized;
        Vector2 knockback = direction * knockbackForce;
        if (!enemyAttack && other.tag == "Enemy") {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            
            enemy.OnHit(fireDmg, knockback);
            Destroy(gameObject);
        } 
        else if (enemyAttack && other.tag == "Player")
        {
            MovePlayerWithKeyboard player= other.gameObject.GetComponent<MovePlayerWithKeyboard>();
            player.OnDamaged(fireDmg, knockback);
            Destroy(gameObject);
        }
    }

    public void increaseDamage()
    {
        fireDmg += 1;
        Debug.Log("Bought fire increase");
        Debug.Log("Fire Damage: " +fireDmg);
    }

    public void increaseSpeed() {
        moveSpeed += 1;
    }
}
