using UnityEngine;
public struct PlayerAnimatorParameters
{
    public int  MovementAction;
    public bool IsGrounded;
    public float  IsMovingForward;
}
public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private Animator Animator;

    public void UpdateGroundAnim(PlayerAnimatorParameters p)
    {
        Animator.SetInteger("MovementAction", p.MovementAction);
        Animator.SetBool("IsGrounded", p.IsGrounded);
        Animator.SetFloat("IsMovingForward", p.IsMovingForward);
    }
}
