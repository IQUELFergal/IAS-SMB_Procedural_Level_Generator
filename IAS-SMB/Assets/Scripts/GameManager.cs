using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] TMP_Text timerText;
    public float timeLeft = 60.0f;
    private bool isGameOver = false;
    public Canvas gameOverUI;

    [SerializeField] TMP_Text scoreText;
    [Min(0)]
    public int score = 0;

    [SerializeField] TMP_Text coinCounterText;
    [Range(0,99)]
    public int coinCounter = 0;

    [SerializeField] TMP_Text lifeCounterText;
    [Range(0, 99)]
    public int lifeCounter = 0;

    void Start()
    {
        Time.timeScale = 1f;
        //gameOverUI.gameObject.SetActive(false);
        UpdateUI();
        StartCoroutine(Timer(timeLeft));
    }

    void Update()
    {
       /* if ((timerText != null) && (timeLeft >= Time.deltaTime))
        {
            timeLeft -= Time.deltaTime;
            System.TimeSpan result = System.TimeSpan.FromSeconds(timeLeft);
            System.DateTime actualResult = System.DateTime.MinValue.Add(result);
            timerText.text = actualResult.ToString("mm:ss");
        }
        else
        {
            if (!isGameOver)
            {
                GameOver();
                isGameOver = true;
            }
        }*/
    }

    IEnumerator Timer(float duration)
    {
        float t = duration;
        System.TimeSpan result = System.TimeSpan.FromSeconds(t);
        System.DateTime actualResult = System.DateTime.MinValue.Add(result);
        timerText.text = actualResult.ToString("mm:ss");
        while (t > 0)
        {
            yield return new WaitForFixedUpdate();
            t -= Time.deltaTime;
            result = System.TimeSpan.FromSeconds(t);
            actualResult = System.DateTime.MinValue.Add(result);
            timerText.text = actualResult.ToString("mm:ss");
        }
        GameOver();
    }

    void GameOver() //Script executé à la fin du timer
    {
        Debug.Log("Game Over");
        Time.timeScale = 0f;
        gameOverUI?.gameObject.SetActive(true);
    }

    public void AddCoin(int n)
    {
        coinCounter += n;
        coinCounterText.text = coinCounter.ToString();
    }

    public void AddLife(int n)
    {
        lifeCounter += n;
        lifeCounterText.text = "X " + lifeCounter.ToString();
    }

    void UpdateUI()
    {
        scoreText.text = score.ToString();
        coinCounterText.text = coinCounter.ToString();
        lifeCounterText.text = lifeCounter.ToString();
    }
}