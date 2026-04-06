using UnityEngine;
using System.Collections;
public class PlayerLimb : MonoBehaviour
{
    [Header("Limb Segments")]
    [SerializeField] private PlayerLimbSegment upperSegment;
    [SerializeField] private PlayerLimbSegment lowerSegment;

    [Header("Launch Settings")]
    [SerializeField] private float launchForce      = 5f;
    [SerializeField] private float launchTorque     = 10f;
    [SerializeField] private float jointFlailTorque = 5f;

    [Header("Despawn Settings")]
    [SerializeField] private float timeToDespawn    = 2f;
    [SerializeField] private float despawnDuration  = 1f;

    public void Launch(Vector2 direction, float speedMultiplier = 1f)
    {
        var targetVelocity = launchForce * speedMultiplier * direction.normalized;

        upperSegment.ApplyImpulse(targetVelocity, launchTorque);
        lowerSegment.ApplyImpulse(targetVelocity, -jointFlailTorque);

        StartCoroutine(Despawn());
    }

    private IEnumerator Despawn()
    {
        yield return new WaitForSeconds(timeToDespawn);

        float elapsed = 0f;
        while (elapsed < despawnDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsed / despawnDuration);
            upperSegment.SetAlpha(alpha);
            lowerSegment.SetAlpha(alpha);
            elapsed += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}
