using UnityEngine;
using System;

public class GrapplingGun : MonoBehaviour
{
    public LayerMask GrappableMask;
    public Transform GunTip;
    [SerializeField] private LineRenderer grappleAimLine;
    [SerializeField] private PlayerMovement playerMovement;

    public event Action<RaycastHit2D> OnSuccessfulGrapple;
    public bool GrappleOn => joint != null;

    private Vector3 grapplePoint;
    private float maxDistance = 100f;
    private SpringJoint2D joint;
    private RaycastHit2D hit, aimHit;

    [Header("Debug")]
    [SerializeField] private bool Debug;

    void Update()
    {
        if (playerMovement.CanMove)
        {
            if (joint == null && PlayerControls.Instance.Mouse1)
            {
                StartGrapple();
            }

            if (joint != null && !PlayerControls.Instance.Mouse1)
            {
                StopGrapple();
            }
        }

        DrawGrappleLine();
    }

    void DrawGrappleLine()
    {
        if (GrappleOn)
        {
            grappleAimLine.enabled = false;
            return;
        }

        grappleAimLine.enabled = true;
        grappleAimLine.positionCount = 2;
        grappleAimLine.SetPosition(0, GunTip.position);

        aimHit = Physics2D.Raycast(GunTip.position, GunTip.right, maxDistance, GrappableMask);
        if (aimHit.collider != null)
        {
            grappleAimLine.startColor = Color.green;
            grappleAimLine.endColor = Color.green;
            grappleAimLine.SetPosition(1, aimHit.point);
        }
        else
        {
            grappleAimLine.startColor = Color.red;
            grappleAimLine.endColor = Color.red;
            grappleAimLine.SetPosition(1, GunTip.position + GunTip.right * 100f);
        }
    }

    void StartGrapple()
    {
        hit = Physics2D.Raycast(GunTip.position, GunTip.right, maxDistance, GrappableMask);

        if (hit.collider != null)
        {
            grapplePoint = hit.point;
            joint = transform.gameObject.AddComponent<SpringJoint2D>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplePoint;

            float distanceFromPoint = Vector3.Distance(transform.position, grapplePoint);

            //The distance grapple will try to keep from grapple point. 
            joint.distance = distanceFromPoint * 0.8f;

            //Adjust these values to fit your game.
            joint.frequency = 2.5f;
            joint.dampingRatio = 0.7f;
        }
    }

    void StopGrapple()
    {
        Destroy(joint);
        OnSuccessfulGrapple.Invoke(hit);
    }

    public Vector3 GetGrapplePoint()
    {
        return grapplePoint;
    }

    void OnDrawGizmos()
    {
        if (!Debug) return;
    }
}