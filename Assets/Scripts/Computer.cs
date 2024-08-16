using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEditor.ShaderGraph.Drawing;
using UnityEngine;

public class Computer : InteractableObject
{

    public bool isPowerOn { get; private set; }
    public bool isSystemOpen { get; private set; }

    [SerializeField]
    private GameObject mainScreen;
    [SerializeField]
    private GameObject resultsScreen;
    private string searchBoxInput;

    private int inputData;

    public QuestGoalCheck questGoal { get; private set; }

    [SerializeField]
    public List<Book> books;



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

    public void OpenSystem()
    {
        Debug.Log("Opening PC system");
        GameManager.Instance.DisableMovement();
        isSystemOpen = true;
        HUDManager.instance.defaultCanvas.gameObject.SetActive(false);
        HUDManager.instance.computerCanvas.gameObject.SetActive(true);
        HUDManager.instance.ShowMouseCursor();
        mainScreen.gameObject.SetActive(true);
        resultsScreen.gameObject.SetActive(false);
    }

    public void CloseSystem()
    {
        Debug.Log("Closing PC system");
        isSystemOpen = false;
        HUDManager.instance.defaultCanvas.gameObject.SetActive(true);
        HUDManager.instance.computerCanvas.gameObject.SetActive(false);
        HUDManager.instance.HideMouseCursor();
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
        searchBoxInput = s;
        Debug.Log(searchBoxInput);
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
                int i ;
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

}
