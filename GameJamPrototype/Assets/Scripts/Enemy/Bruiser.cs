using UnityEngine;

[RequireComponent(typeof(Damageable))]
public class Bruiser : EnemyAI, IKnockable
{
    [Header("References")]
    [SerializeField] private EnemyAnimator animator;
    private Transform player;
    private Damageable damageable;
    private LayerMask playerLayer => LayerMask.GetMask("PlayerHurtbox");
    private LayerMask groundPlatLayer => LayerMask.GetMask("Ground", "Platform");
    private Rigidbody2D Rigidbody;
    private BTState _visible, _moveToAttackRange, _attack, _isDead, _patroling, _waitAfterCombat;
    private EnemyLimbManager limbManager;
    
    [Header("Stats")]
    [SerializeField] float detectionRange = 5f;
    [SerializeField] float attackRange = 1.5f;
    [SerializeField] float moveSpeed = 3f;

    [Header("Melee")]
    [SerializeField] private Color EffectColor;
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private GameObject meleeHitbox;
    [SerializeField] private Transform meleeSpawn;
    [Space]
    [SerializeField] private float damage;
    [SerializeField] private float meleeCooldown;
    private bool _meleeTriggered;
    private float _meleeTimer;

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

    public override void Start()
    {
        base.Start();

        player = GameManager.Instance.Player;
        damageable = GetComponent<Damageable>();
        Rigidbody = GetComponent<Rigidbody2D>();
        limbManager = GetComponent<EnemyLimbManager>();

        BTNodes.Add(Sequence(IsDead));
        BTNodes.Add(Sequence(IsPlayerVisible, MoveToAttackRange, Attack));
        BTNodes.Add(Sequence(WaitAfterCombat));
        BTNodes.Add(Sequence(Patrol));

        DebugNodes.Add(("IsDead", _isDead));
        DebugNodes.Add(("IsVisible", _visible));
        DebugNodes.Add(("MovingToAttackRange", _moveToAttackRange));
        DebugNodes.Add(("Attacking", _attack));
        DebugNodes.Add(("WaitAfterCombat", _waitAfterCombat));
        DebugNodes.Add(("Patroling", _patroling));

        RandomizePatrol();

        damageable.OnDeath += OnDeath;
    }

    public override void Update()
    {
        base.Update();
    }

    private void FixedUpdate()
    {
        Rigidbody.linearVelocity = Velocity + ExternalVelocity;
        ExternalVelocity.x = Mathf.MoveTowards(ExternalVelocity.x, 0f, PhysicsSmooth * Time.fixedDeltaTime);
        ExternalVelocity.y = 0f;
        UpdateFaceDirection();
    }

    void OnDestroy()
    {
        damageable.OnDeath -= OnDeath;
    }

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

    // BT State Changes
    void OnEnterAttackRange()
    {
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
        combatCooldownTimer = CombatCooldown;
    }

    void OnPatrolStart()
    {
        patrolTimer = 0f;
        waitTimer = 0f;
        RandomizePatrol();
    }

    void OnPatrolEnd()
    {
        StopMovement();
    }
    
    private void OnDeath()
    {
        Rigidbody.simulated = false;
        StopMovement();
        animator.TriggerDeath();
        limbManager.RandomDismember();
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
        bool inRange = dist <= attackRange;

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
            return _moveToAttackRange = BTState.Success;
        }
        else
        {
            float sign = Mathf.Sign(transform.position.x - player.position.x);
            facingRight = sign == -1; // Sprites are reversed.

            Velocity = new Vector2(
                PatrolSpeed * (facingRight ? 1f : -1f),
                Rigidbody.linearVelocityY
            );
        }

        animator.UpdateMoveAnim(!inRange);

        return _moveToAttackRange = BTState.Running;
    }

    BTState Attack()
    {
        _meleeTimer += Time.deltaTime;

        if (_meleeTimer >= meleeCooldown && !_meleeTriggered)
        {
            _meleeTriggered = true;
            animator.TriggerMelee();

            var melee = Instantiate(meleeHitbox, meleeSpawn);

            if (melee.TryGetComponent(out MeleeHitbox m))
            {
                m.Initialize(damage, targetLayer, EffectColor);
            }

            _meleeTimer = 0f;
            _meleeTriggered = false;
        }

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

    // Helpers
    private void RandomizePatrol()
    {
        randomPatrolTime = Random.Range(0.5f, 5f);
        randomWaitTime = Random.Range(0.5f, 3f);
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
