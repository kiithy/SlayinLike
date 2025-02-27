using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    [Header("Player Stats")]
    public int maxPlayerHealth = 3;

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
    private HUDManager hudManager;  // Changed from Canvas to HUDManager
    private int totalOrcs;
    private int orcsDefeated;
    public UnityEvent gameWin;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            // Find and keep the HUDManager
            hudManager = FindObjectOfType<HUDManager>();
            if (hudManager != null)
            {
                DontDestroyOnLoad(hudManager.gameObject);  // This will keep the Canvas since it's the same object
            }
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "SecondScene")
        {
            // Count total orcs in the scene
            totalOrcs = FindObjectsOfType<Orc>().Length;
            orcsDefeated = 0;
        }

        if (hudManager != null)
        {
            scoreChange.RemoveAllListeners();
            healthChange.RemoveAllListeners();
            gameOver.RemoveAllListeners();
            gameStart.RemoveAllListeners();
            gameRestart.RemoveAllListeners();
            gameWin.RemoveAllListeners();

            scoreChange.AddListener(hudManager.SetScore);
            healthChange.AddListener(hudManager.SetHealth);
            gameOver.AddListener(hudManager.ShowGameOver);
            gameStart.AddListener(hudManager.GameStart);
            gameRestart.AddListener(hudManager.HideGameOver);
            gameWin.AddListener(hudManager.GameWin);
            SetScore(score);
            SetHealth(health);
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
        health = maxPlayerHealth;
        SetHealth(health);
        gameRestart.Invoke();
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("MainScene");
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
        AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource audioSource in allAudioSources)
        {
            audioSource.mute = true;
        }
        if (gameOverMusic != null)
        {
            // backgroundMusic.PlayOneShot(gameOverMusic);
            AudioSource.PlayClipAtPoint(gameOverMusic, Camera.main.transform.position);
        }

    }

    public void SetHealth(int health)
    {
        healthChange.Invoke(health);
    }


    public void OrcDefeated()
    {
        orcsDefeated++;
        if (SceneManager.GetActiveScene().name == "SecondScene" && orcsDefeated >= totalOrcs)
        {
            GameWin();
        }
    }

    public void GameWin()
    {
        // Optional: Play victory music
        Time.timeScale = 0.0f;
        gameWin.Invoke();
        backgroundMusic.Stop();


    }
}
