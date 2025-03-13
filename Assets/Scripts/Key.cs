using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player")) 
        {
            PlayerMovement player = collision.collider.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.keyCount += 1; 
                Debug.Log("Coin Collected! Total Coins: " + player.coinCount);
                Destroy(gameObject); 
            }
        }
    }
}
