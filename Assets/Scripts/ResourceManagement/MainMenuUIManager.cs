using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUIManager : MonoBehaviour
{

    [Header("Main Menu")]
    [SerializeField]
    public GameObject mainMenu;
    [SerializeField]
    public GameObject chapterSelectMenu;
    [SerializeField]
    public GameObject accessibilityOptionsMainMenu;
    [SerializeField]
    public GameObject creditsMenu;
    [SerializeField]
    public GameObject quitConfirmationMenu;

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
        DontDestroyOnLoad(gameObject);

    }

    public void ReturnToMainMenu()
    {
        mainMenu.SetActive(true);
        chapterSelectMenu.SetActive(false);
        accessibilityOptionsMainMenu.SetActive(false);
        creditsMenu.SetActive(false);
        quitConfirmationMenu.SetActive(false);
    }
}
