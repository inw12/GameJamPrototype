using System.Text;
using UnityEngine;

public class PlayerLimbManager : MonoBehaviour
{
    [SerializeField] private Limb FrontArm, BackArm, FrontLeg, BackLeg;
    public enum ArmState { BothArms, OneArm, NoArms }
    public enum LegState { BothLegs, OneLeg, NoLegs }
    public bool CanBlock { private set; get; }
    public bool CanShoot { private set; get; }
    public bool CanMelee { private set; get; }
    public bool FullyDismembered { private set; get; }
    public ArmState CurrentArmState { private set; get; }
    public LegState CurrentLegState { private set; get; }

    // References
    private CircleCollider2D circle;
    private CapsuleCollider2D collider;
    private Rigidbody2D Rb;

    void Start()
    {
        FullyDismembered = false;
        circle = GetComponent<CircleCollider2D>();
        collider = GetComponent<CapsuleCollider2D>();
        Rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!FullyDismembered)
            UpdateLimbStates();
    }

    void UpdateLimbStates()
    {
        CanBlock = CanShoot = !BackArm.Dismembered;
        CanMelee = !FrontArm.Dismembered;

        // Arms
        if (FrontArm.Dismembered && BackArm.Dismembered)
        {
            CurrentArmState = ArmState.NoArms;
        }
        else if (FrontArm.Dismembered || BackArm.Dismembered)
        {
            CurrentArmState = ArmState.OneArm;
        }
        else if (!FrontArm.Dismembered && !BackArm.Dismembered)
        {
            CurrentArmState = ArmState.BothArms;
        }

        // Legs
        if (FrontLeg.Dismembered && BackLeg.Dismembered)
        {
            CurrentLegState = LegState.NoLegs;
        }
        else if (FrontLeg.Dismembered || BackLeg.Dismembered)
        {
            CurrentLegState = LegState.OneLeg;
        }
        else if (!FrontLeg.Dismembered && !BackLeg.Dismembered)
        {
            CurrentLegState = LegState.BothLegs;
        }

        if (CurrentArmState == ArmState.NoArms && CurrentLegState == LegState.NoLegs)
        {
            collider.enabled = false;
            circle.enabled = true;
            Rb.constraints = RigidbodyConstraints2D.None;
            Rb.AddTorque(50f);
            FullyDismembered = true;
        }
    }
}