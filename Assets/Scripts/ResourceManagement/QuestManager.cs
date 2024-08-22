using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Quest;

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;

    public List<QuestObject> quests; //lista de missões default

    public Quest currentQuest;
    public Quest nextQuest;

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
        quests[0].objectQuest.progress = Quest.QuestStatus.AVAILABLE;
        for (int i = 1; i < quests.Count; i++)
        {
            quests[i].objectQuest.progress = Quest.QuestStatus.NOT_AVAILABLE;
        }
    }

    private void Update()
    {
        if (currentQuest != null)
        { 
        currentQuest.CheckGoals();
        }
    }



    //ACCEPT A QUEST

    public void StartQuest(int questID) //original AcceptQuest
    {
        questID = currentQuest.id;
        currentQuest.progress = QuestStatus.STARTED;
        Debug.Log("Quest " + currentQuest.id + ", named " + currentQuest.title + " is " + currentQuest.progress);
        GameManager.Instance.EndInteraction();
        nextQuest = currentQuest.nextQuest.objectQuest;
        HUDManager.instance.UpdatePauseModeQuestInfo(currentQuest.title, currentQuest.description);
    }



    public void FinishQuest()
    {
        currentQuest.progress = QuestStatus.FINISHED;
        currentQuest.isActive = false;
        currentQuest.isFinished = true;
        RemoveQuest(currentQuest.thisQuest);

        int nextQuestID = currentQuest.id + 1;
        UnlockQuest(nextQuest);
        currentQuest = null;
    }

    public void UnlockQuest(Quest questToUnlock)
    {
        if (nextQuest != null)
        {
            questToUnlock.progress = QuestStatus.AVAILABLE;
        }
    }



    public void RejectQuest()
    {
        GameManager.Instance.EndInteraction();
        currentQuest = null;
    }

    public void RemoveQuest(QuestObject questToRemove)
    {
        quests.Remove(questToRemove);
    }

}
