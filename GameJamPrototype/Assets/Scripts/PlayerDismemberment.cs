using UnityEngine;
public class PlayerDismemberment : MonoBehaviour
{
    [SerializeField] private GameObject[] bodyParts;
    /// [0]: Back Arm
    /// [1]: Front Arm
    /// [2]: Back Leg
    /// [3]: Front Leg

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
    }
}
