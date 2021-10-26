using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

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
        {
            if (wheelActive)
            {
                interWheel.DestroyChildren(0);
                interWheel.gameObject.Toggle(false);
                cam.GetComponent<MouseLook>().cameraLock = false;
            }
        }

        if (wheelActive)
        {
            int id = CheckClosestButton();
            SelectOnWheel(id);
        }
    }

    #region debug
    private void CreateDebugInterWheel()
    {
        cam.GetComponent<MouseLook>().cameraLock = true;

        interWheel.gameObject.Toggle(true);

        Dictionary<string, int> inter = new Dictionary<string, int>();
        inter.Add("inter1", 0);
        inter.Add("inter2", 1);
        inter.Add("inter3", 2);
        inter.Add("inter4", 3);
        inter.Add("inter5", 4);
        inter.Add("inter6", 4);
        inter.Add("inter7", 4);
        inter.Add("inter8", 4);
        inter.Add("inter9", 4);
        inter.Add("inter10", 4);

        float degreesPerItem = 360f / inter.Count;

        for (int i = 0; i < inter.Count; i++)
        {
            GameObject interactionUI = Instantiate(interactionUIPrefab, interWheel);
            interactionUI.transform.Rotate(new Vector3(0, 0, degreesPerItem * i));
            interactionUI.transform.localPosition = interactionUI.transform.up * 288f;
            interactionUI.transform.eulerAngles = Vector3.zero;
            interactionUI.GetComponent<InteractionUI>().interDescText = interDescText;
            interactionUI.GetComponent<InteractionUI>().interactionDesc = inter.Keys.ElementAt(i);
        }
    }
    #endregion

    private void CreateInteractionWheel(IInteractable inter)
    {

        interWheel.gameObject.Toggle(true);

        cam.GetComponent<MouseLook>().cameraLock = true;

        KeyValuePair<string, IInteractable.Interaction>[] interArr = inter.interactions.ToArray();

        wheelSelections = new GameObject[interArr.Length];

        interSelection.fillAmount = (1f / interArr.Length);

        degreesPerItem = 360f / interArr.Length;

        for (int i = 0; i < interArr.Length; i++)
        {
            GameObject interactionUI = Instantiate(interactionUIPrefab, interWheel);
            interactionUI.transform.Rotate(new Vector3(0, 0, degreesPerItem * i));
            interactionUI.transform.localPosition = interactionUI.transform.up * 288f;
            interactionUI.transform.eulerAngles = Vector3.zero;
            interactionUI.GetComponent<InteractionUI>().Setup(interDescText, interArr[i], i);

            wheelSelections[i] = interactionUI;
        }

        wheelActive = true;
    }

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

        return nearestButton.GetComponent<InteractionUI>().selID;
    }

    public void SelectOnWheel(int id)
    {
        interSelection.transform.localEulerAngles = new Vector3(0, 0, id * degreesPerItem - degreesPerItem / 2);
        interDescText.text = wheelSelections[id].GetComponent<InteractionUI>().interactionDesc;

        
    }
}