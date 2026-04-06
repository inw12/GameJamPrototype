using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Transform player;
    private enum BTState { Success, Failure, Running }
    [SerializeField] float detectionRange = 5f;
    [SerializeField] float attackRange = 1.5f;
    [SerializeField] float moveSpeed = 3f;

    public virtual void Start()
    {
        player = GameManager.Instance.Player;
    }

    public virtual void Update()
    {
        BTRoot();
    }

    BTState BTRoot() => Selector(
        Sequence(IsPlayerVisible, ChasePlayer),
        Sequence(IsInAttackRange, Attack)
    );

    BTState Selector(params Func<BTState>[] nodes)
    {
        foreach (var node in nodes)
            if (node() != BTState.Failure) return BTState.Success;
        return BTState.Failure;
    }

    Func<BTState> Sequence(params Func<BTState>[] nodes) => () =>
    {
        foreach (var node in nodes)
            if (node() != BTState.Success) return BTState.Failure;
        return BTState.Success;
    };

    // -- Conditions --
    BTState IsPlayerVisible() {
        float dist = Vector2.Distance(transform.position, player.position);
        return dist <= detectionRange ? BTState.Success : BTState.Failure;
    }

    BTState IsInAttackRange() {
        float dist = Vector2.Distance(transform.position, player.position);
        return dist <= attackRange ? BTState.Success : BTState.Failure;
    }

    // -- Actions --
    BTState ChasePlayer() {
        transform.position = Vector2.MoveTowards(
            transform.position, player.position, moveSpeed * Time.deltaTime);
        return BTState.Running;
    }

    BTState Attack() {
        Debug.Log("Attacking player!");
        return BTState.Success;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

    }
}