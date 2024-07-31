using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    [SerializeField]
    private float moveSpeed = 1f;


    private CharacterController _controller;
    private InputActions _actions;
    private GameObject _mainCamera;

    // Start is called before the first frame update
    void Start()
    {
       _controller = GetComponent<CharacterController>();
        _actions = GetComponent<InputActions>();
        _actions.Default.Enable();

        _actions.Default.Move.performed += Move;

    }

    private void Move(InputAction.CallbackContext obj)
    {
        Debug.Log("Move pressed");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
