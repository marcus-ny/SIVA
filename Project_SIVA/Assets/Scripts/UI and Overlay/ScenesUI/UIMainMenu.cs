using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMainMenu : MonoBehaviour
{
    [SerializeField] Button _newGame;
    [SerializeField] Button _loadGame;
    [SerializeField] Button _quitGame;

    // This keeps track of when the new game button is clicked
    void Start()
    {
        _newGame.onClick.AddListener(StartGame);
        _loadGame.onClick.AddListener(LoadGame);
        _quitGame.onClick.AddListener(QuitGame);
    }

    // This method loads a new game 
    private void StartGame(){
        //ScenesManager.Instance.LoadNewGame();
        ScenesManager.Instance.LoadNewGame();
    }

    private void LoadGame(){
        Debug.Log("Go to load save file scene");
    }

    private void QuitGame(){
        Debug.Log("Quitting Game");
        Application.Quit();
    }
}
