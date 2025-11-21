using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class FinalFade : MonoBehaviour
{
    public float delay = 1f;       // Delay before fade starts
    public float fadeSpeed = 1f;   // Speed of fade (units per second)

    private Image image;

    void Awake()
    {
        image = GetComponent<Image>();

        Color c = image.color;
        c.a = 0f;
        image.color = c;
    }

    void OnEnable()
    {
        PlayerController.Finished += Fade;
    }

    IEnumerator FadeInRoutine()
    {
        if (delay > 0f)
            yield return new WaitForSeconds(delay);

        Color c = image.color;
        while (c.a < 1f)
        {
            c.a += fadeSpeed * Time.unscaledDeltaTime;
            c.a = Mathf.Clamp01(c.a);
            image.color = c;
            yield return null;
        }

        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("Title");
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void Fade ()
    {
        StartCoroutine(FadeInRoutine());
    }

    private void OnDisable()
    {
        PlayerController.Finished -= Fade;
    }
}
