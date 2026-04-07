using UnityEngine;
public class PlayerWeapon : MonoBehaviour
{
    [SerializeField] private Weapon currentWeapon;
    [SerializeField] private Transform attachTo;

    void Start()
    {
        
    }

    // Used to update transform of weapon sprite
    void LateUpdate()
    {
        
    }

    public void SwitchWeapon(Weapon weapon)
    {
        currentWeapon = weapon;
    }
}
