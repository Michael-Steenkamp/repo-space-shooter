using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

/*
"primaryAmmo"
"primaryDamage"
"primaryAmmoRegenSpeed"
"primaryReloadSpeed"
*/

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
public class PrimaryWeaponLogic : WeaponLogic
{
    protected readonly string _logTag;

    [Header("Scriptable Object")]
    [SerializeField] private Weapon primaryWeapon;

    [Header("Components")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private CapsuleCollider2D capsuleCollider;
    [SerializeField] private Rigidbody2D rb;

    public override float damage { get; set; }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();

        spriteRenderer.sprite = primaryWeapon.Sprite;
        damage = primaryWeapon.Damage * GameDataSystem.currentSave.primaryDamageMultiplier;
    }

    private void Start()
    {
        AudioSystem.Instance.PlaySfx(primaryWeapon.ACLP_Fire);
        StartCoroutine(DestroyAfterSetTime());
    }

    private IEnumerator DestroyAfterSetTime()
    {
        yield return new WaitForSeconds(primaryWeapon.Lifetime);
        Destroy(gameObject);
    }

    public void Fire(Vector2 vel)
    {
        rb.linearVelocity = vel;
        rb.linearVelocity += new Vector2(transform.up.x * primaryWeapon.Speed, transform.up.y * primaryWeapon.Speed);
    }
}
