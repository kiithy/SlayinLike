using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }  // Singleton pattern

    // events
    public UnityEvent gameStart;
    public UnityEvent gameRestart;
    public UnityEvent<int> scoreChange;
    public UnityEvent gameOver;
    public UnityEvent<int> healthChange;
    private int score;
    private int health;
    public AudioSource backgroundMusic;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        health = 5;  // Set initial health directly
        SetHealth(health);
        gameStart.Invoke();
        Time.timeScale = 1.0f;
    }

    public void DecreaseHealth(int decrement)
    {
        health -= decrement;
        SetHealth(health);
        if (health <= 0)
        {
            GameOver();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GameRestart()
    {
        // reset score and health
        score = 0;
        SetScore(score);
        health = 5;  // Reset health
        SetHealth(health);
        gameRestart.Invoke();
        Time.timeScale = 1.0f;
    }

    public void IncreaseScore(int increment)
    {
        score += increment;
        SetScore(score);
    }

    public void SetScore(int score)
    {
        scoreChange.Invoke(score);
    }

    public void GameOver()
    {
        Time.timeScale = 0.0f;
        gameOver.Invoke();
        backgroundMusic.Stop();
    }

    public void SetHealth(int health)
    {
        healthChange.Invoke(health);
    }
}
