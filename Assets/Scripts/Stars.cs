using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stars : MonoBehaviour
{
    public Image[] stars;
    public Sprite fullStar;
    public Sprite emptyStar;

    public void UpdateStars(int health)
    {
        for (int i = 0; i < stars.Length; i++)
        {
            if (stars[i] != null)
            {
                stars[i].sprite = (i < health) ? fullStar : emptyStar; 
            }
        }
    }

}
