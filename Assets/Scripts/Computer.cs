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

    public QuestGoalCheck questGoal { get; private set; }


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

}
