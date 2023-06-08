using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGameMenu : MonoBehaviour
{
    [SerializeField] Button _mainMenu;

    // Start is called before the first frame update
    void Start()
    {
        _mainMenu.onClick.AddListener(ExitToMenu);
    }

    private void ExitToMenu(){
        // This fixes the issue of the need to escape pause screen to trigger animation
        Time.timeScale = 1f;
        ScenesManager.Instance.LoadMainMenu();
    }
}
