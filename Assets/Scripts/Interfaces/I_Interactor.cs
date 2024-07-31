using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class I_Interactor : MonoBehaviour
{


    [SerializeField]
    public bool shouldDisplayInstructionMessage;
    [SerializeField]
    public bool shouldDisplayOutline;

    private Outline outline;
    public string interactionMessage;
    public string instructionMessage;
    public string defaultInstructionMessage;
    public UnityEvent onInteract;
    public UnityEvent onRaycastHit;
    public UnityEvent onRaycastLeave;
    public UnityEvent onClick;


    // Start is called before the first frame update

    private void Start()
    {
        outline = GetComponent<Outline>();
        ToggleHighlight(false);
        defaultInstructionMessage = instructionMessage;
    }

    public void RaycastHit()
    {
        onRaycastHit.Invoke();
    }

    public void RaycastLeave()
    {
        onRaycastLeave.Invoke();
    }

    public void Click()
    {
        onClick.Invoke();
    }

 

    public void Interact()
    {
        onInteract.Invoke();
    }

    public void ToggleHighlight(bool value)
    {
        if (value == true)
        {

            onRaycastHit.Invoke();
            Debug.Log("Highlight Toggled:" + value);
            if (shouldDisplayOutline == true)
            {
                outline.enabled = true;
            }
            else
            {
                outline.enabled = false;
            }
            if (shouldDisplayInstructionMessage == true)
            {
                HUDManager.instance.ToggleInteractionOptionText(true);
            }
            else
            {
                HUDManager.instance.ToggleInteractionOptionText(false);
            }
        }
        else
        {
            outline.enabled = false;
            Debug.Log("Highlight Toggled:" + value);

        }


    }
}
