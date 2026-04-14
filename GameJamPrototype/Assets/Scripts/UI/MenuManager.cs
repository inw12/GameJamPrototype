using UnityEngine;
using UnityEngine.U2D.IK;
public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject statusMenu;
    [SerializeField] private GameObject settingsMenu;

    [Header("Things to turn off when paused")]
    [SerializeField] private Transform Player;
    private PlayerMovement playerMovement;
    private PlayerWeapon playerWeapon;
    private PlayerMelee playerMelee;
    private IKManager2D playerIKManager;

    private bool _paused;

    void Start()
    {
        playerMovement = Player.GetComponent<PlayerMovement>();
        playerWeapon = Player.GetComponent<PlayerWeapon>();
        playerMelee = Player.GetComponent<PlayerMelee>();
        playerIKManager = Player.GetComponentInChildren<IKManager2D>();

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
