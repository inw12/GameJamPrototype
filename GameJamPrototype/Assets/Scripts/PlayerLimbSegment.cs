using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerLimbSegment : MonoBehaviour
{
    private Rigidbody2D _rb;

    public void Initialize()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    public void ApplyImpulse(Vector2 v, float t)
    {
        _rb.linearVelocity = v;
        _rb.AddTorque(t, ForceMode2D.Impulse);
    }
}
