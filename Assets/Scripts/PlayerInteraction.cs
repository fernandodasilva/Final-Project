using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using System.Runtime.CompilerServices;
using UnityEngine.ProBuilder.MeshOperations;
using static UnityEngine.Rendering.DebugUI;



#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{

    [SerializeField]
    private LayerMask collectiblesLayerMask;
    [SerializeField]
    private LayerMask charactersLayerMask;
    [SerializeField]
    private LayerMask enemiesLayerMask;
    [SerializeField]
    private LayerMask interactablesLayerMask;
    [SerializeField]
    private LayerMask dropPlaceLayerMask;

    [SerializeField]
    private LayerMask groundLayerMask;
    [SerializeField]
    private LayerMask sceneryLayerMask;


    [SerializeField]
    private InputActionReference interactionAction, dropAction, useAction, toggleAction;
    [SerializeField]
    private InputActionReference pauseAction;

    [SerializeField]
    GameObject holdingPlace;

    GameObject heldObject;
    GameObject interactingObject;


    public bool isHoldingObject;
    public bool isInConversation;


    [SerializeField, Min(1.0f)]
    private float reach = 1.5f;
    I_Interactor currentObject = null;
    InteractableObject currentInteractableObject = null;


    private RaycastHit hit;

    private void Start()
    {
        interactionAction.action.performed += Interact;
        dropAction.action.performed += Drop;
        toggleAction.action.performed += Toggle;
        pauseAction.action.performed += Pause;

        isHoldingObject = false;
        isInConversation = false;
    }

    private void Toggle(InputAction.CallbackContext context)
    {
        Book currentBook;
        if (isHoldingObject && heldObject.GetComponent<Book>())
        {
            currentBook = heldObject.GetComponent<Book>();

            currentBook.ToggleBook();
        }
    }

    private void Drop(InputAction.CallbackContext context)
    {
        isHoldingObject = false;


        if (heldObject != null && !isHoldingObject)
        {

            I_Collector collectionScript = heldObject.GetComponent<I_Collector>();

            collectionScript.Drop();
            if (heldObject.GetComponent<Book>().isInCorrectDropPlace == false)
            {
                heldObject.transform.SetParent(null);
            }

            HUDManager.instance.ToggleBookSheet(false);

        }

        if (isInConversation)
        {
            GameManager.Instance.EndInteraction();
            isInConversation = false;
            ResetInteraction();
        }
    }

    private void Interact(InputAction.CallbackContext context)
    {
        if (hit.collider.GetComponent<InteractableObject>())
        {
            interactingObject = hit.collider.gameObject;
            interactingObject.GetComponent<InteractableObject>().Interact();
        }


        if (hit.collider.GetComponent<I_Collector>())
        {
            Debug.Log(hit.collider.name);
            Rigidbody rb = hit.collider.GetComponent<Rigidbody>();

            heldObject = hit.collider.gameObject;
            isHoldingObject = true;
            if (heldObject.GetComponent<Book>().isInCorrectDropPlace == false)
            {
                heldObject.GetComponent<I_Collector>().Collect();
                heldObject.transform.SetParent(holdingPlace.transform);
                heldObject.transform.position = holdingPlace.transform.position;

                heldObject.transform.localEulerAngles = holdingPlace.transform.localEulerAngles + heldObject.GetComponent<I_Collector>().targetRotation;
                EnableDropping();

                if (rb != null)
                {
                    rb.isKinematic = true;
                }
            }

            return;
        }

    }

    public void Pause(InputAction.CallbackContext context)
    {
        if(GameManager.Instance.IsPaused)
        {
            GameManager.Instance.ResumeGame();
        }
        else
        {
            GameManager.Instance.PauseGame();
        }
    }

   

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * reach, Color.red);
        int lm = collectiblesLayerMask | interactablesLayerMask | charactersLayerMask | enemiesLayerMask | dropPlaceLayerMask;

        //colide com qualquer coisa ao alcance do jogador?
        if (!isHoldingObject && !isInConversation && Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, reach, lm)
            || isHoldingObject && Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, reach, dropPlaceLayerMask))

        {
            I_Interactor newInteractor = hit.collider.GetComponent<I_Interactor>();
            currentInteractableObject = hit.collider.GetComponent<InteractableObject>();
            //Já existe algum item com o interactor ativo e não é o atual?
            if (HUDManager.instance.isPaperSheetOn == false && isInConversation == false && currentObject && newInteractor != currentObject)
            {
                currentObject.ToggleHighlight(false);
            }
            if (newInteractor.enabled)
            {
                SetNewCurrentInteractable(newInteractor);
            }
            else //o interactor não está ativo?
            {
                DisableCurrentInteractable();
                InteractableObject newInteractableObject = hit.collider.GetComponent<InteractableObject>();
                newInteractableObject.ResetInteraction();

                HUDManager.instance.questPanel.SetActive(false);
            }
        }
            else if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, reach, groundLayerMask) ||
                isHoldingObject && Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, reach, sceneryLayerMask))
        {
            DisableCurrentInteractable();
            ResetInteraction();
        }

            else //nada no alcance?
        {

            DisableCurrentInteractable();
            ResetInteraction();
            HUDManager.instance.questPanel.SetActive(false);

            if (isInConversation)
            {
                isInConversation = false;
            }
        }
    }



    void SetNewCurrentInteractable(I_Interactor newInteractor)
    {
        currentObject = newInteractor;
        if (newInteractor.enabled && newInteractor.shouldDisplayOutline == true)
        {
            currentObject.ToggleHighlight(true);
        }
        if (!HUDManager.instance.isPaperSheetOn)
        {
            HUDManager.instance.EnableInteractionBox(currentObject.interactionMessage, currentObject.instructionMessage);
        }

    }

    void DisableCurrentInteractable()
    {
        HUDManager.instance.DisableInteractionBox();
        if (currentObject)
        {
            currentObject.ToggleHighlight(false);
            currentObject = null;
        }
    }

    void ResetInteraction()
    {
        if (currentInteractableObject)
        {
            currentInteractableObject.ResetInteraction();
        }
        else
            return;
    }


    void EnableDropping()
    {
        dropAction.action.Enable();
    }

    void DisableDropping()
    {
        dropAction.action.Disable();
    }

    void EnableToggling()
    {
        toggleAction.action.Enable();
    }

    void DisableToggling()
    {
        toggleAction.action.Disable();
    }
}
