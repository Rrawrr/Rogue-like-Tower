using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public BoardManager boardManager;
    public float turnDelay = 0.1f;
    public int playerFoodPoints = 100;

    [HideInInspector] public bool isPlayerTurn = true;
    private int level = 3;
    private List<Enemy> enemies;
    private bool isEnemiesMoving;

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

    private void InitGame()
    {
        enemies.Clear();
        boardManager.SetupScene(level);
    }

    public void GameOver()
    {
        enabled = false;
    }

    private void Update()
    {
        if (isPlayerTurn || isEnemiesMoving) return;

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
