using UnityEngine;
using UnityEngine.UI;

public class UIGameMenu : MonoBehaviour
{
    [SerializeField] Button _mainMenu;
    public PauseMenu pauseMenu;

    // Start is called before the first frame update
    void Start()
    {
        _mainMenu.onClick.AddListener(ExitToMenu);
        //pauseMenu = gameObject.GetComponentInParent<Transform>().GetComponentInParent<PauseMenu>();
    }

    private void ExitToMenu()
    {
        // This fixes the issue of the need to escape pause screen to trigger animation
        //pauseMenu.pauseMenuUI.SetActive(false);
        pauseMenu.GameIsPaused = false;
        Time.timeScale = 1f;
        ScenesManager.Instance.LoadMainMenu();
    }
}
