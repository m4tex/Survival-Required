using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    Transform cam = Camera.main.transform;

    public float interactionRange;
    RaycastHit hit;
    Pickable pickable;
    IInteractable interactable;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            if (Physics.Raycast(cam.position, cam.forward, out hit))
                if (hit.transform.TryGetComponent<Pickable>(out pickable))
                    pickable.Interact();

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (Physics.Raycast(cam.position, cam.forward, out hit))
                if (hit.transform.TryGetComponent<IInteractable>(out interactable))
                    CreateInteractionWheel(interactable);
        }
    }

    private void CreateInteractionWheel(IInteractable inter)
    {

    }
}
