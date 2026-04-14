using UnityEngine;

public class Limb : MonoBehaviour
{
    [SerializeField] private GameObject LimbPrefab;
    [SerializeField] private Transform Anchor;
    [SerializeField] private Damageable DamageService;
    [SerializeField] private ParticleSystem bloodParticles;
    public bool Dismembered = false;

    void Awake()
    {
        DamageService.OnDeath += Dismember;
        DamageService.Regenerate += Regenerate;
    }

    private void Dismember()
    {
        bloodParticles.Play();
        Dismembered = true;
        gameObject.SetActive(false);
        Instantiate(LimbPrefab, Anchor.position, Quaternion.identity);
    }

    private void Regenerate()
    {
        Dismembered = false;
        gameObject.SetActive(true);

        // blood effect here
        bloodParticles.Play();
    }
}