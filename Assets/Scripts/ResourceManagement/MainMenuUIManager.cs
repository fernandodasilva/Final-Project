using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;
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

    [Header("Paged Menus")]
    [SerializeField]
    public GameObject[] helpPages;
    [SerializeField]
    public GameObject[] creditsPages;
    [SerializeField]
    private int currentHelpPageIndex = 0;
    [SerializeField]
    private int currentCreditsPageIndex = 0;
    [SerializeField]
    private Button previousPageButton;
    [SerializeField]
    private Button nextPageButton;


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
        currentCreditsPageIndex = 0;
        currentHelpPageIndex = 0;

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

    #region HELP MENU

    public void PreviousPageHelpMenu()
    {
        if (currentHelpPageIndex > 0)
        {
            currentHelpPageIndex--;
            nextPageButton.interactable = true;
            for (int i = 1 + currentHelpPageIndex; i == helpPages.Length; i++)
            {
                helpPages[i].SetActive(false);
            }
        }
        else
            currentHelpPageIndex = 0;
            helpPages[0].SetActive(true);
            previousPageButton.interactable = false;
          
    }

    public void NextPageHelpMenu()
    {
        if (currentHelpPageIndex < helpPages.Length)
        {
            previousPageButton.interactable = true;
            currentHelpPageIndex++;
            helpPages[currentHelpPageIndex].SetActive(true);
            for (int i = currentHelpPageIndex - 1; i == 0; i--)
            {
                helpPages[i].SetActive(false);
            }
        }
        else
            currentHelpPageIndex = helpPages.Length;
            helpPages[helpPages.Length].SetActive(true);
            nextPageButton.interactable = false;
    }

    #endregion



}