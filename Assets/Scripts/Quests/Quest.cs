using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[System.Serializable]
public class Quest 
{
    public int id;      //ID da miss�o em si
    public string title; //nome da miss�o, se for necess�rio
    public string description; //descri��o da miss�o, vinda de quem dar a miss�o

    public List<GameObject> questGoals;

    public enum QuestStatus { NOT_AVAILABLE, AVAILABLE, STARTED, FINISHED }
    public bool isFinished;
    public bool isActive;
    public QuestStatus progress; //estado da miss�o atual (enum)

    [SerializeField]
    public string finishMessage; //a mostrar quando a miss�o for completada, vinda de quem dar a miss�o
    private bool allGoalsCleared;

    [SerializeField]
    public GameObject nextQuest;

    //m�todos
    public void StartQuest()
    {
    isActive = true;
    allGoalsCleared = false;
    progress = QuestStatus.STARTED;
        Debug.Log("Quest " + id + ", named " +  title + " " + progress);
    }

    public void FinishQuest()
    {
        isFinished = true;
        isActive = false;
        progress = QuestStatus.FINISHED;
        if (nextQuest != null) 
        {
            nextQuest.GetComponent<Quest>().progress = QuestStatus.AVAILABLE;
        }
        GameManager.Instance.FinishQuest();
        QuestManager.instance.currentQuest.id = 0;
        QuestManager.instance.currentQuest.description = string.Empty;
        QuestManager.instance.currentQuest.progress = QuestStatus.NOT_AVAILABLE;
        QuestManager.instance.currentQuest.isFinished = false;
        QuestManager.instance.currentQuest.isActive = false;
        Debug.Log("Quest " + id + ", named " + title + " " + progress);
    }
    public void CheckGoals()
    {
        allGoalsCleared = true;

            foreach (GameObject go in questGoals)
            {
                QuestGoalCheck goalStatus = go.GetComponent<QuestGoalCheck>();
            if (goalStatus.isComplete == false)
            {
                allGoalsCleared = false;
                Debug.Log("QUEST " + id + " IN PROGRESS");
            }
            else
                allGoalsCleared = true;
                    FinishQuest();
                Debug.Log("QUEST " + id + " FINISHED");
            }
    }

    private void Update()
    {
        if (progress == QuestStatus.STARTED)
        {
            CheckGoals();
        }
    }
}
