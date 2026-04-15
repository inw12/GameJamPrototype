using UnityEngine;
using UnityEngine.UI;
public class PlayerStatusMenu : MonoBehaviour
{
    // Inputs
    private bool MenuActive => PlayerControls.Instance.StatusMenu;

    [Header("Turn Off when Active")]
    [SerializeField] private PlayerWeapon playerWeapon;

    [Header("Menu Container")]
    [SerializeField] private RectTransform container;
    [SerializeField] private float activeSize;
    [SerializeField] private Vector3 activeOffset;
    [SerializeField] private float menuSpeed;
    private Vector3 _defaultSize;
    private Vector3 _defaultPosition;

    [Header("Body Part Buttons")]
    [SerializeField] private Button head;
    [SerializeField] private Button body;
    [SerializeField] private Button armL;
    [SerializeField] private Button armR;
    [SerializeField] private Button legL;
    [SerializeField] private Button legR;
    private Image armLImage, armRImage, legLImage, legRImage;

    [Header("Regeneration Meter")]
    [SerializeField] private Image fillMeter;
    [SerializeField] private Image regenerationMeter;

    [Header("Limbs")]
    [SerializeField] private Limb LeftArm;
    [SerializeField] private Limb RightArm;
    [SerializeField] private Limb LeftLeg;
    [SerializeField] private Limb RightLeg;
    [SerializeField] private Color damagedColor;
    private Color baseColor;

    void Start()
    {
        _defaultSize = container.localScale;
        _defaultPosition = container.position;

        baseColor = head.GetComponent<Image>().color;
        armLImage = armL.GetComponent<Image>();
        armRImage = armR.GetComponent<Image>();
        legLImage = legL.GetComponent<Image>();
        legRImage = legR.GetComponent<Image>();

        ButtonsActive(MenuActive);
    }

    void Update()
    {
        MenuInputLoop();
    }

    void LateUpdate()
    {
        UpdateStatus();
    }

    private void MenuInputLoop()
    {
        playerWeapon.enabled = !MenuActive;

        var targetPosition = MenuActive ? _defaultPosition + activeOffset : _defaultPosition;
        var targetScale = MenuActive ? _defaultSize * activeSize : _defaultSize;

        // Update Position
        container.position = Vector3.Lerp
        (
            container.position,
            targetPosition,
            1f - Mathf.Exp(-menuSpeed * Time.deltaTime)
        );

        // Update Scale
        container.localScale = Vector3.Lerp
        (
            container.localScale,
            targetScale,
            1f - Mathf.Exp(-menuSpeed * Time.deltaTime)
        );

        // Update Buttons
        ButtonsActive(MenuActive);
    }

    private void UpdateStatus()
    {
        CheckLimb(armLImage, LeftArm);
        CheckLimb(armRImage, RightArm);
        CheckLimb(legLImage, LeftLeg);
        CheckLimb(legRImage, RightLeg);
    }

    private void CheckLimb(Image sprite, Limb limb)
    {
        if (limb.Dismembered)
            sprite.color = Color.black;
        else
            sprite.color = Color.Lerp(baseColor, damagedColor, 2f - limb.LimbHitboxLower.GetHealth01() - limb.LimbHitboxUpper.GetHealth01());
    }

    private void ButtonsActive(bool b)
    {
        head.interactable = b;
        body.interactable = b;
        armL.interactable = b;
        armR.interactable = b;
        legL.interactable = b;
        legR.interactable = b;
    }
}
