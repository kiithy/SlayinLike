using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public static HUDManager instance;
    public GameObject gameOverPanel;
    public GameObject scoreText;
    public GameObject healthText;
    private Canvas canvas;

    void Awake()
    {
        // Get the parent Canvas
        canvas = GetComponent<Canvas>();

        if (instance == null)
        {
            instance = this;
            // Keep the entire UI hierarchy
            DontDestroyOnLoad(canvas.gameObject);
            if (gameOverPanel != null)
            {
                gameOverPanel.SetActive(false);
            }
            ConnectToGameManager();
        }
        else
        {
            // If another HUDManager exists, destroy this entire Canvas
            Destroy(canvas.gameObject);
        }
    }

    void ConnectToGameManager()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.scoreChange.AddListener(SetScore);
            GameManager.instance.healthChange.AddListener(SetHealth);
            GameManager.instance.gameOver.AddListener(ShowGameOver);
            GameManager.instance.gameStart.AddListener(GameStart);
            GameManager.instance.gameRestart.AddListener(HideGameOver);
        }
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

