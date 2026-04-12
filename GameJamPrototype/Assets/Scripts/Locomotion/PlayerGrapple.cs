using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerGrapple : MonoBehaviour
{
    public GrapplingGun grapplingGun;
    public bool IsGrappling => _isGrappling;
    private bool _isGrappling;
    [SerializeField] private float GrappleSpeed;
    [SerializeField] private float GrappleLaunchHeight;
    [SerializeField] private Transform attachTo;
    private Rigidbody2D rb;

    private struct GrappleData
    {
        public Vector2 landPos;
    }
    private GrappleData currentGrapple;

    [Header("Debug")]
    public bool ShowDebug;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        grapplingGun.OnSuccessfulGrapple += OnGrappleStart;
    }

    // Update is called once per frame
    void Update()
    {
        // Toggle Gun
        bool grappleOn = PlayerControls.Instance.JumpPressed;
        grapplingGun.gameObject.SetActive(grappleOn);
    }

    void FixedUpdate()
    {
        if (_isGrappling)
        {
            Vector2 direction = (currentGrapple.landPos - (Vector2)transform.position).normalized;
            rb.linearVelocity = direction * GrappleSpeed;

            if (Vector2.Distance(rb.position, currentGrapple.landPos) < 1f)
            {
                rb.linearVelocity = Vector2.zero;
                OnGrappleEnd();
            }
        }
    }

    private void OnGrappleEnd()
    {
        _isGrappling = false;
        LevelManager.Instance.IncludePlayer();
    }

    private void OnGrappleStart(RaycastHit2D hit)
    {
        Debug.Log("Grappling!");

        Vector2 landingPos = hit.point + Vector2.up * GrappleLaunchHeight;

        currentGrapple = new GrappleData()
        {
            landPos = landingPos
        };

        _isGrappling = true;
        LevelManager.Instance.ExcludePlayer();
    }

    private void OnDrawGizmos()
    {
        if (!ShowDebug) return;

        if (_isGrappling)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(currentGrapple.landPos, 0.2f);
            Vector2 direction = (currentGrapple.landPos - (Vector2)transform.position).normalized;
            Gizmos.DrawLine(transform.position, (Vector2)transform.position + direction * 10f);
        }
    }
}
