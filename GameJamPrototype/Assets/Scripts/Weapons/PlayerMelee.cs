using UnityEngine;
public class PlayerMelee : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [Space]
    [SerializeField] private GameObject meleeHitbox;
    [SerializeField] private Transform meleeSpawn;
    [Space]
    [SerializeField] private float damage;
    [SerializeField] private float meleeCooldown;

    private bool _meleeTriggered;
    private float _meleeTimer;

    void Start() => _meleeTimer = meleeCooldown;

    void Update()
    {
        _meleeTimer += Time.deltaTime;

        if (PlayerControls.Instance.Mouse2 && _meleeTimer >= meleeCooldown && !_meleeTriggered)
        {
            _meleeTriggered = true;

            var melee = Instantiate(meleeHitbox, meleeSpawn);
            //if (melee.TryGetComponent(out MeleeHitbox m))
            //{
            //    m.Initialize(hurtbox.gameObject, meleeDamage, Enemy);
            //}

            _meleeTimer = 0f;
            _meleeTriggered = false;
        }
    }
}
