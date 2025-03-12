    using System.Collections;
    using UnityEngine;
    using UnityEngine.UI;

    public class Chest : MonoBehaviour
    {
        [SerializeField] private GameObject coinPrefab; 
        [SerializeField] private Transform spawnPoint;

        private bool isOpened = false;
        private bool isPlayerNearby = false;
        [SerializeField] private float holdTime = 3f;
        private float holdTimer = 0f;

        private Animator anim;


    private void Start()
        {

            anim = GetComponent<Animator>();
            
        }

        private void Update()
        {

            if (isPlayerNearby && !isOpened)
            {
                if (Input.GetKey(KeyCode.E))
                {
                    holdTimer += Time.deltaTime;

                    

                if (holdTimer >= holdTime)
                    {
                        OpenChest();
                    }
                }
                else
                {
                    holdTimer = 0;
                    
                }
            }
        }

        private void OpenChest()
        {
            isOpened = true;
            anim.SetBool("isOpened", true); 
            SpawnCoin();

           
        }

        private void SpawnCoin()
        {
            GameObject coin = Instantiate(coinPrefab, spawnPoint.position, Quaternion.identity);
            Rigidbody2D rb = coin.GetComponent<Rigidbody2D>();

            float randomXForce = Random.Range(-2f, 2f);
            rb.AddForce(new Vector2(randomXForce, 6f), ForceMode2D.Impulse);
        }


        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                isPlayerNearby = true;
                Debug.Log("Player đã chạm vào Chest!");
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("Player rời khỏi Chest!");
                isPlayerNearby = false;
                holdTimer = 0;
               
            }
        }
    }
