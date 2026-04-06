using UnityEngine;
[RequireComponent(typeof(SpriteRenderer), typeof(Rigidbody2D))]
public class PlayerLimbSegment : MonoBehaviour
{
    private Rigidbody2D _rb;
    private SpriteRenderer _sprite;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _sprite = GetComponent<SpriteRenderer>();
    }

    public void ApplyImpulse(Vector2 v, float t)
    {
        _rb.linearVelocity = v;
        _rb.AddTorque(t, ForceMode2D.Impulse);
    }

    public void SetAlpha(float alpha)
    {
        Color c = _sprite.color;
        c.a = alpha;
        _sprite.color = c;
    }
}
