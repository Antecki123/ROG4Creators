using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PauseMenu : MonoBehaviour
{
    [Header("Component References")]
    [SerializeField] private Volume volume;
    [SerializeField] private GameObject HUD;

    [Header("Menu Buttons")]
    [SerializeField] private Button[] buttons;
    [SerializeField] private Image[] buttonsImages;
    [SerializeField] private Sprite activeButton;
    [SerializeField] private Sprite inactiveButton;

    private DepthOfField depth;
    private int weaponIndex;

    private InputControls inputActions;
    private void OnEnable()
    {
        Time.timeScale = 0;
        inputActions.Enable();

        HUD.SetActive(false);
        if (volume.profile.TryGet<DepthOfField>(out depth))
            depth.active = true;
    }

    private void OnDisable()
    {
        Time.timeScale = 1f;
        inputActions.Disable();

        HUD.SetActive(true);
        if (volume.profile.TryGet<DepthOfField>(out depth))
            depth.active = false;
    }

    private void Awake()
    {
        weaponIndex = 0;

        inputActions = new InputControls();
        inputActions.UI.Navigation.performed += ctx => ButtonNavigations((int)ctx.ReadValue<float>());
        inputActions.UI.Accept.performed += ctx => ButtonAccept();
        inputActions.UI.CloseMenu.performed += ctx => UserInterface.instance.OpenPausePanel();
    }

    private void ButtonNavigations(int direction)
    {
        weaponIndex += direction;
        weaponIndex = Mathf.Clamp(weaponIndex, 0, buttonsImages.Length - 1);

        for (int i = 0; i < buttonsImages.Length; i++)
        {
            if (i == weaponIndex)
                buttonsImages[i].sprite = activeButton;
            else
                buttonsImages[i].sprite = inactiveButton;
            SoundManager.instance.Play("MenuButtonsMove");
        }
    }

    private void ButtonAccept()
    {
        buttons[weaponIndex].onClick.Invoke();
        SoundManager.instance.Play("MenuButtonsAccept");
    }

    public void ResumeButton() => UserInterface.instance.OpenPausePanel();
    public void SaveButton() { }
    public void OptionsButton() { }
    public void ExitButton() => Application.Quit();
}