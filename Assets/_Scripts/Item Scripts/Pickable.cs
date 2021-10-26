using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Pickable : MonoBehaviour
{
    float throwForce = 600;
    float distance;

    public float force = 10;

    public bool canHold = true;
    private bool isHolding = false;

    private Transform tempParent;
    private Rigidbody rb;

    private Vector3 iniRot;

    private void Start()
    {
        tempParent = Camera.main.transform.GetChild(0);
        rb = transform.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        distance = Vector3.Distance(transform.position, tempParent.transform.position);

        if (distance >= 10f)
        {
            isHolding = false;
        }

        //Check if it's holding
        if (isHolding)
        {
            rb.angularVelocity = Vector3.zero;
            rb.velocity = (tempParent.position - transform.position) * force;
            

            if (Input.GetMouseButtonDown(1))
            {
                rb.AddForce(tempParent.forward * throwForce);
                isHolding = false;
            }
        }
        else
            rb.useGravity = true;
    }

    public void Interact()
    {
        if (canHold)
        {
            if (isHolding)
                isHolding = false;
            else
            {
                isHolding = true;
                iniRot = transform.eulerAngles;
                rb.useGravity = false;
                rb.detectCollisions = true;
            }
        }
    }
}