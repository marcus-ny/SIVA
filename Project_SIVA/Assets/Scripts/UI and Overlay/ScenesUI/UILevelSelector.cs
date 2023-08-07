using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILevelSelector : MonoBehaviour
{
    [SerializeField] Button _return;
    [SerializeField] Button _tutorialButton;
    [SerializeField] Button _level2Button;
    [SerializeField] Button _level3Button;
    [SerializeField] Button _level4Button;
    [SerializeField] Button _level5Button;

    // This keeps track of when the new game button is clicked
    void Start()
    {
        _return.onClick.AddListener(returnToMainMenu);
        _tutorialButton.onClick.AddListener(loadTutorial);
        _level2Button.onClick.AddListener(loadLevel2);
        _level3Button.onClick.AddListener(loadLevel3);
        _level4Button.onClick.AddListener(loadLevel4);
        _level5Button.onClick.AddListener(loadLevel5);
    }

    private void returnToMainMenu()
    {
        ScenesManager.Instance.LoadMainMenu();
    }

    private void loadTutorial()
    {
        ScenesManager.Instance.LoadScene(ScenesManager.GameScene.Tutorial_P0);
    }

    private void loadLevel2()
    {
        ScenesManager.Instance.LoadScene(ScenesManager.GameScene.C2_L1);
    }

    private void loadLevel3()
    {
        ScenesManager.Instance.LoadScene(ScenesManager.GameScene.C3_L1);
    }

    private void loadLevel4()
    {
        ScenesManager.Instance.LoadScene(ScenesManager.GameScene.C4_L1);
    }

    private void loadLevel5()
    {
        ScenesManager.Instance.LoadScene(ScenesManager.GameScene.C5_L1);
    }
}
