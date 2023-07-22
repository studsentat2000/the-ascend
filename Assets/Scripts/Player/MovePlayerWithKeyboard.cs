using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Door;

public class MovePlayerWithKeyboard : MonoBehaviour, IShopCustomer
{

    public GameController controller;
    public int health;
    public int numOfHearts;
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;
    public SwordAttack swordAttack;
    public float collisionOffset = 0.05f;
    public ContactFilter2D movementFilter;
    List<RaycastHit2D> castCollisions = new();
    public float moveSpeed = 700f;
    PlayerController input;
    Vector2 MoveDirection;
    Rigidbody2D rb;
    public bool canMove = true;
    public GameObject fireball;
    SpriteRenderer spriteRenderer;
    Animator animator;
    bool idleLeft;
    bool isMovingLeft;
    bool isMovingRight;
    bool idleRight;
    bool isMovingUp;
    bool idleUp;
    bool isMovingDown;
    bool idleDown;
    public bool canFire = true;
    public float idleFriction = 0.9f;
    public float maxSpeed = 2.2f;
    public GameObject bomb;
    float fireTimeout = (float)0.6;

    [SerializeField]
    public AudioClip playerClip;

    [SerializeField]
    public AudioClip hitClip;

    public int bossIndex = 1;

    private int bombDamage = 3;
    private int fireDamage = 1;
    private float fireSpeed = 5f;

    public int bombStorage = 3;

    public int currentBombs = 0;

    int gold = 0;
    public TextMeshProUGUI goldText;

    public TextMeshProUGUI bombText;

    [SerializeField]
    public Sword sword;


    bool canBeAttacked = true;

    public bool loadingScene = false;

    private void Awake()
    {
        input = new PlayerController();
        DontDestroyOnLoad(this.gameObject);
    }

    private void OnEnable()
    {
        input.Enable();
        input.PlayerControls.Joystick.performed += MovePlayer;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        Debug.Log("Scene loaded");
        if (this != null)
            transform.position = Vector3.zero;
    }

    private void OnDisable()
    {
        /*input.Disable();
        input.PlayerControls.Joystick.performed -= MovePlayer;
        MoveDirection = Vector3.zero;*/
    }

    void MovePlayer(InputAction.CallbackContext ctx)
    {
        MoveDirection = ctx.ReadValue<Vector2>();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    void Update() {

        if (health > numOfHearts) {
            health = numOfHearts;
        }

        for(int i = 0; i < hearts.Length; i++) {
            if (i < health) {
                hearts[i].sprite = fullHeart;
            } else {
                hearts[i].sprite = emptyHeart;
            }

            if (i < numOfHearts) {
                hearts[i].enabled = true;
            } else {
                hearts[i].enabled = false;
            }
        }

        goldText.text = gold.ToString();
        bombText.text = currentBombs.ToString();
    }
    
    private void OnDestroy() {
        for(int i = 0; i < hearts.Length; i++) {
            if (hearts[i] != null && hearts[i].enabled) {
                hearts[i].enabled = false;
            }
        }
    }

    

    void FixedUpdate()
    {
        if(MoveDirection != Vector2.zero && canMove) {
            idleDown = false;
            idleUp = false;
            idleRight = false;
            idleLeft = false;
            //animations
            int count = rb.Cast(
                MoveDirection,
                movementFilter,
                castCollisions,
                moveSpeed*Time.fixedDeltaTime+collisionOffset
            );


            if (MoveDirection.x != 0) {
                    animator.SetBool("isMovingHor", true);

                    if (MoveDirection.x < 0)
                    {
                        spriteRenderer.flipX = true;
                        isMovingLeft = true;
                        isMovingRight = false;
                    } else {
                        spriteRenderer.flipX = false;
                        isMovingRight = true;
                        isMovingLeft = false;
                    }
                } else {
                    animator.SetBool("isMovingHor", false);
                    isMovingRight = false;
                    isMovingLeft = false;
                }
            if (MoveDirection.y != 0) {
                if (MoveDirection.y < 0) {
                    animator.SetBool("isMovingDown", true);
                    animator.SetBool("isMovingUp", false);
                    isMovingDown = true;
                    isMovingUp = false;
                } else {
                    animator.SetBool("isMovingUp", true);
                    animator.SetBool("isMovingDown", false);
                    isMovingUp = true;
                    isMovingDown = false;
                }
            } else {
                animator.SetBool("isMovingDown", false);
                isMovingDown = false;
                animator.SetBool("isMovingUp", false);
                isMovingUp = false;
            }

            if (canMove && MoveDirection != Vector2.zero)
            {
                //rb.MovePosition(rb.position + MoveDirection * Time.fixedDeltaTime * moveSpeed);

                rb.AddForce(MoveDirection * moveSpeed * Time.fixedDeltaTime, ForceMode2D.Force);
            } else {
                rb.velocity = Vector2.Lerp(rb.velocity, Vector2.zero, idleFriction);
                isMovingDown = false;
                isMovingUp = false;
                isMovingRight = false;
                isMovingLeft = false;
            }
        } else {
            animator.SetBool("isMovingDown", false);
            animator.SetBool("isMovingUp", false);
            animator.SetBool("isMovingHor", false);
            if (isMovingDown) idleDown = true;
            isMovingDown = false;
            if (isMovingUp) idleUp = true;
            isMovingUp = false;
            if (isMovingRight) idleRight = true;
            isMovingRight = false;
            if (isMovingLeft) idleLeft = true;
            isMovingLeft = false;
        }
    }

    void OnHit()
    {
        animator.SetTrigger("swordAttack");
        //controller.ClearedStage();
        //AudioSource.PlayClipAtPoint(hitClip, transform.position);
        
    }

    void OnFireball()
    { 
        if (bossIndex >= 1)
            StartCoroutine(AttackFireball());
    }

    void OnBomb()
    {
        if (bossIndex >= 2)
            AttackBomb();
    }

    IEnumerator EnterRoom(Mover mover) {
        Vector2 targetPosition = mover.gameObject.transform.position;
        Debug.Log("Coroutine");
        while(rb.position != targetPosition) {
            rb.position = Vector3.MoveTowards(rb.position, targetPosition, moveSpeed*Time.fixedDeltaTime);
        }

        yield return new WaitForSeconds(0.5f);
    }

    public void enteredRoom(Mover mover) {
        Debug.Log(mover);
        StartCoroutine(EnterRoom(mover));
    }

    void AttackWithSwordHor() {
        LockMovement();
        if (spriteRenderer.flipX == true) {
            swordAttack.AttackLeft();
        } else {
            swordAttack.AttackRight();
        }
    }

    void AttackUp() {
        Debug.Log("Atack up player");
        LockMovement();
        swordAttack.AttackUp();
    }

    void AttackDown() {
        LockMovement();
        swordAttack.AttackDown();
    }

    void LockMovement() {
        canMove = false;
    }

    void UnlockMovement() {
        canMove = true;
        swordAttack.stopAttack();
    }

    IEnumerator AttackFireball()
    {
        if (canFire) { 
            Fireball firegame = fireball.GetComponent<Fireball>();
            firegame.enemyAttack = false;
            firegame.fireDmg = fireDamage;
            firegame.moveSpeed = fireSpeed;
            if (isMovingUp || idleUp)
            {
                firegame.direction = Fireball.Direction.top;
                Instantiate(firegame, transform.position, Quaternion.identity);
                Debug.Log(transform.position);
            }
            else if (isMovingDown || idleDown)
            {
                firegame.direction = Fireball.Direction.bottom;
                Instantiate(firegame, transform.position, Quaternion.identity);
            }
            else if (isMovingLeft || idleLeft)
            {
                firegame.direction = Fireball.Direction.left;
                Instantiate(firegame, transform.position, Quaternion.identity);
            }
            else if (isMovingRight || idleRight)
            {
                firegame.direction = Fireball.Direction.right;
                Instantiate(firegame, transform.position, Quaternion.identity);
            }
            canFire = false;
            yield return new WaitForSeconds(fireTimeout);
            canFire = true;
        }
    }

    void AttackBomb()
    {
        if (currentBombs > 0) { 
            Debug.Log(transform.position);
            Bomb omb = bomb.GetComponent<Bomb>();
            omb.Damage = bombDamage;
            if (isMovingUp || idleUp)
            {
                omb.direction = Bomb.Direction.TOP;
                Instantiate(omb, transform.position, Quaternion.identity);
                Debug.Log(transform.position);
            }
            else if (isMovingDown || idleDown)
            {
                omb.direction = Bomb.Direction.BOTTOM;
                Instantiate(omb, transform.position, Quaternion.identity);
            }
            else if (isMovingLeft || idleLeft)
            {
                omb.direction = Bomb.Direction.LEFT;
                Instantiate(omb, transform.position, Quaternion.identity);
            }
            else if (isMovingRight || idleRight)
            {
                omb.direction = Bomb.Direction.RIGHT;
                Instantiate(omb, transform.position, Quaternion.identity);
            }
            currentBombs -= 1;
        }
        
    }

    public void OnDamaged(int damage, Vector2 direction) {
        if (canBeAttacked)
        {
            StartCoroutine(damaged());
            AudioSource.PlayClipAtPoint(playerClip, transform.position);
            canMove = false;
            health -= damage;
            if (health <= 0)
            {
                Destroy(this.gameObject);
                /*Destroy(GameObject.FindGameObjectWithTag("Canvas"));
                Destroy(GameObject.FindGameObjectWithTag("Canvas2"));
                Destroy(GameObject.FindGameObjectWithTag("GameController"));
                SceneManager.LoadSceneAsync("Main");*/
                controller.restart();
            }
            rb.AddForce(direction, ForceMode2D.Impulse);
            canMove = true;
        }
    }

    IEnumerator damaged() {
        canBeAttacked = false;
        yield return new WaitForSeconds(0.5f);

        canBeAttacked = true;
        GetComponent<Collider>().enabled = false;

        yield return new WaitForSeconds(1);
        GetComponent<Collider>().enabled = true;

        yield return null;
    }

    public IEnumerator teleported() {
        canMove = false;

        yield return new WaitForSeconds(1);

        rb.velocity = Vector2.zero;  
        rb.position = Vector3.zero;


        yield return new WaitForSeconds(1);

        canMove = true;
    
    }

    public void ClearedRoom() {
        if (currentBombs < bombStorage && bossIndex >= 2)
        {
            currentBombs += 1;
        }
        if (health < hearts.Length)
        {
            health += 1;
        }
    }

    public void ClearedStage()
    {
        this.AddGold(50);
        this.bossIndex += 1;
        if (bossIndex >= 2) {
            currentBombs = bombStorage;
        }
        controller.ClearedStage();
    }

    public void BoughtItem(string itemType, int amount)
    {
        gold -= amount;
        switch (itemType)
        {
            case "sword":
                sword.increaseDamage();
                break;
            case "fire":
                fireDamage += 1;
                break;
            case "bomb":
                bombDamage += 1;
                break;
            case "fireTimeout":
                if (fireTimeout > 0)
                {
                    fireTimeout -= 0.1f;
                }
                break;
            case "fireSpeed":
                fireSpeed += 1;
                break;
        }
        controller.upgraded(itemType);
    }

    public bool CanSpend(int amount)
    {
        if (amount <= gold)
        {
            return true;
        }
        else {
            return false;
        }
    }

    public void SceneLoaded() {
        canMove = true;
    }

    public void SceneLoading() {
        canMove = false;
    }

    public void AddGold(int amount)
    {
        gold += amount;
    }
}
