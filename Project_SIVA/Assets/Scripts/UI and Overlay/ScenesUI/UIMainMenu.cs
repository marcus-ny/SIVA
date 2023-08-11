using UnityEngine;
using UnityEngine.UI;

public class UIMainMenu : MonoBehaviour
{
    [SerializeField] Button _newGame;
    [SerializeField] Button _selectLevel;
    [SerializeField] Button _quitGame;

    // This keeps track of when the new game button is clicked
    void Start()
    {
        _newGame.onClick.AddListener(StartGame);
        _selectLevel.onClick.AddListener(LoadLevelSelector);
        _quitGame.onClick.AddListener(QuitGame);
    }

    // This method loads a new game 
    private void StartGame()
    {
        ScenesManager.Instance.LoadNewGame();
    }

    private void LoadLevelSelector()
    {
        ScenesManager.Instance.LoadScene(ScenesManager.GameScene.LevelSelector);
    }

    private void QuitGame()
    {

        Application.Quit();
    }
}
