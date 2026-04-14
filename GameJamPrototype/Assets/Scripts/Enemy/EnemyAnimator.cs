using UnityEngine;
using UnityEngine.U2D.IK;

[RequireComponent(typeof(Animator))]
public class EnemyAnimator : MonoBehaviour
{
    //[SerializeField] private Transform FrontArm, BackArm;
    [SerializeField] private LimbSolver2D FrontArm, BackArm;
    [SerializeField] private Vector2 RestingArmPosition;
    private Transform FrontArmTarget, BackArmTarget;
    private Animator Animator;

    void Start()
    {
        Animator = GetComponent<Animator>();
        FrontArmTarget = FrontArm.GetChain(0).target;
        BackArmTarget = BackArm.GetChain(0).target;
    }

    public void UpdateMoveAnim(bool isMoving)
    {
        Animator.SetBool("IsMoving", isMoving);
    }

    public void TriggerMelee()
    {
        Animator.SetTrigger("MeleeTrigger");
    }

    public void TriggerDeath()
    {
        Animator.SetBool("IsDead", true);
    }

    public void AimAtPlayer(Vector3 PlayerPos)
    {
        Debug.DrawLine(PlayerPos, transform.position);

        FrontArm.transform.position = PlayerPos;
        BackArm.transform.position = PlayerPos;
    }

    public void DisableArms()
    {
        Debug.Log("Hello?");
        FrontArm.transform.position = RestingArmPosition;
        BackArm.transform.position = RestingArmPosition;
    }

    public void EnableArms()
    {
    }
}