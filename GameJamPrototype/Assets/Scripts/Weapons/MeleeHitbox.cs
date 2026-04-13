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
    private float _damage;
    private LayerMask _targetLayer;

    private GameObject _source;

    private readonly HashSet<Collider2D> _hits = new();

    public void Initialize(float damage, LayerMask targetLayer)
    {
        _damage = damage;
        _targetLayer = targetLayer;

        _hits.Clear();
    }

    // Collision Detection
    void Update()
    {
        var hit = Physics2D.OverlapCircle(hitboxOrigin.position, hitboxRadius, _targetLayer);

        if (!_hits.Add(hit)) return;

        if (hit)
        {
            // * Hit Effect *
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
