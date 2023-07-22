using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BossEnemy : Enemy
{
    //Don't sue me after looking at this code. I know its horrendous, but my time is running out
    public Teleporter Teleporter;
    public int bossHealth;

    private SpriteRenderer spriteRendererBoss;
    private Animator animatorBoss;

    Vector2 direction;

    private Transform playerBoss;

    private int bossDamage = 1;
    private int knockbackForce = 25;

    private void Start()
    {
        spriteRendererBoss = GetComponent<SpriteRenderer>();
        animatorBoss = GetComponent<Animator>();
        playerBoss = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        room = GetComponentInParent<Room>();
        controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

        this.bossHealth = controller.bossHealth;
        moveSpeed = 3;

    }

    private void Update()
    {
        /*if (bossHealth <= 0)
        {
            Destroy(gameObject);
            player.GetComponent<MovePlayerWithKeyboard>().AddGold(Random.Range(3, 9));
            room.enemyList.Remove(this);
        }*/
        if (bossHealth <= 0)
        {
            Teleporter teleporter = Teleporter.GetComponent<Teleporter>();
            teleporter.sceneName = "Hub";
            Instantiate(teleporter, new Vector3(transform.position.x, transform.position.y), Quaternion.identity);
            Destroy(gameObject);
            room.enemyList.Remove(this);
            playerBoss.GetComponent<MovePlayerWithKeyboard>().ClearedStage();
            
            
        }
    }

    private IEnumerator wait() {
        yield return new WaitForSeconds(0.1f);
    }

    private void FixedUpdate()
    {
        if (playerBoss != null)
        {
            direction = (playerBoss.position - transform.position).normalized;

            if (direction.x > 0.6 || direction.x < -0.6)
            {
                if (bossHealth < 8)
                {
                    animatorBoss.SetTrigger("RightHit");
                }
                else
                {
                    animatorBoss.SetTrigger("Right");
                }
                if (direction.x > 0.5)
                {
                    spriteRendererBoss.flipX = false;
                }
                else
                {
                    spriteRendererBoss.flipX = true;
                }
            }
            else if (direction.y != 0)
            {
                if (direction.y < 0.5)
                {
                    if (bossHealth < 8)
                    {
                        animatorBoss.SetTrigger("BottomHit");
                    }
                    else
                    {
                        animatorBoss.SetTrigger("Bottom");
                    }
                }
                else
                {
                    if (bossHealth < 8)
                    {
                        animatorBoss.SetTrigger("TopHit");
                    }
                    else
                    {
                        animatorBoss.SetTrigger("Top");
                    }
                }
            }
            if (playerInRoom)
            {
                MoveBoss();
            }
        }
    }

    public void MoveBoss()
    {
        Vector2 force = new Vector2(direction.x, direction.y) * moveSpeed;
        rb.velocity = force + knocked;
    }

    //Es spawnen einige Bosse die dann wieder destroyed werden, daher spawned der Teleporter sehr sehr oft, wenn ich das in eine OnDestroy Funktion schreibe, daher ist das hier drinnen
    public override void OnHit(int damage, Vector2 knockback)
    {
        AudioSource.PlayClipAtPoint(audioClip, transform.position);
        bossHealth -= damage;
        
        if (knockback != null)
        {
            this.GetKnocked(knockback);
        }
    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Player"))
        {
            Vector2 direction = (other.collider.GetComponent<Collider2D>().transform.position - transform.position).normalized;

            playerBoss.GetComponent<MovePlayerWithKeyboard>().OnDamaged(bossDamage, direction * knockbackForce);
        }
    }
}
