using UnityEngine;
using UnityEngine.Pool;
public class ProjectilePool : MonoBehaviour
{
    [SerializeField] private GameObject projectile;
    [Space]
    [SerializeField] private int defaultCapacity = 25;
    [SerializeField] private int maxCapacity = 100;

    private ObjectPool<GameObject> _pool;

    void Awake()
    {
        // Initialize Pool
        _pool = new ObjectPool<GameObject>
        (
            CreateProjectile,
            OnGetProjectile,
            OnReleaseProjectile,
            OnDestroyProjectile,
            true,
            defaultCapacity,
            maxCapacity
        );
    }

    private GameObject CreateProjectile()
    {
        var p = Instantiate(projectile, transform);
        return p;
    }

    private void OnGetProjectile(GameObject item)
    {
        item.SetActive(true);
    }

    private void OnReleaseProjectile(GameObject item)
    {
        item.SetActive(false);
    }

    private void OnDestroyProjectile(GameObject item) => Destroy(item);

    public void Get(WeaponAttackContext context)
    {
        GameObject item = _pool.Get();
        //if (item.TryGetComponent(out Projectile p))
        //{
        //    p.Initialize(this, context, spawn);
        //}
    }
    
    public void Release(GameObject item) => _pool.Release(item);
}
