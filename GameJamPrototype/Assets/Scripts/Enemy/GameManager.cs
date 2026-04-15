using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Transform Player;
    public static GameManager Instance;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Debug.LogError("Duplicate GameManager");
    }

    public void GameOver()
    {
        
    }
}