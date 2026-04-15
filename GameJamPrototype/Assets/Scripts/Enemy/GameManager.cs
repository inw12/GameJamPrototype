using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public Transform Player;
    public static GameManager Instance;
    [SerializeField] private GameObject EndingCutscene;

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

    public void Restart()
    {
        AudioManager.Instance.PlayMusic("IntroA");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Ending()
    {
        EndingCutscene.SetActive(true);
        AudioManager.Instance.PlayMusic("Outro");
    }
}