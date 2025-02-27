using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    private static HUDManager instance;
    public GameObject gameOverPanel;
    public GameObject scoreText;
    public GameObject healthText;
    public GameObject gameWinPanel;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            if (gameOverPanel != null)
            {
                gameOverPanel.SetActive(false);
            }

            if (gameWinPanel != null)
            {
                gameWinPanel.SetActive(false);
            }
            ConnectToGameManager();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void ConnectToGameManager()
    {
        GameManager.instance.scoreChange.AddListener(SetScore);
        GameManager.instance.healthChange.AddListener(SetHealth);
        GameManager.instance.gameOver.AddListener(ShowGameOver);
        GameManager.instance.gameStart.AddListener(GameStart);
        GameManager.instance.gameRestart.AddListener(HideGameOver);
    }

    // Start is called before the first frame update
    void Start()
    {

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

    public void GameWin()
    { 
        if (gameWinPanel != null)
            gameWinPanel.SetActive(true);
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

    public void OnRestartButtonClick()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.GameRestart();
        }
    }
}

