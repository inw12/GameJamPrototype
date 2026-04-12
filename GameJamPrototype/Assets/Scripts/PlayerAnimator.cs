using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private Animator Animator;

    public void UpdateGroundAnim(bool IsGrounded, MovementState MoveState)
    {
        Animator.SetBool("IsGrounded", IsGrounded);
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
