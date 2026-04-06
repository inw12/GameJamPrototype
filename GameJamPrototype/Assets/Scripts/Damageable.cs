using System;
using UnityEngine;

public class Damageable : MonoBehaviour, IDamageable
{
    public enum DamageState { Alive, Dead }
    public DamageState CurrentState;
    public float Health;
    private float CurrentHealth;
    public event Action OnDeath;
    public event Action OnKill;

    void Start()
    {
        CurrentHealth = Health;
        CurrentState = DamageState.Alive;
    }

    public void TakeDamage(float damage)
    {
        CurrentHealth -= damage;
        if (CurrentHealth <= 0)
        {
            OnDeath?.Invoke();
            CurrentState = DamageState.Dead;
        }
    }

    public void DealDamage(IDamageable entity, float damage)
    {
        entity.TakeDamage(damage);

        if (CurrentState == DamageState.Dead)
            OnKill?.Invoke();
    }
}