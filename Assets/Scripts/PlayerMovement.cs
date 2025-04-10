using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    [Header("Di chuyển & leo thang")]
    public float moveSpeed = 5f;
    public float climbSpeed = 3f;
    private float direction;
    private float verticalInput;

    [Header("Kiểm tra mặt đất & thang")]
    [SerializeField] private LayerMask tilemapLayer;
    [SerializeField] private LayerMask ladderLayer;
    [SerializeField] private Transform raycastOrigin;
    [SerializeField] private float rayLength = 0.5f;

    private bool isFacingRight = true;
    private bool isGrounded = false;
    private bool isClimbing = false;
    private bool onLadder = false;

    [Header("Máu & UI")]
    public int health = 3;
    private int maxHealth = 3;
    private HealthUI healthUI;

    [Header("Thu thập vật phẩm")]
    public int coinCount = 0;
    public int keyCount = 0;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer spriteRenderer;

    [Header("Sao")]
    public Stars starUI; 
    public int star = 3;
    public GameObject WinPanel;
    public GameObject LosePanel;


    private Door door;



    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        healthUI = FindObjectOfType<HealthUI>();

        UpdateHealthUI();
        if (WinPanel != null) WinPanel.SetActive(false);
        if (LosePanel != null) LosePanel.SetActive(false);

        door = FindObjectOfType<Door>();
    }

    void Update()
    {
        direction = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        isGrounded = CheckGround();
        CheckLadder();

        if (onLadder && Mathf.Abs(verticalInput) > 0)
        {
            StartClimbing();
        }
        else if (!onLadder)
        {
            StopClimbing();
        }
    }

    private void FixedUpdate()
    {
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
        rb.velocity = new Vector2(direction * moveSpeed, rb.velocity.y);
        flip();
        anim.SetFloat("move", Mathf.Abs(direction));
    }

    private void PlayerClimb()
    {
        rb.velocity = new Vector2(0, verticalInput * climbSpeed);

    }

    private bool CheckGround()
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

    private void StartClimbing()
    {
        isClimbing = true;
        rb.gravityScale = 0f;
        gameObject.layer = LayerMask.NameToLayer("IgnoreTilemap");
    }

    private void StopClimbing()
    {
        isClimbing = false;
        rb.gravityScale = 1f;
        gameObject.layer = LayerMask.NameToLayer("Player");
    }

    private void flip()
    {
        if ((isFacingRight && direction < 0) || (!isFacingRight && direction > 0))
        {
            isFacingRight = !isFacingRight;
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
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
        else if (other.CompareTag("Door"))
        {
            Door door = other.GetComponent<Door>();
            if (door != null && keyCount >= 1)
            {
                door.Open(); 
                WinGame();   
            }
            else
            {
                Debug.Log("Cần chìa khóa để mở cửa!");
            }
        }
    }




    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Ladder"))
        {
            StopClimbing();
        }
    }

    private void CollectCoin(GameObject coin)
    {
        coinCount++;
        Destroy(coin);
        Debug.Log("Coins: " + coinCount);
    }

    private void CollectKey(GameObject key)
    {
        keyCount++;
        Destroy(key);
        Debug.Log("Key: " + keyCount);

        // Mở cửa ngay sau khi nhặt được khóa
        if (door != null)
        {
            door.Open();  // Gọi hàm mở cửa
        }
    }

    public void TakeDamage(int damage)
    {
        health = Mathf.Clamp(health - damage, 0, maxHealth);
        UpdateHealthUI();
        Debug.Log("Player bị tấn công! Máu còn: " + health);

        if (health <= 0)
        {
            Die();
            
        }
    }

    private void UpdateHealthUI()
    {
        if (healthUI != null)
        {
            healthUI.UpdateHearts(health);
        }
    }

    private void Die()
    {
        Debug.Log("Người chơi đã chết!");

        if (LosePanel != null)
        {
            LosePanel.SetActive(true);
        }
        
        Time.timeScale = 0f;
    }


    public void WinGame()
    {
        Debug.Log("Người chơi đã chiến thắng!");

        if (WinPanel != null)
        {
            WinPanel.SetActive(true);
        }
        
        int starEarned = health; 
        if (starUI != null)
        {
            starUI.UpdateStars(starEarned);
        }
       
        Time.timeScale = 0f;
    }


}