using UnityEngine;

[RequireComponent(typeof(Damageable))]
public class Gunner : EnemyAI
{
    [Header("References")]
    [SerializeField] private EnemyAnimator animator;
    private Transform player;
    private Damageable damageable;
    private LayerMask playerLayer => LayerMask.GetMask("PlayerHurtbox");
    private LayerMask groundPlatLayer => LayerMask.GetMask("Ground", "Platform");
    private Rigidbody2D rb;

    [Header("Stats")]
    [SerializeField] float detectionRange = 5f;
    [SerializeField] float attackRange = 1.5f;
    [SerializeField] float moveSpeed = 3f;

    [Header("Weapons")]
    [SerializeField] private RangedWeapon weapon;
    [SerializeField] private float AimTime;
    private float aimTimer;
    private BTState _visible, _inAttackRange, _moveToAttackRange, _attack, _aim, _isDead, _patroling, _waitAfterCombat;

    [Header("Laser")]
    [Min(1f)]
    [SerializeField] private float LaserColorIntensity = 1f;
    private MaterialPropertyBlock laserMatBlock;
    private Color laserBaseColor;
    private LineRenderer lr;

    [Header("Patrol")]
    [SerializeField] private float PatrolSpeed;
    [SerializeField] private float WaitFrequency;
    [SerializeField] private float GroundRayLength;
    [SerializeField] private float GroundRayOffset;
    private float patrolTimer, waitTimer, randomPatrolTime, randomWaitTime;
    private bool facingRight = false;
    private bool isPatrolling;

    [Header("Combat Cooldown")]
    [SerializeField] private float CombatCooldown = 2f;
    private float combatCooldownTimer;
    private bool playerInRange;

    public override void Start()
    {
        base.Start();

        player = GameManager.Instance.Player;
        damageable = GetComponent<Damageable>();
        lr = GetComponent<LineRenderer>();
        rb = GetComponent<Rigidbody2D>();

        BTNodes.Add(Sequence(IsDead));
        //BTNodes.Add(Sequence(IsPlayerVisible, MoveToAttackRange, Aim, Attack));
        BTNodes.Add(Sequence(IsInAttackRange, Aim, Attack));
        BTNodes.Add(Sequence(WaitAfterCombat));
        BTNodes.Add(Sequence(Patrol));

        DebugNodes.Add(("IsDead", _isDead));
        DebugNodes.Add(("IsVisible", _visible));
        DebugNodes.Add(("InAttackRange", _inAttackRange));
        //DebugNodes.Add(("MovingToAttackRange", _moveToAttackRange));
        DebugNodes.Add(("Aiming", _aim));
        DebugNodes.Add(("Attacking", _attack));
        DebugNodes.Add(("WaitAfterCombat", _waitAfterCombat));
        DebugNodes.Add(("Patroling", _patroling));

        laserMatBlock = new();
        lr.GetPropertyBlock(laserMatBlock);
        laserBaseColor = lr.sharedMaterial.GetColor("_LaserColor");

        RandomizePatrol();
    }

    public override void Update()
    {
        base.Update();
        lr.enabled = _inAttackRange == BTState.Success && _aim != BTState.Success;
    }

    // BT State Changes
    void OnEnterAttackRange()
    {
        aimTimer = 0f;

        if (isPatrolling)
        {
            isPatrolling = false;
            OnPatrolEnd();
        }
    }

    void WhileInAttackRange()
    {
        StopMovement();
    }

    void OnExitAttackRange()
    {
        aimTimer = 0f;
        combatCooldownTimer = CombatCooldown;
    }

    void OnPatrolStart()
    {
        patrolTimer = 0f;
        waitTimer = 0f;
        animator.DisableArms();
        RandomizePatrol();
    }

    void OnPatrolEnd()
    {
        animator.EnableArms();
        StopMovement();
    }

    // Shared

    private void StopMovement()
    {
        rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
        animator.UpdateMoveAnim(false);
    }

    // BT Conditions

    BTState IsPlayerVisible()
    {
        float dist = Vector2.Distance(transform.position, player.position);
        return _visible = dist <= detectionRange ? BTState.Success : BTState.Failure;
    }

    BTState IsInAttackRange()
    {
        float dist = Vector2.Distance(transform.position, player.position);
        bool inRange = dist <= attackRange;

        // Transition: entered attack range
        if (inRange && !playerInRange)
        {
            playerInRange = true;
            OnEnterAttackRange();
        }
        // Transition: left attack range
        else if (!inRange && playerInRange)
        {
            playerInRange = false;
            OnExitAttackRange();
        }

        if (inRange)
        {
            WhileInAttackRange();
            return _inAttackRange = BTState.Success;
        }

        return _inAttackRange = BTState.Failure;
    }

    BTState IsDead()
    {
        if (damageable.IsDead)
            return _isDead = BTState.Success;

        return _isDead = BTState.Failure;
    }

    // BT Actions

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
        animator.AimAtPlayer(player.position);
        DrawLaser();

        aimTimer += Time.deltaTime;

        if (aimTimer >= AimTime)
            return _aim = BTState.Success;

        return _aim = BTState.Running;
    }

    BTState Attack()
    {
        weapon.Attack();
        aimTimer = 0f;
        return _attack = BTState.Success;
    }

    BTState WaitAfterCombat()
    {
        if (combatCooldownTimer <= 0f)
            return _waitAfterCombat = BTState.Failure;

        combatCooldownTimer -= Time.deltaTime;
        StopMovement();

        return _waitAfterCombat = BTState.Running;
    }

    BTState Patrol()
    {
        if (!isPatrolling)
        {
            isPatrolling = true;
            OnPatrolStart();
        }

        patrolTimer += Time.deltaTime;

        if (patrolTimer >= randomPatrolTime)
        {
            waitTimer += Time.deltaTime;

            if (waitTimer > randomWaitTime)
            {
                patrolTimer = 0f;
                waitTimer = 0f;
                RandomizePatrol();
            }
        }
        else
        {
            RaycastHit2D rightEdge = Physics2D.Raycast((Vector2)transform.position + Vector2.right * GroundRayOffset, Vector2.down, GroundRayLength, groundPlatLayer);
            RaycastHit2D leftEdge = Physics2D.Raycast((Vector2)transform.position - Vector2.right * GroundRayOffset, Vector2.down, GroundRayLength, groundPlatLayer);

            if (facingRight && rightEdge.collider == null)
                FlipCharacter();
            else if (!facingRight && leftEdge.collider == null)
                FlipCharacter();

            rb.linearVelocity = new Vector2(
                PatrolSpeed * (facingRight ? 1f : -1f),
                rb.linearVelocity.y
            );
        }

        animator.UpdateMoveAnim(patrolTimer < randomPatrolTime);

        return _patroling = BTState.Success;
    }

    // Helpers

    private void FlipCharacter()
    {
        facingRight = !facingRight;
        var scale = transform.localScale;
        scale.x *= -1f;
        transform.localScale = scale;
    }

    private void RandomizePatrol()
    {
        randomPatrolTime = Random.Range(0.5f, 5f);
        randomWaitTime = Random.Range(0.5f, 3f);
    }

    private void DrawLaser()
    {
        // Laser Position
        RaycastHit2D laserHit = Physics2D.Raycast(weapon.GunTip.position, weapon.GunTip.right, 100f, playerLayer);

        lr.positionCount = 2;
        lr.SetPosition(0, weapon.GunTip.position);

        if (laserHit.collider != null)
            lr.SetPosition(1, laserHit.point);
        else
            lr.SetPosition(1, weapon.GunTip.position + weapon.GunTip.right * 100f);

        // Laser Color
        float t = Mathf.Clamp01(aimTimer / AimTime);
        Color newColor = Color.Lerp(laserBaseColor, laserBaseColor * LaserColorIntensity, t);

        lr.GetPropertyBlock(laserMatBlock);
        laserMatBlock.SetColor("_LaserColor", newColor);
        lr.SetPropertyBlock(laserMatBlock);
    }

    void OnDrawGizmos()
    {
        if (!ShowDebug) return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawRay((Vector2)transform.position + Vector2.right * GroundRayOffset, Vector2.down * GroundRayLength);
        Gizmos.DrawRay((Vector2)transform.position - Vector2.right * GroundRayOffset, Vector2.down * GroundRayLength);
    }

    private void DealDamage(IDamageable player)
    {
        if (player == (IDamageable)GameManager.Instance.Player)
        {

        }
    }
}
