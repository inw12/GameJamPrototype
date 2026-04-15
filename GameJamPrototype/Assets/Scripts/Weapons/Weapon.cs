using System;
using UnityEngine;
using TMPro;

#region *-- Weapon Classes ------------------------------*
// * Base Class
public abstract class Weapon : MonoBehaviour
{
    [Header("Sprites")]
    [SerializeField] protected SpriteRenderer weaponSpriteRenderer;
    [SerializeField] protected Sprite[] sprites;    // should hold only 2 sprites: red for player, green for enemy

    [Header("Stats")]
    public bool EquippedByPlayer;
    protected LayerMask targetLayer;
    private Rigidbody2D rigidbody;
    private PolygonCollider2D collider;

    [Header("UI")]
    [SerializeField] private TextMeshPro TextPrefab;
    [SerializeField] private float HeightOffset = 5f;
    private TextMeshPro text;
    private float highlightTimer = 0f;

    // Events
    public event Action OnAttack;
    protected void AttackEvent() { OnAttack?.Invoke(); }

    // Lifecycle
    protected virtual void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<PolygonCollider2D>();

        text = Instantiate(TextPrefab);
        text.gameObject.SetActive(false);

        UpdateSprites();
    }

    protected virtual void Update()
    {
        text.transform.position = transform.position + Vector3.up * HeightOffset;

        highlightTimer -= Time.deltaTime;
        if (highlightTimer < 0 && text.gameObject.activeSelf)
        {
            text.gameObject.SetActive(false);
        }
    }

    private void LateUpdate()
    {
        text.transform.rotation = Quaternion.identity;
    }

    // Methods
    public virtual void Attack() { }
    public static Weapon Equip(Weapon weapon, Transform WeaponHandle, bool isPlayer = false)
    {
        if (!Helper.IsInstance(weapon))
            weapon = Instantiate(weapon);

        weapon.targetLayer = isPlayer ? LayerMask.GetMask("Enemy") : LayerMask.GetMask("PlayerHurtbox");
        weapon.EquippedByPlayer = isPlayer;

        weapon.transform.SetParent(WeaponHandle);
        weapon.transform.localPosition = Vector3.zero;
        weapon.transform.localRotation = Quaternion.identity;

        weapon.UpdateSprites();

        // Poses reversed for enemies
        weapon.transform.localScale = new Vector3(1f, 1f, 1f);
        if (!isPlayer)
            weapon.transform.localScale = new Vector3(1f, -1f, 1f);

        weapon.GetComponent<PolygonCollider2D>().enabled = false;
        weapon.GetComponent<Rigidbody2D>().simulated = false;

        return weapon;
    }

    public void UpdateSprites()
    {
        weaponSpriteRenderer.sprite = EquippedByPlayer ? sprites[0] : sprites[1];
    }

    public void Unequip()
    {
        transform.SetParent(null);
        rigidbody.simulated = true;
        collider.enabled = true;
    }

    public void OnPlayerNearby()
    {
        highlightTimer = 0.1f;
        text.gameObject.SetActive(true);
    }

    void OnDestroy()
    {
        Destroy(text);
    }
}

// * Ranged Weapons
[RequireComponent(typeof(ProjectilePool))]
public class RangedWeapon : Weapon
{
    [Header("Basic Stats")]
    [SerializeField] protected float damage;
    [SerializeField] protected float fireRate;
    [SerializeField] protected float maxAmmo;
    [SerializeField] protected float bulletSpeed;
    [SerializeField] protected bool isFullAuto;
    [SerializeField] protected bool isTwoHanded;
    [SerializeField] protected Transform gunTip;
    protected ProjectilePool _pool;
    protected float _fireTimer;

    protected override void Start()
    {
        base.Start();
        _pool = GetComponent<ProjectilePool>();
        _fireTimer = fireRate;
    }

    public override void Attack()
    {
        if (_fireTimer >= fireRate)
        {
            var projectile = new ProjectileContext
            {
                ObjectPool = _pool,
                Origin = GunTip.position,
                Direction = Mathf.Sign(transform.root.localScale.x) * GunTip.right,
                BulletSpeed = bulletSpeed,
                HitMask = targetLayer,
                Damage = damage
            };
            _pool.Get(projectile);

            AttackEvent();

            _fireTimer = 0f;
        }
    }

    protected override void Update()
    {
        base.Update();
        _fireTimer += Time.deltaTime;
    }
    public Transform GunTip => gunTip;
}

// * Melee Weapons
public class MeleeWeapon : Weapon
{
    [Header("Basic Stats")]
    [SerializeField] protected float damage;
    [SerializeField] protected float attackSpeed;
    protected float _attackTimer;
}
#endregion

#region *-- Weapon Attack Context Structs ---------------*
public struct ProjectileContext
{
    public ProjectilePool ObjectPool;
    public Vector2 Origin;
    public Vector2 Direction;
    public float BulletSpeed;
    public LayerMask HitMask;
    public float Damage;
}
public struct MeleeAttackContext
{

}
#endregion