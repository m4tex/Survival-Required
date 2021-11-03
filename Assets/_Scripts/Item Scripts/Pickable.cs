using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Pickable : MonoBehaviour
{
    float throwForce = 600;
    float distance;

    public float force = 10;
    public float maxVel;

    public bool canHold = true;
    private bool isHolding = false;

    private static Transform tempParent;
    private Rigidbody rb;

    public void Start()
    {
        rb = GetComponent<Rigidbody>();

        if(tempParent == null)
            tempParent = Camera.main.transform.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        //Check if it's holding
        if (isHolding)
        {
            distance = Vector3.Distance(transform.position, tempParent.transform.position);
            
            rb.angularVelocity = Vector3.zero;
            rb.velocity = (tempParent.position - transform.position) * force;

            if (distance > 4f)
                Interact();

            if (Input.GetMouseButtonDown(1))
            {
                rb.velocity = Vector3.zero;
                rb.AddForce(tempParent.forward * throwForce);
                Interact();
            }
        }    
    }

    public void Interact()
    {
        if (canHold)
        {
            if (isHolding)
            {
                isHolding = false;
                rb.useGravity = true;
            }
            else
            {
                isHolding = true;
                rb.useGravity = false;
            }
        }
    }
}