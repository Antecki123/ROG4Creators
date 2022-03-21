using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject loadGamePanel;
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private GameObject creditsPanel;

    [Header("Menu Buttons")]
    [SerializeField] private GameObject newGameButton;
    [SerializeField] private GameObject loadGameButton;
    [SerializeField] private GameObject optionsButton;
    [SerializeField] private GameObject creditsButton;
    [SerializeField] private GameObject exitButton;

    public void NewGameButton()
    {
        SceneManager.LoadSceneAsync("SampleScene", LoadSceneMode.Single);
        SceneManager.LoadSceneAsync("UI", LoadSceneMode.Additive);

        SceneManager.UnloadSceneAsync("MainMenu", UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
    }

    public void LoadGameButton()
    {

    }

    public void OptionsButton()
    {

    }

    public void CreditsButton()
    {

    }

    public void ExitButton() => Application.Quit();
}
