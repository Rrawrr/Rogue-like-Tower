using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public BoardManager boardManager;
    public OverlayButton overlayButton;
    public float nextLevelDelay = 1f;
    public float levelStartDelay = 2f;
    public float turnDelay = 0.1f;
    public int playerFoodPoints = 100;
    public bool isDoingSetup;

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

    private int level = 0;
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
        SceneManager.sceneLoaded += OnLevelFinishLoading;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishLoading;
    }

    private void OnLevelFinishLoading(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnLevelFinishLoading");
        StartCoroutine(InitGameCoroutine());
    }

    IEnumerator InitGameCoroutine()
    {
        Debug.Log("INIT GAME");

        isDoingSetup = true;
        level++;
        SoundManager.instance.PlayMusic();
        levelImage = GameObject.Find("LevelImage");
        levelText = GameObject.Find("LevelText").GetComponent<Text>();
        overlayButton = levelImage.GetComponentInParent<OverlayButton>();
        overlayButton.SetInteractable(false);
        levelText.text = $"Day {level}";
        levelImage.SetActive(true);

        yield return new WaitForSeconds(levelStartDelay);

        HideLevelImage();
        enemies.Clear();
        boardManager.SetupScene(level);

        isDoingSetup = false;
        isPlayerTurn = true;
    }

    void HideLevelImage()
    {
        levelImage.SetActive(false);
    }

    public void GameOver()
    {
        levelText.text = $"After {level} days, you starved.\n Try again?";
        levelImage.SetActive(true);
        overlayButton.SetInteractable(true);
        level = 0;
        playerFoodPoints = 100;
    }

    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevelCoroutine());
    }

    IEnumerator LoadLevelCoroutine()
    {
        yield return new WaitForSeconds(nextLevelDelay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
