using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Chest : MonoBehaviour
{
    [SerializeField] private float openTime = 3f; // Thời gian giữ phím E để mở hòm
    [SerializeField] private GameObject starPrefab; // Sao rơi ra khi mở hòm
    [SerializeField] private Transform spawnPoint; // Vị trí spawn sao
    [SerializeField] private GameObject pressEText; // UI hướng dẫn nhấn E
    [SerializeField] private Image progressBar; // Thanh tiến trình mở hòm

    private bool isPlayerNearby = false;
    private bool isOpening = false;
    private float holdTime = 0f;

    private void Start()
    {
        pressEText.SetActive(false); // Ẩn UI ban đầu
        progressBar.fillAmount = 0; // Thanh tiến trình trống
    }

    private void Update()
    {
        if (isPlayerNearby)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (!isOpening)
                {
                    isOpening = true;
                    StartCoroutine(OpenChest());
                }
            }
            else
            {
                if (isOpening)
                {
                    StopOpening();
                }
            }
        }
    }

    private IEnumerator OpenChest()
    {
        holdTime = 0f;

        while (holdTime < openTime)
        {
            if (!Input.GetKey(KeyCode.E))
            {
                StopOpening();
                yield break;
            }

            holdTime += Time.deltaTime;
            progressBar.fillAmount = holdTime / openTime; // Cập nhật thanh tiến trình
            yield return null;
        }

        Open();
    }

    private void StopOpening()
    {
        isOpening = false;
        holdTime = 0;
        progressBar.fillAmount = 0;
    }

    private void Open()
    {
        isOpening = false;
        Debug.Log("Hòm đã mở!");

        // Spawn sao
        Instantiate(starPrefab, spawnPoint.position, Quaternion.identity);

        // Vô hiệu hóa hòm
        GetComponent<Collider2D>().enabled = false;
        pressEText.SetActive(false);
        gameObject.SetActive(false); // Ẩn hòm sau khi mở
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerNearby = true;
            pressEText.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerNearby = false;
            pressEText.SetActive(false);
            StopOpening();
        }
    }
}
