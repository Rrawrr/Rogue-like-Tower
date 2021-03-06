using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Tilemaps;
using Quaternion = UnityEngine.Quaternion;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

public class BoardManager : MonoBehaviour
{
    [Serializable]
    public class Count
    {
        public int minimum;
        public int maximum;

        public Count(int min, int max)
        {
            minimum = min;
            maximum = max;
        }
    }

    public int columns = 8;
    public int rows = 8;

    public Count wallCount = new Count(5, 9);
    public Count foodCount = new Count(1, 5);

    public Tile[] floorTiles;
    public Tile[] outerWallTiles;
    public GameObject exit;
    public GameObject[] innerWalls;
    public GameObject[] food;
    public GameObject[] enemies;

    public Tilemap floorTilemap;
    public Tilemap outerWallsTilemap;

    private Transform boardHolder;
    private List<Vector3> gridPositions = new List<Vector3>();

    private void InitializeList()
    {
        gridPositions.Clear();

        for (int x = 1; x < columns - 1; x++)
        {
            for (int y = 1; y < rows - 1; y++)
            {
                gridPositions.Add(new Vector3(x, y, 0f));
            }
        }
    }

    private void BoardSetup()
    {
        boardHolder = new GameObject("Board").transform;

        for (int x = -1; x < columns + 1; x++)
        {
            for (int y = -1; y < rows + 1; y++)
            {
                if (x == -1 || x == columns || y == -1 || y == rows)
                {
                    var toInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];
                    outerWallsTilemap.SetTile(new Vector3Int(x, y, 0), toInstantiate);
                }
                else
                {
                    //GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;
                    //instance.transform.SetParent(boardHolder);
                    var toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)]; 
                    floorTilemap.SetTile(new Vector3Int(x,y,0),toInstantiate);
                }
            }
        }
    }

    private Vector3 RandomPosition()
    {
        int randomIndex = Random.Range(0, gridPositions.Count);
        Vector3 randomPosition = gridPositions[randomIndex];
        gridPositions.RemoveAt(randomIndex);
        return randomPosition;
    }

    private void LayoutAtRandom(GameObject[] tileArray, int min, int max)
    {
        int objectCount = Random.Range(min, max + 1);

        for (int i = 0; i < objectCount; i++)
        {
            Vector3 randomPosition = RandomPosition();
            GameObject tileChoise = tileArray[Random.Range(0, tileArray.Length)];
            Instantiate(tileChoise, randomPosition, Quaternion.identity,boardHolder);
        }
    }

    public void SetupScene(int level)
    {
        BoardSetup();
        InitializeList();
        LayoutAtRandom(innerWalls, wallCount.minimum, wallCount.maximum);
        LayoutAtRandom(food, foodCount.minimum, foodCount.maximum);
        int enemyCount = (int)Mathf.Log(level, 2f);
        LayoutAtRandom(enemies, enemyCount, enemyCount);
        Instantiate(exit, new Vector3(columns - 1, rows - 1, 0f), Quaternion.identity);
    }


}
