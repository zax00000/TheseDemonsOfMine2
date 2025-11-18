using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SimpleSceneManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject Menu;

    private ToggleUIOnEscape toggleUIOnEscape;

    void Start()
    {
        toggleUIOnEscape = GetComponent<ToggleUIOnEscape>();
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ReloadCurrent()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ResumeGame()
    {
        if (pauseMenu != null)
        {
            Menu.SetActive(false);
            toggleUIOnEscape.active = false;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        Time.timeScale = 1f;
    }

    public void END()
    {
        SceneManager.LoadScene("END");
    }

}
