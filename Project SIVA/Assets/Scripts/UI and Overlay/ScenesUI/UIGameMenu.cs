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
        ScenesManager.Instance.LoadMainMenu();
    }
}
