using UnityEngine;
public abstract class Weapon : ScriptableObject
{
    [Header("Stats")]
    public float damage;
    public float fireRate;
    public float maxAmmo;
    public bool isFullAuto;

    // stat management
    protected float _fireTimer;

    public abstract void Attack(WeaponAttackContext context);
}
public class WeaponAttackContext
{
    public Vector2      Origin;
    public Vector2      Direction;
    public LayerMask    HitMask;
    public bool         FacingRight;
}