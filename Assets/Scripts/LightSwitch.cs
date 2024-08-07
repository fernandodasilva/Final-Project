using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSwitch : InteractableObject
{
    // Start is called before the first frame update

    private I_Interactor interactionScript;


    public bool isActive { get; private set; }

    [SerializeField]
    private List<GameObject> lights;

    public QuestGoalCheck questGoal { get; private set; }


    void Start()
    {
        questGoal = GetComponent<QuestGoalCheck>();
        isActive = false;
        foreach (var light in lights)
        {
            light.SetActive(false);
        }
    }



    // Update is called once per frame
    public override void Interact()
    {
        if (!isActive)
        {
            GameManager.Instance.ActivatePower();
            questGoal.CompleteGoal();
            foreach (var light in lights)
            {
                light.SetActive(true);
                isActive = true;
            }
        }
        else
            return;
    }
}
