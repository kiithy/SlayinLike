using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }  // Singleton pattern

    [Header("Player Stats")]
    public int maxPlayerHealth = 3;  // Set this in inspector

    // events
    public UnityEvent gameStart;
    public UnityEvent gameRestart;
    public UnityEvent<int> scoreChange;
    public UnityEvent gameOver;
    public UnityEvent<int> healthChange;
    private int score;
    private int health;
    public AudioSource backgroundMusic;
    public AudioClip gameOverMusic;
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
        health = maxPlayerHealth;  // Use the configurable value
        SetHealth(health);
        gameStart.Invoke();
        Time.timeScale = 1.0f;
    }

    public void DecreaseHealth(int decrement)
    {
        if (health - decrement <= 0)
        {
            health = 0;
            SetHealth(health);
            GameOver();
            return;
        }
        health -= decrement;
        SetHealth(health);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GameRestart()
    {
        score = 0;
        SetScore(score);
        health = maxPlayerHealth;  // Use the same configurable value
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
        if (gameOverMusic != null)
        {
            backgroundMusic.PlayOneShot(gameOverMusic);
        }

    }

    public void SetHealth(int health)
    {
        healthChange.Invoke(health);
    }
}
