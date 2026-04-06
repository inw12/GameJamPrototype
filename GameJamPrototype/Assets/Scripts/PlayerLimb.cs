using UnityEngine;
using System.Collections;
public class PlayerLimb : MonoBehaviour
{
    [Header("Limb Segments")]
    [SerializeField] private PlayerLimbSegment upperSegment;
    [SerializeField] private PlayerLimbSegment lowerSegment;

    [Header("Launch Settings")]
    private float _launchForce;
    private float _launchTorque;
    private float _jointFlailTorque;

    [Header("Despawn Settings")]
    [SerializeField] private float timeToDespawn    = 2f;
    [SerializeField] private float despawnDuration  = 1f;

    public void Initialize(float force, float torque, float jointTorque, Vector2 launchDirection)
    {
        _launchForce = force;
        _launchTorque = torque;
        _jointFlailTorque = jointTorque;

        upperSegment.Initialize();
        lowerSegment.Initialize();

        var targetVelocity = _launchForce * launchDirection.normalized;
        upperSegment.ApplyImpulse(targetVelocity, _launchTorque);
        lowerSegment.ApplyImpulse(targetVelocity, -_jointFlailTorque);
    }
}
