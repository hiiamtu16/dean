using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class bot : MonoBehaviour
{
    public GameObject checkPoint;
    public LayerMask layer;
    public float rayLength = 0.9f;

    public float moveSpeed = 2f;
    private bool isMovingRight = true;

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigidbody;
    private BoxCollider2D boxCollider;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        isMovingRight = Random.Range(0, 2) == 0;

     
        MoveBot();
    }

    // Update is called once per frame
    void Update()
    {
        MoveBot();

        checkObstacles();
        
    }

    private void checkObstacles()
    {
        //raycart kiểm tra vị trí và quỹ đạo của bot
        
        
            //lấy vị trí bắn
            Transform checkPointPosition = checkPoint.transform;
        Vector2 checkX = new Vector2(checkPointPosition.position.x, checkPointPosition.position.y);
            //hướng tia ray phụ thuộc hướng di chuyển
            Vector2 direction = isMovingRight ? Vector2.right : Vector2.left;
             //bắn tia ray
            RaycastHit2D hit = Physics2D.Raycast(checkX, direction, rayLength, layer);


        //hiển thị ray trên screen
        Debug.DrawRay(checkX, direction * rayLength, Color.red);


        if (hit.collider != null )
            {
                isMovingRight = !isMovingRight;
                Flip();
            }
        
    }

    private void MoveBot()
    {
        if (isMovingRight)
        {
            rigidbody.velocity = new Vector2(moveSpeed, rigidbody.velocity.y);
        }
        else
        {
            rigidbody.velocity = new Vector2(-moveSpeed, rigidbody.velocity.y);
        }
    }

  

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            isMovingRight = !isMovingRight;

            Flip();
        }
    }

    private void Flip()
    {
        //lật sprite
        spriteRenderer.flipX = !spriteRenderer.flipX;

        //lật offset
        Vector2 currentoffset = boxCollider.offset;
        currentoffset.x = -currentoffset.x;
        boxCollider.offset = currentoffset;

        //lật hướng bắt tia ray
        Vector3 checkPointLocalPosition = checkPoint.transform.localPosition;
        checkPointLocalPosition.x = -checkPointLocalPosition.x;
        checkPoint.transform.localPosition = checkPointLocalPosition;

    }
}
