using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private Animator Animator;

    // Movement Animations
    public void UpdateLocAnim(MovementState MoveState)
    {
        Animator.SetInteger("MovementAction", (int)MoveState);
    }

    // Arm Weapon Animations
    public void UpdateWeaponAnim(bool weaponEquipped)
    {
        Animator.SetBool("WeaponEquipped", weaponEquipped);

        // Arm animation layer management
        var armLayer = Animator.GetLayerIndex("Arms");
        var armLayerWeight = Animator.GetBool("WeaponEquipped") ? 0f : 1f;
        Animator.SetLayerWeight(armLayer, armLayerWeight);
    }
}
