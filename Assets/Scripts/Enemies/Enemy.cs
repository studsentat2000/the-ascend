using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Enemy : MonoBehaviour
{
    public int health;
    public Rigidbody2D rb;
    public Transform player;
    public Vector2 moveDirection;
    public Room room;

    public GameController controller;

    public float moveSpeed;

    public bool playerInRoom = false;
    bool canFire = false;
    public Fireball fireball;
    public bool canMove = false;
    SpriteRenderer spriteRenderer;

    [SerializeField]
    public AudioClip audioClip;

    Animator animator;

    int knockbackForce = 25;

    int damage = 1; 

    float knockbackDuration = 0.25f;

    private bool canDamage = false;

    public Vector2 knocked = Vector2.zero;

    private int fireDistance;

    private float stunnedDuration;

    private int goldMin;
    private int goldMax;
    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start() {
        controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        player = GameObject.Find("Player").transform;
        //.health += player.GetComponent<MovePlayerWithKeyboard>().bossIndex * 2;
        //this.moveSpeed += player.GetComponent<MovePlayerWithKeyboard>().bossIndex;
        room = GetComponentInParent<Room>();
        setValues();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator.SetTrigger("MovingBottom");
        //StartCoroutine(activateDamage());

        if (controller.difficulty == DIFFICULTY.EASY)
        {
            goldMin = 1;
            goldMax = 5;
        }
        else {
            goldMin = 4;
            goldMax = 10;
        }
    }

    public IEnumerator activateDamage() {
        yield return new WaitForSeconds(0.3f);

        canDamage = true;
        canMove = true;
        canFire = true;

        yield return null;
    }

    void setValues() {
        this.health = controller.enemyHealh;
        this.moveSpeed = controller.enemyMoveSpeed;
        this.stunnedDuration = controller.stunnedDuration;
        this.fireDistance = controller.fireDistance;
    }

    private void Update() {
        if (player && playerInRoom && canDamage)
        {
            float distance = Vector3.Distance(player.transform.position, transform.position);
            if (distance >= fireDistance && canFire)
            {
                StartCoroutine(rangeAttack());
            }
            else if (canMove)
            {
                Vector3 direction = (player.position - transform.position).normalized;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                rb.rotation = angle  + 90f;
                moveDirection = direction;
            }   
        }


        if(health <= 0) {
            Destroy(gameObject);
            player.GetComponent<MovePlayerWithKeyboard>().AddGold(Random.Range(goldMin, goldMax));
            room.enemyList.Remove(this);
        }
    }

    private void FixedUpdate()
    {
        if (canMove && playerInRoom) 
        {
            MoveEnemy();
            /*Vector2 currentPosition = transform.position;
            Vector2 difference = moveDirection - currentPosition;

            float xDifference = difference.x;
            float yDifference = difference.y;
            if (Mathf.Abs(xDifference) < Mathf.Abs(yDifference))
            {
                animator.SetTrigger("MovingBottom");

                if (moveDirection.x > 0)
                {
                    Debug.Log("MovingBottomFlipped");
                    spriteRenderer.flipX = true;
                }
                else
                {
                    Debug.Log("MovingBottomNotFlipped");
                    spriteRenderer.flipX = false;
                }
            }
            else if (Mathf.Abs(xDifference) > Mathf.Abs(yDifference))
            {
                animator.SetTrigger("MovingRight");

                if (moveDirection.y > 0)
                {
                    Debug.Log("MovingRightNotFlipped");
                    spriteRenderer.flipY = true;
                }
                else
                {
                    Debug.Log("MovingRightFlipped");
                    spriteRenderer.flipY = false;
                }
            }*/
        }
    }

    public void MoveEnemy()
    {
        Vector2 force = new Vector2(moveDirection.x, moveDirection.y) * moveSpeed;
        rb.velocity = force + knocked;
    }

    IEnumerator rangeAttack ()
    {
        canFire = false;
        canMove = false;
        Fireball fire = fireball.gameObject.GetComponent<Fireball>();

        fire.fireDmg = 1;
        fire.playerPosition = player.position;
        fire.enemyAttack = true;

        Instantiate(fire, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(1);
        canMove = true;

        yield return new WaitForSeconds(5);
        canFire = true;

    }

    public void NewStage() 
    { 
        
    }

    private void DistanceAttack()
    {
        canFire = false;
        Fireball fire = fireball.gameObject.GetComponent<Fireball>();

        fire.fireDmg = 1;
        fire.playerPosition = player.position;
        fire.enemyAttack = true;

        Instantiate(fire, transform.position, Quaternion.identity);

    }

    public virtual void OnHit(int damage, Vector2 knockback)
    {
        health -= damage;
        AudioSource.PlayClipAtPoint(audioClip, transform.position);
        this.GetKnocked(knockback);
    }

    public void GetKnocked(Vector2 knockback)
    {
        if (knockback != null)
        {
            //rb.AddForce(knockback, ForceMode2D.Impulse);

            knocked = knockback;

            StartCoroutine(ResetKnockback());

        }
    }

    private IEnumerator ResetKnockback()
    {
        yield return new WaitForSeconds(knockbackDuration);
        knocked = Vector2.zero;
        canMove = false;
        yield return new WaitForSeconds(stunnedDuration);
        canMove = true;
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.collider.CompareTag("Player")) {
            Vector2 direction = (other.collider.GetComponent<Collider2D>().transform.position - transform.position).normalized;

            player.GetComponent<MovePlayerWithKeyboard>().OnDamaged(damage, direction*knockbackForce);
        }
    }
}
