using UnityEngine;
public class PlayerDismemberment : MonoBehaviour
{
    [SerializeField] private GameObject[] bodyParts;
    /// [0]: Back Arm
    /// [1]: Front Arm
    /// [2]: Back Leg
    /// [3]: Front Leg

    [Header("Limb Launch Settings")]
    [SerializeField] private GameObject arm;
    [SerializeField] private Transform armSpawn;
    [SerializeField] private float launchForce      = 10f;
    [SerializeField] private float launchTorque     = 30f;
    [SerializeField] private float jointFlailTorque = 15f;
    private bool _armLaunched;

    [Space]
    public bool backArm;
    public bool frontArm;
    public bool backLeg;
    public bool frontLeg;

    void Update()
    {
        bodyParts[0].SetActive(!backArm);
        bodyParts[1].SetActive(!frontArm);
        bodyParts[2].SetActive(!backLeg);
        bodyParts[3].SetActive(!frontLeg);

        if (!_armLaunched && (backArm || frontArm))
        {
            _armLaunched = true;
            var p = Instantiate(arm, armSpawn.position, Quaternion.identity);
            if (p.TryGetComponent(out PlayerLimb limb))
            {
                limb.Initialize(launchForce, launchTorque, jointFlailTorque, Random.insideUnitCircle * 0.2f);
            }
        }
    }
}
