using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;


public class ToggleUIOnEscape : MonoBehaviour
{
    [SerializeField] private GameObject targetUI;
    private UI_PanelManager panelManager;
    private UICanvasGroupController canvasController;
    private PauseUI pauseUI;

    public bool active = false;

    public bool block;

    private void Start()
    {
        panelManager = GetComponent<UI_PanelManager>();
        canvasController = targetUI.GetComponent<UICanvasGroupController>();
        pauseUI = GetComponent<PauseUI>();
        block = false;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !block)
        {
            if (targetUI != null)
            {
                if (!active)
                {
                    ON();

                }

                else
                {
                    OFF();
                }
            }
        }
    }

    public void ON()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        active = true;
        canvasController?.Show();
        Time.timeScale = 0f;
    }
    public void OFF()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        active = false;
        Time.timeScale = 1f;
        pauseUI?.HidePanel();
        canvasController?.Hide();
        
    }
    public void OFF2()
    {
        active = false;
        Time.timeScale = 1f;
        pauseUI?.HidePanel();
        canvasController?.Hide();
    }
    private void OnEnable()
    {
        PlayerController.Finished += BlockOn;
    }

    private void OnDisable()
    {
        PlayerController.Finished -= BlockOn;
    }

    private void BlockOn()
    {
       block = true;
    }
}
