using UnityEngine;
[RequireComponent(typeof(CircleCollider2D))]
public class WeaponPickup : MonoBehaviour
{
    public string PromptText { get; private set; }

    [SerializeField] private GameObject weaponCounterpart;
    [SerializeField] private SpriteRenderer highlightSprite;
    [SerializeField] private float pickupRadius;

    private CircleCollider2D _collider;

    void Start()
    {
        PromptText = "(E)";

        _collider = GetComponent<CircleCollider2D>();
        _collider.radius = pickupRadius;

        highlightSprite.enabled = false;
    }

    // returns GameObject to be collected 
    public GameObject GetItem() => weaponCounterpart;

    // turns on/off highlight around sprite to let player know
    // that the object is/isn't pick up-able
    public void TogglePrompt()
    {
        highlightSprite.enabled = true;
    }
}
