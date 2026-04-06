using UnityEngine;

[RequireComponent(typeof(Damageable))]
public class Soldier : Enemy
{
    public override void Start()
    {
        base.Start();

        OnAttack += Attack;
    }

    public override void Update()
    {
        base.Update();
    }

    private void Attack()
    {
        
    }
}
