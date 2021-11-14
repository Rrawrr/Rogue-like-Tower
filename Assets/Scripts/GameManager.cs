using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public BoardManager boardManager;
    public int playerFoodPoints = 100;

    [HideInInspector] public bool isPlayerTurn = true;
    private int level = 3;

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
        boardManager = GetComponent<BoardManager>();
        InitGame();
    }

    private void InitGame()
    {
        boardManager.SetupScene(level);
    }

    public void GameOver()
    {
        enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
