using UnityEngine;

public class Loader : MonoBehaviour
{
    public GameManager gameManager;

    void Awake()
    {
        if (GameManager.instance == null)
        {
            Debug.Log("Instantiating GameManager");
            Instantiate(gameManager);
        }

        Application.targetFrameRate = 120;
    }
}
