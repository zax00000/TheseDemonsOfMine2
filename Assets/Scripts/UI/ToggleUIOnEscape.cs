using UnityEngine;

public class ToggleUIOnEscape : MonoBehaviour
{
    [SerializeField] private GameObject targetUI;

    public bool active = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Escape key pressed");

            if (targetUI != null)
            {
                if (!active)
                {
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                    active = true;
                    targetUI.SetActive(true);
                    Time.timeScale = 0f; 

                }

                else 
                {
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;
                    active = false;
                    targetUI.SetActive(false);
                    Time.timeScale = 1f;
                }
            }
        }
    }
}
