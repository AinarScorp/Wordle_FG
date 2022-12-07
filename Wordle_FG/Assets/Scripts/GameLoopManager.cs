using UnityEngine;
using UnityEngine.SceneManagement;
public class GameLoopManager : MonoBehaviour
{
    static GameLoopManager instance;

    bool gameIsBeingReplayed;
    bool restartAsEasyGame;
    bool gameWasWon;
    string goalWord = "Game has not been played";
    
    
    GameManager gameManager;

    public string TheWord => goalWord;

    public bool GameWasWon => gameWasWon;

    void Awake()
    {
        if (instance != null)
        {
            DestroyImmediate(this.gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);
        SceneManager.sceneLoaded += (scene, mode) => { OnSceneLoaded(); };
    }

    
    void OnSceneLoaded()
    {        
        gameManager = GameObject.FindWithTag("GameManager")?.GetComponent<GameManager>();

        SetupTheGame();
        TurnOffCanvas();
    }

    void SetupTheGame()
    {
        if (gameManager == null)
        {
            return;
        }
        gameManager.GameHasStarted += () =>
        {
            gameIsBeingReplayed = true;
            goalWord = gameManager.GetGoalWord().TheWord;
        };
        gameManager.GameIsOver += gameIsWon => gameWasWon = gameIsWon;
    }

    void TurnOffCanvas()
    {
        if (!gameIsBeingReplayed)
        {
            return;
        }
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas ==null || gameManager == null)
        {
            return;
        }

        canvas.gameObject.SetActive(false);
        gameManager.StartTheGame(restartAsEasyGame);
    }

    public void RestartGame(bool restartEasyGame)
    {
        restartAsEasyGame = restartEasyGame;
        ChangeScene(true);
    }

    public void ChangeScene(bool toMainPlayScene)
    {
        int sceneIndex = toMainPlayScene ? 0 : 1;
        SceneManager.LoadScene(sceneIndex);
    }
}
