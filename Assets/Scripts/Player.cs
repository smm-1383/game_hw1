using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(CapsuleCollider2D))]
public class Player : MonoBehaviour
{
    [SerializeField] float jumpForce = 12f;
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundRadius = 0.1f;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] int maxLives = 3;
    [SerializeField] float invincibleTime = 1f;
    [SerializeField] Color flashColor = Color.red;
    [SerializeField] int flashCount = 3;
    [SerializeField] float dashDuration = 0.5f, dashCooldown = 1f, dashBuffer = 0.15f;
    [SerializeField] Color dashColor = new Color(0.4f, 0.9f, 1f);
    [SerializeField] Vector2 crouchScale = new Vector2(1.15f, 0.55f);

    public int Lives { get; private set; }
    public int MaxLives => maxLives;

    static readonly int GroundedHash = Animator.StringToHash("Grounded");
    static readonly int HitHash = Animator.StringToHash("Hit");
    static readonly int DeadHash = Animator.StringToHash("Dead");

    Rigidbody2D rb;
    Animator anim;
    SpriteRenderer sr;
    InputAction jump, dash, crouch;
    bool dead, flashing, dashing, crouching;
    float nextDash, dashQueued = -1f;
    CapsuleCollider2D cap;
    Vector2 capSize, capOffset;
    Transform visual;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        sr = GetComponentInChildren<SpriteRenderer>();
        cap = GetComponent<CapsuleCollider2D>();
        visual = sr.transform;
        capSize = cap.size; capOffset = cap.offset;
        Lives = maxLives;
        jump = new InputAction("Jump", InputActionType.Button, "<Keyboard>/space");
        dash = new InputAction("Dash", InputActionType.Button, "<Keyboard>/leftShift");
        dash.AddBinding("<Keyboard>/rightShift");
        crouch = new InputAction("Crouch", InputActionType.Button, "<Keyboard>/downArrow");
        crouch.AddBinding("<Keyboard>/s");
    }

    void OnEnable() { jump.Enable(); dash.Enable(); crouch.Enable(); }
    void OnDisable() { jump.Disable(); dash.Disable(); crouch.Disable(); }

    void Update()
    {
        if (dead || Time.timeScale == 0f) return;

        bool grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);
        anim.SetBool(GroundedHash, grounded || crouching);

        bool jumped = jump.WasPressedThisFrame() && grounded;
        if (jumped)
            rb.linearVelocity = new Vector2(0f, jumpForce);

        if (dash.WasPressedThisFrame()) dashQueued = Time.time + dashBuffer;
        if (Time.time >= nextDash && Time.time <= dashQueued)
        {
            dashQueued = -1f;
            nextDash = Time.time + dashCooldown;
            StartCoroutine(DashRoutine());
        }

        bool resting = crouching && Mathf.Abs(rb.linearVelocity.y) < 0.1f;
        SetCrouch(crouch.IsPressed() && (grounded || resting) && !jumped && !dashing);
        if (!flashing) sr.color = dashing ? dashColor : Color.white;
    }

    void SetCrouch(bool on)
    {
        if (on == crouching) return;
        crouching = on;
        visual.localScale = on ? new Vector3(crouchScale.x, crouchScale.y, 1f) : Vector3.one;
        float feet = capOffset.y - capSize.y / 2f;
        cap.size = new Vector2(capSize.x, on ? capSize.y * 0.5f : capSize.y);
        cap.offset = new Vector2(capOffset.x, feet + cap.size.y / 2f);
        cap.direction = cap.size.y >= cap.size.x ? CapsuleDirection2D.Vertical : CapsuleDirection2D.Horizontal;
    }

    IEnumerator DashRoutine()
    {
        dashing = true;
        SetCrouch(false);
        GameManager.Instance.DashActive = true;
        yield return new WaitForSeconds(dashDuration);
        dashing = false;
        GameManager.Instance.DashActive = false;
    }

    public void TakeHit()
    {
        if (flashing || dead || GameManager.Instance.IsOver) return;
        Lives--;
        if (Lives <= 0)
        {
            dead = true;
            SetCrouch(false);
            anim.SetTrigger(DeadHash);
            GameManager.Instance.GameOver();
        }
        else
        {
            anim.SetTrigger(HitHash);
            StartCoroutine(FlashRoutine());
        }
    }

    IEnumerator FlashRoutine()
    {
        flashing = true;
        float step = invincibleTime / (flashCount * 2);
        for (int i = 0; i < flashCount; i++)
        {
            sr.color = flashColor;
            yield return new WaitForSeconds(step);
            sr.color = Color.white;
            yield return new WaitForSeconds(step);
        }
        flashing = false;
    }

    void OnDrawGizmosSelected()
    {
        if (!groundCheck) return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
    }
}
