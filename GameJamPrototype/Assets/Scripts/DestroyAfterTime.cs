using UnityEngine;
public class DestroyAfterTime : MonoBehaviour
{
    [SerializeField] private float timeToDestroy;
    private float _destroyTimer;

    void Update()
    {
        _destroyTimer += Time.deltaTime;
        if (_destroyTimer >= timeToDestroy)
        {
            Destroy(gameObject);
            return;
        }
    }
}
