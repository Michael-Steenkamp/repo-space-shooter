using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Animator))]
public class DynamicEnemyLogic : MonoBehaviour, IDamageable
{
    protected readonly string _logTag;

    [Header("Scriptable Object")]
    [SerializeField] private DynamicEnemy dynamicEnemy;

    [Header("Components")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private new Collider2D collider;
    [SerializeField] private Animator animator;

    [Header("Properties")]
    [SerializeField] private float maxHealth;
    [SerializeField] private float currentHealth;
    [SerializeField] private Vector2 moveDirection;
    [SerializeField] private GameObject target;

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

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();

        spriteRenderer.sprite = dynamicEnemy.Sprite;
        animator.runtimeAnimatorController = dynamicEnemy.AnimatorController;

        maxHealth = dynamicEnemy.Health;
        currentHealth = maxHealth;
    }

    public void SetTarget(GameObject target)
    {
        this.target = target;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<WeaponLogic>(out WeaponLogic weapon))
        {
            ApplyDamage(weapon.GetDamage());
            Destroy(collision.gameObject);
        }
    }

    Vector3 direction;
    private void Update()
    {
        if (isAlive)
        {
            direction = (target.transform.position - transform.position).normalized;
            transform.position += direction * dynamicEnemy.MaxSpeed * Time.deltaTime;
        }
    }

    public void ApplyDamage(float damage)
    {
        AudioSystem.Instance.PlaySfx(dynamicEnemy.ACLP_Hit);
        currentHealth -= damage;
        Debug.Log("Health: " + currentHealth);
        if (currentHealth <= 0) { DeathHandler(); }
    }

    public void DeathHandler()
    {
        AudioSystem.Instance.PlaySfx(dynamicEnemy.ACLP_Death);
        isAlive = false;
        Destroy(gameObject);
    }
}
