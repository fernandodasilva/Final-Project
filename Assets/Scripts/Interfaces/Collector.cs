using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Collector : MonoBehaviour, I_Collectible
{
    [SerializeField]
    public GameObject player;
    [SerializeField]
    public GameObject holdingPosition;

    private Rigidbody itemRB;
    public UnityEvent onCollect;
    public UnityEvent onDrop;
    private Outline itemOutline;

    private Collider itemCollider;

    public bool isOnUse { get; private set; }
    public bool canBeCollected { get; private set; }


    [SerializeField]
    public Vector3 targetRotation;

    private void Awake()
    {
        itemRB = GetComponent<Rigidbody>();
        itemCollider = GetComponent<Collider>();
        itemOutline = GetComponent<Outline>();
        canBeCollected = true;
        player = GameObject.FindGameObjectWithTag("Player");
        holdingPosition = GameObject.FindGameObjectWithTag("HoldingPosition");
    }

    private void Start()
    {
        isOnUse = false;
    }

    public void Collect()
    {
        isOnUse = true;

        itemRB.isKinematic = true;
        itemCollider.isTrigger = true;
        itemOutline.enabled = false;
    }

    public void Drop()
    {
        isOnUse = false;
        Debug.Log(this.name + " is on use: " + isOnUse);

        if (transform.root != transform && transform.root != transform.GetComponent<DropPlace>())
        {
            itemRB.isKinematic = false;
            itemCollider.isTrigger = false;
            itemOutline.enabled = true;
        }
    }

    public void Deactivate()
    {
        isOnUse = false;
        canBeCollected = false;

        itemOutline.enabled = false;
    }
}
