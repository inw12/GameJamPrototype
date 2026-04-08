using UnityEngine;
public class Projectile : MonoBehaviour
{
    // Range
    [SerializeField] private float maxRange = 100f;
    private float _distanceThisFrame;
    private float _distanceTraveled;

    // Stats
    private ProjectileContext _projectileContext;

    public void Initialize(ProjectileContext context)
    {
        _projectileContext = context;
        transform.position = _projectileContext.Origin;
        _distanceTraveled = 0f;
    }

    // Bullet Travel
    void FixedUpdate()
    {
        // movement
        _distanceThisFrame = _projectileContext.BulletSpeed * Time.fixedDeltaTime;
        transform.position += (Vector3)(_projectileContext.Direction * _distanceThisFrame);

        // return to object pool after traveling max distance
        _distanceTraveled += _distanceThisFrame;
        if (_distanceTraveled >= maxRange)
        {
            _projectileContext.ObjectPool.Release(gameObject);
        }
    }

    // Collision Detection
    void Update()
    {
        var distanceThisFrame = _projectileContext.BulletSpeed * Time.deltaTime;

        //DebugContext(_projectileContext);

        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, _projectileContext.Direction, distanceThisFrame, _projectileContext.HitMask);
        if (hitInfo.collider != null)
        {
            //Debug.Log(hitInfo.collider.name);
            bool success = hitInfo.collider.TryGetComponent(out Damageable target);

            if (success)
            {
                Debug.Log($"Projectile hit {target.name}");
                target.TakeDamage(_projectileContext.Damage);

                _projectileContext.ObjectPool.Release(gameObject);
            }
        }
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
