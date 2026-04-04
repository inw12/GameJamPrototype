using UnityEngine;

public class PlayerRadial : MonoBehaviour
{
    [Header("Spawn Position")]
    [SerializeField] private Vector2 PlayerOffset;

    [Header("Radial Menu")]
    [SerializeField] private RadialMenu RadialMenu;
    [SerializeField] private float HoldTime;
    private float holdTimer;


    void Start()
    {
        RadialMenu.gameObject.SetActive(false);
    }

    void Update()
    {
        // Update Pos
        Vector2 menuPos = Camera.main.WorldToScreenPoint((Vector2)transform.position + PlayerOffset);
        RadialMenu.rect.position = menuPos;

        // Input
        // if (PlayerControls.Instance.InteractPressed)
        //     holdTimer += Time.deltaTime;
        // else
        //     holdTimer = 0f;

        RadialMenu.gameObject.SetActive(PlayerControls.Instance.InteractPressed);
    }
}
