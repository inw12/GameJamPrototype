using UnityEngine;
[RequireComponent(typeof(CircleCollider2D))]
public class WeaponPickup : MonoBehaviour
{
    public string PromptText { get; private set; }

    [SerializeField] private GameObject weaponCounterpart;
    [SerializeField] private GameObject pickupPrompt;
    [SerializeField] private float pickupRadius;

    private CircleCollider2D _collider;
    private float _highlightTimer;

    void Start()
    {
        PromptText = "(E)";

        _collider = GetComponent<CircleCollider2D>();
        _collider.radius = pickupRadius;

        pickupPrompt.SetActive(false);
    }

    void Update()
    {
        if (pickupPrompt.activeInHierarchy) _highlightTimer += Time.deltaTime;

        if (_highlightTimer >= 0.1f)
        {
            pickupPrompt.SetActive(false);
        }
    }

    // returns GameObject to be collected 
    public GameObject GetItem() => weaponCounterpart;
    public void DestroyObject() => Destroy(transform.parent.gameObject);

    // interactable feedback toggle (highlight + prompt text)
    public void TogglePrompt()
    {
        pickupPrompt.SetActive(true);
        _highlightTimer = 0f;
    }
}
