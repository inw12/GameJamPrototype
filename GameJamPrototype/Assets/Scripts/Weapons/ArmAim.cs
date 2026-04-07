using UnityEngine;
public class ArmAim : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    [Space]
    [SerializeField] private Transform frontArmTarget;
    [SerializeField] private Transform backArmTarget;
    [Space]
    [SerializeField] [Range(0f, 360)] private float rotationOffset;

    private bool _active;

    public void UpdateArms()
    {
        if (_active)
        {
            // position
            var mousePos = PlayerControls.Instance.GetMouseWorldPosition();
            frontArmTarget.position = mousePos;

            // rotation
            var angle = Mathf.Atan2(mousePos.x, mousePos.y) * Mathf.Rad2Deg - rotationOffset;
            angle = playerMovement.IsFacingRight() ? -angle : angle;
            frontArmTarget.rotation = Quaternion.Euler(0f, 0f, angle);
        }
    }

    public void WeaponArmsActive(bool b) => _active = b;
}
