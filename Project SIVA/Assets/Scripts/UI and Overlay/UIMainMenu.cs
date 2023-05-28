using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMainMenu : MonoBehaviour
{
    [SerializeField] Button _newGame;

    void Start()
    {
        _newGame.onClick.AddListener(StartGame);
    }

    private void StartGame(){
        ScenesManager.Instance.LoadNewGame();
    }
}
