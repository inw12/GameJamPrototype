using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    private Vector2 MousePosition => PlayerControls.Instance.Mouse;
    [SerializeField] private Camera Prefab;
    [SerializeField] private Vector2 Offset;
    [SerializeField] private float z;
    [SerializeField] private float Size;
    [SerializeField] private float CameraSmooth;
    [Range(0f, 1f)]
    [SerializeField] private float ViewPortScrollRange;
    [SerializeField] private float CameraMaxRange;
    [SerializeField] private float CameraScrollSpeed;
    private Camera camera;
    private Vector3 cameraPos, targetPos;
    private Rigidbody2D rigidbody;

    [Header("Debug")]
    [SerializeField] private bool ShowDebug = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        camera = Instantiate(Prefab, transform.position, Quaternion.identity);
        camera.orthographicSize = Size;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCamera();
    }

    void LateUpdate()
    {
        camera.transform.position = cameraPos;
    }

    void UpdateCamera()
    {
        Vector3 mouseViewPos = camera.ScreenToViewportPoint(MousePosition);
        Vector3 playerPos = rigidbody.position + new Vector2(Offset.x, Offset.y);

        camera.orthographicSize = Size;

        // If within scroll range
        if (mouseViewPos.x < 1f - ViewPortScrollRange && mouseViewPos.x > ViewPortScrollRange)
        {
            targetPos = playerPos;
        }
        else
        {
            if (mouseViewPos.x >= 1f - ViewPortScrollRange)
                targetPos = playerPos + CameraMaxRange * Vector3.right;
            else
                targetPos = playerPos - CameraMaxRange * Vector3.right;
        }

        targetPos += -Vector3.forward * z;

        cameraPos = Vector3.Lerp(cameraPos, targetPos, CameraSmooth * Time.deltaTime);
    }

    void OnDrawGizmos()
    {
        if (!ShowDebug) return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, new Vector3(CameraMaxRange * 2, 1f, transform.position.z));
    }
}
