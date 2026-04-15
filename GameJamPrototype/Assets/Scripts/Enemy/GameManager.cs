using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public Transform Player;
    public static GameManager Instance;
    public bool GameIsOver => _gameOver;
    private bool _gameOver = false;
    [SerializeField] private GameObject EndingCutscene;
    [SerializeField] private GameObject DeathScreen;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Debug.LogError("Duplicate GameManager");
    }

    public void GameOver()
    {
        _gameOver = true;
        DeathScreen.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
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