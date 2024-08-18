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
    public static HUDManager instance;
    private UIModeManager modeManager;

    [SerializeField]
    public Canvas defaultCanvas;
    [SerializeField]
    public Canvas computerCanvas;
    [SerializeField]
    public Canvas pauseMenuCanvas;

    [SerializeField]
    private GameObject pickupUI;

    [Header("Object Interaction Box")]
    [SerializeField]
    TMP_Text interactionText;

    [SerializeField]
    GameObject objectDescriptionBox;

    [SerializeField]
    public GameObject paperSheet;

    [SerializeField]
    public TMP_Text papersheetTitle;
    [SerializeField]
    public TMP_Text papersheetText;

    [SerializeField]
    public TMP_Text interactionOptionText;

    [SerializeField]
    public TMP_Text questTitleText;
    [SerializeField]
    public TMP_Text questDescriptionText;

    [SerializeField]
    public string correctDropPlaceText;
    [SerializeField]
    public string wrongDropPlaceText;
    [SerializeField]
    public string defaultDropPlaceText;
    [SerializeField]
    public string bookAlreadyInDropPlaceText;

    [SerializeField]
    GameObject pausePanel;

    [SerializeField]
    public GameObject questPanel;

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


    [Header("Quest Panel Buttons")]
    //botões
    [SerializeField]
    public GameObject acceptButton;
    [SerializeField]
    public GameObject rejectButton;

    private QuestObject currentQuestObject;

    [Header("Pause Menu")]
    //canvases
    [SerializeField]
    public GameObject mainPauseMenu;
    [SerializeField]
    public GameObject inGameSettingsMenu;
    [SerializeField]
    public GameObject inGameQuitConfirmationMenu;




    public enum DarkModeTextColor { White, Yellow };
    public DarkModeTextColor darkTextColor {get; private set;}

    public bool isPaperSheetOn { get; private set; }

    public bool isQuestAvailable = false;
    public bool isQuestFinished = false;
    public bool isQuestRunning = false;
    private bool isQuestPanelActive = false;

    [SerializeField]
    public string startedQuestMessage;
    [SerializeField]
    public string anotherQuestInProgressMessage;
    [SerializeField]
    public string questNotAvailableMessage;

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
    }

    public void EnableInteractionBox(string text, string clickText)
    {
        if (GetSheetStatus() == false)
        {
            interactionText.text = text;
            interactionOptionText.text = clickText;
            objectDescriptionBox.SetActive(true);
        }
    }

    private bool GetSheetStatus()
    {
        if (objectDescriptionBox.activeSelf == true)
        {
            return true;
        }
        else { return false; }
    }

    public void DisableInteractionBox()
    {
        objectDescriptionBox.SetActive(false);
    }

    public void ResetCursor()
    {
        currentCursor.GetComponent<RawImage>().texture = defaultCursor;
    }


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


    public void SwitchPauseMenuCanvas(bool value)
    {
        if (value == true)
        {
            defaultCanvas.gameObject.SetActive(false);
            pauseMenuCanvas.gameObject.SetActive(true);
            ShowMouseCursor();
        }
        else
        {
            defaultCanvas.gameObject.SetActive(true);
            pauseMenuCanvas.gameObject.SetActive(false);
            HideMouseCursor();
        }
    }

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

    public void ReturnToMainMenu()
    {

    }

}
