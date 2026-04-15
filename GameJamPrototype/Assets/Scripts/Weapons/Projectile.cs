using System;
using UnityEngine;
public class Projectile : MonoBehaviour
{
    // Range
    [SerializeField] private float maxRange = 100f;
    [SerializeField] private ParticleSystem BulletHitEffect;
    private float _distanceTraveled;

    // Stats
    private ProjectileContext _projectileContext;

    public void Initialize(ProjectileContext context)
    {
        _projectileContext = context;
        transform.right = _projectileContext.Direction;
        transform.position = _projectileContext.Origin;
        _distanceTraveled = 0f;
    }

    // Bullet Travel
    void FixedUpdate()
    {
        float distanceThisFrame = _projectileContext.BulletSpeed * Time.fixedDeltaTime;
        Vector2 direction = _projectileContext.Direction.normalized;
        Vector2 startPos = transform.position;

        // Sweep ahead before moving
        RaycastHit2D hitInfo = Physics2D.Raycast(
            startPos,
            direction,
            distanceThisFrame,
            _projectileContext.HitMask
        );

        if (hitInfo.collider != null)
        {
            transform.position = hitInfo.point;
            OnHit(hitInfo);
            return;
        }

        // Move only if nothing was hit
        transform.position = startPos + direction * distanceThisFrame;

        // Track range
        _distanceTraveled += distanceThisFrame;
        if (_distanceTraveled >= maxRange)
        {
            _projectileContext.ObjectPool.Release(gameObject);
        }
    }

    void OnHit(RaycastHit2D hit)
    {
        Instantiate(BulletHitEffect, hit.point, Quaternion.Euler(hit.normal));
        //Debug.Log(hitInfo.collider.name);
        bool success = hit.collider.TryGetComponent(out Damageable target);
        if (success)
        {
            Debug.Log($"Projectile hit {target.name}");
            target.TakeDamage(_projectileContext.Damage);

        }
        _projectileContext.ObjectPool.Release(gameObject);
    }

    public void DebugContext(ProjectileContext ctx)
    {
        Debug.Log($"Origin: {ctx.Origin}");
        Debug.Log($"Direction: {ctx.Direction}");
        Debug.Log($"BulletSpeed: {ctx.BulletSpeed}");
        Debug.Log($"Damage: {ctx.Damage}");
        Debug.Log($"HitMask: {LayerMask.LayerToName((int)Mathf.Log(ctx.HitMask.value, 2))}");
    }
}
