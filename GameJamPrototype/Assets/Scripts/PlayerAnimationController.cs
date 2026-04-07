using UnityEngine;
public struct PlayerAnimatorParameters
{
    public int  MovementAction;
}
[RequireComponent(typeof(Animator))]
public class PlayerAnimationController : MonoBehaviour
{
    public bool weaponEquipped;
    private Animator animator;

    public void Initialize()
    {
        animator = GetComponent<Animator>();
    }

    public void UpdateAnimator(PlayerAnimatorParameters parameters)
    {
        animator.SetInteger("MovementAction", parameters.MovementAction);
        animator.SetBool("WeaponEquipped", weaponEquipped);

        // Arm animation layer management
        var armLayer = animator.GetLayerIndex("Arms");
        var pistolArmLayer = animator.GetLayerIndex("PistolArms");

        var armLayerWeight = animator.GetBool("WeaponEquipped") ? 0f : 1f;
        var pistolLayerWeight = animator.GetBool("WeaponEquipped") ? 1f : 0f;

        animator.SetLayerWeight(armLayer, armLayerWeight);
        animator.SetLayerWeight(pistolArmLayer, pistolLayerWeight);
    }
}
