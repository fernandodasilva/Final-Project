using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestObject : InteractableObject
{ 


    public Quest objectQuest;
    public QuestButtonScript acceptButton;


    [SerializeField]
    private GameObject questMarker;
    [SerializeField]
    private Image questImage;

    [SerializeField]
    private Sprite questAvailableSprite;
    [SerializeField]
    private Sprite questReceivableSprite;

    void SetQuestMarker()
    {

        {
            questMarker.SetActive(false);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        SetQuestMarker();
        HUDManager.instance.questDescriptionText.text = this.objectQuest.description;
        acceptButton = HUDManager.instance.acceptButton.GetComponent<QuestButtonScript>();
        StartDialog();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void Interact()
    {

            StartDialog();
            HUDManager.instance.acceptButton.GetComponent<QuestButtonScript>().questToAccept = objectQuest;


         if (HUDManager.instance.questDescriptionText.text == sentences[index])
        {
            NextLine();
        }
        else
        {
            StopAllCoroutines();
            HUDManager.instance.questDescriptionText.text = sentences[index];
        }

        if (objectQuest.progress == Quest.QuestStatus.AVAILABLE)
        {
            HUDManager.instance.ShowMouseCursor();

            GameManager.Instance.DisableMovement();
            HUDManager.instance.acceptButton.SetActive(true);
            HUDManager.instance.rejectButton.SetActive(true);
            QuestManager.instance.currentQuest = objectQuest;
            return;

        }

        else
        {
            HUDManager.instance.acceptButton.SetActive(false);
            HUDManager.instance.rejectButton.SetActive(false);

        }


        if (objectQuest.progress == Quest.QuestStatus.STARTED)
        {
            ResetInteraction();
            HUDManager.instance.questDescriptionText.text = string.Empty;
            sentences.Clear();
            sentences.Add(HUDManager.instance.startedQuestMessage);

            GameManager.Instance.EnableMovement();
            HUDManager.instance.HideMouseCursor();
            return;

        }
        if (objectQuest.progress == Quest.QuestStatus.NOT_AVAILABLE)
        {
            ResetInteraction();
            HUDManager.instance.questDescriptionText.text = string.Empty;
            sentences.Add(HUDManager.instance.questNotAvailableMessage);

            GameManager.Instance.EnableMovement();
            HUDManager.instance.HideMouseCursor();
            return;

        }
        if (objectQuest.progress == Quest.QuestStatus.FINISHED)
        {
            ResetInteraction();
            HUDManager.instance.questDescriptionText.text = string.Empty;
            sentences.Add(objectQuest.finishMessage);

            GameManager.Instance.EnableMovement();
            HUDManager.instance.HideMouseCursor();
            return;
        }
    }

    public override void ResetInteraction()
    {
        index = 0;
    }


    [SerializeField, TextArea(1, 5)]
    public List<string> sentences = new List<string>();

    public bool startedDialog { get; private set; }
    private int index;
    [SerializeField]
    private float typingTime;

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

        OpenQuestWindow();
        index = 0;
            sentences.Add(objectQuest.description);



            StartCoroutine(WriteLine());
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

    public void OpenQuestWindow()
    {
        HUDManager.instance.questPanel.SetActive(true);
        HUDManager.instance.questTitleText.text = objectQuest.title;
        HUDManager.instance.questDescriptionText.text = objectQuest.description;

    }


    public void AcceptQuest()
    {
        objectQuest.StartQuest();
        GameManager.Instance.CurrentQuest = objectQuest;
    }

}
