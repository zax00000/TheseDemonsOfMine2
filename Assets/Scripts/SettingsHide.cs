using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class SettingsHide : MonoBehaviour
{
    public CanvasGroup canvasGroup;

    [Header("Optional Settings")]
    public bool startVisible = true;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        SetVisible(startVisible);
        Hide();
    }

    /// <summary>
    /// Show or hide the panel visually and interactively, without deactivating it.
    /// </summary>
    public void SetVisible(bool visible)
    {
        canvasGroup.alpha = visible ? 1f : 0f;
        canvasGroup.interactable = visible;
        canvasGroup.blocksRaycasts = visible;
    }

    public void Hide() => SetVisible(false);
    public void Show() => SetVisible(true);
    public void Toggle() => SetVisible(canvasGroup.alpha == 0f);
}
