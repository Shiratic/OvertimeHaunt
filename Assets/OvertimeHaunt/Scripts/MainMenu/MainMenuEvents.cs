using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
public class MainMenuEvents : MonoBehaviour
{
    private UIDocument _document;

    private VisualElement _rootMenu;
    private VisualElement _quitMenu;
    

    private Button _button;
    private Button _quitButton;
   


    [SerializeField] private string _startLevelName;

    private List<Button> _menuButtons = new List<Button>();

    private AudioSource _audioSource;
    private void Awake()
    {
        UnityEngine.Cursor.visible = true;
        _audioSource = GetComponent<AudioSource>();
        _document = GetComponent<UIDocument>();

        _rootMenu = _document.rootVisualElement.Q("RootMenu");
        _quitMenu = _document.rootVisualElement.Q("QuitMenu");
      
        _button = _document.rootVisualElement.Q("StartGameButton") as Button;
        _button.RegisterCallback<ClickEvent>(OnPlayGameClick);

        _quitButton = _document.rootVisualElement.Q("Quit") as Button;
        _quitButton.RegisterCallback<ClickEvent>(OnQuitButtonClick);

 
        _menuButtons = _document.rootVisualElement.Query<Button>().ToList();
        for (int i = 0; i < _menuButtons.Count; i++)
        {
            _menuButtons[i].RegisterCallback<ClickEvent>(OnAllButtonsClick);
        }

    }

    private void OnDisable()
    {
        _quitButton.UnregisterCallback<ClickEvent>(OnQuitButtonClick);
        _button.UnregisterCallback<ClickEvent>(OnPlayGameClick);

        for (int i = 0; i < _menuButtons.Count; i++)
        {
            _menuButtons[i].UnregisterCallback<ClickEvent>(OnAllButtonsClick);
        }
    }

    private void OnPlayGameClick(ClickEvent evt)
    {
        Debug.Log("You pressed the Start Button");
        SceneManager.LoadScene(_startLevelName);
    }

    private void OnAllButtonsClick(ClickEvent evt)
    {
        _audioSource.Play();
    }

    private void OnQuitButtonClick(ClickEvent evt)
    {
        Debug.Log("Quit Button Click");
        Application.Quit();
    }




}
