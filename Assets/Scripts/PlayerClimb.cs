using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerClimb : MonoBehaviour
{
    [SerializeField] float climbSpeed = 3f;
    private bool isClimbing = false;
    private Rigidbody2D rigidbody;
    private Collider2D playerCollider;
    [SerializeField] private LayerMask groundLayer;
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isClimbing = true;
            rigidbody.gravityScale = 0f;
            rigidbody.velocity = Vector2.zero;

            gameObject.layer = LayerMask.NameToLayer("IgnoreTilemap");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isClimbing = false ;
            rigidbody.gravityScale = 1f;

            gameObject.layer = LayerMask.NameToLayer("Player");

            PushOutOfWall();

        }
    }

    private void FixedUpdate()
    {
        if (isClimbing) 
        {
            float climbInput = Input.GetAxisRaw("Vertical");
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, climbInput * climbSpeed);

            
        }
    }

    private void PushOutOfWall()
    {

        // Kiểm tra xem nhân vật có bị kẹt trong Tilemap không
        Collider2D hit = Physics2D.OverlapBox(transform.position, playerCollider.bounds.size, 0, groundLayer);

        if (hit != null)
        {
            Debug.Log("Bị kẹt trong Tilemap! Đẩy nhân vật ra ngoài.");

            Vector2[] directions = { Vector2.right, Vector2.left, Vector2.up, Vector2.down }; // Kiểm tra 4 hướng
            float pushDistance = 0.3f; // Khoảng cách đẩy ra ngoài

            foreach (Vector2 dir in directions)
            {
                RaycastHit2D checkHit = Physics2D.Raycast(transform.position, dir, pushDistance, groundLayer);

                if (checkHit.collider == null) // Nếu hướng này không bị chặn
                {
                    transform.position += (Vector3)(dir * pushDistance);
                    Debug.Log("Đẩy nhân vật theo hướng: " + dir);
                    return; // Dừng kiểm tra sau khi đẩy
                }
            }
        }

    }
}
