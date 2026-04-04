using UnityEngine;
public struct PlayerAnimatorParameters
{
    public int  MovementAction;
    public bool WeaponEquipped;
}
[RequireComponent(typeof(Animator))]
public class PlayerAnimationController : MonoBehaviour
{
    private Animator animator;

    public void Initialize()
    {
        animator = GetComponent<Animator>();
    }

    public void UpdateAnimator(PlayerAnimatorParameters parameters)
    {
        animator.SetInteger("MovementAction", parameters.MovementAction);
        animator.SetBool("WeaponEquipped", parameters.WeaponEquipped);

        // Arm animation layer management
        var layer = animator.GetLayerIndex("Arms");
        var armLayerWeight = animator.GetBool("WeaponEquipped") ? 0f : 1f;
        animator.SetLayerWeight(layer, armLayerWeight);
    }
}
