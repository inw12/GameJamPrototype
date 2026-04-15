using System.Collections.Generic;
using UnityEngine;
public class MeleeHitbox : MonoBehaviour
{
    [SerializeField] private Transform hitboxOrigin;
    [SerializeField] private float hitboxRadius;
    [Space]
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Color activeColor;
    [SerializeField] private Color inactiveColor;

    // stats
    private float _damage, _knockback, _knockbackUpward;
    private LayerMask _targetLayer;

    private readonly HashSet<Collider2D> _hits = new();

    public void Initialize(float damage, float knockback, float knockbackUpward, LayerMask targetLayer)
    {
        _damage = damage;
        _knockback = knockback;
        _knockbackUpward = knockbackUpward;
        _targetLayer = targetLayer;

        _hits.Clear();
    }

    public void Initialize(float damage, LayerMask targetLayer, Color effectColor)
    {
        _damage = damage;
        _targetLayer = targetLayer;
        activeColor = effectColor;

        _hits.Clear();
    }

    void Start()
    {
        AudioManager.Instance.PlaySFXAt("MeleeSwipe", transform.position);
    }

    // Collision Detection
    void Update()
    {
        var hit = Physics2D.OverlapCircle(hitboxOrigin.position, hitboxRadius, _targetLayer);

        if (!_hits.Add(hit)) return;

        if (hit)
        {
            bool canDamage = hit.TryGetComponent(out Damageable hittable);
            bool canKb = hit.TryGetComponent(out IKnockable enemyAI);
            AudioManager.Instance.PlaySFXAt("MeleeHit", transform.position);

            if (canDamage)
            {
                hittable.TakeDamage(_damage);
            }

            if (canKb)
            {
                enemyAI.ApplyForce(_knockback, _knockbackUpward);
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(hitboxOrigin.position, hitboxRadius);
    }

    public void SetActive() => sprite.color = activeColor;
    public void SetInactive() => sprite.color = inactiveColor;
    public void OnDestroy() => Destroy(gameObject);
}
