using UnityEngine;
using UnityEngine.U2D.IK;

[RequireComponent(typeof(Animator))]
public class EnemyAnimator : MonoBehaviour
{
    //[SerializeField] private Transform FrontArm, BackArm;
    [SerializeField] private LimbSolver2D FrontArm, BackArm;
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

    public void AimAtPlayer(Vector3 PlayerPos)
    {
        FrontArm.weight = 1f;
        BackArm.weight = 1f;

        FrontArmTarget.transform.position = PlayerPos;
        BackArmTarget.transform.position = PlayerPos;
    }

    public void DisableArms()
    {
        FrontArm.weight = 0f;
        BackArm.weight = 0f;
    }

    public void EnableArms()
    {
        FrontArm.weight = 1f;
        BackArm.weight = 1f;
    }
}