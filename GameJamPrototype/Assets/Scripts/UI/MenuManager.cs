using UnityEngine;
using UnityEngine.U2D.IK;
public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject statusMenu;
    [SerializeField] private GameObject settingsMenu;

    [Header("Things to turn off when paused")]
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerWeapon playerWeapon;
    [SerializeField] private PlayerMelee playerMelee;
    [SerializeField] private IKManager2D playerIKManager;

    private bool _paused;

    void Start()
    {
        playerMovement.enabled = playerWeapon.enabled = playerMelee.enabled = true;
        playerIKManager.weight = 1f;

        statusMenu.SetActive(true);
        settingsMenu.SetActive(false);

        _paused = false;
    }

    void Update()
    {
        // Settings Menu
        if (PlayerControls.Instance.SettingsMenu)
        {
            TogglePause();
            ToggleSettingsMenu();
        }
    }

    private void TogglePause()
    {
        _paused = !_paused;

        playerMovement.enabled = playerWeapon.enabled = playerMelee.enabled = !_paused;
        playerIKManager.weight = _paused ? 0f : 1f;
    }

    private void ToggleSettingsMenu()
    {
        statusMenu.SetActive(!_paused);
        settingsMenu.SetActive(_paused);

        var anim = settingsMenu.GetComponent<Animator>();
        anim.SetTrigger("Highlighted");
    }
}
