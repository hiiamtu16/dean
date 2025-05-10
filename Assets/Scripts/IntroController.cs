using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class IntroController : MonoBehaviour
{
    public GameObject playButton;
    public float delayBeforeShow = 3f;
    public float pulseSpeed = 1.5f;
    public float scaleAmount = 1.1f;

    private bool isPulsing = false;
    private Vector3 originalScale;

    void Start()
    {
        originalScale = playButton.transform.localScale;
        playButton.SetActive(false);
        StartCoroutine(ShowPlayButtonWithDelay());
    }

    IEnumerator ShowPlayButtonWithDelay()
    {
        yield return new WaitForSeconds(delayBeforeShow);
        playButton.SetActive(true);
        isPulsing = true;
    }

    void Update()
    {
        if (isPulsing)
        {
            float scale = Mathf.Lerp(1f, scaleAmount, (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f);
            playButton.transform.localScale = originalScale * scale;
        }
    }

    
}
