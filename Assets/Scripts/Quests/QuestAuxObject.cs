using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestAuxObject : InteractableObject
{

    private Interactor interactionScript;
    // Start is called before the first frame update

    [SerializeField]
    public string postitTitle;
    [SerializeField]
    public List<string> sentences = new List<string>();
    [SerializeField]
    private float typingTime;

    private int index;
    public bool startedDialog { get; private set; }


    public override void Interact()
    {

        HUDManager.instance.questTitleText.text = postitTitle;
        HUDManager.instance.questDescriptionText.text = sentences[index];

        HUDManager.instance.acceptButton.SetActive(false);
        HUDManager.instance.rejectButton.SetActive(false);

        if (!startedDialog)
        {
            StartDialog();
            index = 0;
        }
        else if (HUDManager.instance.questDescriptionText.text == sentences[index])
        {
            NextLine();
        }
        else
        {
            StopAllCoroutines();
            HUDManager.instance.questDescriptionText.text = sentences[index];

        }

    }

    public void NextLine()
    {
        index++;

        if (index < sentences.Count)
        {
            StartCoroutine(WriteLine());
        }

        else
        {
            ResetInteraction();
            startedDialog = false;
            GameManager.Instance.EndInteraction();
        }
    }

    private void StartDialog()
    {
        startedDialog = true;
        index = 0;

        HUDManager.instance.questPanel.SetActive(true);
        StartCoroutine(WriteLine());
    }

    public override void ResetInteraction()
    {
        base.ResetInteraction();
        startedDialog = false;
        index = 0;

    }

    IEnumerator WriteLine()
    {
        HUDManager.instance.questDescriptionText.text = string.Empty;

        foreach (char ch in sentences[index].ToCharArray())
        {
            HUDManager.instance.questDescriptionText.text += ch;
            yield return new WaitForSeconds(typingTime);
        }
    }
}
