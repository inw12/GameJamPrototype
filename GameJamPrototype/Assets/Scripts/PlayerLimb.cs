using UnityEngine;
public class PlayerLimb : MonoBehaviour
{
    [Header("Limb Segments")]
    [SerializeField] private PlayerLimbSegment upperSegment;
    [SerializeField] private PlayerLimbSegment lowerSegment;

    [Header("Launch Settings")]
    [SerializeField] private float _launchForce = 5f;
    [SerializeField] private float _launchTorque = 25f;
    [SerializeField] private float _jointFlailTorque = 10f;

    [Header("Despawn Settings")]
    [SerializeField] private float timeToDespawn = 2f;
    [SerializeField] private float despawnDuration = 1f;

    public void Awake()
    {
        var targetVelocity = _launchForce * Random.insideUnitCircle;
        if (upperSegment != null)
        {
            upperSegment.ApplyImpulse(targetVelocity, _launchTorque);
            upperSegment.Initialize();
        }

        if (lowerSegment != null)
        {
            lowerSegment.Initialize();
            lowerSegment.ApplyImpulse(targetVelocity, -_jointFlailTorque);
        }
    }
}
