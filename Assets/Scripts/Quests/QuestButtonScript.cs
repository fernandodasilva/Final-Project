using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestButtonScript : MonoBehaviour
{

    public int questID;
    public Text questTitle;

    private GameObject acceptButton;
    private QuestButtonScript acceptButtonScript;

    public Quest questToAccept;

    // Start is called before the first frame update

    private void Start()
    {

        acceptButton = HUDManager.instance.acceptButton;
        acceptButtonScript = acceptButton.GetComponent<QuestButtonScript>();

    }

    private void Update()
    {
        QuestManager.instance.currentQuest.id = questID;

    }

    public void AcceptQuest()
    {
 
        HUDManager.instance.questPanel.SetActive(false);
        GameManager.Instance.EnableMovement();
        HUDManager.instance.HideMouseCursor();
        QuestManager.instance.StartQuest(questID);


    }

    public void RejectQuest()
    {
        GameManager.Instance.EndInteraction();
    }

}
