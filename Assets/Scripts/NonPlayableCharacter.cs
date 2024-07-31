using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class NonPlayableCharacter : MonoBehaviour
    //TO BE DELETED LATER
{

    // Start is called before the first frame update

    [SerializeField]
    public string characterName;
    [SerializeField, TextArea(1, 5)]
    public string[] sentences;

    public bool startedDialog { get; private set; }
    private int index;
    [SerializeField]
    private float typingTime;

    private void Start()
    {
        HUDManager.instance.questDescriptionText.text = "";
        StartDialog();
    }

    private void Update()
    {
        if (!startedDialog)
        {
            StartDialog();
        }
    }

    public void Interact()
    {
        if (!startedDialog)
        {
            StartDialog();

        }
        if (startedDialog && HUDManager.instance.questDescriptionText.text == sentences[index])
        {
            NextLine();
        }
        else
        {
            StopAllCoroutines();
        }
    }

    public void NextLine()
    {
            if (index < sentences.Length -1)
            {
            index++;
            HUDManager.instance.questDescriptionText.text = "";
            StartCoroutine(WriteLine());
            }

        else
        {
            startedDialog = false;
        }
        Debug.Log("Line number:" + index);
    }

    private void StartDialog()
    {
        startedDialog = true;
        index = 0;
        HUDManager.instance.questDescriptionText.text = "";
        HUDManager.instance.questPanel.SetActive(true);
    //    HUDManager.instance.ShowQuestPanel(true);
        HUDManager.instance.questTitleText.text = characterName;
        StartCoroutine(WriteLine());
    }

    IEnumerator WriteLine()
    {

        foreach (char ch in sentences[index].ToCharArray())
        {
            HUDManager.instance.questDescriptionText.text += ch;
            yield return new WaitForSeconds(typingTime);
        }
    }
}
