using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http.Headers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Computer : InteractableObject
{

    public bool isPowerOn { get; private set; }
    public bool isSystemOpen { get; private set; }

    [SerializeField]
    private GameObject mainScreen;
    [SerializeField]
    private GameObject resultsScreen;

    [SerializeField]
    private TMP_Dropdown searchDropdown;
    [SerializeField]
    private TMP_InputField searchInputField;
    
    public QuestGoalCheck questGoal { get; private set; }

    [SerializeField]
    public List<Book> books;

    [Header("Controle da pesquisa")]
    [SerializeField]
    private int inputData;
    [SerializeField]
    private string selectedOption;

    [Header("Variáveis de teste")]
    [SerializeField]
    private string searchBoxInput;
    [SerializeField]
    private int searchBoxInput_asInt;



    public override void Interact()
    {
        if (GameManager.Instance.isPowerOn && !isPowerOn)
        {
            PowerOn();
        }
        if (isPowerOn)
        {
            OpenSystem();
        }
    }



    // Start is called before the first frame update
    void Start()
    {
        isPowerOn = false;
        isSystemOpen = false;
        questGoal = GetComponent<QuestGoalCheck>();

    }

    // Update is called once per frame

    public void PowerOn()
    {
        isPowerOn = true;
        Debug.Log("Computador ligado");
        questGoal.CompleteGoal();
    }

    public void ReturnToSearchMenu()
    {
        mainScreen.gameObject.SetActive(true);
        resultsScreen.gameObject.SetActive(false);
    }

    public void OpenSystem()
    {
        if (!isSystemOpen)
        {
            Debug.Log("Opening PC system");
            GameManager.Instance.DisableMovement();
            isSystemOpen = true;
            HUDManager.instance.defaultCanvas.gameObject.SetActive(false);
            HUDManager.instance.computerCanvas.gameObject.SetActive(true);
            HUDManager.instance.ShowMouseCursor();
            GameManager.Instance.ToggleComputerUse(true);
            ReturnToSearchMenu();
        }

    }

    public void CloseSystem()
    {
        Debug.Log("Closing PC system");
        isSystemOpen = false;
        HUDManager.instance.defaultCanvas.gameObject.SetActive(true);
        HUDManager.instance.computerCanvas.gameObject.SetActive(false);
        HUDManager.instance.HideMouseCursor();
        GameManager.Instance.ToggleComputerUse(false);
        GameManager.Instance.EnableMovement();
    }

    public void HandleInputData(int val)
    {
        if (val == 0)
        {
            inputData = 0;
            //search by title
        }

        if (val == 1) 
        {
            inputData = 1;
            //search by author
        }

        if (val == 2)
        {
            inputData = 2;
            //search by subject
        }

        if (val == 3)
        {
            inputData = 3;
            //search by system number
        }

        if (val == 4)
        {
            inputData = 4;
            //search by call number
        }
    }


    public void ShowSearchResults()
    {
        mainScreen.gameObject.SetActive(false);
        resultsScreen.gameObject.SetActive(true);

    }

    public void ReadStringInput(string s)
    {
        if (s == string.Empty)
            return;

        searchBoxInput = s;

        searchBoxInput_asInt = int.Parse(searchBoxInput);
    }

    public void ClearInput()
    {
        searchBoxInput = string.Empty;
        searchBoxInput_asInt = 0;
        searchInputField.text = String.Empty;
    }

    public Book GetBookByTitle(string bookName)
    {
        foreach(Book _book in books)
        {
            //
        }

        return null;
    }

    public Book GetBookByAuthor(string authorName)
    {
        return null;
    }

    public Book GetBookBySubject(string bookSubject)
    {
        return null;
    }

    public Book GetBookByID(int bookID)
    {
        return null;
    }

    public Book GetBookByCallNumber(int bookCallNumber)
    {
        return null;
    }

    public void StartSearch()
    {
        if (searchBoxInput != null || searchBoxInput == "" || searchBoxInput == " ")
        {
            switch (inputData)
            {
                case 0:
                    GetBookByTitle(searchBoxInput);
                    ShowSearchResults();
                    break;
                case 1:
                    GetBookByAuthor(searchBoxInput);
                    ShowSearchResults();
                    break;
                case 2:
                    GetBookBySubject(searchBoxInput);
                    ShowSearchResults();
                    break;
                case 3:
                    int i;
                    bool isInt = int.TryParse(searchBoxInput, out i);

                    if (isInt)
                    {
                        GetBookByID(i);
                        ShowSearchResults();
                        break;
                    }
                    else
                    {
                        searchBoxInput = string.Empty;
                        searchBoxInput = "Book ID must be a number";
                        break;
                    }

                case 4:
                    int j;
                    bool is_int = int.TryParse(searchBoxInput, out j);

                    if (is_int)
                    {
                        GetBookByCallNumber(j);
                        ShowSearchResults();
                        break;
                    }
                    else
                    {
                        searchBoxInput = string.Empty;
                        searchBoxInput = "Book Call Number must be a number";
                        break;
                    }
            }


        }
        else
        {
            mainScreen.GetComponentInChildren<InputField>().placeholder.GetComponent<Text>().text = "No search terms";
        }
    }

    public void GetSearchDropdownValue()
    {
        int pickedValue = searchDropdown.value;
        selectedOption = searchDropdown.options[pickedValue].text;
    }


    public void PreviousSearchResult()
    {

    }

    public void NextSearchResult() 
    {
    
    }
}
