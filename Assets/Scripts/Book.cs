using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using static UnityEditor.Progress;
using UnityEngine.Events;
using TMPro;



public class Book : I_Collector
{


    private GameObject mainCamera;
    public bool isInCorrectDropPlace;
    public bool isInUse;

    public bool paperSheetActive = false;

    [SerializeField]
    public string bookTitle;
    [SerializeField]
    public string bookText;
    [SerializeField]
    public string bookSubject;
    [SerializeField]
    public string bookAuthor;
    [SerializeField]
    public int id;
    [SerializeField]
    public int tagNumber;

    [SerializeField]
    private DropPlace targetDropPlace;

    private I_Interactor interactorScript;
    private I_Collector collectionScript;


    private void Awake()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        interactorScript = GetComponent<I_Interactor>();
        collectionScript = GetComponent<I_Collector>();
        isInUse = collectionScript.isOnUse;
    }

    // Update is called once per frame

    public void ToggleBook()
    {
        if (HUDManager.instance.paperSheet.activeSelf)
        {
            HUDManager.instance.paperSheet.SetActive(false);
        }
        else
        HUDManager.instance.paperSheet.SetActive(true);
        HUDManager.instance.papersheetTitle.text = bookTitle;
        HUDManager.instance.papersheetText.text = bookText;
    }


    public void DeactivateInteraction() //a usar quando o livro for colocado no lugar certo
    {
       Deactivate();
       interactorScript.GetComponent<Outline>().enabled = false;
    
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject currentItem = other.gameObject;

        if (collectionScript.isOnUse && currentItem.GetComponent<DropPlace>() != null)
        {
            DropPlace dp = currentItem.GetComponent<DropPlace>();
            if (id == dp.id)
            {
                HUDManager.instance.CorrectDropPlace();
                isInCorrectDropPlace = true;
                Debug.Log("Current Book ID: " + id + ", current drop place ID:" + dp.id + ", correct: " + isInCorrectDropPlace);
            }
            else
            {
                HUDManager.instance.WrongDropPlace();
                isInCorrectDropPlace = false;
                Debug.Log("Current Book ID: " + id + ", current drop place ID:" + dp.id + ", correct place: " + isInCorrectDropPlace);

            }
        }

        if (!collectionScript.isOnUse && currentItem.GetComponent<DropPlace>() != null)
        {
            DropPlace dp = currentItem.GetComponent<DropPlace>();
            if (id == dp.id)
            {

                isInCorrectDropPlace = true;

                    collectionScript.Deactivate();
                    dp.GetComponent<Outline>().enabled = false;
                    this.transform.SetParent(dp.transform, false);
                    dp.Evaluate();
                    //e se eu colocar uns lerps aqui?
                    this.transform.position = dp.transform.position;
                    this.transform.eulerAngles = dp.transform.eulerAngles + targetRotation;



                Debug.Log("Current Book ID: " + id + ", current drop place ID:" + dp.id + ", correct: " + isInCorrectDropPlace);
            }
            else
            {
                HUDManager.instance.WrongDropPlace();
                isInCorrectDropPlace = false;
                Debug.Log("Current Book ID: " + id + ", current drop place ID:" + dp.id + ", correct place: " + isInCorrectDropPlace);

            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        GameObject currentItem = other.gameObject;
        if(currentItem.GetComponent<DropPlace>() != null)
        {
            interactorScript.instructionMessage = interactorScript.defaultInstructionMessage;
        }
    }


}