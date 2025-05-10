using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Di chuyển & leo thang")]
    public float moveSpeed = 5f;
    public float climbSpeed = 3f;
    private float direction;
    private float verticalInput;

    [Header("Raycast")]
    [SerializeField] private LayerMask tilemapLayer;
    [SerializeField] private LayerMask ladderLayer;
    [SerializeField] private Transform raycastOrigin;
    [SerializeField] private float rayLength = 0.5f;

    private bool isFacingRight = true;
    private bool isClimbing = false;
    private bool onLadder = false;

    private Rigidbody2D rb;
    private Animator anim;
    private GameController gameController;  

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        gameController = FindObjectOfType<GameController>();
    }
    void Update()
    {
        direction = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        CheckLadder();

        if (onLadder && Mathf.Abs(verticalInput) > 0)
            StartClimbing();
        else if (!onLadder)
            StopClimbing();
    }

    void FixedUpdate()
    {
        if (isClimbing)
            rb.velocity = new Vector2(0, verticalInput * climbSpeed);
        else
            rb.velocity = new Vector2(direction * moveSpeed, rb.velocity.y);

        if (!isClimbing)
        {
            Flip();
            anim.SetFloat("move", Mathf.Abs(direction));
        }
    }

    void CheckLadder()
    {
        RaycastHit2D hitUp = Physics2D.Raycast(raycastOrigin.position, Vector2.up, rayLength, ladderLayer);
        RaycastHit2D hitDown = Physics2D.Raycast(raycastOrigin.position, Vector2.down, rayLength, ladderLayer);
        onLadder = hitUp.collider != null || hitDown.collider != null;
    }

    void StartClimbing()
    {
        isClimbing = true;
        rb.gravityScale = 0f;
        gameObject.layer = LayerMask.NameToLayer("IgnoreTilemap");
    }

    void StopClimbing()
    {
        isClimbing = false;
        rb.gravityScale = 1f;
        gameObject.layer = LayerMask.NameToLayer("Player");
    }

    void Flip()
    {
        if ((isFacingRight && direction < 0) || (!isFacingRight && direction > 0))
        {
            isFacingRight = !isFacingRight;
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        gameController.HandleTriggerEnter(other);  
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Ladder"))
            StopClimbing();
    }

    
    public void TakeDamage(int damage)
    {
        gameController.TakeDamage(damage);  
    }

}
