using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class UI_PanelManager : MonoBehaviour
{
    [Header("Panels to control")]
    public GameObject[] panels;

    [Header("Audio Settings")]
    public AudioSource audioSource;
    public AudioClip openSound;
    public AudioClip closeSound;
    [Range(0f, 1f)] public float volume = 1f;

    private int currentIndex = -1;

    void Awake()
    {
        if (!audioSource)
            audioSource = GetComponent<AudioSource>();

        // Ensure panels are hidden at startup
        ResetPanels();
    }

    void OnEnable()
    {
        // Extra safety: reset when object is re-enabled
        ResetPanels();
    }

    private void ResetPanels()
    {
        foreach (GameObject panel in panels)
        {
            if (panel != null)
                panel.SetActive(false);
        }
        currentIndex = -1;
    }

    public void ShowPanel(int index)
    {
        if (index < 0 || index >= panels.Length) return;

        for (int i = 0; i < panels.Length; i++)
        {
            bool shouldBeActive = (i == index);
            bool isCurrentlyActive = panels[i].activeSelf;

            if (shouldBeActive && !isCurrentlyActive)
            {
                panels[i].SetActive(true);
                PlaySound(openSound);
            }
            else if (!shouldBeActive && isCurrentlyActive)
            {
                panels[i].SetActive(false);
                PlaySound(closeSound);
            }
        }

        currentIndex = index;
    }

    private IEnumerator PlayGame()
    {
        yield return new WaitForSeconds(0.5f);

        // Optional: reset time scale before loading
        Time.timeScale = 1f;

        SceneManager.LoadScene("GameLevel");
    }

    public void HideAllPanels()
    {
        foreach (GameObject panel in panels)
        {
            if (panel.activeSelf)
            {
                panel.SetActive(false);
                PlaySound(closeSound);
            }
        }
        currentIndex = -1;
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip == null || audioSource == null) return;
        audioSource.PlayOneShot(clip, volume);
    }

    public void StartGame()
    {
        StartCoroutine(PlayGame());
    }
}
