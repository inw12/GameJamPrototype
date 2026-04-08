using UnityEngine;

[RequireComponent(typeof(Damageable))]
public class Swordsman : EnemyAI
{

    public override void Start()
    {
        base.Start();

        //OnAttack += Attack;
    }

    public override void Update()
    {
        base.Update();
    }

    private void Attack()
    {
        
    }

    private void DealDamage(IDamageable player)
    {
        if (player == (IDamageable)GameManager.Instance.Player)
        {
            
        }
    }
}
