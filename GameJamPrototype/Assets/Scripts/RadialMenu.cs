using UnityEngine;
using UnityEngine.UI;

public class RadialMenu : MonoBehaviour
{
    // Inputs
    private Vector2 MousePosition => PlayerControls.Instance.Mouse;
    private bool MousePressed => PlayerControls.Instance.Mouse1;

    [Header("Tiles")]
    public RectTransform rect;
    [SerializeField] private Image Top, Right, Left, Bottom;
    [SerializeField] private Color On, Off, Active;
    [SerializeField] private float PressTime = 2f;
    private float pressTimer = 0f;
    private Image activeTile;


    [Header("Debug")]
    public bool Debug;
    private Vector2 mouseAngle;

    void Start()
    {
        rect = GetComponent<RectTransform>();
    }

    void Update()
    {
        if (gameObject.activeSelf)
        {

            if (!MousePressed)
            {
                mouseAngle = ((Vector2)rect.position - MousePosition).normalized;

                bool isTop = Vector2.Angle(Vector2.up, mouseAngle) >= 135f;
                bool isRight = Vector2.Angle(Vector2.right, mouseAngle) >= 135f;
                bool isLeft = Vector2.Angle(Vector2.left, mouseAngle) >= 135f;
                bool isBottom = Vector2.Angle(Vector2.down, mouseAngle) >= 135f;

                Top.color = isTop ? On : Off;
                Right.color = isRight ? On : Off;
                Left.color = isLeft ? On : Off;
                Bottom.color = isBottom ? On : Off;

                if (isTop)
                    activeTile = Top;
                if (isRight)
                    activeTile = Right;
                if (isLeft)
                    activeTile = Left;
                if (isBottom)
                    activeTile = Bottom;
            }

            
            pressTimer += MousePressed ? Time.deltaTime : -Time.deltaTime * 2f;
            pressTimer = Mathf.Clamp(pressTimer, 0f, PressTime);

                activeTile.color = Color.Lerp(On, Active, pressTimer / PressTime);
        }


    }

    void OnGUI()
    {
        if (!Debug) return;
        GUILayout.Label($"Up: {Vector2.Angle(Vector2.up, mouseAngle)}");
        GUILayout.Label($"Right: {Vector2.Angle(Vector2.right, mouseAngle)}");
        GUILayout.Label($"Left: {Vector2.Angle(Vector2.left, mouseAngle)}");
        GUILayout.Label($"Down: {Vector2.Angle(Vector2.down, mouseAngle)}");
    }
}