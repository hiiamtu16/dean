using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float climbSpeed = 3f;

    [SerializeField] private LayerMask tilemapLayer;
    [SerializeField] private LayerMask ladderLayer;
    public Transform raycastOrigin;
    [SerializeField] private float rayLength;

    private bool isFacingRight = true;
    private bool isGrounded;
    private bool isClimbing = false;
    private bool onLadder = false;

    public int coinCount = 0;
    public int keyCount = 0;

    public int health = 3;
    private int maxHealth = 3;
    private HealthUI healthUI;

    private float direction;
    private float verticalInput;
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        healthUI = FindObjectOfType<HealthUI>();
    }

    void Update()
    {
        direction = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        CheckLadder(); 

        if (onLadder && Mathf.Abs(verticalInput) > 0)
        {
            isClimbing = true;
            rb.gravityScale = 0f;
            gameObject.layer = LayerMask.NameToLayer("IgnoreTilemap");
        }
    }

    private void FixedUpdate()
    {
        isGrounded = CheckGround();

        if (isClimbing)
        {
            PlayerClimb();
        }
        else
        {
            PlayerMove();
        }
    }

    private void PlayerMove()
    {
        if (!isGrounded) direction = 0;

        rb.velocity = new Vector2(direction * moveSpeed, rb.velocity.y);
        flip();

        anim.SetFloat("move", Mathf.Abs(direction));
    }

    private void PlayerClimb()
    {
        rb.velocity = new Vector2(0, verticalInput * climbSpeed);

        
        if (!onLadder)
        {
            isClimbing = false;
            rb.gravityScale = 1f;
            gameObject.layer = LayerMask.NameToLayer("Player"); 
        }

        anim.SetFloat("climb", Mathf.Abs(verticalInput));
    }


    public bool CheckGround()
    {
        RaycastHit2D hit = Physics2D.Raycast(raycastOrigin.position, Vector2.down, rayLength, tilemapLayer);
        Debug.DrawRay(raycastOrigin.position, Vector2.down * rayLength, Color.red);

        return hit.collider != null;
    }

    private void CheckLadder()
    {
        RaycastHit2D hitUp = Physics2D.Raycast(raycastOrigin.position, Vector2.up, rayLength, ladderLayer);
        RaycastHit2D hitDown = Physics2D.Raycast(raycastOrigin.position, Vector2.down, rayLength, ladderLayer);

        Debug.DrawRay(raycastOrigin.position, Vector2.up * rayLength, Color.green);
        Debug.DrawRay(raycastOrigin.position, Vector2.down * rayLength, Color.blue);

        onLadder = hitUp.collider != null || hitDown.collider != null;
    }


    void flip()
    {
        if (isFacingRight && direction < 0 || !isFacingRight && direction > 0)
        {
            isFacingRight = !isFacingRight;
            Vector3 size = transform.localScale;
            size.x *= -1;
            transform.localScale = size;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Ladder"))
        {
            onLadder = false;
            isClimbing = false;
            rb.gravityScale = 1f;
            gameObject.layer = LayerMask.NameToLayer("Player");
        }
    }



    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Coin"))
        {
            CollectCoin(other.gameObject);
        }

        else if (other.CompareTag("Key"))
        {
            CollectKey(other.gameObject);
        }

        else if (other.CompareTag("Bot"))
        {
            TakeDamage(1);
        }
    }

    private void CollectCoin(GameObject coin)
    {
        coinCount += 1;
        Destroy(coin);
        Debug.Log("Coins: " + coinCount);
    }

    private void CollectKey(GameObject key)
    {
        keyCount += 1;
        Destroy(key);
        Debug.Log("Key: " + keyCount);
    }

    private void TakeDamage(int damage)
    {
        health -= damage;
        health = Mathf.Clamp(health, 0, maxHealth);

        
        if (healthUI != null)
        {
            healthUI.UpdateHearts(health);
        }

        Debug.Log("Player bị tấn công! Máu còn: " + health);

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player đã chết!");
        //out màn
    }

}
