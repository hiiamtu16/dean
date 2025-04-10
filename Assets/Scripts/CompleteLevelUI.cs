using UnityEngine;
using UnityEngine.UI;

public class CompleteLevelUI : MonoBehaviour
{
    public Image[] stars; 
    public Sprite fullStar; 
    public Sprite emptyStar; 

    public GameObject winPanel; 

    void Start()
    {
        winPanel.SetActive(false); 
    }

    public void ShowWinPanel(int starCount)
    {
        winPanel.SetActive(true); 
        UpdateStars(starCount);
    }

    void UpdateStars(int count)
    {
        count = Mathf.Clamp(count, 0, 3); 

        for (int i = 0; i < stars.Length; i++)
        {
            stars[i].sprite = (i < count) ? fullStar : emptyStar;
        }
    }
}
    