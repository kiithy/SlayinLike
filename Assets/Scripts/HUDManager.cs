using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUDManager : MonoBehaviour
{
    public GameObject gameOverPanel;
    public GameObject scoreText;
    public GameManager gameManager;
    public GameObject healthText;

    // Start is called before the first frame update
    void Start()
    {
        // Subscribe to GameManager events
        if (gameManager != null)
        {
            gameManager.scoreChange.AddListener(SetScore);
            gameManager.gameOver.AddListener(ShowGameOver);
            gameManager.gameStart.AddListener(GameStart);
            gameManager.gameRestart.AddListener(HideGameOver);
            gameManager.healthChange.AddListener(SetHealth);
        }
        else
        {
            Debug.LogWarning("GameManager reference not set in HUDManager!");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GameStart()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

    public void SetScore(int score)
    {
        if (scoreText != null)
            scoreText.GetComponent<TextMeshProUGUI>().text = "Score: " + score.ToString();
    }

    public void ShowGameOver()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
    }

    public void HideGameOver()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

    public void SetHealth(int health)
    {
        if (healthText != null)
            healthText.GetComponent<TextMeshProUGUI>().text = "Health: " + health.ToString();
    }
}

