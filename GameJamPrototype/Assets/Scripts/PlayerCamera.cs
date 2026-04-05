using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private Camera prefab;
    [SerializeField] private Vector2 offset;
    private Camera camera;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        camera = Instantiate(prefab);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateCamera();
    }

    void UpdateCamera()
    {
        camera.transform.position = transform.position + new Vector3(offset.x, offset.y, 0f);
    }
}
