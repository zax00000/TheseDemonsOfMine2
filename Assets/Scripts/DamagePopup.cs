using TMPro;
using UnityEngine;

public class DamagePopup : MonoBehaviour
{
    public TextMeshPro textMesh;
    public float floatSpeed = 1f;
    public float lifetime = 1f;
    public float fadeOutSpeed = 2f;

    private Vector3 moveDirection = Vector3.up;
    private float timer;
    private Color originalColor;

    public void Setup(int damageAmount, Color color)
    {
        textMesh.text = damageAmount.ToString();
        textMesh.color = color;
        originalColor = color;
        timer = 0f;
    }

    void Update()
    {
        // Float upward
        transform.position += moveDirection * floatSpeed * Time.deltaTime;

        // Billboard
        if (Camera.main != null)
        {
            transform.forward = Camera.main.transform.forward;
        }

        // Fade out
        timer += Time.deltaTime;
        float fade = 1f - (timer / lifetime);
        textMesh.color = new Color(originalColor.r, originalColor.g, originalColor.b, fade);

        if (timer >= lifetime)
        {
            Destroy(gameObject);
        }
    }
}
