using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    [SerializeField] private LayerMask tilemapLayer;
    public Transform raycastOrigin;
    public float rayLength ;


    private bool isFacingRight = true;
    private bool isGrounded;

    public int coinCount = 0;
    public int keyCount = 0;

    public int health = 3;
    private int maxHealth = 3;
    private HealthUI healthUI;

    private float direction;
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
    }

    private void FixedUpdate()
    {
        isGrounded = CheckGround();
        PlayerMove();
    }

    private void PlayerMove()
    {
        direction = Input.GetAxis("Horizontal");

        if (!isGrounded)
            direction = 0; // Ngăn di chuyển nếu không đứng trên nền hợp lệ

        rb.velocity = new Vector2(direction * moveSpeed, rb.velocity.y);
        flip();

        anim.SetFloat("move", Mathf.Abs(direction));
    }

    public bool CheckGround()
    {
        RaycastHit2D hit = Physics2D.Raycast(raycastOrigin.position, Vector2.down, rayLength, tilemapLayer);
        Debug.DrawRay(raycastOrigin.position, Vector2.down * rayLength, Color.red);

        if (hit.collider != null)
        {
            string hitLayerName = LayerMask.LayerToName(hit.collider.gameObject.layer);
            return hitLayerName == "Tilemap" || hitLayerName == "IgnoreTilemap";
        }
        return false;
    }



    void flip()
    {
        if (isFacingRight && direction < 0 || !isFacingRight && direction > 0)
        {
            isFacingRight = !isFacingRight;
            Vector3 size = transform.localScale;
            size.x = size.x * -1;
            transform.localScale = size;
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

        // Cập nhật UI trái tim
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
