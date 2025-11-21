using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class UICanvasGroupController : MonoBehaviour
{
    private CanvasGroup canvasGroup;

    [Header("Optional Settings")]
    public bool startVisible = true;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        SetVisible(startVisible);
        Hide();
    }

    /// <summary>
    /// Show or hide the UI group.
    /// </summary>
    public void SetVisible(bool visible)
    {
        canvasGroup.alpha = visible ? 1f : 0f;
        canvasGroup.interactable = visible;
        canvasGroup.blocksRaycasts = visible;
    }

    /// <summary>
    /// Instantly hide the UI.
    /// </summary>
    public void Hide() => SetVisible(false);

    /// <summary>
    /// Instantly show the UI.
    /// </summary>
    public void Show() => SetVisible(true);

    /// <summary>
    /// Toggle visibility.
    /// </summary>
    public void Toggle() => SetVisible(canvasGroup.alpha == 0f);
}
