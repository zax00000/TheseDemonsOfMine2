using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SimpleSceneManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject Menu;

    private ToggleUIOnEscape toggleUIOnEscape;

    void Start()
    {
        toggleUIOnEscape = GetComponent<ToggleUIOnEscape>();
    }

    public void ResumeGame()
    {
    toggleUIOnEscape?.OFF();
    }

    public void END()
    {
        SceneManager.LoadScene("END");
    }
    public void Back()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void Play()
    {
        SceneManager.LoadScene("GameLevel");
    }
    public void PauseBack()
    {
        if (pauseMenu != null)
        {
            toggleUIOnEscape.OFF2();
        }
        SceneManager.LoadScene("MainMenu");
    }
}
