using UnityEngine;

[RequireComponent(typeof(Damageable))]
[RequireComponent(typeof(LineRenderer))]
public class Gunner : EnemyAI
{
    #region References
    private Transform player;
    private Damageable damageable;
    private LineRenderer lr;
    private LayerMask playerLayer => LayerMask.GetMask("PlayerHurtbox");
    #endregion

    [Header("Stats")]
    [SerializeField] float detectionRange = 5f;
    [SerializeField] float attackRange = 1.5f;
    [SerializeField] float moveSpeed = 3f;

    [Header("Weapons")]
    [SerializeField] private RangedWeapon weapon;
    [SerializeField] private float AimTime;
    private float aimTimer;
    private BTState _visible, _inAttackRange, _moveToAttackRange, _attack, _aim, _isDead;

    public override void Start()
    {
        base.Start();

        player = GameManager.Instance.Player;
        damageable = GetComponent<Damageable>();

        lr = GetComponent<LineRenderer>();

        BTNodes.Add(Sequence(IsDead));
        //BTNodes.Add(Sequence(IsPlayerVisible, MoveToAttackRange, Aim, Attack));
        BTNodes.Add(Sequence(IsInAttackRange, MoveToAttackRange, Aim, Attack));

        DebugNodes.Add(("IsDead", _isDead));
        DebugNodes.Add(("IsVisible", _visible));
        DebugNodes.Add(("InAttackRange", _inAttackRange));
        //DebugNodes.Add(("MovingToAttackRange", _moveToAttackRange));
        DebugNodes.Add(("Aiming", _aim));
        DebugNodes.Add(("Attacking", _attack));
    }

    public override void Update()
    {
        base.Update();
    }

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

    BTState IsDead()
    {
        if (damageable.IsDead)
            return _isDead = BTState.Success;

        return _isDead = BTState.Failure;
    }

    // Actions
    BTState MoveToAttackRange()
    {
        float dist = Vector2.Distance(transform.position, player.position);

        if (dist > attackRange)
        {
            transform.position = Vector2.MoveTowards(
                transform.position, player.position, moveSpeed * Time.deltaTime);

            aimTimer = 0f;

            return _moveToAttackRange = BTState.Running;
        }
        else
        {
            return _moveToAttackRange = BTState.Success;
        }
    }

    BTState Aim()
    {
        Vector3 direction = -(player.position - weapon.transform.position).normalized;
        weapon.transform.right = direction;
        aimTimer += Time.deltaTime;

        RaycastHit2D laserHit = Physics2D.Raycast(weapon.GunTip.position, direction, 100f, playerLayer);

        lr.positionCount = 2;
        lr.SetPosition(0, weapon.GunTip.position);

        if (laserHit.collider != null)
            lr.SetPosition(1, laserHit.point);
        else
            lr.SetPosition(1, weapon.GunTip.position + direction * 100f);

        if (aimTimer >= AimTime)
        {
            return _aim = BTState.Success;
        }

        return _aim = BTState.Running;
    }

    BTState Attack()
    {
        Debug.Log("Attacking player");
        weapon.Attack();
        return _attack = BTState.Running;
    }


    void OnDrawGizmos()
    {
        if (!ShowDebug) return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    private void DealDamage(IDamageable player)
    {
        if (player == (IDamageable)GameManager.Instance.Player)
        {

        }
    }
}
