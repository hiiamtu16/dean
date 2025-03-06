using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    private bool isFacingRight = true;

    private float direction;
    private Rigidbody2D rigidbody;
    private Animator anim;


    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        PlayerMove();
    }

    private void PlayerMove()
    {
        direction = Input.GetAxis("Horizontal");
        rigidbody.velocity = new Vector2(direction * moveSpeed, rigidbody.velocity.y);

        flip();

        anim.SetFloat("move", Mathf.Abs(direction));
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

    
}
