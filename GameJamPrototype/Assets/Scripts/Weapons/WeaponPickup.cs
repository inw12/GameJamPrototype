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
    public void DestroyObject() => Destroy(transform.parent.gameObject);

    // interactable feedback toggle (highlight + prompt text)
    public void TogglePrompt()
    {
        highlightSprite.enabled = true;
    }
}
