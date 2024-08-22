using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUIManager : MonoBehaviour
{
    [SerializeField]
    private Canvas MainMenuUICanvas;

    [Header("Main Menu")]
    [SerializeField]
    public GameObject mainMenu;
    [SerializeField]
    public GameObject HelpMenu;
    [SerializeField]
    public GameObject accessibilityOptionsMainMenu;
    [SerializeField]
    public GameObject creditsMenu;
    [SerializeField]
    public GameObject quitConfirmationMenu;

    [Header("Loading Screen")]
    [SerializeField]
    public GameObject loadingScreen;
    [SerializeField]
    public Slider loadingSlider;
    

    public static MainMenuUIManager instance;
    private UIModeManager modeManager;

    // Start is called before the first frame update

    private void Awake()
    {
        if (instance == null) 
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void ReturnToMainMenu()
    {
        mainMenu.SetActive(true);
        HelpMenu.SetActive(false);
        accessibilityOptionsMainMenu.SetActive(false);
        creditsMenu.SetActive(false);
        quitConfirmationMenu.SetActive(false);
    }

    public void OpenHelpMenu()
    {
        mainMenu.SetActive(false);
        HelpMenu.SetActive(true);
        accessibilityOptionsMainMenu.SetActive(false);
        creditsMenu.SetActive(false);
        quitConfirmationMenu.SetActive(false);
    }

    public void OpenAccessibilityOptionsMenu()
    {
        mainMenu.SetActive(false);
        HelpMenu.SetActive(false);
        accessibilityOptionsMainMenu.SetActive(true);
        creditsMenu.SetActive(false);
        quitConfirmationMenu.SetActive(false);
    }

    public void OpenCreditsMenu()
    {
        mainMenu.SetActive(false);
        HelpMenu.SetActive(false);
        accessibilityOptionsMainMenu.SetActive(false);
        creditsMenu.SetActive(true);
        quitConfirmationMenu.SetActive(false);
    }

    public void OpenQuitConfirmationMenu()
    {
        mainMenu.SetActive(false);
        HelpMenu.SetActive(false);
        accessibilityOptionsMainMenu.SetActive(false);
        creditsMenu.SetActive(false);
        quitConfirmationMenu.SetActive(true);
    }

    public void LoadLevel()
    {
        mainMenu.SetActive(false);
        HelpMenu.SetActive(false);
        accessibilityOptionsMainMenu.SetActive(false);
        creditsMenu.SetActive(false);
        quitConfirmationMenu.SetActive(false);
        loadingScreen.SetActive(true);
    }

    public void HideLoadingScreen()
    {
        MainMenuUICanvas.gameObject.SetActive(false);
    }

    public void ShowMainMenu()
    {
        MainMenuUICanvas.gameObject.SetActive(true);
    }
}