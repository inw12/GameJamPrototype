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
        if (Physics.Raycast(transform.position, _projectileContext.Direction, out RaycastHit hitInfo, distanceThisFrame, _projectileContext.HitMask))
        {
            /// *************************************
            /// *** Collision Implementation Here ***
            /// *************************************
            return;
        }
    }
}
