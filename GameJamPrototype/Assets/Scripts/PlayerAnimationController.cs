using UnityEngine;
public struct PlayerAnimatorParameters
{
    public int MovementAction;
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
    }
}
