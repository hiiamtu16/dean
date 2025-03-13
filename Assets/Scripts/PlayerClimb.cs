using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerClimb : MonoBehaviour
{
    [SerializeField] private float climbSpeed = 3f;
    

    private bool isClimbing = false;
    private Rigidbody2D rb;
    

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isClimbing = true;
            rb.gravityScale = 0f;
            rb.velocity = Vector2.zero;

            gameObject.layer = LayerMask.NameToLayer("IgnoreTilemap");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isClimbing = false;
            rb.gravityScale = 1f;
            gameObject.layer = LayerMask.NameToLayer("Player");
        }
    }

    private void Update()
    {
        

        float climbInput = Input.GetAxisRaw("Vertical");
        
    }
}
