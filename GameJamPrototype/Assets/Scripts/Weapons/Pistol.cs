using UnityEngine;
public class Pistol : RangedWeapon
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
