using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Sword : MonoBehaviour
{
    // References
    public Animator childAnimator;
    public Transform handContainer;
    public Transform backContainer;
    public Transform sword;

    // Sword positions
    public Vector3 swordHandPos;
    public Vector3 swordHandRot;
    public Vector3 swordBackPostion;
    public Vector3 swordBackRot;

    // State
    private bool equipped = false;
    private bool attackOnCooldown = false;
    private bool slash2 = false;
    private bool slash3 = false;
    private bool s2 = false;
    private bool s3 = false;
    private bool parry = true;
    private bool isParryHeld = false;
    private bool isParryTimeActive = false;

    private Coroutine attackCooldownRoutine;
    private Coroutine saveCooldownRoutine;
    private Coroutine parryCooldownRoutine;
    private Coroutine parryTimeRoutine;


    // Components
    private SwordHitbox SwordHitbox;
    private CharacterController controller;
    private PlayerController playerController;
    private ParentConnection parentConnection;
    private PlayerHealth playerHealth;

    // Root motion cache
    private Vector3 rootMotionDeltaPosition = Vector3.zero;
    private Quaternion rootMotionDeltaRotation = Quaternion.identity;
    private bool applyRootMotionThisFrame = false;

    public float saveCd;
    public float parryCd;

    public bool isParrying = false;
    public bool isHeld = false;

    public bool isDead = true;

    [Header("Sounds")]

    [SerializeField] private AudioSource parrySource;
    [SerializeField] private AudioSource s1Source;
    [SerializeField] private AudioSource s2Source;
    [SerializeField] private AudioSource s3Source;

    [SerializeField] public Image eye;

    [SerializeField] private float gravity = -9.81f;
    private float verticalVelocity = 0f;

    public bool parryTime = false;

    [SerializeField] public Image parryUI;
    [SerializeField] public Image A1UI;
    [SerializeField] public Image A2UI;
    [SerializeField] public Image A3UI;
    [SerializeField] public Image ARing;

    void Start()
    {
        A1UI.enabled = true;
        A2UI.enabled = false;
        A3UI.enabled = false;
        SetAlphaParry(1f);
        SetAlpha(0f);
        playerHealth = GetComponent<PlayerHealth>();
        playerController = GetComponent<PlayerController>();
        SwordHitbox = GetComponentInChildren<SwordHitbox>();
        controller = GetComponent<CharacterController>();
        parentConnection = GetComponentInChildren<ParentConnection>();

        if (childAnimator == null)
            childAnimator = GetComponentInChildren<Animator>();

        childAnimator.applyRootMotion = true;
    }

    void Update()
    {
        if (isDead) return;

        if (isParryHeld && parry && !isHeld)
        {
            if (!equipped)
            {
                childAnimator.SetTrigger("Parry");
            }
            else
            {
                parentConnection?.P();
            }
        }
    }

    void FixedUpdate()
    {
        if (!applyRootMotionThisFrame || controller == null || playerController == null) return;

        // Apply gravity if not grounded
        if (!controller.isGrounded)
        {
            verticalVelocity += gravity * Time.fixedDeltaTime;
        }
        else
        {
            verticalVelocity = 0f;
        }

        Vector3 motion = rootMotionDeltaPosition;

        // Blend gravity into root motion
        motion.y += verticalVelocity * Time.fixedDeltaTime;

        if (!playerController.IsDashing())
        {
            controller.Move(motion);
            transform.rotation *= rootMotionDeltaRotation;
        }
        else
        {
            controller.Move(new Vector3(0, motion.y, 0));
        }

        rootMotionDeltaPosition = Vector3.zero;
        rootMotionDeltaRotation = Quaternion.identity;
        applyRootMotionThisFrame = false;
    }

    public bool ShouldApplyRootMotion()
    {
        return childAnimator.GetCurrentAnimatorStateInfo(0).IsTag("RootMotion");
    }

    public void ApplyRootMotionFromChild(Vector3 deltaPos, Quaternion deltaRot)
    {
        rootMotionDeltaPosition += deltaPos;
        rootMotionDeltaRotation *= deltaRot;
        applyRootMotionThisFrame = true;
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (isDead || !context.started || attackOnCooldown || childAnimator == null) return;

        if (!equipped && isHeld)
        {
            childAnimator.SetTrigger("HeldDraw");
            attackOnCooldown = true;
        }
        else if (isHeld)
        {
            childAnimator.SetTrigger("HeldAttack");
            attackOnCooldown = true;
        }
        else if (!equipped && !slash2 && !slash3)
        {
            childAnimator.SetTrigger("Draw");
            attackOnCooldown = true;
        }
        else if (equipped && !slash2 && !slash3)
        {
            childAnimator.SetTrigger("1");
            attackOnCooldown = true;
        }
        else if (equipped && slash2 && !slash3)
        {
            childAnimator.SetTrigger("2");
            slash2 = false;
            attackOnCooldown = true;
            s2 = true;
        }
        else if (equipped && !slash2 && slash3)
        {
            playerController?.JumpingSlash();
            childAnimator.SetTrigger("3");
            slash3 = false;
            attackOnCooldown = true;
            s3 = true;
        }
        else if (!equipped && !slash2 && slash3)
        {
            playerController?.JumpingSlash();
            childAnimator.SetTrigger("SpinDraw");
            slash3 = false;
            attackOnCooldown = true;
            s3 = true;
        }

            RestartSaveCooldown();
            UpdateUI();
            AlphaAttack();
    }

    private IEnumerator SaveCooldown()
    {
        yield return new WaitForSecondsRealtime(saveCd);
        if (isDead) yield break;
        childAnimator.SetTrigger("Save");
        saveCooldownRoutine = null;
    }

    public void RestartSaveCooldown()
    {
        if (saveCooldownRoutine != null)
        {
            StopCoroutine(saveCooldownRoutine);
        }
        saveCooldownRoutine = StartCoroutine(SaveCooldown());
    }

    public void PlaceSwordToHand()
    {
        sword.SetParent(handContainer, false);
        sword.localPosition = swordHandPos;
        sword.localEulerAngles = swordHandRot;
        equipped = true;
    }

    public void PlaceSwordToBack()
    {
        sword.SetParent(backContainer, false);
        sword.localPosition = swordBackPostion;
        sword.localEulerAngles = swordBackRot;
        equipped = false;
    }

    public void Slash2()
    {
        slash2 = true;
        attackOnCooldown = false;
        StartCoroutine(ResetSlash2());
        UpdateUI();
        AlphaAttack();
    }

    private IEnumerator ResetSlash2()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        slash2 = false;
        UpdateUI();
        AlphaAttack();
    }

    public void Slash3()
    {
        slash3 = true;
        attackOnCooldown = false;
        StartCoroutine(ResetSlash3());
        s2 = false;
        UpdateUI();
        AlphaAttack();
    }

    private IEnumerator ResetSlash3()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        slash3 = false;
        UpdateUI();
        AlphaAttack();
    }

    public void OnParry(InputAction.CallbackContext context)
    {
        if (isDead || childAnimator == null) return;

        if (context.started)
        {
            isParryHeld = true;
        }
        else if (context.canceled)
        {
            isParryHeld = false;
        }
    }

    public void Parry()
    {
        if (isDead) return;

        SetAlphaParry(0.2f);
        parry = false;
        isParrying = true;
        childAnimator.SetTrigger("Parry2");
        RestartParryCooldown();
        RestartSaveCooldown();
        SwordHitbox?.ParryHitbox();

        if (attackOnCooldown && !s2 && !s3)
        {
            Slash2();
        }
        if (s2 && !s3)
        {
            Slash3();
        }
        if (!s2 && s3)
        {
            playerController?.JumpingSlashFinished();
        }
    }


    public void FinishParry()
    {
        isParrying = false;
        SwordHitbox?.ParryHitboxEnd();
    }

    public void setS3False()
    {
        s3 = false;
        attackOnCooldown = false;
        UpdateUI();
        AlphaAttack();
    }

    private IEnumerator ParryCooldown()
    {
        yield return new WaitForSecondsRealtime(parryCd);
        SetAlphaParry(1f);
        parryCooldownRoutine = null;
        parry = true;
    }

    public void RestartParryCooldown()
    {
        if (parryCooldownRoutine != null)
        {
            StopCoroutine(parryCooldownRoutine);
        }
        parryCooldownRoutine = StartCoroutine(ParryCooldown());
    }

    public bool IsParrying()
    {
        return isParrying;
    }

    public void Held()
    {
        isHeld = true;
    }

    public void NotHeld()
    {
        isHeld = false;
    }

    public void ParryTime()
    {
        parryTime = true;
        playerHealth.damaged = true;
        if (parrySource != null)
        {
            parrySource.Play();
        }
        if (isParryTimeActive) return;
        isParryTimeActive = true;
        StartCoroutine(FadeAlphaToPointOneRealtime(5f));
        parryTimeRoutine = StartCoroutine(SlowWorldExceptPlayer(0.1f, 5f));
        Debug.Log("Parry Time Activated");
    }

    private IEnumerator SlowWorldExceptPlayer(float slowFactor, float duration)
    {
        isParryTimeActive = true;

        Time.timeScale = slowFactor;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        childAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
        playerController.UseUnscaledTime(true);

        yield return new WaitForSecondsRealtime(duration);

        parryTime = false;
        playerHealth.damaged = false;
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;

        childAnimator.updateMode = AnimatorUpdateMode.Normal;
        playerController.UseUnscaledTime(false);

        isParryTimeActive = false; // Unlock after cooldown

    }
    public void CancelParryTime()
    {
        if (parryTimeRoutine != null)
        {
            StopCoroutine(parryTimeRoutine);
            parryTimeRoutine = null;
        }

        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
        childAnimator.updateMode = AnimatorUpdateMode.Normal;
        playerController.UseUnscaledTime(false);
        isParryTimeActive = false;
    }

    public void DisableSword()
    {
        isDead = true;
    }

    public void EnableSword()
    {
        isDead = false;
    }

    public void SlashSound()
    {
        int index = Random.Range(0, 3);

        switch (index)
        {
            case 0:
                s1Source?.Play();
                break;
            case 1:
                s2Source?.Play();
                break;
            case 2:
                s3Source?.Play();
                break;
        }
    }

    public void SetAlpha(float alpha)
    {
        if (eye != null)
        {
            Color color = eye.color;
            color.a = Mathf.Clamp01(alpha); // Clamp between 0 and 1
            eye.color = color;
        }
    }
    public IEnumerator FadeAlphaToPointOneRealtime(float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            float alpha = Mathf.Lerp(1f, 0f, t);
            SetAlpha(alpha);
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }
        SetAlpha(0f); // Ensure it ends at 0.1
    }
    public void SetAlphaParry(float alpha)
    {
        if (parryUI != null)
        {
            Color color = parryUI.color;
            color.a = Mathf.Clamp01(alpha); // Clamp between 0 and 1
            parryUI.color = color;
        }
    }

    public void UpdateUI()
    {
        if (attackOnCooldown && isHeld)
        {
            A1UI.enabled = true;
            A2UI.enabled = false;
            A3UI.enabled = false;
            return;
        }
        else if (attackOnCooldown && !s2 && !s3)
        {
            A1UI.enabled = false;
            A2UI.enabled = true;
            A3UI.enabled = false;
        }
        else if (attackOnCooldown && s2 && !s3)
        {
            A1UI.enabled = false;
            A2UI.enabled = false;
            A3UI.enabled = true;
        }
        else if (attackOnCooldown && !s2 && s3)
        {
            A1UI.enabled = true;
            A2UI.enabled = false;
            A3UI.enabled = false;
        }
        else if (!attackOnCooldown && !slash2 && !slash3)
        {
            A1UI.enabled = true;
            A2UI.enabled = false;
            A3UI.enabled = false;
        }
        else if (!attackOnCooldown && slash2 && !slash3)
        {
            A1UI.enabled = false;
            A2UI.enabled = true;
            A3UI.enabled = false;
        }
        else if (!attackOnCooldown && !slash2 && slash3)
        {
            A1UI.enabled = false;
            A2UI.enabled = false;
            A3UI.enabled = true;
        }
        return;
    }

    private void AlphaAttack()
    {
        if (attackOnCooldown)
        {
            SetAlphaAttack(.2f);
        }
        else
        {
            SetAlphaAttack(1f);
        }
    }

    private void SetAlphaAttack(float alpha)
    {
        if (ARing != null)
        {
            Color color = ARing.color;
            color.a = Mathf.Clamp01(alpha); // Clamp between 0 and 1
            ARing.color = color;
        }
    }
}