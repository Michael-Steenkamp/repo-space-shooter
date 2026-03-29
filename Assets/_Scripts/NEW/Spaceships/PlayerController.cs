using System;
using UnityEngine;
using UnityEngine.InputSystem;

/*
"shieldUnlocked",
"secondaryUnlocked",
"boostUnlocked",
"autoFireUnlocked",

DONE - "maxHp"
"hpRegenDelay"
"hpRegenSpeed"

"secondaryAmmo"
"secondaryDamage"
"secondaryAmmoRegenSpeed"
"secondaryReloadSpeed"

"boostDuration"
"boostDelay"

"shieldDurability"
"shieldRegenDelay"
*/

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour, IDamageable, IRegenerative
{
    static readonly string _logTag = "PlayerController";

    [Header("Scriptable Object")]
    [SerializeField] private Spaceship spaceship;

    [Header("Components")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody2D rb;

    [Header("Properties")]
    [SerializeField] private float maxHealth;
    [SerializeField] private float currentHealth;
    [SerializeField] private Vector2 moveDirection;

    [Header("Weapons")]
    [SerializeField] private PrimaryWeaponLogic primaryWeapon;

    [Header("States")]
    [SerializeField] private bool isAlive = true;
    private bool IsAlive
    {
        get { return isAlive; }
        set
        {
            isAlive = value;
            animator.SetBool("IsAlive", isAlive);
        }
    }
    [SerializeField] private bool isMoving = false;
    private bool IsMoving
    {
        get { return isMoving; }
        set
        {
            isMoving = value;
            //animator.SetBool("IsMoving", isMoving);
        }
    }
    [SerializeField] private bool isAttacking = false;
    private bool IsAttacking
    {
        get { return isAttacking; }
        set
        {
            isAttacking = value;
            //animator.SetBool("IsAttacking", isAttacking);
        }
    }
    [SerializeField] private bool isSprinting = false;
    private bool IsSprinting
    {
        get { return isSprinting; }
        set
        {
            isSprinting = value;
            //animator.SetBool("IsSprinting", isSprinting);
        }
    }

    private InputAction moveAction;
    private InputAction primaryAttackAction;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        //circleCollider = GetComponent<CircleCollider2D>();
    }

    private void Update()
    {
        if (IsAlive)
        {
            RotateToMousePosition();
            HandleAttacking();
            HandleMoving();
        }
    }

    private void HandleMoving()
    {
        if (moveAction.IsPressed())
        {
            IsMoving = true;
            moveDirection = moveAction.ReadValue<Vector2>();
        }

        if (moveAction.WasReleasedThisFrame())
        {
            IsMoving = false;
            moveDirection = Vector2.zero;
        }
    }

    private void HandleAttacking()
    {
        // Primary
        if (primaryAttackAction.WasPressedThisFrame())
        {
            Instantiate(primaryWeapon, transform.position, transform.rotation).Fire(rb.linearVelocity);
        }

        // Secondary
        //if(secondaryAttackAction.WasReleasedThisFrame())
        //{

        //}
    }

    private void FixedUpdate()
    {
        if (IsAlive && IsMoving)
        {
            rb.linearVelocity = Vector2.ClampMagnitude(rb.linearVelocity + (moveDirection * spaceship.Acceleration), spaceship.MaxSpeed);
        }
    }

    public void Initialize(PlayerInput playerInput)
    {
        LogSystem.Instance.Log("Initializing Player Controller... " + playerInput, LogType.Info, _logTag);
        moveAction = playerInput.actions["Move"];
        primaryAttackAction = playerInput.actions["Primary Attack"];

        spriteRenderer.sprite = this.spaceship.Idle;
        animator.runtimeAnimatorController = this.spaceship.AnimatorController;

        maxHealth = this.spaceship.Health * GameDataSystem.currentSave.maxHpMultiplier;
        currentHealth = maxHealth;

        IsAlive = true;
    }

    private void RotateToMousePosition()
    {
        // Get mouse position
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        // Rotate spaceship towards mouse position
        Vector2 direction = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);
        transform.up = direction;
    }

    public void ApplyDamage(float damage)
    {
        AudioSystem.Instance.PlaySfx(spaceship.ACLP_Hit);
        Math.Clamp(currentHealth -= currentHealth, 0, maxHealth);
        if (currentHealth <= 0) { DeathHandler(); }
    }

    public void ApplyHealth(float health)
    {
        Math.Clamp(health += health, 0, maxHealth);
    }

    public void DeathHandler()
    {
        LogSystem.Instance.Log("Death Handler", LogType.Todo, _logTag);
    }
}
