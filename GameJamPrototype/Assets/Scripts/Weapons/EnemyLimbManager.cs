using UnityEngine;

public class EnemyLimbManager : MonoBehaviour
{
    [SerializeField] private GameObject ArmPrefab, LegPrefab;
    [SerializeField] private GameObject FrontArmSprite, BackArmSprite, FrontLegSprite, BackLegSprite;
    [SerializeField] private Transform FrontArmAnchor, BackArmAnchor, FrontLegAnchor, BackLegAnchor;
    [SerializeField] private ParticleSystem BloodFX;
    
    public void RandomDismember()
    {
        int r = Random.Range(0, 4);

        switch (r)
        {
            case 0:
                Dismember(FrontArmSprite, FrontArmAnchor);
                break;
            case 1:
                Dismember(BackArmSprite, BackArmAnchor);
                break;
            case 2:
                Dismember(FrontLegSprite, FrontLegAnchor);
                break;
            case 3:
                Dismember(BackLegSprite, BackLegAnchor);
                break;
        }
    }
    
    private void Dismember(GameObject limb, Transform Anchor)
    {
        limb.SetActive(false);
        Instantiate(BloodFX, limb.transform.position, Quaternion.identity, Anchor);
        var l = Instantiate(ArmPrefab, Anchor.position, Quaternion.identity);
        var rb = l.GetComponent<Rigidbody2D>();
    }
}