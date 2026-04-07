using UnityEngine;
public class ArmAim : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    [Space]
    [SerializeField] private Transform frontArmTarget;
    [SerializeField] private Transform backArmTarget;

    private bool _active;

    public void UpdateArms()
    {
        if (_active)
        {
            // position
            var mousePos = PlayerControls.Instance.GetMouseWorldPosition();
            frontArmTarget.position = mousePos;

            // rotation
            var angle = Mathf.Atan2(mousePos.y - transform.position.y, mousePos.x - transform.position.x) * Mathf.Rad2Deg;
            angle = playerMovement.IsFacingRight() ? angle + 180f : angle;
            frontArmTarget.rotation = Quaternion.Euler(0f, 0f, angle);
        }
    }

    public void WeaponArmsActive(bool b) => _active = b;
}
