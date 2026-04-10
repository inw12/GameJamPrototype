using UnityEngine;
using UnityEngine.UI;
public class PlayerStatusMenu : MonoBehaviour
{
    // Inputs
    private bool MenuActive => PlayerControls.Instance.StatusMenu;

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

    [Header("Regeneration Meter")]
    [SerializeField] private Image fillMeter;
    [SerializeField] private Image regenerationMeter;
    [SerializeField] private float fillSpeed;

    public float currentRegen;
    public float maxRegen;
    private float _regenTimer;

    void Start()
    {
        _defaultSize        = container.localScale;
        _defaultPosition    = container.position;

        ButtonsActive(MenuActive);
    }

    void Update()
    {
        var targetPosition  = MenuActive ? _defaultPosition + activeOffset : _defaultPosition;
        var targetScale     = MenuActive ? _defaultSize * activeSize : _defaultSize;

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
