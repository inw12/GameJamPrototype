using UnityEngine;
public class PlayerDismemberment : MonoBehaviour
{
    [SerializeField] private GameObject frontArm;
    [SerializeField] private GameObject backArm;
    [SerializeField] private GameObject frontLeg;
    [SerializeField] private GameObject backLeg;
    [Space]
    public bool frontArmAttached = true;
    public bool backArmAttached = true;
    public bool frontLegAttached = true;
    public bool backLegAttached = true;

    private bool _frontArmSevered;
    private bool _backArmSevered;
    private bool _frontLegSevered;
    private bool _backLegSevered;

    [Header("Dismemberment Settings")]
    [SerializeField] private GameObject armPrefab;
    [SerializeField] private GameObject legPrefab;
    [Space]
    [SerializeField] private float launchForce      = 10f;
    [SerializeField] private float launchTorque     = 30f;
    [SerializeField] private float jointFlailTorque = 15f;

    void Update()
    {
        frontArm.SetActive(frontArmAttached);
        backArm.SetActive(backArmAttached);
        frontLeg.SetActive(frontLegAttached);
        backLeg.SetActive(backLegAttached);

        if (!_frontArmSevered && !frontArmAttached)
        {
            _frontArmSevered = true;
            var v = Instantiate(armPrefab, frontArm.transform.position, Quaternion.identity);
            if (v.TryGetComponent(out PlayerLimb limb))
            {
                limb.Initialize(launchForce, launchTorque, jointFlailTorque, Random.insideUnitCircle);
            }
        }

        if (!_backArmSevered && !backArmAttached)
        {
            _backArmSevered = true;
            var v = Instantiate(armPrefab, backArm.transform.position, Quaternion.identity);
            if (v.TryGetComponent(out PlayerLimb limb))
            {
                limb.Initialize(launchForce, launchTorque, jointFlailTorque, Random.insideUnitCircle);
            }
        }

        if (!_frontLegSevered && !frontLegAttached)
        {
            _frontLegSevered = true;
            var v = Instantiate(legPrefab, frontLeg.transform.position, Quaternion.identity);
            if (v.TryGetComponent(out PlayerLimb limb))
            {
                limb.Initialize(launchForce, launchTorque, jointFlailTorque, Random.insideUnitCircle);
            }
        }

        if (!_backLegSevered && !backLegAttached)
        {
            _backLegSevered = true;
            var v = Instantiate(legPrefab, backLeg.transform.position, Quaternion.identity);
            if (v.TryGetComponent(out PlayerLimb limb))
            {
                limb.Initialize(launchForce, launchTorque, jointFlailTorque, Random.insideUnitCircle);
            }
        }
    }
}
