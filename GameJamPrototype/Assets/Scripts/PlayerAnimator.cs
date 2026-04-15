using UnityEngine;
public struct PlayerAnimatorParameters
{
    public int MovementAction;
    public bool IsGrounded;
    public float  IsMovingForward;
}
public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private Animator Animator;
    public void TriggerRanged()
    {
        Animator.SetTrigger("RangeTrigger");
    }

    public void TriggerPickup()
    {
        Animator.SetTrigger("PickupTrigger");
    }

    public void UpdateGroundAnim(PlayerAnimatorParameters p)
    {
        Animator.SetFloat("MovementAction", p.MovementAction);
        Animator.SetBool("IsGrounded", p.IsGrounded);
        Animator.SetFloat("IsMovingForward", p.IsMovingForward);
    }
}
