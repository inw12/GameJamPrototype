using System;
using UnityEngine;

public class Damageable : MonoBehaviour, IDamageable
{
    private enum DamageState { Alive, Dead }
    private DamageState CurrentState;
    public bool IsDead => CurrentState == DamageState.Dead;
    public float Health;
    private float currentHealth;
    public event Action OnDeath; // On limbs, On enemies
    public event Action Regenerate;
    public event Action<IDamageable> OnDealingDamage;
    public event Action OnDamageTaken;

    [Header("Player Only - Limb Regeneration")]
    [SerializeField] private float fillSpeed;
    public float PassiveRegenPerSecond;
    private float _maxRegen = 100f;
    private float _currentRegen = 0f;
    //public event Action OnKill; // 

    void Start()
    {
        currentHealth = Health;
        CurrentState = DamageState.Alive;
    }

    void Update()
    {
        _currentRegen += PassiveRegenPerSecond * Time.deltaTime;
        _currentRegen = Mathf.Clamp(_currentRegen, 0f, _maxRegen);
    }

    public void TakeDamage(float damage)
    {
        if (CurrentState == DamageState.Dead) return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0f, Health);

        Debug.Log($"{this.name} is taking {damage} damage.");

        OnDamageTaken?.Invoke();
        if (currentHealth <= 0)
        {
            CurrentState = DamageState.Dead;
            OnDeath?.Invoke();
        }
    }

    // limb healing (fixed amount)
    public void Heal()
    {
        if (CurrentState is DamageState.Dead)
        {
            Regenerate?.Invoke();
            CurrentState = DamageState.Alive;
        }

        currentHealth += 10f;
        currentHealth = Mathf.Clamp(currentHealth, 0f, Health);
        //Debug.Log(CurrentHealth);
    }

    public float GetHealth01()
    {
        return currentHealth / Health;
    }

    public float GetRegen01()
    {
        return _currentRegen / _maxRegen;
    }

    public void DealDamage(IDamageable entity, float damage)
    {
        entity.TakeDamage(damage);

        OnDealingDamage?.Invoke(entity);

        // if (CurrentState == DamageState.Dead)
        //     OnKill?.Invoke();
    }
}