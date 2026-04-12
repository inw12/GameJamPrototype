using UnityEngine;
public struct PlayerAnimatorParameters
{
    public int  MovementAction;
    public bool IsGrounded;
    public float IsMovingForward;   // -1: False | 1: True
}
[RequireComponent(typeof(Animator))]
public class PlayerAnimationController : MonoBehaviour
{
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Movement Animations
    public void UpdateAnimator(PlayerAnimatorParameters parameters)
    {
        animator.SetInteger("MovementAction", parameters.MovementAction);
        animator.SetBool("IsGrounded", parameters.IsGrounded);
        animator.SetFloat("IsMovingForward", parameters.IsMovingForward);
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
