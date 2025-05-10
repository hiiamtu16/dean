using UnityEngine;

public class GameController : MonoBehaviour
{
    private static GameController instance;
    public static GameController Instance => instance;

    [Header("Máu & UI")]
    public int health = 3;
    private int maxHealth = 3;
    public HealthUI healthUI;
    public GameObject LosePanel;

    [Header("Sao & chiến thắng")]
    public Stars starUI;  
    public GameObject WinPanel;

    [Header("Vật phẩm")]
    public int coinCount = 0;
    public int keyCount = 0;

    private Door door;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            return;
        }
        if(instance.gameObject.GetInstanceID() != this.gameObject.GetInstanceID())
        {
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        Time.timeScale = 1f;
        door = FindObjectOfType<Door>();

        if (LosePanel != null) LosePanel.SetActive(false);
        if (WinPanel != null) WinPanel.SetActive(false);

        UpdateHealthUI();  
        UpdateStarUI();    
    }

    public void HandleTriggerEnter(Collider2D other)
    {
        if (other.CompareTag("Coin"))
        {
            coinCount++;
            Destroy(other.gameObject);
            Debug.Log("Coins: " + coinCount);
        }
        else if (other.CompareTag("Key"))
        {
            keyCount++;

            Destroy(other.gameObject);
            Debug.Log("Keys: " + keyCount);

            if (door != null)
                door.Open();
        }
        else if (other.CompareTag("Bot"))
        {
            TakeDamage(1);  
        }
        else if (other.CompareTag("Door"))
        {
            if (keyCount >= 1)
            {
                WinGame();

            }
            else
            {
                Debug.Log("Cần chìa khóa để mở cửa!");
            }
            
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            health = 0;
            Die();
        }

       
        UpdateHealthUI();  
        UpdateStarUI();    
    }

    public void UpdateHealthUI()
    {
        if (healthUI != null)
        {
            healthUI.UpdateHearts(health);
        }
    }

    public void UpdateStarUI()
    {
        if (starUI != null)
        {
            starUI.UpdateStars(health);  
        }
    }

    void Die()
    {
        Debug.Log("Người chơi đã chết!");
        if (LosePanel != null)
            LosePanel.SetActive(true);

        Time.timeScale = 0f;
    }

    void WinGame()
    {
        Debug.Log("Người chơi đã thắng!");

        if (WinPanel != null)
            WinPanel.SetActive(true);

        int starEarned = health;
        if (starUI != null)
            starUI.UpdateStars(starEarned);  

        Time.timeScale = 0f;
    }

    public void AddCoin()
    {
        coinCount++;
        Debug.Log("Coin Collected! Total Coins: " + coinCount);
    }

    public void AddKey()
    {
        keyCount++;
        Debug.Log("Key: " + keyCount);
        Door.Instance.Open();

    }
}
