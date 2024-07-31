using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGoalCheck : MonoBehaviour
{

    
    public bool isComplete { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        isComplete = false;
    }

    // Update is called once per frame
    public void Evaluate()
    {
        if (transform.root.childCount > 0)
        {
            CompleteGoal();
        }
        else
            return;
    }


    public void CompleteGoal()
    {
        isComplete = true;
        Debug.Log("Goal complete? " + isComplete);
    }
}
