using System.Text;
using UnityEngine;

public class PlayerLimbManager : MonoBehaviour
{
    [SerializeField] private Limb FrontArm, BackArm, FrontLeg, BackLeg;
    public enum ArmState { BothArms, OneArm, NoArms }
    public enum LegState { BothLegs, OneLeg, NoLegs }
    public ArmState CurrentArmState { private set; get; }
    public LegState CurrentLegState { private set; get; }

    void Update()
    {
        // Arms
        if (FrontArm.Dismembered && BackArm.Dismembered)
        {
            CurrentArmState = ArmState.NoArms;
        }
        else if (FrontArm.Dismembered || BackArm.Dismembered)
        {
            CurrentArmState = ArmState.OneArm;
        }
        else
        {
            CurrentArmState = ArmState.BothArms;
        }

        // Legs
        if (FrontArm.Dismembered && BackArm.Dismembered)
        {
            CurrentLegState = LegState.NoLegs;
        }
        else if (FrontArm.Dismembered || BackArm.Dismembered)
        {
            CurrentLegState = LegState.OneLeg;
        }
        else
        {
            CurrentLegState = LegState.BothLegs;
        }
    }

    void OnGUI()
    {
        StringBuilder sb = new();

    }
}