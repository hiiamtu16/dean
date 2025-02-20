using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float climbSpeed = 4.0f;

    private Rigidbody2D rigidbody;
    

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        PlayerMove();
    }

    private void PlayerMove()
    {
        // Di chuyển nhân vật ngang
        float moveInput = Input.GetAxis("Horizontal");
        rigidbody.velocity = new Vector2(moveInput * moveSpeed, rigidbody.velocity.y);
    }

    
    private void OnTriggerEnter2D(Collider2D other)
    {

    }

    
}
