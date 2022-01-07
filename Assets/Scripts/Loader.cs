using UnityEngine;
using UnityEngine.SceneManagement;

public class Loader : MonoBehaviour
{
    void Awake()
    {
        Application.targetFrameRate = 60;
        SceneManager.LoadScene("app");
    }
}
