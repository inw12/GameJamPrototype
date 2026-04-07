using UnityEngine;
public class ArmAim : MonoBehaviour
{
    [SerializeField] private Transform frontArmTarget;
    [SerializeField] private Transform backArmTarget;

    private bool _active;

    public void UpdateArms()
    {
        if (_active)
        {
            var mousePos = PlayerControls.Instance.GetMouseWorldPosition();
            frontArmTarget.position = mousePos;
        }
    }

    public void WeaponArmsActive(bool b) => _active = b;
}
