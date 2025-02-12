using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody =  this.gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMove();
    }

    private void PlayerMove()
    {
        if (rigidbody != null)
        {
            // Lấy giá trị di chuyển từ bàn phím (phím A và D hoặc mũi tên trái/phải)
            float moveInput = Input.GetAxis("Horizontal"); // "Horizontal" mặc định là phím A/D và mũi tên trái/phải

            // Tính toán lực di chuyển
            Vector3 moveDirection = new Vector2(moveInput, 0f);  // Di chuyển theo trục X

            // Áp dụng lực cho Rigidbody
            rigidbody.velocity = new Vector2(moveDirection.x * moveSpeed, rigidbody.velocity.y);
        }
    }
}
