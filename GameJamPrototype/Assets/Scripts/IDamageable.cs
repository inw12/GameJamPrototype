using System;
public interface IDamageable
{
    event Action OnKill;
    event Action OnDeath;
    void TakeDamage(float damage);
    void DealDamage(IDamageable entity, float damage);
}


