using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public BoardManager boardManager;
    public float levelStartDelay = 2f;
    public float turnDelay = 0.1f;
    public int playerFoodPoints = 100;

    [HideInInspector] public bool isPlayerTurn = true;
    private int level = 1;
    private List<Enemy> enemies;
    private bool isEnemiesMoving;

    private Text levelText;
    private GameObject levelImage;
    private bool isDoingSetup;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishLoading;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishLoading;
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
        enemies = new List<Enemy>();
        boardManager = GetComponent<BoardManager>();
        InitGame();
    }

    private void OnLevelFinishLoading(Scene scene, LoadSceneMode mode)
    {
        level++;
        InitGame();
    }



    private void InitGame()
    {
        isDoingSetup = true;
        levelImage = GameObject.Find("LevelImage");
        levelText = GameObject.Find("LevelText").GetComponent<Text>();
        levelText.text = $"Day {level}";
        levelImage.SetActive(true);

        StartCoroutine(HideLevelImageCoroutine());

        enemies.Clear();
        boardManager.SetupScene(level);
    }

    IEnumerator HideLevelImageCoroutine()
    {
        yield return new WaitForSeconds(levelStartDelay);

        levelImage.SetActive(false);
        isDoingSetup = false;
    }

    public void GameOver()
    {
        levelText.text = $"After {level} days, you starved.";
        levelImage.SetActive(true);
        enabled = false;
    }

    private void Update()
    {
        if (isPlayerTurn || isEnemiesMoving || isDoingSetup) return;

        StartCoroutine(MoveEnemiesCoroutine());
    }

    public void AddEnemyToList(Enemy script)
    {
        enemies.Add(script);
    }

    private IEnumerator MoveEnemiesCoroutine()
    {
        isEnemiesMoving = true;
        yield return new WaitForSeconds(turnDelay);

        if (enemies.Count == 0)
        {
            yield return new WaitForSeconds(turnDelay);
        }

        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].MoveEnemy();
            yield return new WaitForSeconds(enemies[i].moveTime);
        }

        isPlayerTurn = true;
        isEnemiesMoving = false;
    }
    
}
