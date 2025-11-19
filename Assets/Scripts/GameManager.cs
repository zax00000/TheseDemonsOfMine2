using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private GameObject currentPlayer;

    [Header("Player Settings")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform healthBarFill;
    [SerializeField] private Image eye;
    [SerializeField] public Image ringUI;
    [SerializeField] public Image parryUI;
    [SerializeField] public Image A1UI;
    [SerializeField] public Image A2UI;
    [SerializeField] public Image A3UI;
    [SerializeField] public Image ARing;

    [Header("Camera Settings")]
    [SerializeField] private CameraFollow cameraFollow;

    [Header("Anxiety Spawn Points")]

    [SerializeField] private GameObject anxietyPrefab;
    [SerializeField] private Transform a1;
    [SerializeField] private Transform a2;
    [SerializeField] private Transform a3;
    [SerializeField] private Transform a4;
    [SerializeField] private Transform a5;
    [SerializeField] private Transform a6;

    [Header("Stress Spawn Points")]

    [SerializeField] private GameObject stressPrefab;
    [SerializeField] private Transform s1;
    [SerializeField] private Transform s3;
    [SerializeField] private Transform s4;
    [SerializeField] private Transform s5;
    [SerializeField] private Transform s6;
    [SerializeField] private Transform s7;

    [Header("Fear Spawn")]

    [SerializeField] private GameObject fearPrefab;
    [SerializeField] private Transform F;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        SpawnPlayer();
        SpawnAnxiety();
        SpawnStress();
        SpawnFear();
    }

    public void SpawnPlayer()
    {
        if (playerPrefab == null || spawnPoint == null || cameraFollow == null)
        {
            Debug.LogError("Missing references in GameManager!");
            return;
        }

        if (currentPlayer != null)
        {
            Destroy(currentPlayer);
        }

        currentPlayer = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);

        // Assign camera target
        cameraFollow.target = currentPlayer.transform;

        // Assign AI target
        if (AIManager.Instance != null)
        {
            AIManager.Instance.Target = currentPlayer.transform;
        }

        // Assign health bar transform
        PlayerHealth playerHealth = currentPlayer.GetComponent<PlayerHealth>();
        if (playerHealth != null && healthBarFill != null)
        {
            playerHealth.healthBarTransform = healthBarFill;
        }

        // Assign dash indicator
        PlayerController playerController = currentPlayer.GetComponent<PlayerController>();
        if (playerController != null && ringUI != null)
        {
            playerController.ringUI = ringUI;
        }

        // Assign UI Parry
        Sword playersword = currentPlayer.GetComponent<Sword>();
        if (playersword != null && eye != null)
        {
            playersword.eye = eye;
            playersword.parryUI = parryUI;
            playersword.A1UI = A1UI;
            playersword.A2UI = A2UI;
            playersword.A3UI = A3UI;
            playersword.ARing = ARing;
        }
    }

    public void SpawnAnxiety()
    {
        Instantiate(anxietyPrefab, a1.position, a1.rotation);
        Instantiate(anxietyPrefab, a2.position, a2.rotation);
        Instantiate(anxietyPrefab, a3.position, a3.rotation);
        Instantiate(anxietyPrefab, a4.position, a4.rotation);
        Instantiate(anxietyPrefab, a5.position, a5.rotation);
        Instantiate(anxietyPrefab, a6.position, a6.rotation);
    }

    public void SpawnStress()
    {
        Instantiate(stressPrefab, s1.position, s1.rotation);
        Instantiate(stressPrefab, s3.position, s3.rotation);
        Instantiate(stressPrefab, s4.position, s4.rotation);
        Instantiate(stressPrefab, s5.position, s5.rotation);
        Instantiate(stressPrefab, s6.position, s6.rotation);
        Instantiate(stressPrefab, s7.position, s7.rotation);
    }

    public void SpawnFear()
    {
        Instantiate(fearPrefab, F.position, F.rotation);
    }
}
