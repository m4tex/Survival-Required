using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class Interaction : MonoBehaviour
{
    public Transform cam;

    public float interactionRange;
    RaycastHit hit;
    Pickable pickable;
    IInteractable interactable;

    bool wheelActive;
    public Transform interWheel;
    public Image interSelection;
    public TMPro.TMP_Text interDescText;

    public GameObject interactionUIPrefab;
    public GameObject[] wheelSelections;
    public static Interaction ins;
    private float degreesPerItem;
    private int currentSelection = -1;

    private KeyValuePair<string, IInteractable.Interaction>[] interArr;

    private void Awake() =>
        ins = this;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            if (Physics.Raycast(cam.position, cam.forward, out hit))
                if (hit.transform.root.TryGetComponent<Pickable>(out pickable))
                    pickable.Interact();

        if (Input.GetKeyDown(KeyCode.F))
            if (Physics.Raycast(cam.position, cam.forward, out hit))
                if (hit.transform.root.TryGetComponent<IInteractable>(out interactable))
                    CreateInteractionWheel(interactable);

        if (Input.GetKeyUp(KeyCode.F))
            if (wheelActive)
                CloseWheel();

        if (wheelActive)
        {
            int id = CheckClosestButton();
            SelectOnWheel(id);

            if (Input.GetMouseButtonDown(0))
            {
                interArr[id].Value();
                CloseWheel();
            }
        }
    }

    private void CloseWheel()
    {
        interWheel.GetChild(2).DestroyChildren();
        interWheel.gameObject.Toggle(false);
        cam.GetComponent<MouseLook>().cameraLock = false;
        wheelActive = false;
    }

    private void CreateInteractionWheel(IInteractable inter)
    {

        interWheel.gameObject.Toggle(true);

        cam.GetComponent<MouseLook>().cameraLock = true;

        interArr = inter.interactions.ToArray();

        wheelSelections = new GameObject[interArr.Length];

        interSelection.fillAmount = (1f / interArr.Length);

        degreesPerItem = 360f / interArr.Length;

        for (int i = 0; i < interArr.Length; i++)
        {
            GameObject interactionUI = Instantiate(interactionUIPrefab, interWheel.GetChild(2));
            interactionUI.transform.Rotate(new Vector3(0, 0, degreesPerItem * i));
            interactionUI.transform.localPosition = interactionUI.transform.up * 288f;
            interactionUI.transform.eulerAngles = Vector3.zero;

            wheelSelections[i] = interactionUI;
        }

        wheelActive = true;
    }

    //make this a for loop to simplify and remove FindIndex
    public int CheckClosestButton()
    {
        GameObject nearestButton = null;

        float closestDistanceSqr = Mathf.Infinity;

        Vector3 mousePos = Input.mousePosition;

        foreach (GameObject potentialTarget in wheelSelections)
        {
            Vector3 directionToTarget = potentialTarget.transform.position - mousePos;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if(dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                nearestButton = potentialTarget;
            }
        }

        return wheelSelections.FindIndex(nearestButton);
    }

    public void SelectOnWheel(int id)
    {
        if(id != currentSelection)
        {
            interSelection.transform.DOLocalRotate(new Vector3(0, 0, id * degreesPerItem - degreesPerItem / 2), 0.1f);
            interDescText.text = interArr[id].Key;
            currentSelection = id;
        }
    }
}