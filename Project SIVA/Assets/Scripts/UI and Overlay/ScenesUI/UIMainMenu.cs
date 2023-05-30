using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMainMenu : MonoBehaviour
{
    [SerializeField] Button _newGame;

    // This keeps track of when the new game button is clicked
    void Start()
    {
        _newGame.onClick.AddListener(StartGame);
    }

    // This method loads a new game 
    private void StartGame(){
        ScenesManager.Instance.LoadNewGame();
    }
}
