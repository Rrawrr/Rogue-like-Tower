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

    private bool _isPlayerTurn;
    public  bool isPlayerTurn
    {
        get => _isPlayerTurn;
        set
        {
            _isPlayerTurn = value;
            if (!_isPlayerTurn && !isEnemiesMoving && !isDoingSetup)
            {
                StartCoroutine(MoveEnemiesCoroutine());
            }
        }
    }

    private int level = 1;
    private List<Enemy> enemies;
    private bool isEnemiesMoving;

    private Text levelText;
    private GameObject levelImage;

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
    }

    private void OnEnable()
    {
        StartCoroutine(InitGameCoroutine());
        SceneManager.sceneLoaded += OnLevelFinishLoading;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishLoading;
    }

    private void OnLevelFinishLoading(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnLevelFinishLoading");
        level++;
        StartCoroutine(InitGameCoroutine());
    }

    IEnumerator InitGameCoroutine()
    {
        if(isDoingSetup) yield break;

        Debug.Log("INIT GAME");

        isDoingSetup = true;
        levelImage = GameObject.Find("LevelImage");
        levelText = GameObject.Find("LevelText").GetComponent<Text>();
        levelText.text = $"Day {level}";
        levelImage.SetActive(true);

        yield return new WaitForSeconds(levelStartDelay);

        HideLevelImage();
        enemies.Clear();
        boardManager.SetupScene(level);

        isPlayerTurn = true;
    }

    void HideLevelImage()
    {
        levelImage.SetActive(false);
        isDoingSetup = false;
    }

    public void GameOver()
    {
        levelText.text = $"After {level} days, you starved.";
        levelImage.SetActive(true);
        enabled = false;
    }

    public void AddEnemyToList(Enemy script)
    {
        enemies.Add(script);
    }

    private IEnumerator MoveEnemiesCoroutine()
    {
        //Debug.Log("MoveEnemiesCoroutine");
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

            while (enemies[i].isMoving)
            {
                yield return null;
            }

        }

        isEnemiesMoving = false;
        isPlayerTurn = true;
    }
    
}
