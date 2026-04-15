using UnityEngine;

[RequireComponent(typeof(Damageable))]
public class Gunner : EnemyAI, IKnockable
{
    [Header("References")]
    [SerializeField] private EnemyAnimator animator;
    private Transform player;
    private Damageable damageable;
    private Rigidbody2D Rigidbody;
    private LayerMask playerLayer => LayerMask.GetMask("PlayerHurtbox");
    private LayerMask groundPlatLayer => LayerMask.GetMask("Ground", "Platform");
    private EnemyLimbManager limbManager;

    [Header("Stats")]
    [SerializeField] float detectionRange = 5f;
    [SerializeField] float attackRange = 1.5f;
    [SerializeField] float moveSpeed = 3f;

    [Header("Weapons")]
    [SerializeField] private Transform attachTo;
    [SerializeField] private RangedWeapon weapon;
    [SerializeField] private float AimTime;
    private float aimTimer;
    private BTState _visible, _inAttackRange, _moveToAttackRange, _attack, _aim, _isDead, _patroling, _waitAfterCombat;

    [Header("Laser")]
    [Min(1f)]
    [SerializeField] private float LaserColorIntensity = 1f;
    private MaterialPropertyBlock laserMatBlock;
    private Color laserBaseColor, laserBaseAlpha;
    private LineRenderer lr;

    [Header("Patrol")]
    [SerializeField] private float PatrolSpeed;
    [SerializeField] private float WaitFrequency;
    [SerializeField] private float GroundRayLength;
    [SerializeField] private float GroundRayOffset;
    [SerializeField] private float PhysicsSmooth;
    private float patrolTimer, waitTimer, randomPatrolTime, randomWaitTime;
    private bool facingRight = false;
    private bool isPatrolling;
    private Vector2 Velocity, ExternalVelocity;

    [Header("Combat Cooldown")]
    [SerializeField] private float CombatCooldown = 2f;
    private float combatCooldownTimer;
    private bool playerInRange;

    public void Start()
    {
        player = GameManager.Instance.Player;
        damageable = GetComponent<Damageable>();
        lr = GetComponent<LineRenderer>();
        Rigidbody = GetComponent<Rigidbody2D>();
        limbManager = GetComponent<EnemyLimbManager>();

        BTNodes.Add(Sequence(IsDead));
        //BTNodes.Add(Sequence(IsPlayerVisible, MoveToAttackRange,IsInAttackRange, Aim, Attack));
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
        laserBaseAlpha = laserBaseColor;
        laserBaseAlpha.a = 0f;

        weapon = (RangedWeapon)Weapon.Equip(weapon, attachTo);

        damageable.OnDeath += OnDeath;

        Debug.Log(weapon.gameObject);

        RandomizePatrol();
    }

    public override void Update()
    {
        base.Update();
        lr.enabled = _inAttackRange == BTState.Success && _aim != BTState.Success && _isDead != BTState.Success;
    }

    private void FixedUpdate()
    {
        Rigidbody.linearVelocity = Velocity + ExternalVelocity;
        ExternalVelocity = Vector2.Lerp(ExternalVelocity, Vector2.zero, PhysicsSmooth * Time.fixedDeltaTime);
        ExternalVelocity.y = 0f;
        UpdateFaceDirection();
    }

    void OnDestroy()
    {
        damageable.OnDeath -= OnDeath;
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
        animator.EnableArms();
        RandomizePatrol();
    }

    void OnPatrolEnd()
    {
        animator.DisableArms();
        StopMovement();
    }

    private void OnDeath()
    {
        Rigidbody.simulated = false;
        StopMovement();
        animator.TriggerDeath();
        weapon.Unequip();
        limbManager.RandomDismember();
        AudioManager.Instance.PlaySFXAt("Dismemberment", transform.position);
    }

    // Shared
    private void StopMovement()
    {
        Velocity = new Vector2(0f, Rigidbody.linearVelocityY);
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
        {
            return _isDead = BTState.Success;
        }

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

        float sign = Mathf.Sign(transform.position.x - player.position.x);
        facingRight = sign == -1; // Sprites are reversed.

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
            StopMovement();

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
                facingRight = false;
            else if (!facingRight && leftEdge.collider == null)
                facingRight = true;

            Velocity = new Vector2(
                PatrolSpeed * (facingRight ? 1f : -1f),
                Rigidbody.linearVelocityY
            );
        }

        animator.UpdateMoveAnim(patrolTimer < randomPatrolTime);

        return _patroling = BTState.Success;
    }

    // Functions
    private void UpdateFaceDirection()
    {
        // Sprites are reversed.
        var scale = transform.localScale;
        transform.localScale = new Vector3(facingRight ? -Mathf.Abs(scale.x) : Mathf.Abs(scale.x), scale.y, scale.z);
    }

    public void ApplyForce(float knockback, float upward)
    {
        Vector2 force = knockback * transform.right * (facingRight ? -1 : 1f) + Vector3.up * upward;
        ExternalVelocity = force;
    }

    private void RandomizePatrol()
    {
        randomPatrolTime = Random.Range(0.5f, 5f);
        randomWaitTime = Random.Range(0.5f, 3f);
    }

    private void DrawLaser()
    {
        // Laser Position
        RaycastHit2D laserHit = Physics2D.Raycast(weapon.GunTip.position, weapon.GunTip.right * (facingRight ? -1f : 1f), 100f, playerLayer);

        lr.positionCount = 2;
        lr.SetPosition(0, weapon.GunTip.position);

        if (laserHit.collider != null)
            lr.SetPosition(1, laserHit.point);
        else
            lr.SetPosition(1, weapon.GunTip.position + weapon.GunTip.right * 100f);

        // Laser Color
        float t = Mathf.Clamp01(aimTimer / AimTime);
        Color newColor = Color.Lerp(laserBaseAlpha, laserBaseColor * LaserColorIntensity, t * t);

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
}
