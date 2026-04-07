using UnityEngine;
[RequireComponent(typeof(ProjectilePool))]
public class Pistol : RangedWeapon
{
    [SerializeField] private Transform bulletSpawn;

    void Start()
    {
        _pool = GetComponent<ProjectilePool>();
        _fireTimer = fireRate;
    }

    void Update()
    {
        _fireTimer += Time.deltaTime;
    }

    public override void Attack(Vector2 mousePos)
    {
        if (_fireTimer >= fireRate)
        {
            var direction = ((Vector3)mousePos - bulletSpawn.position).normalized;
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
