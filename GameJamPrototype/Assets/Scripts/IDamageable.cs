using System;
public interface IDamageable
{
    //event Action OnKill;
    event Action<IDamageable> OnDealingDamage;
    event Action OnDamageTaken;
    event Action OnDeath;
    void TakeDamage(float damage);
    void DealDamage(IDamageable entity, float damage);
}


