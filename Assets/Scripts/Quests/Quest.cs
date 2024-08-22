using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;


[System.Serializable] //ok � serializable mesmo
public class Quest
{
    public int id;      //ID da miss�o em si - SER� NECESS�RIO para pegar os dados da miss�o pelo ID
    public string title; //nome da miss�o
    public string description; //descri��o da miss�o
    public int nextQuestID;

    public List<GameObject> questGoals;

    public enum QuestStatus { NOT_AVAILABLE, AVAILABLE, STARTED, FINISHED } //ok
    public bool isFinished;
    public bool isActive;
    public QuestStatus progress; //estado da miss�o atual (enum)

    public string finishMessage; //a mostrar quando a miss�o for completada
    private bool allGoalsCleared;

    [SerializeField]
    public QuestObject nextQuest; //pr�xima quest, se houver alguma
    public QuestObject thisQuest;

    

    //m�todos
    public void StartQuest()
    {

    allGoalsCleared = false;
    progress = QuestStatus.STARTED;
    QuestManager.instance.currentQuest = this;
    QuestManager.instance.currentQuest.id = id;
    HUDManager.instance.UpdatePauseModeQuestInfo(title, description);

    }

    public void FinishQuest()
    {
        progress = QuestStatus.FINISHED;
        isFinished = true;
        isActive = false;


        GameManager.Instance.FinishQuest();
        Debug.Log("Quest " + id + ", named " + title + " " + progress);

        if (nextQuest != null)
        {
            QuestManager.instance.UnlockQuest(nextQuest.objectQuest);
            Debug.Log("Quest " + nextQuest.objectQuest.id + " available");
        }
    }
    public void CheckGoals()
    {
        foreach (GameObject go in questGoals)
        {
            QuestGoalCheck goalStatus = go.GetComponent<QuestGoalCheck>();
            if (goalStatus.isComplete == false)
            {
                allGoalsCleared = false;
                Debug.Log("QUEST " + id + " IN PROGRESS");
                return;
            }

            else
                allGoalsCleared = true;
        }
        if (allGoalsCleared)
        {
            FinishQuest();
        }
    }
}
