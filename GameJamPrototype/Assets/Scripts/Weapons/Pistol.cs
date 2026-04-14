using UnityEngine;
[RequireComponent(typeof(ProjectilePool))]
public class Pistol : RangedWeapon
{
    protected override void Start()
    {
        base.Start();
        _pool = GetComponent<ProjectilePool>();
        _fireTimer = fireRate;
    }

    void Update()
    {
        _fireTimer += Time.deltaTime;
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
            audioSource.PlayOneShot(audioSource.clip);

            _fireTimer = 0f;
        }
    }
}
