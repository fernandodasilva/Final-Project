using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[System.Serializable] //ok é serializable mesmo
public class Quest 
{
    public int id;      //ID da missão em si - SERÁ NECESSÁRIO para pegar os dados da missão pelo ID
    public string title; //nome da missão
    public string description; //descrição da missão
    public int nextQuestID;

    public List<GameObject> questGoals;

    public enum QuestStatus { NOT_AVAILABLE, AVAILABLE, STARTED, FINISHED } //ok
    public bool isFinished;
    public bool isActive;
    public QuestStatus progress; //estado da missão atual (enum)

    public string finishMessage; //a mostrar quando a missão for completada
    private bool allGoalsCleared;

    [SerializeField]
    public GameObject nextQuest; //próxima quest, se houver alguma

    //métodos
    public void StartQuest()
    {

    allGoalsCleared = false;
    progress = QuestStatus.STARTED;

    }

    public void FinishQuest()
    {
        progress = QuestStatus.FINISHED;
        isFinished = true;
        isActive = false;


        GameManager.Instance.FinishQuest();
        Debug.Log("Quest " + id + ", named " + title + " " + progress);

        if (nextQuest.GetComponent<Quest>() != null)
        {
            nextQuest.GetComponent<Quest>().progress = QuestStatus.AVAILABLE;
            Debug.Log("Quest " + nextQuest.GetComponent<Quest>().id + " available");
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
