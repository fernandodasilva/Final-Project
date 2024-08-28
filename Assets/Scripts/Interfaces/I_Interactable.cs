using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface I_Interactable
{
    // Start is called before the first frame update
    public void ToggleHighlight(bool value) { }

    public void OnRaycastHit();
    public void OnRaycastLeave();

    
}
