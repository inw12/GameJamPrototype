using UnityEngine;
public class AssaultRifle : RangedWeapon
{
    protected override void Start()
    {
        base.Start();
        OnAttack += PistolShot;
    }

    private void PistolShot()
    {
        AudioManager.Instance.PlaySFX("PistolShot");
    }
}
