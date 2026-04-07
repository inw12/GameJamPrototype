using UnityEngine;
public struct PlayerAnimatorParameters
{
    public int  MovementAction;
}
[RequireComponent(typeof(Animator))]
public class PlayerAnimationController : MonoBehaviour
{
    private Animator animator;

    public void Initialize()
    {
        animator = GetComponent<Animator>();
    }

    // Movement Animations
    public void UpdateAnimator(PlayerAnimatorParameters parameters)
    {
        animator.SetInteger("MovementAction", parameters.MovementAction);
    }

    // Arm Weapon Animations
    public void UpdateAnimator(bool weaponEquipped)
    {
        animator.SetBool("WeaponEquipped", weaponEquipped);

        // Arm animation layer management
        var armLayer = animator.GetLayerIndex("Arms");
        var armLayerWeight = animator.GetBool("WeaponEquipped") ? 0f : 1f;
        animator.SetLayerWeight(armLayer, armLayerWeight);
    }
}
