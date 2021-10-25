using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    public Transform cam;

    public float interactionRange;
    RaycastHit hit;
    Pickable pickable;
    IInteractable interactable;

    public Transform interWheel;
    public TMPro.TMP_Text interDesc;

    public GameObject interactionUIPrefab;

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

        if (Input.GetKeyDown(KeyCode.G))
            CreateDebugInterWheel();
    }

    private void CreateDebugInterWheel()
    {
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
        }
    }

    private void CreateInteractionWheel(IInteractable inter)
    {

    }
}
