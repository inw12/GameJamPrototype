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
    public ArmState CurrentArmState { private set; get; }
    public LegState CurrentLegState { private set; get; }

    void Update()
    {
        UpdateLimbStates();
    }

    void UpdateLimbStates()
    {
        CanBlock = CanShoot = !FrontArm.Dismembered;
        CanMelee = !BackArm.Dismembered;

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
    }


    void OnGUI()
    {
        StringBuilder sb = new();

    }
}