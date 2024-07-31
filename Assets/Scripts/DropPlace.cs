using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DropPlace : MonoBehaviour
{


    [SerializeField]
    public int id;
    public bool isHovered { get;private set; }

    [SerializeField]
    public Vector3 itemRotation;
    [SerializeField]
    public Vector3 itemPosition;


    private I_Interactor interactionScript;

    public QuestGoalCheck questGoal {  get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        interactionScript = GetComponent<I_Interactor>();
        questGoal = GetComponent<QuestGoalCheck>();
        isHovered = false;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Book>() != null)
        {
            isHovered = true;
            Debug.Log(other.name + " entered the drop place? " + isHovered + ". " +  other.name + " is in correct drop place? " + other.GetComponent<Book>().isInCorrectDropPlace);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Book>() != null)
        {

            isHovered = false;

        }
    }

    public void Evaluate()
    {
        if (transform.childCount > 0)
        {
            questGoal.Evaluate();
        }
        else
            return;
    }



}
