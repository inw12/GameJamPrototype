using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private PlatformEffector2D platforms;
    [SerializeField] private PlatformEffector2D stairs;
    public static LevelManager Instance;
    private LayerMask playerLayer => GameManager.Instance.Player.gameObject.layer;

    public void Awake()
    {
        Instance = this;
    }

    public void ExcludePlayer()
    {
        platforms.colliderMask &= ~playerLayer;
        //stairs.colliderMask &= ~playerLayer;
    }

    public void IncludePlayer()
    {
        platforms.colliderMask |= playerLayer;
        //stairs.colliderMask |= playerLayer;
    }

}