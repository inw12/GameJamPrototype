using UnityEngine;

[RequireComponent(typeof(Damageable))]
public class Limb : MonoBehaviour
{
    void Start()
    {
        GetComponent<Damageable>().OnDeath += LoseLimb;
    }
    
    private void LoseLimb()
    {
        
    }
}