using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Xml;
using JetBrains.Annotations;

public class HUDManager : MonoBehaviour
{
    #region Variáveis de managers
    public static HUDManager instance;
    private UIModeManager modeManager;
    #endregion

    #region Variáveis de Canvas
    [SerializeField]
    public Canvas defaultCanvas;
    [SerializeField]
    public Canvas computerCanvas;
    [SerializeField]
    public Canvas pauseMenuCanvas;
    #endregion

    [SerializeField]
    private GameObject pickupUI;

    [Header("Object Interaction Box")]
    [SerializeField]
    TMP_Text interactionText;

    #region folha do livro
    [Header("Book cover representation")]
    [SerializeField]
    public GameObject paperSheet;
    [SerializeField]
    public Material defaultPaperSheetMaterial;
    private Material currentPaperSheetMaterial;


    [SerializeField]
    public TMP_Text papersheetTitle;
    [SerializeField]
    public TMP_Text papersheetText;

    #endregion

    [SerializeField]
    public TMP_Text interactionOptionText;
    [SerializeField]
    GameObject objectDescriptionBox;

    [SerializeField]
    public TMP_Text questTitleText;
    [SerializeField]
    public TMP_Text questDescriptionText;
    [SerializeField]
    public GameObject questPanel;

    #region Variáveis de drop place
    [SerializeField]
    public string correctDropPlaceText;
    [SerializeField]
    public string wrongDropPlaceText;
    [SerializeField]
    public string defaultDropPlaceText;
    [SerializeField]
    public string bookAlreadyInDropPlaceText;
    #endregion

    #region Cursores do mouse
    private GameObject currentCursor;

    [Header("Mouse Cursors")]
    [SerializeField]
    GameObject cursorPointer;
    [SerializeField]
    Texture2D defaultCursor;
    [SerializeField]
    Texture2D selectableCursor;
    [SerializeField]
    Texture2D interactableCursor;
    [SerializeField]
    Texture2D characterCursor;
    [SerializeField]
    Texture2D enemyCursor;
    [SerializeField]
    Texture2D collectibleCursor;
    [SerializeField]
    Texture2D dropPlaceCursor;
    #endregion

    [Header("Quest Panel Buttons")]
    //botões
    [SerializeField]
    public GameObject acceptButton;
    [SerializeField]
    public GameObject rejectButton;
    private QuestObject currentQuestObject;

    #region Menu de pausa
    [Header("Pause Menu")]
    //canvases
    [SerializeField]
    public GameObject mainPauseMenu;
    [SerializeField]
    public GameObject inGameSettingsMenu;
    [SerializeField]
    public GameObject inGameQuitConfirmationMenu;
    [SerializeField]
    public GameObject inGameHelpMenu;
    [SerializeField]
    public TMP_Text currentQuestTitle;
    [SerializeField]
    public TMP_Text currentQuestDescription;
    #endregion



    #region Variáveis do modo de UI (claro ou escuro)
    public enum DarkModeTextColor { White, Yellow };
    public DarkModeTextColor darkTextColor {get; private set;}
    #endregion

    public bool isPaperSheetOn { get; private set; }


    public bool isQuestAvailable = false;
    public bool isQuestFinished = false;
    public bool isQuestRunning = false;
    private bool isQuestPanelActive = false;

    #region Textos do Quest Panel
    [SerializeField]
    public string startedQuestMessage;
    [SerializeField]
    public string anotherQuestInProgressMessage;
    [SerializeField]
    public string questNotAvailableMessage;
    #endregion

    #region Awake (singleton) / Start
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

        HideQuestPanel();
        modeManager = GetComponent<UIModeManager>();
    }


    private void Start()
    {
       currentCursor = cursorPointer;
       currentCursor.GetComponent<RawImage>().texture = defaultCursor;
       DisableInteractionBox();
       questDescriptionText.text = string.Empty;
        defaultCanvas.enabled = true;
        computerCanvas.gameObject.SetActive(false);
        questPanel.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        darkTextColor = DarkModeTextColor.White;
        currentPaperSheetMaterial = defaultPaperSheetMaterial;
    }
    #endregion

    #region Funções para ativar ou desativar a caixa de interação
    public void EnableInteractionBox(string text, string clickText)
    {
        if (GetSheetStatus() == false)
        {
            interactionText.text = text;
            interactionOptionText.text = clickText;
            objectDescriptionBox.SetActive(true);
        }
    }

    public void DisableInteractionBox()
    {
        objectDescriptionBox.SetActive(false);
    }
    #endregion

    private bool GetSheetStatus()
    {
        if (objectDescriptionBox.activeSelf == true)
        {
            return true;
        }
        else { return false; }
    }

    #region Funções para alterar o ícone do cursor
    public void ChangeCursor_SELECTABLE()
    {
        currentCursor.GetComponent<RawImage>().texture = selectableCursor;
    }

    public void ChangeCursor_INTERACTABLE()
    {
        currentCursor.GetComponent<RawImage>().texture = interactableCursor;
    }

    public void ChangeCursor_CHARACTER()
    {
        currentCursor.GetComponent<RawImage>().texture = characterCursor;
    }

    public void ChangeCursor_ENEMY()
    {
        currentCursor.GetComponent<RawImage>().texture = enemyCursor;
    }

    public void ChangeCursor_COLLECTIBLE()
    {
        currentCursor.GetComponent<RawImage>().texture = collectibleCursor;
    }

    public void ChangeCursor_DROPPLACE()
    {
        currentCursor.GetComponent<RawImage>().texture = dropPlaceCursor;
    }

    public void ResetCursor()
    {
        currentCursor.GetComponent<RawImage>().texture = defaultCursor;
    }
    #endregion

    public void ToggleBookSheet(bool value)
    {
        if (value == true)
        {
            isPaperSheetOn = true;
            paperSheet.SetActive(true);
        }
        else
        {
            isPaperSheetOn = false;
            paperSheet.SetActive(false);
        }
    }

    public void ToggleInteractionOptionText(bool value)
    {
        if (value == true)
        {
            interactionOptionText.enabled = true;
        }
        else
        {
            interactionOptionText.enabled=false;

        }
    }

    #region Quest Panel
    public void ShowQuestPanel()
    {

            isQuestPanelActive = true;
            questPanel.SetActive(isQuestPanelActive);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void HideQuestPanel()
    {
        isQuestAvailable = false;
        isQuestRunning = false;
        isQuestPanelActive = false;
        questPanel.SetActive(isQuestPanelActive);

        questTitleText.text = "";
        questDescriptionText.text = "";

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

    }
    #endregion

    #region Mostrar ou esconder o ponteiro do mouse
    public void ShowMouseCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void HideMouseCursor() 
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    #endregion


    #region Mudar entre canvas
    public void SwitchCanvas(bool value)
    {
        if (value == true)
        {
            defaultCanvas.gameObject.SetActive(false);
            computerCanvas.gameObject.SetActive(true);
            ShowMouseCursor();
        }
        else
        {
            defaultCanvas.gameObject.SetActive(true);
            computerCanvas.gameObject.SetActive(false);
            HideMouseCursor();
        }
    }

    public void ShowPauseCanvas(bool value)
    {
            defaultCanvas.gameObject.SetActive(!value);
            pauseMenuCanvas.gameObject.SetActive(value);

        if (value == true)
        {
            ShowMouseCursor();
        }
            else
            HideMouseCursor();
 
    }
    #endregion

    public void CorrectDropPlace()
    {
        interactionOptionText.text = correctDropPlaceText;
    }

    public void WrongDropPlace()
    {
        interactionOptionText.text = wrongDropPlaceText;
    }

    public void ToggleInterfaceMode()
    {
        modeManager.ResolveColorMode();
    }

    public DarkModeTextColor ChangeDarkModeTextColor(DarkModeTextColor color)
    {
        if(color == DarkModeTextColor.White)
        {
            color = DarkModeTextColor.Yellow;
        }
        else if (color == DarkModeTextColor.Yellow)
        {
            color = DarkModeTextColor.White;
        }

        return color;
    }

    #region Controle de paineis do menu de pausa
    public void ReturnToMainPauseMenu()
    {
        mainPauseMenu.gameObject.SetActive(true);
        inGameSettingsMenu.gameObject.SetActive(false);
        inGameHelpMenu.gameObject.SetActive(false);
        inGameQuitConfirmationMenu.gameObject.SetActive(false);
    }
    
    public void OpenInGameSettingsMenu()
    {
        mainPauseMenu.gameObject.SetActive(false);
        inGameSettingsMenu.gameObject.SetActive(true);
        inGameHelpMenu.gameObject.SetActive(false);
        inGameQuitConfirmationMenu.gameObject.SetActive(false);
    }

    public void OpenInGameHelpMenu()
    {
        mainPauseMenu.gameObject.SetActive(false);
        inGameSettingsMenu.gameObject.SetActive(false);
        inGameHelpMenu.gameObject.SetActive(true);
        inGameQuitConfirmationMenu.gameObject.SetActive(false);
    }

    public void OpenConfirmInGameReturnToMainMenuPanel()
    {
        mainPauseMenu.gameObject.SetActive(false);
        inGameSettingsMenu.gameObject.SetActive(false);
        inGameHelpMenu.gameObject.SetActive(false);
        inGameQuitConfirmationMenu.gameObject.SetActive(true);
    }
    #endregion

    public void ReturnToDefaultMaterial()
    {
//        paperSheet = defaultPaperSheetMaterial;
    }

    #region Informação da quest no menu de pause
    public void UpdatePauseModeQuestInfo(string questTitle, string questDescription)
    {
        currentQuestTitle.text = questTitle;
        currentQuestDescription.text = questDescription;
    }

    public void ResetPauseModeQuestInfo()
    {
        currentQuestTitle.text = "None";
        currentQuestDescription.text = "No quest in progress";
    }
    #endregion
}
