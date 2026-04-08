using UnityEngine;

public class Limb : MonoBehaviour
{
    [SerializeField] private GameObject LimbPrefab;
    [SerializeField] private Transform Anchor;
    [SerializeField] private Damageable DamageService;
    public bool Dismembered = false;

    void Start()
    {
        DamageService.OnDeath += Dismember;
    }

    private void Dismember()
    {
        Dismembered = true;
        gameObject.SetActive(false);
        Instantiate(LimbPrefab, Anchor.position, Quaternion.identity);
    }

    private void Regenerate()
    {
        gameObject.SetActive(true);

        // Regrow logic
    }
}