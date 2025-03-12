using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    private bool isFacingRight = true;

    public int coinCount = 0;

    private float direction;
    private Rigidbody2D rb;
    private Animator anim;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        PlayerMove();
    }

    private void PlayerMove()
    {
        direction = Input.GetAxis("Horizontal");
        GetComponent<Rigidbody>().velocity = new Vector2(direction * moveSpeed, GetComponent<Rigidbody>().velocity.y);

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


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Coin"))
        {
            CollectCoin(other.gameObject);
        }
    }

    private void CollectCoin(GameObject coin)
    {
        coinCount += 1; 
        Destroy(coin); 
        Debug.Log("Coins: " + coinCount);
    }


}
