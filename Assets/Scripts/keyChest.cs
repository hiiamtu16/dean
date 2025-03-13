using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class keyChest : MonoBehaviour
{
    [SerializeField] private GameObject keyPrefab;
    [SerializeField] private Transform spawnKey;

    private bool isOpened = false;
    private bool isPlayerNearby = false;
    [SerializeField] private float holdTime = 3f;
    private float holdTimer = 0f;

    [SerializeField] private Slider progressBar;

    private Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
        progressBar.gameObject.SetActive(false);
        progressBar.value = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (progressBar != null)
        {
            progressBar.transform.position = transform.position + new Vector3(0, 1.5f, 0);
        }

        if (isPlayerNearby && !isOpened)
        {
            if (Input.GetKey(KeyCode.E))
            {
                holdTimer += Time.deltaTime;
                progressBar.gameObject.SetActive(true);
                progressBar.value = holdTimer / holdTime;


                if (holdTimer >= holdTime)
                {
                    OpenChest();
                }
            }
            else
            {
                holdTimer = 0;
                progressBar.value = 0;
                progressBar.gameObject.SetActive(false);

            }
        }
    }

    private void OpenChest()
    {
        isOpened = true;
        anim.SetBool("isOpened", true);
        SpawnKey();

        progressBar.gameObject.SetActive(false);

    }

    private void SpawnKey()
    {
        GameObject coin = Instantiate(keyPrefab, spawnKey.position, Quaternion.identity);
        Rigidbody2D rb = coin.GetComponent<Rigidbody2D>();

        float randomXForce = Random.Range(-1f, 1f);
        rb.AddForce(new Vector2(randomXForce, 4f), ForceMode2D.Impulse);
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
