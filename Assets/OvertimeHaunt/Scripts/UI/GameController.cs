using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
public class GameController : MonoBehaviour
{
    private GameState _currentState = GameState.Playing;

    int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    //private VisualElement _gameplayMenuVisualTree;
    private VisualElement _pauseMenuVisualTree;

    private Label _objectiveLabel;
    private Label _enemiesLeftLabel;

    private Button _quitButton;
    private Button _resumeButton;


    private VisualElement _loseMenuVisualTree;
    private Button _loseRetryButton;
    private Button _loseQuitButton;

    private VisualElement _winMenuVisualTree;
    private Button _winRetryButton;
    private Button _winQuitButton;


    private List<Button> _buttons = new List<Button>();

    private AudioSource _audioSource;

    private int _totalEnemies;
    private int _enemiesRemaining;

    [SerializeField] GameObject _door;
    [SerializeField] GameObject _door2;
    [SerializeField] GameObject _door3;
    [SerializeField] GameObject _door4;
    [SerializeField] GameObject _door5;
    [SerializeField] GameObject _door6;




    void Start()
    {
        CountEnemies();

        // Find UI Document for the objective display
        UIDocument objectiveDoc = GameObject.Find("ObjectiveUI").GetComponent<UIDocument>();
        VisualElement root = objectiveDoc.rootVisualElement;

        _objectiveLabel = root.Q<Label>("ObjectiveLabel");
        _enemiesLeftLabel = root.Q<Label>("EnemiesLeftLabel");

        UpdateEnemyUI();
    }

    private void UpdateEnemyUI()
    {
        if (_objectiveLabel != null)
            _objectiveLabel.text = "Objective: Defeat all enemies";

        if (_enemiesLeftLabel != null)
            _enemiesLeftLabel.text = $"Enemies Left: {_enemiesRemaining}";
    }


    public void EnemyDefeated()
    {
        _enemiesRemaining--;
        UpdateEnemyUI();

        if (_enemiesRemaining <= 0 && _currentState == GameState.Playing)
        {
            AllEnemiesDefeated();
        }
    }

    private void AllEnemiesDefeated()
    {
        if (_currentState != GameState.Playing)
            return;

        //_currentState = GameState.Won; // Prevents multiple calls
        UpdateEnemyUI();
        OpenDoor(); // Door logic still runs

    }

    private void CountEnemies()
    {
        GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");

        List<GameObject> realEnemies = new List<GameObject>();
        foreach (GameObject obj in allEnemies)
        {
            // Ensure it’s not a breakable (some designers may accidentally use both tags)
            if (!obj.CompareTag("Breakable"))
                realEnemies.Add(obj);
        }

        _totalEnemies = realEnemies.Count;
        _enemiesRemaining = _totalEnemies;
    }

    public void RecountEnemies()
    {
        CountEnemies();
        UpdateEnemyUI();
    }

    private void Awake()
    {

        _audioSource = GetComponent<AudioSource>();
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        //_gameplayMenuVisualTree = root.Q("GameplayMenuVisualTree");
        _pauseMenuVisualTree = root.Q("PauseMenuVisualTree");
        _winMenuVisualTree = root.Q("WinMenuVisualTree");
        _loseMenuVisualTree = root.Q("LoseMenuVisualTree");

        _objectiveLabel = root.Q<Label>("ObjectiveLabel");
        _enemiesLeftLabel = root.Q<Label>("EnemyCountLabel");



        _quitButton = root.Q("QuitButton") as Button;
        _quitButton.RegisterCallback<ClickEvent>(OnQuitButtonClick);

        _resumeButton = root.Q("ResumeButton") as Button;
        _resumeButton.RegisterCallback<ClickEvent>(OnResumeButtonClick);

        _winQuitButton = root.Q("WinQuitButton") as Button;
        _winQuitButton.RegisterCallback<ClickEvent>(OnWinQuitButtonClick);

        _winRetryButton = root.Q("WinRetryButton") as Button;
        _winRetryButton.RegisterCallback<ClickEvent>(OnWinRetryButtonClick);

        _loseQuitButton = root.Q("LoseQuitButton") as Button;
        _loseQuitButton.RegisterCallback<ClickEvent>(OnLoseQuitButtonClick);

        _loseRetryButton = root.Q("LoseRetryButton") as Button;
        _loseRetryButton.RegisterCallback<ClickEvent>(OnLoseRetryButtonClick);


        _buttons = root.Query<Button>().ToList();
        foreach (Button button in _buttons)
        {
            button.RegisterCallback<ClickEvent>(OnAnyButtonClick);
        }

        //_gameplayMenuVisualTree.style.display = DisplayStyle.Flex;
        _pauseMenuVisualTree.style.display = DisplayStyle.None;
        _winMenuVisualTree.style.display = DisplayStyle.None;
        _loseMenuVisualTree.style.display = DisplayStyle.None;


    }

     void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && _currentState == GameState.Playing)
        {
            OpenPauseMenu();
        }
    }

    private void OpenPauseMenu()
    {
        Debug.Log("Activate Pause Menu!");

        _pauseMenuVisualTree.style.display = DisplayStyle.Flex;
        _currentState = GameState.Paused;
   
        UnityEngine.Cursor.visible = true;
        // Optional: freeze game time
        Time.timeScale = 0f;
    }

    private void OnQuitButtonClick(ClickEvent evt)
    {

        SceneManager.LoadScene("MainMenu");
        Debug.Log("Quit!");

        Time.timeScale = 1f;
    }

    private void OnResumeButtonClick(ClickEvent evt)
    {
        Debug.Log("Resume!");
        _pauseMenuVisualTree.style.display = DisplayStyle.None;
        _currentState = GameState.Playing;

        // Optional: unfreeze game time
        Time.timeScale = 1f;
    }

    public void DisplayWinMenu()
    {
        _winMenuVisualTree.style.display = DisplayStyle.Flex;
        _currentState = GameState.Won;

        if (_objectiveLabel != null)
            _objectiveLabel.style.display = DisplayStyle.None;
        if (_enemiesLeftLabel != null)
            _enemiesLeftLabel.style.display = DisplayStyle.None;

        UnityEngine.Cursor.visible = true;
        // Optional: freeze game
        Time.timeScale = 0f;
    }

    private void OnWinRetryButtonClick(ClickEvent evt)
    {
        _winMenuVisualTree.style.display = DisplayStyle.None;
        SceneManager.LoadScene(currentSceneIndex);
        Time.timeScale = 1f;
    }

    private void OnWinQuitButtonClick(ClickEvent evt)
    {

        SceneManager.LoadScene("MainMenu");
        Debug.Log("Quit!");

        Time.timeScale = 1f;
    }

    public void DisplayLoseMenu()
    {
        _loseMenuVisualTree.style.display = DisplayStyle.Flex;
        _currentState = GameState.Lost;

        if (_objectiveLabel != null)
            _objectiveLabel.style.display = DisplayStyle.None;
        if (_enemiesLeftLabel != null)
            _enemiesLeftLabel.style.display = DisplayStyle.None;

        UnityEngine.Cursor.visible = true;

        // Optional: freeze game
        Time.timeScale = 0f;
    }

    private void OnLoseRetryButtonClick(ClickEvent evt)
    {
        _loseMenuVisualTree.style.display = DisplayStyle.None;
        SceneManager.LoadScene(currentSceneIndex);
        Time.timeScale = 1f;

    }

    private void OnLoseQuitButtonClick(ClickEvent evt)
    {

        SceneManager.LoadScene("MainMenu");
        Debug.Log("Quit!");

        Time.timeScale = 1f;
    }

    private void OpenDoor()
    {
        _door.SetActive(false);
        _door2.SetActive(false);
        _door3.SetActive(false);
        _door4.SetActive(false);
        _door5.SetActive(false);
        _door6.SetActive(false);
    }

    private void OnAnyButtonClick(ClickEvent evt)
    {
        Debug.Log("Any Button");
        _audioSource.Play();
    }

    private void OnDisable()
    {
        _quitButton.UnregisterCallback<ClickEvent>(OnQuitButtonClick);
        _resumeButton.UnregisterCallback<ClickEvent>(OnResumeButtonClick);
        foreach (Button button in _buttons)
        {
            button.UnregisterCallback<ClickEvent>(OnAnyButtonClick);
        }
    }

    public enum GameState
    {
        Playing,
        Paused,
        Won,
        Lost
    }

}
