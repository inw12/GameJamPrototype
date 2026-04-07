using UnityEngine;
[RequireComponent(typeof(ProjectilePool))]
public class Pistol : RangedWeapon
{
    [SerializeField] private Transform bulletSpawn;

    public void Initialize()
    {
        _pool = GetComponent<ProjectilePool>();
        _fireTimer = fireRate;
    }

    public override void Attack(Vector2 direction)
    {
        if (_fireTimer >= fireRate)
        {
            var projectile = new ProjectileContext
            {
                ObjectPool  = _pool,
                Origin      = bulletSpawn.position,
                Direction   = direction,
                BulletSpeed = bulletSpeed,
                HitMask     = targetLayer                
            };
            _pool.Get(projectile);

            _fireTimer = 0f;
        }
    }
}
