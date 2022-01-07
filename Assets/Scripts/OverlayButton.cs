using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverlayButton : MonoBehaviour
{
    [SerializeField] private Button button;

    void OnEnable()
    {
        button.onClick.AddListener(ButtonClickHandler);
    }

    void OnDisable()
    {
        button.onClick.RemoveListener(ButtonClickHandler);
    }

    void ButtonClickHandler()
    {
        GameManager.instance.LoadNextLevel();
    }

    public void SetInteractable(bool flag)
    {
        button.interactable = flag;
    }
}
