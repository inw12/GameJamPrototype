using UnityEngine;
public class Projectile : MonoBehaviour
{
    // Range
    [SerializeField] private float maxRange = 100f;
    private float _distanceThisFrame;
    private float _distanceTraveled;

    // Stats
    private WeaponAttackContext _context;
    private float _speed;

    // Object Pool
    private ProjectilePool _pool;

    public void Initialize(WeaponAttackContext context, float bulletSpeed, ProjectilePool pool)
    {
        _context = context;
        _speed = bulletSpeed;
        _pool = pool;
    }

    // Bullet Travel
    void FixedUpdate()
    {
        // Update distance to travel this frame
        _distanceThisFrame = _speed * Time.fixedDeltaTime;

        // Travel forward
        transform.position += (Vector3)(_context.Direction * _distanceThisFrame);
        
        // Return to object pool after travelling a certain distance;
        _distanceTraveled += _distanceThisFrame;
        if (_distanceTraveled >= maxRange)
        {
            _pool.Release(gameObject);
        }
    }

    // Collision Detection
    void Update()
    {
        var distanceThisFrame = _speed * Time.deltaTime;
        if (Physics.Raycast(transform.position, _context.Direction, out RaycastHit hitInfo, distanceThisFrame, _context.HitMask))
        {
            /// *************************************
            /// *** Collision Implementation Here ***
            /// *************************************
            return;
        }
    }
}
