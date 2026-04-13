using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EnemyAnimator : MonoBehaviour
{
    private Animator Animator;

    void Start()
    {
        Animator = GetComponent<Animator>();
    }

    public void UpdateAnimatorParams()
    {

    }

    public void AimAtPlayer()
    {

    }
}