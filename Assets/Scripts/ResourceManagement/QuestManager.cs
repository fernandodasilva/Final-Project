using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Quest;

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;

    public List<GameObject> quests = new List<GameObject>(); //lista de missões default

    public Quest currentQuest;

    private bool allQuestsComplete;

    //a fazer: permitir que o jogador faça apenas uma quest por vez

    //criador de singleton
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
    }

    private void Start()
    {

        quests[0].GetComponent<Quest>().progress = Quest.QuestStatus.AVAILABLE;
        for (int i = 1; i < quests.Count; i++)
        {
            quests[i].GetComponent<Quest>().progress = Quest.QuestStatus.NOT_AVAILABLE;
        }
    }




    //ACCEPT A QUEST

    public void StartQuest(int questID) //original AcceptQuest
    {
        questID = currentQuest.id;
        currentQuest.progress = QuestStatus.STARTED;
        Debug.Log("Quest " + currentQuest.id + ", named " + currentQuest.title + " is " + currentQuest.progress);
        GameManager.Instance.EndInteraction();

    }



    public void RejectQuest()
    {
        GameManager.Instance.EndInteraction();
    }

}
