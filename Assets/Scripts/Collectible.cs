using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Jobs;

public class Collectible : MonoBehaviour
{

    [SerializeField]
    public bool KeepPosition { get; private set; }

    private Rigidbody myRigidbody;

    [SerializeField]
    public UnityEvent OnUse { get; private set; }

    private void Awake()
    {
        myRigidbody = GetComponent<Rigidbody>();
    }

    public void Use(GameObject actor)
    {
        OnUse?.Invoke();
    }


}
