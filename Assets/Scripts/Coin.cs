using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            GameController gameController = FindObjectOfType<GameController>();
            if (gameController != null)
            {
                gameController.AddCoin(); // Gọi hàm cộng coin
                Destroy(gameObject);
            }
        }
    }
}
