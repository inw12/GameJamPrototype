using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Transform player;
    private enum BTState { Success, Failure, Running }
    [SerializeField] float detectionRange = 5f;
    [SerializeField] float attackRange = 1.5f;
    [SerializeField] float moveSpeed = 3f;
    public event Action OnAttack;

    [Header("Debug")]
    public bool ShowDebug = false;
    private BTState _visible, _chase, _inAttackRange, _attack;

    public virtual void Start()
    {
        player = GameManager.Instance.Player;
    }

    public virtual void Update()
    {
        BTRoot();
    }

    // Tree
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

    // Conditions
    BTState IsPlayerVisible()
    {
        float dist = Vector2.Distance(transform.position, player.position);
        return _visible = dist <= detectionRange ? BTState.Success : BTState.Failure;
    }

    BTState IsInAttackRange()
    {
        float dist = Vector2.Distance(transform.position, player.position);
        return _inAttackRange = dist <= attackRange ? BTState.Success : BTState.Failure;
    }

    // Actions
    BTState ChasePlayer()
    {
        if (_inAttackRange == BTState.Success)
            return _chase = BTState.Failure;

        transform.position = Vector2.MoveTowards(
            transform.position, player.position, moveSpeed * Time.deltaTime);


        return _chase = BTState.Running;
    }

    BTState Attack()
    {
        Debug.Log("Attacking player!");
        OnAttack?.Invoke();
        return _attack = BTState.Success;
    }

    void OnDrawGizmos()
    {
        if (!ShowDebug) return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, 220, 300));
        GUILayout.Label("Behavior Tree");

        DrawNode("IsPlayerVisible", _visible);
        DrawNode("ChasePlayer", _chase);
        DrawNode("IsInAttackRange", _inAttackRange);
        DrawNode("Attack", _attack);

        GUILayout.EndArea();
    }

    void DrawNode(string label, BTState state)
    {
        Color dot = state == BTState.Success ? Color.green
                  : state == BTState.Failure ? Color.red
                  : Color.gray;

        GUILayout.BeginHorizontal();
        GUI.color = dot;
        GUILayout.Label("●", GUILayout.Width(20));
        GUI.color = Color.white;
        GUILayout.Label(label);
        GUILayout.EndHorizontal();
    }
}