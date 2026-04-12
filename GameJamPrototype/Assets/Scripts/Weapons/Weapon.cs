using UnityEngine;

#region *-- Weapon Classes ------------------------------*
// * Base Class
public abstract class Weapon : MonoBehaviour
{
    [SerializeField] protected LayerMask targetLayer;

    //public virtual void Attack() {}
    public virtual void Attack() {}
}

// * Ranged Weapons
public class RangedWeapon : Weapon
{
    [Header("Basic Stats")]
    [SerializeField] protected float damage;
    [SerializeField] protected float fireRate;
    [SerializeField] protected float maxAmmo;
    [SerializeField] protected float bulletSpeed;
    [SerializeField] protected bool isFullAuto;
    [SerializeField] protected bool isTwoHanded;
    protected ProjectilePool _pool;
    protected float _fireTimer;
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
    public ProjectilePool   ObjectPool;
    public Vector2          Origin;
    public Vector2          Direction;
    public float            BulletSpeed;
    public LayerMask        HitMask;
    public float            Damage;
}
public struct MeleeAttackContext
{
    
}
#endregion