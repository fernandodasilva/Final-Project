using System.Collections;
using System.Collections.Generic;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif
using UnityEngine;


public class PauseManager : MonoBehaviour
{

    [SerializeField]
    private InputActionReference pauseInput;

    // Start is called before the first frame update
    void Start()
    {
        pauseInput.action.performed += Pause;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Pause(InputAction.CallbackContext context)
    {

    }
}
