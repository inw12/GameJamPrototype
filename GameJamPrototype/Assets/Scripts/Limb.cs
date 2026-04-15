using UnityEngine;

public class Limb : MonoBehaviour
{
    [SerializeField] private GameObject LimbPrefab;
    [SerializeField] private Transform Anchor;
    public Damageable LimbHitboxUpper, LimbHitboxLower;   // Lower has no utility other than being an extension of the limb.
    [SerializeField] private ParticleSystem bloodParticles;
    public bool Dismembered => isDismembered;
    private bool isDismembered = false;

    void Awake()
    {
        LimbHitboxUpper.OnDeath += Dismember;
        LimbHitboxUpper.Regenerate += Regenerate;
        LimbHitboxLower.OnDeath += Dismember;
        LimbHitboxLower.Regenerate += Regenerate;
    }

    private void Dismember()
    {
        LimbHitboxUpper.GetComponent<Collider2D>().enabled = !LimbHitboxUpper.IsDead;
        LimbHitboxLower.GetComponent<Collider2D>().enabled = !LimbHitboxLower.IsDead;
        if (LimbHitboxUpper.IsDead && LimbHitboxLower.IsDead)
        {
            bloodParticles.Play();
            AudioManager.Instance.PlaySFXAt("Dismemberment", transform.position);
            TurnOffLimb();
            Instantiate(LimbPrefab, Anchor.position, Quaternion.identity);
        }
    }

    private void TurnOffLimb()
    {
        isDismembered = true;
        gameObject.SetActive(false);
    }

    private void TurnOnLimb()
    {
        isDismembered = false;
        gameObject.SetActive(true);
        LimbHitboxUpper.GetComponent<Collider2D>().enabled = true;
        LimbHitboxLower.GetComponent<Collider2D>().enabled = true;
    }

    private void Regenerate()
    {
        bloodParticles.Play();
        TurnOnLimb();
    }
}