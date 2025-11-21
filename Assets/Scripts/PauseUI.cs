using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseUI : MonoBehaviour
{
    [Header("Panels to control")]
    public GameObject panel;
    private SettingsHide settingsHide;

    void Awake()
    {
        settingsHide = panel.GetComponent<SettingsHide>();
    }

    public void ShowPanel()
    {
    settingsHide?.Show();
    }
    public void HidePanel()
    {
        settingsHide?.Hide();
    }
}
