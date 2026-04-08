using System;
using UnityEngine;

public class Damageable : MonoBehaviour, IDamageable
{
    public enum DamageState { Alive, Dead }
    public DamageState CurrentState;
    public float Health;
    private float CurrentHealth;
    public event Action OnDeath; // On limbs, On enemies
    public event Action<IDamageable> OnDealingDamage;
    public event Action OnDamageTaken;
    //public event Action OnKill; // 

    void Start()
    {
        CurrentHealth = Health;
        CurrentState = DamageState.Alive;
    }

    public void TakeDamage(float damage)
    {
        CurrentHealth -= damage;

        Debug.Log($"{this.name} is taking {damage} damage.");

        OnDamageTaken?.Invoke();
        if (CurrentHealth <= 0)
        {
            OnDeath?.Invoke();
            CurrentState = DamageState.Dead;
        }
    }

    public float GetCurrentHealth()
    {
        return CurrentHealth;
    }

    public void DealDamage(IDamageable entity, float damage)
    {
        entity.TakeDamage(damage);

        OnDealingDamage?.Invoke(entity);

        // if (CurrentState == DamageState.Dead)
        //     OnKill?.Invoke();
    }
}