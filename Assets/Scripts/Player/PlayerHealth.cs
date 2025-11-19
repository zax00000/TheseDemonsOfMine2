using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking.PlayerConnection;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;


public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth;

    [Header("UI Health Bar")]
    [SerializeField] public Transform healthBarTransform; // This should be the fill sprite's transform

    public bool damaged = false;

    private DamageFlash damageFlash;

    private ParentConnection parentConnection;

    private PlayerController playerController;

    private Sword playerSword;

    private bool isDead = false;

    private CharacterController controller;

    [Header("Sounds")]

    [SerializeField] private AudioSource hit1Source;
    [SerializeField] private AudioSource hit2Source;
    [SerializeField] private AudioSource hit3Source;

    [SerializeField] private GameObject damagePopupPrefab;

    [SerializeField] private Transform damagePopupAnchor;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        playerSword = GetComponent<Sword>();
        playerController = GetComponent<PlayerController>();
        parentConnection = GetComponentInChildren<ParentConnection>();
        damageFlash = GetComponentInChildren<DamageFlash>();
        currentHealth = maxHealth;
        UpdateHealthBar();
        damageFlash?.HealthUpdate(currentHealth);

    }

    public void TakeDamage(float damage)
    {
        if (!damaged && !isDead)
        {
            damaged = true;
            ShowDamagePopup(damage);
            currentHealth = Mathf.Clamp(currentHealth - damage, 0f, maxHealth);
            damageFlash?.Flash(currentHealth);
            UpdateHealthBar();

            int index = Random.Range(0, 3);

            switch (index)
            {
                case 0:
                    hit1Source?.Play();
                    break;
                case 1:
                    hit2Source?.Play();
                    break;
                case 2:
                    hit3Source?.Play();
                    break;
            }
            Debug.Log("Player Health: " + currentHealth);
            StartCoroutine(DamageCooldown());

            if (currentHealth <= 0)
            {
                isDead = true;
                PlayerEvents.OnPlayerDeath?.Invoke();
                parentConnection?.Death();
                playerController?.DisableControls();
                playerSword?.DisableSword();
            }
        } return;
    }

    IEnumerator DamageCooldown()
    {
        yield return new WaitForSecondsRealtime(.3f);
        damaged = false;
    }

    private void UpdateHealthBar()
    {
        if (healthBarTransform != null)
        {
            float healthPercent = currentHealth / maxHealth;
            healthBarTransform.localScale = new Vector3(healthPercent, 1f, 1f);
        }
    }
    public void Fall()
    {
        currentHealth = 0;
        UpdateHealthBar();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene("END");
    }
    private void ShowDamagePopup(float damageAmount)
    {
        if (damagePopupPrefab == null || damagePopupAnchor == null) return;

        Vector3 spawnPos = damagePopupAnchor.position + Random.insideUnitSphere * 0.2f;
        GameObject popup = Instantiate(damagePopupPrefab, spawnPos, Quaternion.identity);
        DamagePopup popupScript = popup.GetComponent<DamagePopup>();
        if (popupScript != null)
        {
            popupScript.Setup(Mathf.RoundToInt(damageAmount), Color.red);
        }
    }
}
