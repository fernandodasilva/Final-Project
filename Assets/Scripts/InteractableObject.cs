using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableObject : MonoBehaviour
{


    // Start is called before the first frame update
    public Interactor interactor { get; private set; }

    public abstract void Interact();

    public virtual void ResetInteraction()
    {
        HUDManager.instance.ResetCursor();
    }

}
