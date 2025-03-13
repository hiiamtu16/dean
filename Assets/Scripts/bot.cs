using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Bot : MonoBehaviour
{
    [SerializeField] private GameObject checkPoint;
    public LayerMask tilemapLayer; 
    [SerializeField] private float rayLength = 0.9f;
    [SerializeField] private float moveSpeed = 2f;

    private bool isFacingRight = true;
    private float direction ; 

    private Animator anim;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        direction = UnityEngine.Random.Range(0, 2) * 2 - 1;

        isFacingRight = direction == 1;
        if (!isFacingRight) flip();
    }

    void Update()
    {
        MoveBot();

        checkObstacles();

        UpdateRayDirection();
    }

    private void MoveBot()
    {
     
        rb.velocity = new Vector2(direction * moveSpeed, rb.velocity.y);
        anim.SetFloat("move", Mathf.Abs(direction));
    }

    private void flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 size = transform.localScale;
        size.x *= -1;
        transform.localScale = size;
    }

    private void checkObstacles()
    {
        Transform checkPointPosition = checkPoint.transform;
        Vector2 checkX = new Vector2(checkPointPosition.position.x, checkPointPosition.position.y);

        Vector2 rayDirection = isFacingRight ? Vector2.right : Vector2.left;
        RaycastHit2D hit = Physics2D.Raycast(checkX, rayDirection, rayLength, tilemapLayer);

        Debug.DrawRay(checkX, rayDirection * rayLength, Color.red);

        if (hit.collider != null && hit.collider.GetComponent<Tilemap>() != null)
        {
            direction *= -1;
            flip();
        }
    }
    private void UpdateRayDirection()
    {
        isFacingRight = direction > 0; // Nếu direction > 0, bot quay phải, ngược lại quay trái
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player va chạm với bot!");
            //trừ máu
        }
    }
}
